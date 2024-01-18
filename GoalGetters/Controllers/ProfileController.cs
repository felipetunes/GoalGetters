using GoalGetters.Interfaces;
using GoalGetters.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GoalGetters.Controllers
{
    public class ProfileController : Controller
    {
        public ProfileController()
        {
           
        }

        public async Task<IActionResult> Index(string searchName)
        {
            if (!string.IsNullOrEmpty(searchName))
            {
                PlayerTeamViewModel viewModel = new PlayerTeamViewModel();

                ApiService<Player> apiServicePlayer = new ApiService<Player>(new HttpClient());
                viewModel.Players = await apiServicePlayer.GetByName(searchName);

                ApiService<Team> apiServiceTeam = new ApiService<Team>(new HttpClient());
                viewModel.Teams = await apiServiceTeam.GetByName(searchName);
                
                return View(viewModel);
            }

            return View(new PlayerTeamViewModel());
        }

        // GET: PlayerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PlayerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlayerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: PlayerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PlayerController/Edit/5
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

        // GET: PlayerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PlayerController/Delete/5
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
