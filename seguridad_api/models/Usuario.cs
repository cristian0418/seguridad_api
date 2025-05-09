using Microsoft.AspNetCore.Identity;

public class Usuario : IdentityUser
{
    public string? Rol { get; set; }

    // Nueva propiedad
    public bool Activo { get; set; } = true;
}