using GoalGetters.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Security.Policy;
using GoalGetters.Service;

namespace GoalGetters.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly UserService _apiServiceUser;

        // GET: UserController

        public UserController(IHttpClientFactory clientFactory, UserService serviceUser)
        {
            _clientFactory = clientFactory;
            _apiServiceUser = serviceUser;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            // Busque o usuário no banco de dados através da API
            var userInDb = await _apiServiceUser.Login(user.Username, user.Password);

            if (!string.IsNullOrEmpty(userInDb.Token))
            {
                // Se as credenciais estiverem corretas, crie uma ClaimsIdentity
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    // Você pode incluir mais claims aqui conforme necessário
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Suponha que 'userPhoto' seja a foto do usuário que você obteve após o login
                // Converta a foto do usuário para uma string Base64
                if (userInDb.Photo != null)
                {
                    var userPhotoBase64 = Convert.ToBase64String(userInDb.Photo);
                    HttpContext.Session.SetString("UserPhoto", userPhotoBase64);
                }

                // Faça login no usuário
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                HttpContext.Session.SetString("User", user.Username);

                return RedirectToAction("Index", "Home");
            }

            // Se chegamos até aqui, o login falhou
            return RedirectToAction("Index", "User");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: UserController/Details/5
        public ActionResult Details(int? id)
        {
            return View();
        }

        // GET: UserController/Create
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

                // Extrai a foto da Request.Form.Files
                IFormFile photo = Request.Form.Files["photo"];
                byte[] photoBytes = null;
                if (photo != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await photo.CopyToAsync(memoryStream);
                        photoBytes = memoryStream.ToArray();
                    }
                }

                // Chama o método na sua service
                await _apiServiceUser.Register(username, password, photoBytes);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
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

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
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
