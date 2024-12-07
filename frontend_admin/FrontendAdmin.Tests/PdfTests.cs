using System.IO;
using System.Threading.Tasks;
using frontend_admin.Controllers;
using frontend_admin.Data;
using frontend_admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

public class PdfTests
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ReportesController> _logger;

    public PdfTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new ApplicationDbContext(options);

        _logger = NullLogger<ReportesController>.Instance;

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        var usuario = new Usuario
        {
            IdUsuario = 1,
            Nombre = "Usuario Prueba",
            Correo = "usuario@prueba.com",
            Clave = "clave123",
            DNI = "12345678"
        };
        _context.Usuarios.Add(usuario);

        var reporte = new Reportes
        {
            IdReporte = 1,
            Usuario = usuario,
            FechaReporte = DateTime.Now,
            TotalImpactos = 5,
            PromedioPrecision = 85,
            Detalles = "[1,2,3]"
        };
        _context.Reportes.Add(reporte);

        _context.SaveChanges();
    }

    [Fact]
    public async Task ExportPdf_ReturnsPdfFile()
    {
        // Arrange
        int reportId = 1;
        var controller = new ReportesController(_context, _logger);

        // Act
        var result = await controller.ExportPdf(reportId) as FileContentResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("application/pdf", result.ContentType);

        var outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), "test_output");
        var outputPath = Path.Combine(outputDirectory, $"Reporte_{reportId}.pdf");

        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        await File.WriteAllBytesAsync(outputPath, result.FileContents);

        Assert.True(File.Exists(outputPath), "El archivo PDF no fue creado correctamente.");
    }
}
