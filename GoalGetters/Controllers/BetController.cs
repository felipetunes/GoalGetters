using GoalGetters.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

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

        // POST: Bet/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,TeamName,Points")] Bet bet)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction(nameof(Index));
            }
            return View(bet);
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

        [HttpPost]
        public ActionResult UpdatePartialView(string ChosenOdds, string SelectedOutcome, string MatchId, string HomeTeam, string VisitingTeam, string homeTeamOdds, string drawOdds, string visitingTeamOdds, string betAmount)
        {
            Live match = new Live();
            match.DrawOdds = Convert.ToDecimal(drawOdds);
            match.HomeTeamOdds = Convert.ToDecimal(homeTeamOdds);
            match.VisitingTeamOdds = Convert.ToDecimal(visitingTeamOdds);

            var returnCash = CalculatePossibleReturn(SelectedOutcome, Convert.ToDecimal(betAmount), match);

            var bet = new Bet
            {
                ChosenOdds = Convert.ToDecimal(ChosenOdds),
                SelectedOutcome = SelectedOutcome,
                MatchId = Convert.ToInt32(MatchId),
                HomeTeam = HomeTeam,
                VisitingTeam = VisitingTeam,
                PossibleReturn = returnCash
            };

            // Crie uma nova lista de Bets para teste
            var bets = new List<Bet> { bet };

            return PartialView("BetBar", bets);
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
                case "1":
                    return betAmount * match.HomeTeamOdds;
                case "2":
                    return betAmount * match.VisitingTeamOdds;
                case "Empate":
                    return betAmount * match.DrawOdds;
                default:
                    throw new ArgumentException("Resultado selecionado inválido.", nameof(selectedOutcome));
            }
        }
    }
}
