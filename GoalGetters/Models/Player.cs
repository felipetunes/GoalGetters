using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace GoalGetters.Models
{
    public class Player
    {
        public int Id { get; set; }
        public int IdTeam { get; set; }
        [Display(Name = "Nome")]
        public string Name { get; set; }
        [Display(Name = "Cidade")]
        public string City { get; set; }
        [Display(Name = "País")]
        public string Country { get; set; }
        [Display(Name = "Nascimento")]
        public DateTime Birth { get; set; }
        [Display(Name = "Altura")]
        public float? Height { get; set; }
        [Display(Name = "Posição")]
        public Helper.Enums.Position? Position { get; set; }

        [Display(Name = "Time")]
        public string? TeamName { get; set; }
        [Display(Name = "Número")]
        public int? ShirtNumber { get; set; }
        [Display(Name = "Foto")]
        public string? ImagePath { get; set; }
        [Display(Name = "Idade")]
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