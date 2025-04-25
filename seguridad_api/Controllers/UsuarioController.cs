using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace seguridad_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        // 🔐 Endpoint protegido por autenticación
        [HttpGet("perfil")]
        [Authorize]
        public IActionResult ObtenerPerfil()
        {
            var nombreUsuario = User.Identity?.Name;

            return Ok(new
            {
                Mensaje = "¡Acceso concedido!",
                Usuario = nombreUsuario
            });
        }
    }
}
