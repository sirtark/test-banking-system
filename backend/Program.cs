using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OriginSolutions.Data;
using OriginSolutions.Dto;
using OriginSolutions.Entities;
using OriginSolutions.Entities.Operations;
using OriginSolutions.Services;

var builder = WebApplication.CreateBuilder(args);

// Load AppSettings from appsettings.json
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Register services
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<UBKGenService>();
builder.Services.AddScoped<AliasGenService>();
builder.Services.AddScoped<CardGenService>();
builder.Services.AddSqlServer<AppDbContext>(builder.Configuration.GetConnectionString("LAN"));
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => 
            policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
});

var app = builder.Build();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Endpoint to get account info by alias or UBK
app.MapGet("account/info", async([FromQuery] string aliasOrUbk, IUnitOfWork context) => {
    if(Account.UBK_Regex().IsMatch(aliasOrUbk))
    {
        if(await context.AccountRepository.GetByUBK(aliasOrUbk) is not Account found)
            return Results.NotFound($"Account with UBK {aliasOrUbk} was not found.");
        return Results.Ok(new {
            found.Owner,
            found.NID,
            found.Alias,
            found.UBK
        });
    }
    else if(Account.Alias_Regex().IsMatch(aliasOrUbk))
    {
        if(await context.AccountRepository.GetByAlias(aliasOrUbk) is not Account found)
            return Results.NotFound($"Account with Alias {aliasOrUbk} was not found.");
        return Results.Ok(new {
            found.Owner,
            found.NID,
            found.Alias,
            found.UBK
        });
    }
    return Results.Conflict("Invalid Alias or UBK was received.");
});

// Endpoint to create a new account
app.MapPost("account/create", async([FromBody] AccountDto account, UBKGenService ubkGen, AliasGenService aliasGen, CardGenService cardGen, IUnitOfWork context, IOptions<AppSettings> appSettings) => {
    var settings = appSettings.Value;

    // Validate the account owner's name, NID, and PIN format
    if(!Account.Name_Regex().IsMatch(account.Owner))
        return Results.Conflict($"Invalid fullname was received: {account.Owner}");
    if(!Account.Nid_Regex().IsMatch(account.Nid))
        return Results.Conflict($"Invalid NID was received: {account.Nid}");
    if(!Card.Pin_Regex().IsMatch(account.Pin))
        return Results.Conflict($"Invalid PIN was received: {account.Pin}");
    
    var newAccount = new Account(){
        Owner = account.Owner.ToUpper(),
        NID = uint.Parse(account.Nid),
        UBK = ubkGen.GenerateUBK(),
        Balance = settings.InitialAccountBalance, // Use the configurable initial balance
        Alias = aliasGen.GenerateAlias(),
        Cards = []
    };

    // Generate a unique card number using the configured prefix
    string generatedCard;
    while((generatedCard = cardGen.GenerateCardNumber(settings.CardPrefix)) is not null && (await context.CardRepository.GetByNumber(generatedCard)) is not null)
    {}

    var hashedPin = BitConverter.ToString(SHA256.HashData(Encoding.UTF8.GetBytes(account.Pin))).Replace("-", "").ToLower();
    var accountOwnerCard = new Card(){
        Owner = newAccount.Owner,
        NID = newAccount.NID,
        Number = generatedCard!,
        Pin = hashedPin,
        Cvv = Random.Shared.Next(0, 1000).ToString("D3"),
        Expire = DateOnly.FromDateTime(DateTime.Now.AddMonths(36)),
        FK_Account = newAccount.UBK,
        Account = newAccount
    };

    await context.AccountRepository.Add(newAccount);
    await context.CardRepository.Add(accountOwnerCard);
    await context.CompleteAsync();
    
    return Results.Ok(new {
        newAccount.NID,
        newAccount.UBK,
        newAccount.Alias,
        accountOwnerCard.Number
    });
});

// Endpoint for login
app.MapPost("account/login", async([FromBody] LoginDto login, IUnitOfWork context, IOptions<AppSettings> appSettings) => {
    var settings = appSettings.Value;

    if(!Card.Number_Regex().IsMatch(login.CardNumber))
        return Results.Conflict($"Invalid card number was received: {login.CardNumber}");
    if(!Card.Pin_Regex().IsMatch(login.Pin))
        return Results.Conflict($"Invalid card pin was received: {login.Pin}");
    
    var hashedPin = BitConverter.ToString(SHA256.HashData(Encoding.UTF8.GetBytes(login.Pin))).Replace("-", "").ToLower();
    var foundCard = await context.CardRepository.GetByNumber(login.CardNumber);
    
    if(foundCard is null)
        return Results.NotFound($"Card with number {login.CardNumber} doesn't exist.");
    else if(foundCard.Blocked)
        return Results.Conflict("Card has been blocked due to multiple invalid login attempts.");
    else if(foundCard.Pin != hashedPin)
    {
        foundCard.LoginFails++;
        if(foundCard.LoginFails > settings.MaxLoginAttempts)
        {
            foundCard.Blocked = true;
            if(await context.SessionRepository.Exists(login.CardNumber))
                await context.SessionRepository.Close(login.CardNumber);
        }
        await context.CompleteAsync();
        return Results.Conflict($"{(foundCard.Blocked ? "Card has been blocked due to multiple invalid login attempts." : "Incorrect PIN entered.")}");
    }

    var token = await context.SessionRepository.Create(login.CardNumber);
    foundCard.LoginFails = 0; // Reset failed login attempts
    await context.CompleteAsync();
    
    return Results.Ok(token);
});

