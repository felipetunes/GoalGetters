using GoalGetters.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Numerics;

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
            var lives = await _apiServiceLive.GetAll<Live>("getalltoday");
            var now = DateTime.Now;

            // Filtra as partidas ao vivo para incluir apenas aquelas que estão ocorrendo dentro de 90 minutos após o DateMatch
            lives = lives.Where(live => live.DateMatch <= now && live.DateMatch.AddMinutes(105) >= now).ToList();

            // Obtenha todos os detalhes da equipe de uma vez e armazene-os em um dicionário
            var teamDetails = await GetTeamDetails(lives);

            foreach (var live in lives)
            {
                live.HomeTeamName = teamDetails[live.IdTeam1].Name;
                live.VisitingTeamName = teamDetails[live.IdTeam2].Name;

                // Calcula o tempo de jogo e o status da partida
                var (elapsedTime, statusMatch) = CalculatePlayingTimeAndStatus(live.DateMatch, DateTime.Now);
                live.ElapsedTime = elapsedTime;
                live.StatusMatch = statusMatch;
            }

            return View(lives);
        }

        private async Task<Dictionary<int, Team>> GetTeamDetails(List<Live> lives)
        {
            var teamIds = lives.SelectMany(live => new[] { live.IdTeam1, live.IdTeam2 }).Distinct();
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
        public ActionResult CreateLive()
        {
            return View();
        }

        // POST: LiveController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateLive(IFormCollection collection)
        {
            try
            {
                var team1 = await _apiServiceTeam.GetByName(collection["hometeamname"]);
                var team2 = await _apiServiceTeam.GetByName(collection["visitingteamname"]);

                // Cria um novo objeto Live a partir dos dados do formulário
                Live live = new Live
                {
                    IdTeam1 =  team1.FirstOrDefault().Id,
                    IdTeam2 = team2.FirstOrDefault().Id,
                    IdChampionship = int.Parse(collection["idchampionship"]),
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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LiveController/DeleteLive/5
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

        private (int, string) CalculatePlayingTimeAndStatus(DateTime inicioPartida, DateTime agora)
        {
            int elapsedTime = (int)(agora - inicioPartida).TotalMinutes;
            string statusMatch;

            if (elapsedTime >= 45 && elapsedTime < 60)
            {
                statusMatch = "Intervalo";
            }
            else if (elapsedTime >= 60)
            {
                statusMatch = "2ª tempo";
            }
            else
            {
                statusMatch = "1ª tempo";
            }

            return (elapsedTime, statusMatch);
        }
    }
}
