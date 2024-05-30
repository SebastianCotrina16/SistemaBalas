import React, { useState, useEffect } from 'react';
import axios from 'axios';

function Settings() {
  const [numShots, setNumShots] = useState(5);

  useEffect(() => {
    axios.get('http://localhost:5003/settings')
      .then(response => setNumShots(response.data.settings.numShots))
      .catch(error => console.error('Error fetching settings:', error));
  }, []);

  const handleSubmit = (event) => {
    event.preventDefault();
    axios.post('http://localhost:5003/settings', { numShots })
      .then(response => alert('Settings updated successfully'))
      .catch(error => console.error('Error updating settings:', error));
  };

  return (
    <div className="Settings min-h-screen flex items-center justify-center bg-gray-100">
      <div className="bg-white p-8 rounded-lg shadow-lg w-full max-w-md">
        <h1 className="text-2xl font-bold mb-6 text-center">Admin Dashboard</h1>
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <label className="block text-gray-700">Number of Shots</label>
            <input
              type="number"
              value={numShots}
              onChange={e => setNumShots(e.target.value)}
              className="w-full p-2 border rounded mt-2"
              required
            />
          </div>
          <button
            type="submit"
            className="w-full bg-blue-500 text-white p-2 rounded hover:bg-blue-600 transition duration-200"
          >
            Update
          </button>
        </form>
      </div>
    </div>
  );
}

export default Settings;
