using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace GoalGetters.Models
{
    public class Team
    {
        public int Id { get; set; }
        [Display(Name = "Nome")]
        public string Name { get; set; }
        [Display(Name = "Cidade")]
        public string City { get; set; }
        [Display(Name = "País")]
        public string Country { get; set; }
        public IPagedList<Player> Players { get; set; }
        public string Color1 { get; set; }
        [Display(Name = "Jogadores no plantel")]
        public int PlayersCount { get; set; }
        [Display(Name = "Média etária")]
        public string AverageAge { get; set; }
    }
}
