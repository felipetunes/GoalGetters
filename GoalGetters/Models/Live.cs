using System.ComponentModel.DataAnnotations;

namespace GoalGetters.Models
{
    public class Live
    {
        public int Id { get; set; }
        [Display(Name = "Mandante")]
        public int IdTeam1 { get; set; }
        [Display(Name = "Visitante")]
        public int IdTeam2 { get; set; }
        [Display(Name = "Campeonato")]
        public int IdChampionship { get; set; }
        [Display(Name = "Data da partida")]
        public DateTime DateMatch { get; set; }
        [Display(Name = "Estádio")]
        public string Stadium { get; set; }
        [Display(Name = "Mandante")]
        public string HomeTeamName { get; set; }
        [Display(Name = "Visitante")]
        public string VisitingTeamName { get; set; }
        [Display(Name = "Tempo")]
        public int ElapsedTime { get; internal set; }
        [Display(Name = "Status")]
        public string StatusMatch { get; internal set; }
        [Display(Name = "Gols")]
        public int TeamPoints1 { get; internal set; }
        [Display(Name = "Gols")]
        public int TeamPoints2 { get; internal set; }
    }
}
