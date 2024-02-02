using System.ComponentModel.DataAnnotations;

namespace GoalGetters.Helper
{
    public class Enums
    {
        public enum Position
        {
            [Display(Name = "Goleiro")]
            Goalkeeper,

            [Display(Name = "Zagueiro")]
            Defender,

            [Display(Name = "Lateral Direito")]
            RightBack,

            [Display(Name = "Lateral Esquerdo")]
            LeftBack,

            [Display(Name = "Meio Campo")]
            Midfielder,

            [Display(Name = "Atacante")]
            Forward,

            [Display(Name = "Centroavante")]
            Striker
        }
        public enum Championship
        {
            [Display(Name = "Copa do Mundo")]
            CopaDoMundo,

            [Display(Name = "Champions League")]
            ChampionsLeague,

            [Display(Name = "Premier League")]
            PremierLeague,

            [Display(Name = "La Liga")]
            LaLiga,

            [Display(Name = "Série A")]
            SerieA,

            [Display(Name = "Bundesliga")]
            Bundesliga,

            [Display(Name = "Copa Libertadores da América")]
            CopaLibertadoresDaAmérica,

            [Display(Name = "Campeonato Brasileiro")]
            CampeonatoBrasileiro,

            [Display(Name = "Paulista A1")]
            PaulistaA1,
        }
    }
}
