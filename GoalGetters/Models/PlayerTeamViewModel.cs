using X.PagedList;

namespace GoalGetters.Models
{
    public class PlayerTeamViewModel
    {
        public IPagedList<Player> Players { get; set; }
        public IPagedList<Team> Teams { get; set; }
    }
}