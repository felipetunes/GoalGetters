using GoalGetters.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoalGetters.Controllers
{
    public class BetController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApiService<Live> _apiServiceLive;
        private readonly ApiService<Bet> _apiServiceBet;

        public BetController(IHttpClientFactory clientFactory, ApiService<Live> apiServiceLive, ApiService<Bet> apiServiceBet)
        {
            _clientFactory = clientFactory;
            _apiServiceLive = apiServiceLive;
            _apiServiceBet = apiServiceBet;
        }
        // GET: BetController
        public ActionResult Index()
        {
            return View();
        }

        // GET: BetController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BetController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BetController/Create
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

        // GET: BetController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BetController/Edit/5
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

        // GET: BetController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BetController/Delete/5
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

        // Método para lidar com a aposta do usuário
        [HttpPost]
        public async Task<IActionResult> PlaceBet(int matchId, string selectedOutcome, decimal betAmount)
        {
            var match = await _apiServiceLive.GetById(matchId);

            if (match == null)
            {
                return NotFound();
            }

            // Valida a entrada do usuário
            if (betAmount <= 0)
            {
                ModelState.AddModelError("BetAmount", "A quantidade da aposta deve ser maior que zero.");
            }

            if (!ModelState.IsValid)
            {
                return View(match);
            }

            // Calcula o retorno possível com base no resultado selecionado
            decimal possibleReturn = CalculatePossibleReturn(selectedOutcome, betAmount, match);

            // Cria uma nova aposta e salva no banco de dados
            var bet = new Bet
            {
                MatchId = matchId,
                SelectedOutcome = selectedOutcome,
                BetAmount = betAmount,
                PossibleReturn = possibleReturn
            };

            await _apiServiceBet.Insert(bet);

            // Passa o retorno possível para a view
            ViewBag.PossibleReturn = possibleReturn;
            return View(match);
        }

        private decimal CalculatePossibleReturn(string selectedOutcome, decimal betAmount, Live match)
        {
            switch (selectedOutcome)
            {
                case "HomeTeam":
                    return betAmount * match.HomeTeamOdds;
                case "VisitingTeam":
                    return betAmount * match.VisitingTeamOdds;
                case "Draw":
                    return betAmount * match.DrawOdds;
                default:
                    throw new ArgumentException("Resultado selecionado inválido.", nameof(selectedOutcome));
            }
        }
    }
}
