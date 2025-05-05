using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Classes_Compartilhadas.Data;
using Classes_Compartilhadas.Models;
using Classes_Compartilhadas.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LojaVirtualApi.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInMenager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ApplicationDbContext _context;
        private readonly VendedorService _vendedorService;

        public AuthController(SignInManager<IdentityUser> signInManager,
                                ApplicationDbContext context,
                                UserManager<IdentityUser> userManager,
                                IOptions<JwtSettings> jwtSettings,
                                VendedorService vendedorService)
        {
            _signInMenager = signInManager;
            _userManager = userManager;
            _context = context;
            _jwtSettings = jwtSettings.Value;
            _vendedorService = vendedorService;
        }



        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("registrar")]
        public async Task<ActionResult> Registrar(Vendedor registerUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = registerUser.Email,
                    Email = registerUser.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, registerUser.Senha);

                if (result.Succeeded)
                {
                    registerUser.Id = user.Id;
                    await _vendedorService.InserirVendedor(registerUser);
                    return Created();
                }
            }
            return Problem("Falha ao registrar o usuario");

        }


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _signInMenager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);
            if (result.Succeeded)
            {
                var token = await GerarJwt(loginUser.Email);
                return Ok(new { token });
            }

            return Problem("Usuario ou senha incorreta");
        }

        private async Task<string> GerarJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Segredo);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtSettings.Emissor,
                Audience = _jwtSettings.Audiencia,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }








    }
}
