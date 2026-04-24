using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LorincMarton_WebAPI.Modells
{
    public class Partner
    {
        [Key]
        public int Id { get; set; }
        public string PartnerNev { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public List<Szerzodes>? Szerzodes { get; set; }
    }
}
