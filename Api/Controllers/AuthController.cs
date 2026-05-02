using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Siscomat.Core.DTOs;
using Siscomat.Core.Entities;
using Siscomat.Core.Interfaces;
using Siscomat.Services;
using System.Security.Claims;

namespace Siscomat.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        protected readonly AuthService _authService;

        public AuthController(AuthService loginService)
        {
            _authService = loginService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var gestor = await _authService.LoginAsync(loginDto);
            if (gestor == null)
            {
                return Unauthorized(new { message = "Correo o contraseña incorrectos" });
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, gestor.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{gestor.Nombre} {gestor.Apellido1} {gestor.Apellido2 ?? ""}"),
                new Claim(ClaimTypes.Email, gestor.Correo),
                new Claim(ClaimTypes.Role, gestor.EsAdmin ? "Admin" : "Gestor")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok(new
            {
                message = "Inicio de sesión exitoso",
                user = new
                {
                    id = gestor.Id,
                    nombre = gestor.Nombre,
                    esAdmin = gestor.EsAdmin
                }
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Sesión cerrada exitosamente" });
        }

        [HttpGet("access-denied")]
        public IActionResult AccessDenied()
        {
            return Unauthorized(new { message = "Acceso denegado" });
        }

        [Authorize]
        [HttpGet("logged")]
        public IActionResult Logged()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok($"Bienvenido, {userName}");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto, [FromServices] IGestorRepository gestorRepository)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            var gestor = new Gestor
            {
                Nombre = registerDto.Nombre,
                Apellido1 = registerDto.Apellido1,
                Apellido2 = registerDto.Apellido2,
                Correo = registerDto.Correo,
                PasswordHash = passwordHash,
                EsAdmin = true
            };
            await gestorRepository.AddAsync(gestor);
            return Ok(new { message = "Registro exitoso" });
        }
    }
}
