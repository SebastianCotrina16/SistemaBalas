const express = require('express');
const cors = require('cors');

const app = express();
const port = 5003;

app.use(cors());
app.use(express.json());

const USERS = [{ username: 'admin', password: 'password' }];

let examSettings = {
  numShots: 5
};

app.post('/login', (req, res) => {
  const { username, password } = req.body;
  const user = USERS.find(u => u.username === username && u.password === password);
  if (user) {
    res.json({ success: true });
  } else {
    res.json({ success: false });
  }
});

app.get('/settings', (req, res) => {
  res.json({ settings: examSettings });
});

app.post('/settings', (req, res) => {
  const { numShots } = req.body;
  if (numShots != null) {
    examSettings.numShots = numShots;
    res.json({ status: 'success', settings: examSettings });
  } else {
    res.status(400).json({ status: 'error', message: 'numShots is required' });
  }
});

app.listen(port, () => {
  console.log(`Admin backend listening at http://localhost:${port}`);
});
