using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace GoalGetters.Models
{
    public class User
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }

        [Required]
        [JsonProperty("username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [JsonProperty("password")]
        public string Password { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Sua foto")]
        [JsonProperty("photo")]
        public byte[]? Photo { get; set; }

        [Display(Name = "Lucro")]
        [JsonProperty("cash")]
        public double? Cash { get; set; }
        public string Token { get; set; }
    }
}
