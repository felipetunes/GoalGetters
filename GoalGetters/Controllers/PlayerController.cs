using GoalGetters.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GoalGetters.Controllers
{
    public class PlayerController : Controller
    {
        private readonly HttpClient _client;

        public PlayerController()
        {
            _client = new HttpClient();
        }

        public async Task<IActionResult> Index(string playerName)
        {
            if (!string.IsNullOrEmpty(playerName))
            {
                string url = $"http://localhost:8080/api/v1/player/getbyname/{Uri.EscapeDataString(playerName)}";
                HttpResponseMessage response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsAsync<IEnumerable<Player>>();

                    // Obtenha todos os times de uma vez
                    HttpResponseMessage responseTeams = await _client.GetAsync("http://localhost:8080/api/v1/team/getall");
                    if (responseTeams.IsSuccessStatusCode)
                    {
                        var teams = await responseTeams.Content.ReadAsAsync<IEnumerable<Team>>();
                        var teamDict = teams.ToDictionary(t => t.Id, t => t);

                        foreach (var player in data)
                        {
                            if (player.IdTeam != 0 && player.IdTeam != null && teamDict.ContainsKey(player.IdTeam))
                            {
                                player.TeamName = teamDict[player.IdTeam].Name;
                            }
                        }

                        return View(data);
                    }
                    else
                    {
                        // Handle the error
                        return View("Error");
                    }
                }
                else
                {
                    // Handle the error
                    return View("Error");
                }
            }
            return View();
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