// Endpoint for logout
app.MapPost("account/logout", async([FromHeader] string sessionToken, IUnitOfWork context) => {
     if(!Session.Token_Regex().IsMatch(sessionToken))
        return Results.SignOut();
    await context.SessionRepository.Close(sessionToken);
    await context.CompleteAsync();
    return Results.Ok();
});

// Endpoint to query balance
app.MapGet("operation/balance-query", async ([FromHeader] string sessionToken, IUnitOfWork context) => {
    if(!Session.Token_Regex().IsMatch(sessionToken))
        return Results.SignOut();
    
    var account = await context.SessionRepository.GetAccountByToken(sessionToken);
    if(account is null)
        return Results.NotFound("SignOut");
    
    var card = await context.SessionRepository.GetCardByToken(sessionToken);
    var entryId = await context.OperationEntryRepository.Add(card.Number, account.UBK, OperationType.BalanceQuery);
    
    await context.BalanceQueryRepository.Add(new BalanceQueryOperation()
    {
        FK_Entry = entryId,
        FK_Card = card.Number,
        FK_Account = account.UBK,
        Balance = account.Balance,
        Entry = default!,
        Account = default!,
        Card = default!
    });

    await context.CompleteAsync();
    return Results.Ok(account.Balance);
});

// Endpoint for card transaction
app.MapPost("operation/card-transaction", async ([FromHeader] string sessionToken, CardTransactionDto cardTransaction, IUnitOfWork context, IOptions<AppSettings> appSettings) => {
    var settings = appSettings.Value;

    if(!Session.Token_Regex().IsMatch(sessionToken))
        return Results.SignOut();
    
    var account = await context.SessionRepository.GetAccountByToken(sessionToken);
    if(account is null)
        return Results.NotFound("SignOut");
    
    if(cardTransaction.Amount > account.Balance)
        return Results.Conflict("Not enough money in your account.");
    
    if(cardTransaction.Amount == 0)
        return Results.Conflict("Amount cannot be 0,00.");

    Account destAccount = default!;
    if(Account.Alias_Regex().IsMatch(cardTransaction.AliasOrUBK))
        destAccount = await context.AccountRepository.GetByAlias(cardTransaction.AliasOrUBK);
    else if(Account.UBK_Regex().IsMatch(cardTransaction.AliasOrUBK))
        destAccount = await context.AccountRepository.GetByUBK(cardTransaction.AliasOrUBK);
    else
        return Results.Conflict($"Invalid Alias or UBK received: {cardTransaction.AliasOrUBK}");

    if(destAccount is null)
        return Results.NotFound($"Account with Alias or UBK {cardTransaction.AliasOrUBK} not found.");
    
    account.Balance -= cardTransaction.Amount;
    destAccount.Balance += cardTransaction.Amount;

    var card = await context.SessionRepository.GetCardByToken(sessionToken);
    var entryId = await context.OperationEntryRepository.Add(card.Number, account.UBK, OperationType.CardOriginatedTransaction);
    
    await context.TransactionRepository.Add(new TransactionOperation()
    {
        FK_OriginAccount = account.UBK,
        FK_DestinationnAccount = destAccount.UBK,
        FK_AuthorCard = card.Number,
        Amount = cardTransaction.Amount,
        OriginNewBalance = account.Balance,
        DestinationNewBalance = destAccount.Balance,
        FK_Entry = entryId,
        Entry = default!,
        OriginAccount = default!,
        DestinationAccount = default!,
        AuthorCard = default!
    });

    await context.CompleteAsync();
    return Results.Ok(account.Balance);
});

// Endpoint for card withdrawal
app.MapPost("operation/card-withdraw", async([FromHeader] string sessionToken, [FromBody] uint amount, IUnitOfWork context, IOptions<AppSettings> appSettings) => {
    var settings = appSettings.Value;

    if(!Session.Token_Regex().IsMatch(sessionToken))
        return Results.SignOut();

    var account = await context.SessionRepository.GetAccountByToken(sessionToken);
    if(account is null)
        return Results.NotFound("SignOut");
    
    if(amount < settings.CardMinimalWithdraw)
         return Results.Conflict($"The minimum withdrawal is {settings.CardMinimalWithdraw / 100},{settings.CardMinimalWithdraw % 100:D2}");
    if(amount > account.Balance)
        return Results.Conflict("There is not enough money in your account.");
    if(settings.CardWithdrawMinMultiple > 0 && amount % settings.CardWithdrawMinMultiple != 0)
        return Results.Conflict($"Please enter a multiple of {settings.CardWithdrawMinMultiple}");
    
    account.Balance -= amount;
    await context.CompleteAsync();
    return Results.Ok(account.Balance);
});

// Run the app
app.Run(app.Configuration["ListenTo"]);