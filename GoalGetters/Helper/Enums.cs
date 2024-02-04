using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

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
            [Display(Name = "Campeonato Brasileiro | Série A")]
            CampeonatoBrasileiroA = 1,
            [Display(Name = "Campeonato Brasileiro | Série B")]
            CampeonatoBrasileiroB = 2,
            [Display(Name = "Champions League")]
            ChampionsLeague = 3,
            [Display(Name = "Premier League")]
            PremierLeague = 4,
            [Display(Name = "La Liga")]
            LaLiga = 5,
            [Display(Name = "Serie A")]
            SerieA = 6,
            [Display(Name = "Bundesliga")]
            Bundesliga = 7,
            [Display(Name = "Copa Libertadores da América")]
            CopaLibertadoresDaAmérica = 8,
            [Display(Name = "Paulista A1")]
            PaulistaA1 = 9,
            [Display(Name = "Supercopa")]
            SupercopaBrasil = 10,
            [Display(Name = "Série A")]
            CopaMundo = 11,
            [Display(Name = "Liga Portugal")]
            LigaPortugal = 12,
            [Display(Name = "Eredivisie")]
            Eredivisie = 13,
            [Display(Name = "Primera División")]
            PrimeraDivision = 14,
            [Display(Name = "Superliga")]
            Superliga = 15,
            [Display(Name = "K League")]
            KLeague = 16,
            [Display(Name = "Major League Soccer")]
            MajorLeagueSoccer = 17,
        }

        public enum Stadium
        {
            [Display(Name = "Allianz Parque")]
            AllianzParque = 1,
            [Display(Name = "Neo Química Arena")]
            NeoQuímicaArena = 2,
            [Display(Name = "Morumbi")]
            Morumbi = 3,
            [Display(Name = "Maracanã")]
            Maracana = 4,
            [Display(Name = "Mané Garrincha")]
            ManeGarrincha = 5,
            [Display(Name = "Mineirão")]
            Mineirao = 6,
            [Display(Name = "Mineirão")]
            Castelao = 7,
            [Display(Name = "Arruda")]
            Arruda = 8,
            [Display(Name = "Arena do Grêmio")]
            ArenaGremio = 9,
            [Display(Name = "Mangueirão")]
            Mangueirao = 10,
            [Display(Name = "Santiago Bernabéu")]
            SantiagoBernabeu = 11,
            [Display(Name = "Lluís Companys")]
            LluisCompanys = 12,
            [Display(Name = "Spotify Camp Nou")]
            CampNou = 13,
            [Display(Name = "La Bombonera")]
            LaBombonera = 14,
        }
    }
}
