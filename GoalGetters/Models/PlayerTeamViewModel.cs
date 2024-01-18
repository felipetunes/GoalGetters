namespace GoalGetters.Models
{
    public class PlayerTeamViewModel
    {
        public IEnumerable<Player> Players { get; set; }
        public IEnumerable<Team> Teams { get; set; }
    }
}