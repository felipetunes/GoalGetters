using System.ComponentModel.DataAnnotations;

namespace GoalGetters.Models
{
    public class Championship
    {
            public int Id { get; set; } // Identificador único para o campeonato

            [Display(Name = "Nome do Campeonato")]
            public string Name { get; set; } // Nome do campeonato

            [Display(Name = "Ano")]
            public int Year { get; set; } // Ano do campeonato

            // Lista de partidas que fazem parte deste campeonato
            public List<Match> Matches { get; set; }
    }
}
