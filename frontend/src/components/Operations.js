import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Container, Button, Typography, Paper, Box } from '@mui/material';
import api from '../services/api';

const Operations = () => {
  const navigate = useNavigate();

  const handleOperation = (operation) => {
    if (operation === 'balance') {
      navigate('/balance');
    } else if (operation === 'withdrawal') {
      navigate('/withdrawal');
    } else if (operation === 'transaction') {
      navigate('/transaction');
    }
  };

  const handleQuit = async () => {
    try {
      const token = localStorage.getItem('sessionToken');
      if (!token) {
        // Si no hay token, redirige a la página de inicio directamente
        navigate('/');
        return;
      }

      // Hacer el POST a /account/logout
      const response = await api.post('account/logout', {}, {
        headers: {
          sessionToken: token,
        },
      });

      console.log("Logout response:", response);

      if (response.status === 200) {
        // Limpiar sessionToken de localStorage
        localStorage.removeItem('sessionToken');
        
        // Redirigir a la página de inicio
        navigate('/');
      } else {
        alert('Error during logout');
      }
    } catch (error) {
      console.error('Error logging out:', error);
      alert('Error logging out');
    }
  };

  return (
    <Container maxWidth="sm" sx={{ mt: 4 }}>
      <Paper elevation={3} sx={{ p: 4, textAlign: 'center' }}>
        <Typography variant="h5" gutterBottom>
          Select an Operation
        </Typography>
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
          <Button variant="contained" onClick={() => handleOperation('balance')}>
            Balance
          </Button>
          <Button variant="contained" onClick={() => handleOperation('withdrawal')}>
            Withdrawal
          </Button>
          <Button variant="contained" onClick={() => handleOperation('transaction')}>
            Transaction
          </Button>
        </Box>

        <Box sx={{ mt: 2, display: 'flex', gap: 2, justifyContent: 'center' }}>
          <Button 
            variant="outlined" 
            color="error" 
            onClick={handleQuit}
          >
            Quit
          </Button>
        </Box>
      </Paper>
    </Container>
  );
};

export default Operations;
