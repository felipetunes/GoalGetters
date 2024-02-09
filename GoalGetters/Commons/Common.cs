using GoalGetters.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoalGetters.Commons
{
    public class Common
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ApiService<Live> _apiServiceLive;

        public Common(ApiService<Live> apiServiceLive)
        {
            _apiServiceLive = apiServiceLive;
        }

        public async Task CalculateOdds(Live live)
        {
            // Calcula as odds com base no número de vitórias, empates e desempenho recente
            CalculateMatchOdds(live);

            //// Atualiza o jogo com as novas odds
            //await _apiServiceLive.UpdateLive(live);
        }

        public void CalculateMatchOdds(Live match)
        {
            decimal totalMatches = match.HomeTeamWins + match.VisitingTeamWins + match.Draws;

            if (totalMatches != 0)
            {
                decimal homeWinProbability = ((decimal)match.HomeTeamWins + match.HomeTeamRecentPerformance) / totalMatches;
                decimal visitingWinProbability = ((decimal)match.VisitingTeamWins + match.VisitingTeamRecentPerformance) / totalMatches;
                decimal drawProbability = (decimal)match.Draws / totalMatches;

                // Ajusta as odds com base no tempo de jogo
                decimal timeFactor = (90 - match.GameTime) / 90.0m;
                homeWinProbability *= timeFactor;
                visitingWinProbability *= timeFactor;
                drawProbability *= (1 - timeFactor);

                // Ajusta as odds para garantir um lucro modesto para a casa de apostas
                decimal houseEdge = 0.05m; // 5% de lucro para a casa

                if (homeWinProbability != 0)
                {
                    match.HomeTeamOdds = 1 / (homeWinProbability * (1 - houseEdge));
                }

                if (visitingWinProbability != 0)
                {
                    match.VisitingTeamOdds = 1 / (visitingWinProbability * (1 - houseEdge));
                }

                if (drawProbability != 0)
                {
                    match.DrawOdds = 1 / (drawProbability * (1 - houseEdge));
                }
            }
        }
    }
}
