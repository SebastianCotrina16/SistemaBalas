const express = require('express');
const cors = require('cors');
const { Client } = require('pg');

const app = express();
const port = 80;

app.use(cors());
app.use(express.json());

const client = new Client({
  connectionString: 'postgresql://SebastianCotrina16:OWQvYgs8o6Mm@ep-flat-block-17023118.us-east-2.aws.neon.tech/test?sslmode=require',
});


client.connect()
  .then(() => console.log('Conexión exitosa a PostgreSQL sin pooler'))
  .catch(err => console.error('Error al conectar con PostgreSQL:', err));

app.get('/settings', async (req, res) => {
  try {
    const result = await client.query('SELECT numerodisparos FROM examenesconfiguracion LIMIT 1');
    const numShots = result.rows[0]?.numerodisparos || 0;

    res.json({
      status: 'success',
      settings: {
        numShots,
      },
    });
  } catch (err) {
    console.error('Error al obtener los settings:', err);
    res.status(500).json({
      status: 'error',
      message: 'Error al obtener los ajustes de la base de datos',
    });
  }
});

app.listen(port, () => {
  console.log(`API escuchando en http://localhost:${port}`);
});
