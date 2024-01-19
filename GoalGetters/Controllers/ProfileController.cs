using GoalGetters.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Numerics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GoalGetters.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApiService<Player> _apiServicePlayer;
        private readonly ApiService<Team> _apiServiceTeam;

        public ProfileController(IHttpClientFactory clientFactory, ApiService<Player> apiServicePlayer, ApiService<Team> apiServiceTeam)
        {
            _clientFactory = clientFactory;
            _apiServicePlayer = apiServicePlayer;
            _apiServiceTeam = apiServiceTeam;
        }

        public async Task<IActionResult> Index(string searchName)
        {
            if (!string.IsNullOrEmpty(searchName))
            {
                var viewModel = await SearchPlayerTeam(searchName);
                return View(viewModel);
            }

            return View(new PlayerTeamViewModel());
        }

        // GET: PlayerController/Details/5
        public ActionResult DetailsPlayer(int id)
        {
            return View();
        }

        // GET: PlayerController/Create
        public ActionResult CreatePlayer()
        {
            return View();
        }

        // POST: PlayerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePlayer(IFormCollection collection)
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
        public async Task<ActionResult> EditPlayer(int id)
        {
            var player = await _apiServicePlayer.GetById(id);
            return View(player);
        }

        // POST: PlayerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPlayer(int id, IFormCollection collection)
        {
            try
            {
                // Extrai os valores do formulário
                string name = collection["name"];
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("Name cannot be null or empty.", nameof(name));
                }

                if (!int.TryParse(collection["idteam"], out int idteam))
                {
                    throw new ArgumentException("Invalid team ID.", nameof(idteam));
                }

                string city = collection["city"];
                if (string.IsNullOrEmpty(city))
                {
                    throw new ArgumentException("City cannot be null or empty.", nameof(city));
                }

                string country = collection["country"];
                if (string.IsNullOrEmpty(country))
                {
                    throw new ArgumentException("Country cannot be null or empty.", nameof(country));
                }

                if (!DateTime.TryParse(collection["birth"], out DateTime birth))
                {
                    throw new ArgumentException("Invalid birth date.", nameof(birth));
                }

                string height = collection["height"];
                if (string.IsNullOrEmpty(height))
                {
                    throw new ArgumentException("Height cannot be null or empty.", nameof(height));
                }

                // Atualiza o jogador
                Player updatedPlayer = await _apiServicePlayer.UpdatePlayer(id, name, idteam, city, country, birth, height);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Retorna a View com a mensagem de erro
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }


        // GET: PlayerController/Delete/5
        public ActionResult DeletePlayer(int id)
        {
            return View();
        }

        // POST: PlayerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePlayer(int id, IFormCollection collection)
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

        public async Task<PlayerTeamViewModel> SearchPlayerTeam(string searchName)
        {
            PlayerTeamViewModel viewModel = new PlayerTeamViewModel();

            // Fetch players and teams in parallel
            Task<IEnumerable<Player>> getPlayersTask = _apiServicePlayer.GetByName(searchName);
            Task<IEnumerable<Team>> getTeamsTask = _apiServiceTeam.GetByName(searchName);
            await Task.WhenAll(getPlayersTask, getTeamsTask);

            viewModel.Players = getPlayersTask.Result;
            viewModel.Teams = getTeamsTask.Result;

            if (viewModel.Players != null)
            {
                foreach (var player in viewModel.Players)
                {
                    var team = await _apiServiceTeam.GetById(player.IdTeam);
                    player.TeamName = team.Name;
                }
            }
            return viewModel;
        }
    }
}
