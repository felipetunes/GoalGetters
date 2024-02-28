using X.PagedList;

namespace GoalGetters.Models
{
    public class SearchViewModel
    {
        public IPagedList<Player> Players { get; set; }
        public IPagedList<Team> Teams { get; set; }
    }
}
