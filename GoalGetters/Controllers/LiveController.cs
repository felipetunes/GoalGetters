using GoalGetters.Commons;
using GoalGetters.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Reflection;

namespace GoalGetters.Controllers
{
    public class LiveController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApiService<Live> _apiServiceLive;
        private readonly ApiService<Team> _apiServiceTeam;

        public LiveController(IHttpClientFactory clientFactory, ApiService<Live> apiServiceLive, ApiService<Team> apiServiceTeam)
        {
            _clientFactory = clientFactory;
            _apiServiceLive = apiServiceLive;
            _apiServiceTeam = apiServiceTeam;
        }

        // GET: LiveController
        public async Task<IActionResult> Index()
        {
            try
            {
                var lives = await _apiServiceLive.GetAll<Live>("getalltoday");
                if (lives == null)
                {
                    return View(new List<Live>());
                }

                var filteredLives = FilterLives(lives);
                var teamDetails = await GetTeamDetails(filteredLives);
                var processedLives = ProcessLives(filteredLives, teamDetails);

                // Calcula as odds para cada partida ao vivo
                Common common = new Common(_apiServiceLive);
                foreach (var live in processedLives)
                {
                    await common.CalculateOdds(live);
                }

                return View(processedLives);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        private List<Live> FilterLives(List<Live> lives)
        {
            var now = DateTime.Now;
            return lives.Where(live => live.DateMatch <= now && live.DateMatch.AddMinutes(105) >= now).ToList();
        }

        private List<Live> ProcessLives(List<Live> lives, Dictionary<int, GoalGetters.Models.Team> teamDetails)
        {
            foreach (var live in lives)
            {
                if (live.HomeTeam != null && teamDetails.ContainsKey(live.HomeTeam.Id))
                {
                    live.HomeTeam.Name = teamDetails[live.HomeTeam.Id].Name;
                }

                if (live.VisitingTeam != null && teamDetails.ContainsKey(live.VisitingTeam.Id))
                {
                    live.VisitingTeam.Name = teamDetails[live.VisitingTeam.Id].Name;
                }

                var (elapsedTime, statusMatch) = CalculatePlayingTimeAndStatus(live.DateMatch, DateTime.Now);
                live.GameTime = elapsedTime;
                live.StatusMatch = statusMatch;
            }

            return lives;
        }

        private async Task<Dictionary<int, Team>> GetTeamDetails(List<Live> lives)
        {
            var teamIds = lives.SelectMany(live => new[] { live.HomeTeam.Id, live.VisitingTeam.Id }).Distinct();
            var teamTasks = teamIds.Select(id => _apiServiceTeam.GetById(id));
            var teams = await Task.WhenAll(teamTasks);
            return teams.ToDictionary(team => team.Id, team => team);
        }

        // GET: LiveController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LiveController/Create
        [Authorize]
        public ActionResult CreateLive()
        {
            return View();
        }

        // POST: LiveController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> CreateLive(IFormCollection collection)
        {
            try
            {
                var team1 = await _apiServiceTeam.GetByName(collection["hometeamname"]);
                var team2 = await _apiServiceTeam.GetByName(collection["visitingteamname"]);

                // Cria um novo objeto Live a partir dos dados do formulário
                Live live = new Live
                {
                    HomeTeam = team1.FirstOrDefault(),
                    VisitingTeam = team2.FirstOrDefault(),
                    Championship = new Championship { Id = Convert.ToInt32(collection["idchampionship"]) },
                    DateMatch = DateTime.Parse(collection["datematch"]),
                    Stadium = collection["stadium"],
                    TeamPoints1 = int.Parse(collection["teampoints1"]),
                    TeamPoints2 = int.Parse(collection["teampoints2"])
                };

                await _apiServiceLive.Insert(live);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                // Se algo der errado, retorna para a mesma View para o usuário corrigir os dados
                return View();
            }
        }


        // GET: LiveController/EditLive/5
        [Authorize]
        public async Task<ActionResult> EditLive(int id)
        {
            var live = await _apiServiceLive.GetById(id);

            var competitions = Enum.GetNames(typeof(Helper.Enums.Championship));
            ViewBag.CompetitionList = new SelectList(competitions);

            return View(live);
        }

        // POST: LiveController/EditLive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditLive(int id, IFormCollection collection)
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

        // GET: LiveController/DeleteLive/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LiveController/DeleteLive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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

        private (int, string) CalculatePlayingTimeAndStatus(DateTime inicioPartida, DateTime agora)
        {
            int gameTime = (int)(agora - inicioPartida).TotalMinutes;
            string statusMatch;

            if (gameTime >= 45 && gameTime < 60)
            {
                statusMatch = "Intervalo";
            }
            else if (gameTime >= 60)
            {
                statusMatch = "2ª tempo";
            }
            else
            {
                statusMatch = "1ª tempo";
            }

            return (gameTime, statusMatch);
        }
    }
}
