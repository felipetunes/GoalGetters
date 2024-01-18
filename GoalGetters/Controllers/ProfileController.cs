using GoalGetters.Models;
using Microsoft.AspNetCore.Mvc;

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
