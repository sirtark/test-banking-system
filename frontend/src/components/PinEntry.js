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

  const handleCardNumberChange = (e) => {
    const formattedCardNumber = formatCardNumber(e.target.value);
    setCardNumber(formattedCardNumber);
  };

  // Allow only numeric characters in the PIN
  const handlePinChange = (e) => {
    const numericPin = e.target.value.replace(/\D/g, '');
    setPin(numericPin);
  };

  const handleCardSubmit = async () => {
    const cardNumberWithoutSpaces = cardNumber.replace(/\D/g, '');

    // Validate card number and PIN
    if (cardNumberWithoutSpaces.length !== 16) {
      setErrorMessage('Card number must have exactly 16 digits.');
      return;
    }

    if (pin.length !== 4) {
      setErrorMessage('PIN must have exactly 4 digits.');
      return;
    }

    // Clear error message before making the request
    setErrorMessage('');

    try {
      const response = await api.post('/account/login', { cardNumber: cardNumberWithoutSpaces, pin });

      if (response.status === 200 && response.data?.sessionToken) {
        const sessionToken = response.data.sessionToken; // Token returned by API
        localStorage.setItem('sessionToken', sessionToken);
        navigate('/operations', { state: { cardNumber } });
      } else {
        // Display error from the response if available
        setErrorMessage(response.data?.message || 'Card not found, blocked, or invalid PIN');
      }
    } catch (error) {
      console.error("Login error:", error);
      setErrorMessage(error.response?.data?.message || 'An error occurred. Please try again.');
    }
  };

  return (
    <Container maxWidth="sm" sx={{ mt: 4 }}>
      <Paper elevation={3} sx={{ p: 4 }}>
        <Typography variant="h5" gutterBottom>
          Enter Card Details
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
            inputProps={{ maxLength: 19 }} // 16 digits + 3 spaces
            helperText="Format: XXXX XXXX XXXX XXXX"
            error={!!errorMessage}
          />
          <TextField
            label="PIN"
            variant="outlined"
            type="password"
            value={pin}
            onChange={handlePinChange}
            fullWidth
            inputProps={{ maxLength: 4 }}
            helperText="4 digits required (decimal characters only)"
            error={!!errorMessage}
          />
          {errorMessage && (
            <FormHelperText error>{errorMessage}</FormHelperText>
          )}
          <Button
            variant="contained"
            color="primary"
            onClick={handleCardSubmit}
            disabled={!cardNumber || pin.length !== 4 || cardNumber.replace(/\D/g, '').length !== 16}
          >
            Accept
          </Button>
        </Box>
      </Paper>
    </Container>
  );
};

export default Home;
