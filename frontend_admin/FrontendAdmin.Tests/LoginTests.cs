using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using frontend_admin.Controllers;
using frontend_admin.Data;
using frontend_admin.Models;
using System.Threading.Tasks;

namespace FrontendAdmin.Tests
{
    public class LoginTests
    {
        private UsuariosController GetControllerWithInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            var context = new ApplicationDbContext(options);
            var logger = Mock.Of<ILogger<UsuariosController>>();

            // Mock usuario en la base de datos
            context.Usuarios.Add(new Usuario
            {
                Nombre = "Test User",
                Correo = "valid@example.com",
                Clave = "password123",
                DNI = "12345678"
            });
            context.SaveChanges();

            return new UsuariosController(context, logger);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsViewResult()
        {
            // Arrange
            var controller = GetControllerWithInMemoryDb();
            string correo = "valid@example.com";
            string clave = "password123";

            // Act
            var result = await controller.Login(correo, clave);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async Task Login_Authentication_ReturnsViewResult_WhenCredentialsAreValid()
        {
            // Arrange
            var controller = GetControllerWithInMemoryDb();
            string correo = "valid@example.com";
            string clave = "password123";

            // Act
            var result = await controller.Login(correo, clave);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

    }
}
