using frontend_admin.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace frontend_admin.Controllers
{
    public class TiradorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TiradorController> _logger;

        public TiradorController(ApplicationDbContext context, ILogger<TiradorController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Cargando la vista de todos los tiradores.");

            var tiradores = await _context.Usuarios.ToListAsync();
            return View(tiradores);
        }
    }
}
