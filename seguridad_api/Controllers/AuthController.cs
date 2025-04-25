using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using seguridad_api.DTOs;
using seguridad_api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace seguridad_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        //  para registrar un nuevo usuario
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            // Crear un nuevo usuario
            var user = new Usuario
            {
                UserName = request.Username,
                Email = request.Email,
                // Asignar un rol predeterminado, como "Cliente"
                Rol = "Cliente" // Aquí puedes modificar para asignar un rol predeterminado
            };

            // Crear el usuario en la base de datos
            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                // Si el registro fue exitoso, puedes agregar roles adicionales si es necesario
                // Ejemplo: Asignar el rol "Administrador" si el usuario tiene un nombre específico
                if (request.Username == "admin")  // Esto es solo un ejemplo
                {
                    await _userManager.AddToRoleAsync(user, "Administrador");
                }

                // Retornar un mensaje de éxito
                return Ok("Usuario registrado exitosamente");
            }

            // Si hubo errores, devolverlos
            return BadRequest(result.Errors);
        }

        // Ruta para iniciar sesión (Autenticación de usuario)
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            // Intentar iniciar sesión con las credenciales proporcionadas
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return Unauthorized("Usuario no encontrado");
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

            if (result.Succeeded)
            {
                // Generar y devolver el JWT
                var token = GenerateJwtToken(user);
                return Ok(new { token });
            }

            return Unauthorized("Credenciales incorrectas");
        }

        // Generar JWT para el usuario autenticado
        private string GenerateJwtToken(Usuario user)
        {
            // Configuración de los parámetros JWT
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
