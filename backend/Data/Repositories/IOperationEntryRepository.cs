using OriginSolutions.Entities;
namespace OriginSolutions.Data.Repositories
{
    public interface IOperationEntryRepository
    {
        public Task<int> Add(string card, string ubk, OperationType type);
        public Task<IEnumerable<OperationEntry>> GetAllByUBK(string ubk);
        public Task<IEnumerable<OperationEntry>> GetAllByCard(string cardNumber);
        public Task<IEnumerable<OperationEntry>> GetAllByType(OperationType type);
    }
}