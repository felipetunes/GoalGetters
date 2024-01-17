using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GoalGetters.Models
{
    public class Player
    {
        public int Id { get; set; }
        public int IdTeam { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime Birth { get; set; }
        public string? Height { get; set; }
        public string? TeamName { get; set; }
        public int Age
        {
            get
            {
                if (Birth.Year != 0001)
                {
                    var today = DateTime.Today;
                    var age = today.Year - Birth.Year;
                    if (Birth.Date > today.AddYears(-age)) age--;
                    return age;
                }
                return 0;
            }
        }
    }
}