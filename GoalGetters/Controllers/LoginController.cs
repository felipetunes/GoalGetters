using GoalGetters.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace GoalGetters.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApiService<User> _apiServiceUser;

        // GET: LoginController

        public LoginController(IHttpClientFactory clientFactory, ApiService<User> apiServiceUser)
        {
            _clientFactory = clientFactory;
            _apiServiceUser = apiServiceUser;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            // Busque o usuário no banco de dados através da API
            var userInDb = await _apiServiceUser.Login<User>(user.Username, user.Password);

            if (userInDb != null)
            {
                // Verifique a senha
                if (BCrypt.Net.BCrypt.Verify(user.Password, userInDb.Password))
                {
                    // Se as credenciais estiverem corretas, crie uma ClaimsIdentity
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        // Você pode incluir mais claims aqui conforme necessário
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Faça login no usuário
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    HttpContext.Session.SetString("User", user.Username);

                    return RedirectToAction("Index", "Home");
                }
            }

            // Se chegarmos até aqui, algo falhou, redisplay form
            return View(user);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: LoginController/Details/5
        public ActionResult Details(int? id)
        {
            return View();
        }

        // GET: LoginController/Create
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(IFormCollection collection)
        {
            try
            {
                // Extrai o nome de usuário e a senha da IFormCollection
                string username = collection["username"];
                string password = collection["password"];

                // Chama o método na sua service
                await _apiServiceUser.Register(username, password);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
