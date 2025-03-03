import React, { useState, useEffect } from 'react';
import { Container, Typography, Paper, CircularProgress, Box, Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';

const Balance = () => {
  const [balance, setBalance] = useState(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchBalance = async () => {
      try {
        const token = localStorage.getItem('sessionToken');
        if (!token) {
          // Redirigir automáticamente a la página de inicio si la sesión ha expirado
          navigate('/');
          return;
        }
        console.log("Fetching balance with token:", token);

        const response = await api.get('operation/balance-query', {
          headers: { sessionToken: token }
        });
        console.log("Balance response:", response);
        console.log("Response data:", response.data);

        // Ajusta según la estructura de la respuesta
        if (typeof response.data === 'number') {
          setBalance(response.data);
        } else if (response.data && response.data.balance !== undefined) {
          setBalance(response.data.balance);
        } else if (response.data.data && response.data.data.balance !== undefined) {
          setBalance(response.data.data.balance);
        } else {
          console.error("Unexpected response format:", response.data);
          alert('Error fetching balance: Unexpected response format.');
        }
      } catch (error) {
        console.error('Error fetching balance:', error);
        alert('Error fetching balance');
      } finally {
        setLoading(false);
      }
    };

    fetchBalance();
  }, []);

  const handleQuit = async () => {
    try {
      const token = localStorage.getItem('sessionToken');
      if (!token) {
        alert('No session token found');
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
        {loading ? (
          <Box display="flex" justifyContent="center">
            <CircularProgress />
          </Box>
        ) : (
          <Typography variant="h5">
            {balance !== null ? `Your Balance: $${balance}` : 'No balance available'}
          </Typography>
        )}

        <Box sx={{ mt: 2, display: 'flex', gap: 2, justifyContent: 'center' }}>
          <Button 
            variant="outlined" 
            color="error" 
            onClick={() => navigate('/operations')}
          >
            Back to Operations
          </Button>
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

export default Balance;
