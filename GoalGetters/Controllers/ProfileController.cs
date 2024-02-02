using GoalGetters.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Numerics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using X.PagedList;
using X.PagedList.Mvc.Core;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public async Task<IActionResult> Index(string searchName = "")
        {
            if (!string.IsNullOrEmpty(searchName))
            {
                HttpContext.Session.SetString("SearchName", searchName); // Salva o searchName na sessão

                var profile = await SearchProfile(searchName);
                return View(profile);
            }

            return View(new PlayerTeamViewModel());
        }

        // GET: PlayerController/Details/5
        public async Task<ActionResult> DetailsPlayer(int id)
        {
            var player = await _apiServicePlayer.GetById(id);
            var team = await _apiServiceTeam.GetById(player.IdTeam);
            player.TeamName = team.Name;
            return View(player);
        }

        // GET: ProfileController/Details/5
        public async Task<ActionResult> DetailsTeam(int id, int page=1)
        {
            var team = await _apiServiceTeam.GetById(id);
            var pl = await _apiServiceTeam.GetPlayersByTeamId(id);

            if (pl != null && pl.Count != 0)
            {
                // Ordenar a lista antes de paginar
                var orderedPlayers = pl.OrderBy(p => p.Position);

                team.PlayersCount = pl.Count();
                team.AverageAge = pl.Average(p => p.Age).ToString("0.#");
                team.Players = orderedPlayers.ToPagedList(page, 11);

                foreach (var player in team.Players)
                {
                    player.TeamName = team.Name;
                }
            }
            return View(team);
        }

        // GET: PlayerController/Create
        public ActionResult CreateTeam()
        {

            ViewBag.CountryList = new SelectList(GetCountryNames());

            return View();
        }

        // POST: PlayerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTeam(IFormCollection collection)
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

        // GET: PlayerController/Create
        public ActionResult CreatePlayer()
        {
            ViewBag.CountryList = new SelectList(GetCountryNames());

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

        // GET: EntityController/Edit/5
        public async Task<ActionResult> EditPlayer(int id)
        {
            var player = await _apiServicePlayer.GetById(id);

            var selectedCountry = string.IsNullOrEmpty(player.Country) ? "" : player.Country;

            ViewBag.CountryList = new SelectList(GetCountryNames(), selectedCountry);

            return View(player);
        }

        // GET: EntityController/Edit/5
        public async Task<ActionResult> EditTeam(int id)
        {
            var team = await _apiServiceTeam.GetById(id);

            var selectedCountry = string.IsNullOrEmpty(team.Country) ? "" : team.Country;

            ViewBag.CountryList = new SelectList(GetCountryNames(), selectedCountry);

            return View(team);
        }

        private IEnumerable<string> GetCountryNames()
        {
            var countries = ISO3166.Country.List.Select(c => c.TwoLetterCode);
            var translatedCountries = countries.Select(country => IsoNames.CountryNames.GetName(new CultureInfo("pt"), country))
                                               .Where(name => !string.IsNullOrEmpty(name))
                                               .OrderBy(name => name);
            return translatedCountries;
        }


        // POST: EntityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPlayer(int id, string entity, IFormCollection collection)
        {
            try
            {
                Edit(id, entity, collection);
                return RedirectToAction("Index");
            }
            catch
            {
                return NotFound();
            }
        }

        // POST: EntityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTeam(int id, string entity, IFormCollection collection)
        {
            try
            {
                Edit(id, entity, collection);
                return RedirectToAction("Index");
            }
            catch
            {
                return NotFound();
            }
        }
        public async Task<ActionResult> Edit(int id, string entity, IFormCollection collection)
        {
            try
            {
                // Extrai os valores do formulário
                string name = collection["name"];
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("Name cannot be null or empty.", nameof(name));
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

                if (entity == "player")
                {
                    if (!int.TryParse(collection["idteam"], out int idteam))
                    {
                        throw new ArgumentException("Invalid team ID.", nameof(idteam));
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

                    string position = collection["position"];
                    if (string.IsNullOrEmpty(position))
                    {
                        throw new ArgumentException("Position cannot be null or empty.", nameof(position));
                    }

                    // Atualiza o jogador
                    Player updatedPlayer = await _apiServicePlayer.Update<Player>(id, name, city, country, idteam, birth, height, position);
                }
                else if (entity == "team")
                {
                    // Atualiza o time
                    Team updatedTeam = await _apiServiceTeam.Update<Team>(id, name, city, country);
                }
                else
                {
                    return NotFound();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Retorna a View com a mensagem de erro
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }

        }

        // GET: PlayerController/Delete/5
        public async Task<ActionResult> DeletePlayer(int id)
        {
            var player = await _apiServicePlayer.GetById(id);
            return View(player);
        }

        // GET: PlayerController/Delete/5
        public async Task<ActionResult> DeleteTeam(int id)
        {
            var team = await _apiServiceTeam.GetById(id);
            return View(team);
        }

        // POST: PlayerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeletePlayer(int id, IFormCollection collection)
        {
            try
            {
                await _apiServicePlayer.Delete(id, "player");

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<PlayerTeamViewModel> SearchProfile(string searchName)
        {
            PlayerTeamViewModel viewModel = new PlayerTeamViewModel();

            // Fetch players and teams in parallel
            Task<List<Player>> getPlayersListTask = _apiServicePlayer.GetByName(searchName);
            Task<List<Team>> getTeamsListTask = _apiServiceTeam.GetByName(searchName);
            await Task.WhenAll(getPlayersListTask, getTeamsListTask);

            IPagedList<Player> getPlayersTask = getPlayersListTask.Result.ToPagedList();
            IPagedList<Team> getTeamsTask = getTeamsListTask.Result.ToPagedList();

            viewModel.Players = getPlayersTask;
            viewModel.Teams = getTeamsTask;

            if (viewModel.Players != null)
            {
                foreach (var player in viewModel.Players)
                {
                    var team = await _apiServiceTeam.GetById(player.IdTeam);
                    if (team != null)
                    {
                        player.TeamName = team.Name;
                    }
                }
            }
            return viewModel;
        }
    }
}
