using System.ComponentModel.DataAnnotations;

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
        public List<Player> Players { get; set; }
    }
}
