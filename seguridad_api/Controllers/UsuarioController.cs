using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using seguridad_api.Models;
using Microsoft.EntityFrameworkCore;

namespace seguridad_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;

        public UsuarioController(UserManager<Usuario> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet("activos")]
        public async Task<IActionResult> ObtenerUsuariosActivos()
        {
            var usuariosActivos = await _userManager.Users
                                                    .Where(u => u.Activo)
                                                    .ToListAsync();

            return Ok(usuariosActivos);
        }
    }
}
