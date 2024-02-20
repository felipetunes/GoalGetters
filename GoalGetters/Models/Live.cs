using System.ComponentModel.DataAnnotations;
using static GoalGetters.Helper.Enums;

namespace GoalGetters.Models
{
    public class Live
    {
        // Identificador único para a partida
        public int Id { get; set; }

        // Equipe da casa
        [Display(Name = "Time Mandante")]
        public Team HomeTeam { get; set; }

        // Equipe visitante
        [Display(Name = "Time Visitante")]
        public Team VisitingTeam { get; set; }

        // Campeonato da partida
        [Display(Name = "Campeonato")]
        public Championship Championship { get; set; }

        // Data da partida
        [Display(Name = "Data da partida")]
        public DateTime DateMatch { get; set; }

        // Estádio da partida
        [Display(Name = "Estádio")]
        public string Stadium { get; set; }

        // Status da partida
        [Display(Name = "Status")]
        public string StatusMatch { get; set; }
        // Campo para rastrear o tempo de jogo
        [Display(Name = "Tempo de Jogo")]
        public int GameTime { get; set; }

        // Pontos da equipe da casa
        [Display(Name = "Gols do Mandante")]
        public int TeamPoints1 { get; set; }

        // Pontos da equipe visitante
        [Display(Name = "Gols do Visitante")]
        public int TeamPoints2 { get; set; }

        // Vitórias da equipe da casa
        [Display(Name = "Vitórias do Mandante")]
        public int HomeTeamWins { get; set; }

        // Vitórias da equipe visitante
        [Display(Name = "Vitórias do Visitante")]
        public int VisitingTeamWins { get; set; }

        // Empates
        [Display(Name = "Empates")]
        public int Draws { get; set; }

        // Desempenho recente da equipe da casa
        [Display(Name = "Desempenho recente do Mandante")]
        public decimal HomeTeamRecentPerformance { get; set; }

        // Desempenho recente da equipe visitante
        [Display(Name = "Desempenho recente do Visitante")]
        public decimal VisitingTeamRecentPerformance { get; set; }

        // Odds da equipe da casa
        public decimal HomeTeamOdds { get; set; }

        // Odds da equipe visitante
        public decimal VisitingTeamOdds { get; set; }

        // Odds de empate
        public decimal DrawOdds { get; set; }

        // Lista de apostas feitas nesta partida
        public List<Bet> Bets { get; set; }
    }

}
