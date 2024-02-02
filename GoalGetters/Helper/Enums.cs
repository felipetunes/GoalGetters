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
            [Display(Name = "Campeonato Brasileiro")]
            CampeonatoBrasileiro = 1,

            [Display(Name = "Copa do Mundo")]
            CopaDoMundo = 2,

            [Display(Name = "Champions League")]
            ChampionsLeague = 3,

            [Display(Name = "Premier League")]
            PremierLeague = 4,

            [Display(Name = "La Liga")]
            LaLiga = 5,

            [Display(Name = "Série A")]
            SerieA = 6,

            [Display(Name = "Bundesliga")]
            Bundesliga = 7,

            [Display(Name = "Copa Libertadores da América")]
            CopaLibertadoresDaAmérica = 8,

            [Display(Name = "Paulista A1")]
            PaulistaA1 = 9
        }

//#ToDo
//        public enum Stadium
//        {
//            [Display(Name = "Allianz Parque")]
//            Allianz Parque = 1,

//        }
    }
}
