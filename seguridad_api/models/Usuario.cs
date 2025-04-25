using Microsoft.AspNetCore.Identity;

namespace seguridad_api.Models
{
    public class Usuario : IdentityUser
    {
        public string? Rol { get; set; } // Asegúrate que esté esta propiedad si usas roles personalizados
    }
}
