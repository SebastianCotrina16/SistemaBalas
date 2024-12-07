using frontend_admin.Data;
using frontend_admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace frontend_admin.Controllers
{
    public class ExamenController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ExamenController> _logger;

        public ExamenController(ApplicationDbContext context, ILogger<ExamenController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Examen/Setup
        [HttpGet]
        public async Task<IActionResult> Setup()
        {
            _logger.LogInformation("Entrando a la vista de configuración del examen.");

            var configuracion = await _context.ExamenesConfiguracion.FindAsync(1);
            if (configuracion == null)
            {
                _logger.LogWarning("No se encontró la configuración del examen. Creando una nueva con Id = 1.");
                configuracion = new ExamenConfiguracion { Id = 1, NumeroDisparos = 0 };
                _context.Add(configuracion);
                await _context.SaveChangesAsync();
            }

            return View(configuracion);
        }

        // POST: Examen/Setup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Setup(ExamenConfiguracion model)
        {
            _logger.LogInformation("Intentando actualizar la configuración del examen con {0} disparos.", model.NumeroDisparos);

            if (ModelState.IsValid)
            {
                var configuracion = await _context.ExamenesConfiguracion.FindAsync(1); 

                if (configuracion != null)
                {
                    configuracion.NumeroDisparos = model.NumeroDisparos;
                    _context.Update(configuracion);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Configuración de examen actualizada correctamente.");
                    return RedirectToAction("Setup");
                }
                else
                {
                    _logger.LogError("No se pudo encontrar la configuración con Id = 1 para actualizar.");
                }
            }

            _logger.LogWarning("Modelo de configuración de examen no válido.");
            return View(model);
        }
    }
}
