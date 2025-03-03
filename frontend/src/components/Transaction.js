import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Container, TextField, Button, Typography, Paper, Box, Card, CardContent, FormHelperText } from '@mui/material';
import api from '../services/api';

const Transaction = () => {
  const [aliasOrUBK, setAliasOrUBK] = useState('');
  const [amount, setAmount] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const [accountInfo, setAccountInfo] = useState(null);
  const navigate = useNavigate();

  // Function to fetch account information
  const fetchAccountInfo = async (aliasOrUBK) => {
    try {
      const response = await api.get('account/info', { params: { aliasOrUbk: aliasOrUBK } });
      setAccountInfo(response.data); // Store the response in the state
      setErrorMessage('');
    } catch (error) {
      if (error.response) {
        setErrorMessage(error.response.data || 'Unexpected error occurred');
      } else {
        setErrorMessage('Error fetching account information');
      }
      setAccountInfo(null);
    }
  };

  const handleTransaction = async () => {
    try {
      const token = localStorage.getItem('sessionToken');
      if (!token) {
        setErrorMessage('Session expired. Please log in again.');
        return;
      }
      if (!aliasOrUBK || isNaN(amount) || amount <= 0) {
        setErrorMessage('Please enter a valid alias/UBK and amount.');
        return;
      }
      const payload = {
        aliasOrUBK,
        amount: parseInt(amount, 10),
      };

      const response = await api.post(
        'operation/card-transaction',
        payload,
        {
          headers: {
            'Content-Type': 'application/json',
            sessionToken: token,
          }
        }
      );

      if (response.status === 200) {
        alert('Transaction successful');
        setAliasOrUBK('');
        setAmount('');
        navigate('/operations');
      } else {
        setErrorMessage('Error: Transaction failed');
      }
    } catch (error) {
      if (error.response && error.response.status === 409) {
        setErrorMessage('Error: Amount exceeds balance or transaction blocked');
      } else {
        setErrorMessage('Unexpected error, please try again.');
      }
    }
  };

  const handleAliasOrUBKChange = (e) => {
    setAliasOrUBK(e.target.value);
    setAccountInfo(null); // Reset account info while user is typing
    setErrorMessage('');
  };

  const handleAliasOrUBKBlur = () => {
    if (aliasOrUBK) {
      fetchAccountInfo(aliasOrUBK);
    }
  };

  return (
    <Container maxWidth="sm" sx={{ mt: 4 }}>
      <Paper elevation={3} sx={{ p: 4 }}>
        <Typography variant="h5" gutterBottom>
          Enter Transaction Details
        </Typography>
        <Box 
          component="form" 
          noValidate 
          autoComplete="off" 
          sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}
        >
          <TextField
            label="Alias/UBK"
            variant="outlined"
            value={aliasOrUBK}
            onChange={handleAliasOrUBKChange}
            onBlur={handleAliasOrUBKBlur}
            fullWidth
            placeholder="Enter Alias or UBK"
          />
          <TextField
            label="Amount"
            variant="outlined"
            type="number"
            value={amount}
            onChange={(e) => setAmount(e.target.value)}
            fullWidth
            placeholder="Enter Amount"
          />
          {errorMessage && (
            <FormHelperText error>{errorMessage}</FormHelperText>
          )}
          {accountInfo && (
            <Card sx={{ mt: 2 }}>
              <CardContent>
                <Typography variant="h6">Account Information</Typography>
                <Typography variant="body1"><strong>Owner:</strong> {accountInfo.owner}</Typography>
                <Typography variant="body1"><strong>NID:</strong> {accountInfo.nid}</Typography>
                <Typography variant="body1"><strong>Alias:</strong> {accountInfo.alias}</Typography>
                <Typography variant="body1"><strong>UBK:</strong> {accountInfo.ubk}</Typography>
              </CardContent>
            </Card>
          )}
          <Button 
            variant="contained" 
            color="primary" 
            onClick={handleTransaction} 
            disabled={!aliasOrUBK || !amount || !accountInfo}
          >
            Submit Transaction
          </Button>
          <Box sx={{ mt: 2, display: 'flex', gap: 2 }}>
            <Button variant="outlined" color="error" onClick={() => navigate('/operations')}>
              Back to Operations
            </Button>
            <Button variant="outlined" color="error" onClick={() => {
              setErrorMessage('');  // Clear any existing error messages before navigating
              navigate('/');
            }}>
              Quit
            </Button>
          </Box>
        </Box>
      </Paper>
    </Container>
  );
};

export default Transaction;
