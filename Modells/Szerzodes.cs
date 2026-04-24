using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LorincMarton_WebAPI.Modells
{
    public class Szerzodes
    {
        [Key]
        public int Id { get; set; }
        public string SzerzodesSzam { get; set; }
        public bool IgazgatoJovahagyta { get; set; }
        public string SzerzodesTargya { get; set; }
        public int PartnerId { get; set; }
        [JsonIgnore]
        [ForeignKey("PartnerId")]
        public Partner? Partner { get; set; }
    }
}
