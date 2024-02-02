using GoalGetters.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            // Chama a API para obter todas as partidas ao vivo de hoje
            var lives = await _apiServiceLive.GetAll<Live>("live/getalltoday");

            // Obtém a hora atual
            var now = DateTime.Now;

            // Filtra as partidas ao vivo para incluir apenas aquelas que estão ocorrendo dentro de 90 minutos após o DateMatch
            lives = lives.Where(live => live.DateMatch <= now && live.DateMatch.AddMinutes(105) >= now).ToList();

            foreach (var live in lives)
            {
                var team1 = await _apiServiceTeam.GetById(live.IdTeam1);
                live.HomeTeamName = team1.Name;

                var team2 = await _apiServiceTeam.GetById(live.IdTeam2);
                live.VisitingTeamName = team2.Name;

                // Calcula o tempo de jogo e o status da partida
                var (elapsedTime, statusMatch) = CalculatePlayingTimeAndStatus(live.DateMatch, DateTime.Now);
                live.ElapsedTime = elapsedTime;
                live.StatusMatch = statusMatch;
            }

            return View(lives);
        }


        // GET: LiveController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LiveController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LiveController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
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

                await _apiServiceTeam.Insert(live);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                // Se algo der errado, retorna para a mesma View para o usuário corrigir os dados
                return View();
            }
        }


        // GET: LiveController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LiveController/Edit/5
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

        // GET: LiveController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LiveController/Delete/5
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
                statusMatch = "Segundo tempo";
            }
            else
            {
                statusMatch = "Primeiro tempo";
            }

            return (elapsedTime, statusMatch);
        }
    }
}
