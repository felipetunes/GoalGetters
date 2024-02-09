﻿using System.ComponentModel.DataAnnotations;

namespace GoalGetters.Models
{
    public class Bet
    {
        public int Id { get; set; } // Identificador único para a aposta

        public int MatchId { get; set; } // Identificador da partida na qual a aposta é feita

        public int UserId { get; set; } // Identificador do usuário que fez a aposta

        public string SelectedOutcome { get; set; } // O resultado selecionado pelo usuário ("HomeTeam", "VisitingTeam" ou "Draw")

        public decimal BetAmount { get; set; } // A quantidade de dinheiro apostada pelo usuário

        public decimal PossibleReturn { get; set; } // O retorno possível se a aposta for bem-sucedida
    }
}
