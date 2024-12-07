import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import StartExam from './components/StartExam';
import FacialRecognition from './components/FacialRecognition';
import Dashboard from './components/Dashboard';
import Results from './components/Result';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<StartExam />} />
        <Route path="/facial-recognition" element={<FacialRecognition />} />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/results" element={<Results />} />
      </Routes>
    </Router>
  );
}

export default App;
