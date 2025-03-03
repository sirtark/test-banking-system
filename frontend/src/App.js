import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Home from './components/Home';
import Operations from './components/Operations';
import Balance from './components/Balance';
import Withdrawal from './components/Withdrawal';
import PinEntry from './components/PinEntry';
import Transaction from './components/Transaction'; // Si lo usas
import CreateAccount from './components/CreateAccount'; // Importamos el componente de crear cuenta

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/operations" element={<Operations />} />
        <Route path="/balance" element={<Balance />} />
        <Route path="/withdrawal" element={<Withdrawal />} />
        <Route path="/pin" element={<PinEntry />} />
        <Route path="/transaction" element={<Transaction />} />
        <Route path="/create-account" element={<CreateAccount />} /> {/* Agregamos la ruta para la creación de cuenta */}
      </Routes>
    </Router>
  );
}

export default App;
