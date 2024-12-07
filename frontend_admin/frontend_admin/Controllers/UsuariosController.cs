using Microsoft.AspNetCore.Mvc;
using frontend_admin.Data;
using frontend_admin.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace frontend_admin.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(ApplicationDbContext context, ILogger<UsuariosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Usuarios/Register
        public IActionResult Register()
        {
            _logger.LogInformation("Entrando a la vista de registro.");
            return View();
        }

        // POST: Usuarios/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Usuario usuario)
        {
            _logger.LogInformation("Intentando registrar usuario con DNI: {DNI}", usuario.DNI);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo inválido, devolviendo errores de validación.");
                foreach (var error in ModelState.Values)
                {
                    foreach (var subError in error.Errors)
                    {
                        _logger.LogWarning("Error de validación: {Error}", subError.ErrorMessage);
                    }
                }
                return View(usuario);
            }

            try
            {
                var externalUserExists = await _context.Users.AnyAsync(eu => eu.dni == usuario.DNI);
                if (!externalUserExists)
                {
                    _logger.LogWarning("DNI no encontrado en la tabla 'users'.");
                    ModelState.AddModelError("", "El DNI no está vinculado a un usuario de reconocimiento facial.");
                    return View(usuario);
                }

                _logger.LogInformation("DNI encontrado en 'users'. Procediendo a guardar el usuario.");

                _context.Add(usuario);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Usuario registrado con éxito.");
                return RedirectToAction("Login");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Error al intentar guardar el usuario en la base de datos.");
                ModelState.AddModelError("", "Error al guardar el usuario. Intenta nuevamente.");
                return View(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado durante el registro del usuario.");
                ModelState.AddModelError("", "Ocurrió un error inesperado. Intenta nuevamente.");
                return View(usuario);
            }
        }

        // GET: Usuarios/Login
        public IActionResult Login()
        {
            _logger.LogInformation("Entrando a la vista de login.");
            return View();
        }

        // POST: Usuarios/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string correo, string clave)
        {
            _logger.LogInformation("Intentando iniciar sesión con correo: {Correo}", correo);

            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo && u.Clave == clave);

                if (usuario != null)
                {
                    var externalUser = await _context.Users.FirstOrDefaultAsync(eu => eu.dni == usuario.DNI);
                    if (externalUser != null)
                    {
                        HttpContext.Session.SetString("usuario", usuario.Nombre);
                        _logger.LogInformation("Sesión iniciada para el usuario: {Nombre}", usuario.Nombre);
                        return RedirectToAction("Dashboard", "Home");
                    }
                    else
                    {
                        _logger.LogWarning("DNI no vinculado a un usuario de reconocimiento facial.");
                        ModelState.AddModelError(string.Empty, "El DNI no está vinculado a un usuario de reconocimiento facial.");
                        return View();
                    }
                }

                _logger.LogWarning("Credenciales inválidas para el correo: {Correo}", correo);
                ModelState.AddModelError(string.Empty, "Credenciales inválidas");
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado durante el inicio de sesión.");
                ModelState.AddModelError("", "Ocurrió un error inesperado. Intenta nuevamente.");
                return View();
            }
        }

        // GET: Usuarios/Logout
        public IActionResult Logout()
        {
            _logger.LogInformation("Cerrando sesión.");
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Usuarios");
        }
    }
}
