# Usar una imagen base de Node.js
FROM node:14

# Establecer el directorio de trabajo en el contenedor
WORKDIR /app

# Copiar los archivos de paquete de Node.js
COPY package*.json ./

# Instalar las dependencias
RUN npm install

# Copiar el contenido de la aplicación en el contenedor
COPY . .

# Compilar la aplicación
RUN npm run build

# Instalar un servidor estático para servir la aplicación
RUN npm install -g serve

# Exponer el puerto en el que corre el frontend
EXPOSE 3000

# Comando para ejecutar la aplicación
CMD ["serve", "-s", "build"]
