import React, { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import Webcam from 'react-webcam';
import axios from 'axios';

function Dashboard() {
  const [shots, setShots] = useState(0);
  const [maxShots, setMaxShots] = useState(5); // Estado para la cantidad máxima de tiros
  const navigate = useNavigate();
  const location = useLocation();
  const user = location.state?.user;

  useEffect(() => {
    
    axios.get('http://localhost:5003/settings')
      .then(response => {
        setMaxShots(response.data.settings.numShots);
      })
      .catch(error => {
        console.error('Error fetching settings:', error);
      });
  }, []);

  useEffect(() => {
    if (shots >= maxShots) {
      navigate('/results', { state: { user } });
    }
  }, [shots, maxShots, navigate, user]);

  const handleShot = () => {
    setShots(shots + 1);
  };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-900 text-white p-4">
      <div className="text-center">
        <h1>Examen de {user}</h1>
        <Webcam audio={false} height={720} width={1280} />
        <button onClick={handleShot}>Simulate Shot</button>
        <p>Shots fired: {shots}</p>
        <p>Maximum shots allowed: {maxShots}</p>
      </div>
    </div>
  );
}

export default Dashboard;
