using System.ComponentModel.DataAnnotations;

namespace GoalGetters.Models
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        // Outras propriedades conforme necessário, mas sem a propriedade Password
        [DataType(DataType.Upload)]
        [Display(Name = "Sua foto")]
        public byte[] Photo { get; set; }
        [Display(Name = "Lucro")]
        public decimal Cash { get; set; }
    }

}
