using GoalGetters.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

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
                    decimal homeOdds = 1 / (homeWinProbability * (1 - houseEdge));
                    match.HomeTeamOdds = homeOdds > 0 ? homeOdds : 0;
                }

                if (visitingWinProbability != 0)
                {
                    decimal visitingOdds = 1 / (visitingWinProbability * (1 - houseEdge));
                    match.VisitingTeamOdds = visitingOdds > 0 ? visitingOdds : 0;
                }

                if (drawProbability != 0)
                {
                    decimal drawOdds = 1 / (drawProbability * (1 - houseEdge));
                    match.DrawOdds = drawOdds > 0 ? drawOdds : 0;
                }
            }
        }

        public static string? GetDisplayName(Enum value)
        {
            return value.GetType()
                        .GetMember(value.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        ?.GetName();
        }

        public static IEnumerable<string> GetCountryNames()
        {
            var countries = ISO3166.Country.List.Select(c => c.TwoLetterCode);
            var translatedCountries = countries.Select(country => IsoNames.CountryNames.GetName(new CultureInfo("pt"), country))
                                               .Where(name => !string.IsNullOrEmpty(name))
                                               .OrderBy(name => name);
            return translatedCountries;
        }
    }
}

