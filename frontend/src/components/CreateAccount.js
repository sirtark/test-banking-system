import React, { useState } from 'react';
import { TextField, Button, Box, Container, Paper, Typography, FormHelperText, Card, CardContent } from '@mui/material';
import api from '../services/api';
import { useNavigate } from 'react-router-dom';

const CreateAccount = () => {
  const [dni, setDni] = useState('');
  const [fullName, setFullName] = useState('');
  const [pin, setPin] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const [accountInfo, setAccountInfo] = useState(null); // State to hold the account data
  const navigate = useNavigate();

  // Function to format DNI with spaces
  const formatDni = (value) => {
    return value.replace(/\D/g, '').replace(/(\d{2})(\d{3})(\d{3})/, '$1 $2 $3').trim();
  };

  const handleDniChange = (e) => {
    const formattedDni = formatDni(e.target.value);
    setDni(formattedDni);
  };

  const handlePinChange = (e) => {
    const numericPin = e.target.value.replace(/\D/g, '');
    setPin(numericPin);
  };

  const handleCreateAccount = async () => {
    try {
      // Remove spaces before sending to the API
      const formattedDni = dni.replace(/\s+/g, ''); 
      const response = await api.post('/account/create', { nid: formattedDni, owner: fullName, pin });
      console.log("Account creation response:", response);  // Log the full response to see the data

      if (response.status === 200) {
        alert('Account created successfully!');
        setAccountInfo(response.data); // Set the received account info
      } else {
        setErrorMessage('Account creation failed. Please try again.');
      }
    } catch (error) {
      console.error("Account creation error:", error);
      setErrorMessage('An error occurred while creating the account.');
    }
  };

  const handleBackToLogin = () => {
    navigate('/'); // Navigate to the login page
  };

  return (
    <Container maxWidth="sm" sx={{ mt: 4 }}>
      <Paper elevation={3} sx={{ p: 4 }}>
        <Typography variant="h5" gutterBottom>
          Create New Account
        </Typography>
        {!accountInfo ? ( // If accountInfo is null, show the form
          <Box component="form" noValidate autoComplete="off" sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
            <TextField
              label="DNI"
              variant="outlined"
              value={dni}
              onChange={handleDniChange}
              fullWidth
              inputProps={{ maxLength: 11 }} // DNI formatted as xx xxx xxx
              helperText="Format: XX XXX XXX"
              error={!!errorMessage}
            />
            <TextField
              label="Full Name"
              variant="outlined"
              value={fullName}
              onChange={(e) => setFullName(e.target.value)}
              fullWidth
            />
            <TextField
              label="PIN"
              variant="outlined"
              type="password"
              value={pin}
              onChange={handlePinChange}
              fullWidth
              inputProps={{ maxLength: 4 }}
              helperText="4 digits required"
              error={!!errorMessage}
            />
            {errorMessage && <FormHelperText error>{errorMessage}</FormHelperText>}
            <Button
              variant="contained"
              color="primary"
              onClick={handleCreateAccount}
              disabled={dni.length !== 10 || fullName === '' || pin.length !== 4} // Ensure all fields are filled
            >
              Create Account
            </Button>
          </Box>
        ) : (
          // If accountInfo exists, show the account details and the "Back to Login" button
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
            <Card sx={{ mt: 4 }}>
              <CardContent>
                <Typography variant="h6">Account Information</Typography>
                <Typography variant="body1"><strong>NID:</strong> {accountInfo.nid}</Typography>
                <Typography variant="body1"><strong>UBK:</strong> {accountInfo.ubk}</Typography>
                <Typography variant="body1"><strong>Alias:</strong> {accountInfo.alias}</Typography>
                <Typography variant="body1"><strong>Card Number:</strong> {accountInfo.number}</Typography>
              </CardContent>
            </Card>
            <Button
              variant="outlined"
              color="primary"
              onClick={handleBackToLogin}
            >
              Back to Login
            </Button>
          </Box>
        )}
      </Paper>
    </Container>
  );
};

export default CreateAccount;
