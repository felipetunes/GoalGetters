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

            [Display(Name = "Centro Avante")]
            Striker
        }
    }
}
