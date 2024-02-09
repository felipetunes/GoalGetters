namespace GoalGetters.Models
{
    public class Match
    {
        public int Id { get; set; } // Identificador único para a partida

        public int ChampionshipId { get; set; } // Identificador do campeonato
        public Championship Championship { get; set; } // Campeonato da partida
    }
}
