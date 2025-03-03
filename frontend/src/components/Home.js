import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Container, TextField, Button, Typography, Paper, Box, FormHelperText } from '@mui/material';
import api from '../services/api';

const Home = () => {
  const [cardNumber, setCardNumber] = useState('');
  const [pin, setPin] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const navigate = useNavigate();

  // Function to format card number with spaces every 4 digits
  const formatCardNumber = (value) => {
    return value.replace(/\D/g, '').replace(/(.{4})(?=.)/g, '$1 ').trim();
  };

  // Function to remove spaces from card number (for submission)
  const removeSpaces = (value) => {
    return value.replace(/\s+/g, ''); // Removes all spaces
  };

  const handleCardNumberChange = (e) => {
    const formattedCardNumber = formatCardNumber(e.target.value);
    setCardNumber(formattedCardNumber); // Maintain formatted card number with spaces
  };

  // Allow only numeric characters in the PIN
  const handlePinChange = (e) => {
    const numericPin = e.target.value.replace(/\D/g, '');
    setPin(numericPin);
  };

  const handleCardSubmit = async () => {
    try {
      // Remove spaces before sending to the API
      const cardNumberWithoutSpaces = removeSpaces(cardNumber);

      console.log("Sending request with:", { cardNumber: cardNumberWithoutSpaces, pin });
      const response = await api.post('/account/login', { cardNumber: cardNumberWithoutSpaces, pin });
      console.log("Response received:", response);

      if (response.status === 200) {
        const sessionToken = response.data; // Token returned by API
        localStorage.setItem('sessionToken', sessionToken);
        navigate('/operations', { state: { cardNumber } });
      } else {
        // Set the error message as the response data
        setErrorMessage(response.data || 'Unknown error occurred');
      }
    } catch (error) {
      console.error("Login error:", error);

      // Ensure we capture the error message correctly
      setErrorMessage(error.response?.data || 'An error occurred while processing your request. Please try again.');
    }
  };

  const handleCreateAccount = () => {
    // Navigate to the create account page
    navigate('/create-account');
  };

  return (
    <Container maxWidth="sm" sx={{ mt: 4 }}>
      <Paper elevation={3} sx={{ p: 4 }}>
        <Typography variant="h5" gutterBottom>
          Login
        </Typography>
        <Box
          component="form"
          noValidate
          autoComplete="off"
          sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}
        >
          <TextField
            label="Card Number"
            variant="outlined"
            value={cardNumber}
            onChange={handleCardNumberChange}
            fullWidth
            inputProps={{
              maxLength: 19, // 16 digits + 3 spaces
            }}
            helperText="Format: XXXX XXXX XXXX XXXX"
            error={!!errorMessage} // Show error if any
          />
          <TextField
            label="PIN"
            variant="outlined"
            type="password"
            value={pin}
            onChange={handlePinChange}
            fullWidth
            inputProps={{
              maxLength: 4,
            }}
            helperText="4 digits required (decimal characters only)"
            error={!!errorMessage} // Show error if any
          />
          {errorMessage && (
            <FormHelperText error>{errorMessage}</FormHelperText>
          )}
          <Button
            variant="contained"
            color="primary"
            onClick={handleCardSubmit}
            disabled={cardNumber.length !== 19 || pin.length !== 4} // Card number with spaces, should be 19 characters
          >
            Accept
          </Button>

          <Button
            variant="outlined"
            color="secondary"
            onClick={handleCreateAccount}
            sx={{ mt: 2 }}
          >
            Create New Account
          </Button>
        </Box>
      </Paper>
    </Container>
  );
};

export default Home;
