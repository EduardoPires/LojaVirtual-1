using Classes_Compartilhadas.Data;
using Classes_Compartilhadas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LojaVirtualMvc.Controllers
{
    public class VendedoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public VendedoresController(ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }



        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(Vendedor vendedor)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = vendedor.Email,
                    Email = vendedor.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, vendedor.Senha);

                if (result.Succeeded)
                {
                    vendedor.Id = user.Id;
                    _context.Add(vendedor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login", "Vendedores");
                }
            }
            return View(vendedor);
        }


        // GET: Vendedores/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Vendedores/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string senha)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(email, senha, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email ou senha inválidos.");
                }
            }
            return View();
        }

    }
}
