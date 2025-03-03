import React, { useState } from 'react';
import { Container, Paper, Typography, TextField, Button, Box } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';

const Withdrawal = () => {
  const [amount, setAmount] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const navigate = useNavigate();

  const handleWithdrawal = async () => {
    try {
      const token = localStorage.getItem('sessionToken');
      if (!token) {
        // Redirigir automáticamente a la página de inicio si la sesión ha expirado
        navigate('/');
        return;
      }

      const filteredAmount = amount.replace(/[^0-9]/g, '');
      const numericAmount = parseInt(filteredAmount, 10);

      if (isNaN(numericAmount) || numericAmount <= 0) {
        setErrorMessage('Please enter a valid amount.');
        return;
      }

      console.log("Sending withdrawal request with:", numericAmount, "Token:", token);

      const response = await api.post(
        'operation/card-withdraw',
        numericAmount,
        {
          headers: {
            'Content-Type': 'application/json',
            sessionToken: token,
          }
        }
      );

      console.log("Withdrawal response:", response);

      if (response.status === 200) {
        setAmount('');
        setErrorMessage('');
        alert('Withdrawal successful');
      } else {
        setErrorMessage('Error: Amount exceeds balance or invalid transaction');
      }
    } catch (error) {
      console.error("Withdrawal error:", error);
      if (error.response) {
        if (error.response.data === "SignOut") {
          // Redirigir automáticamente a la página de inicio en caso de "SignOut"
          navigate('/');
        } else {
          setErrorMessage(error.response.data || 'Unexpected error occurred');
        }
      }
    }
  };

  return (
    <Container maxWidth="sm" sx={{ mt: 4 }}>
      <Paper elevation={3} sx={{ p: 4 }}>
        <Typography variant="h5" gutterBottom>
          Enter Withdrawal Amount
        </Typography>
        <Box 
          component="form" 
          noValidate 
          autoComplete="off" 
          sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}
        >
          <TextField
            label="Amount"
            variant="outlined"
            type="text"
            value={amount}
            onChange={(e) => {
              setAmount(e.target.value.replace(/[^0-9]/g, ''));
              setErrorMessage('');
            }}
            placeholder="Amount"
            fullWidth
          />
          <Button 
            variant="contained" 
            color="primary" 
            onClick={handleWithdrawal} 
            disabled={!amount}
          >
            Accept
          </Button>
          <Box sx={{ mt: 2, display: 'flex', gap: 2 }}>
            <Button variant="outlined" color="error" onClick={() => navigate('/operations')}>
              Back to Operations
            </Button>
            <Button variant="outlined" color="error" onClick={() => navigate('/')}>
              Quit
            </Button>
          </Box>
          {errorMessage && (
            <Typography variant="body2" color="error">
              {errorMessage}
            </Typography>
          )}
        </Box>
      </Paper>
    </Container>
  );
};

export default Withdrawal;
