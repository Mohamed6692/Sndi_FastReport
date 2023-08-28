using System.ComponentModel.DataAnnotations;

namespace ActeAdministratif.Models
{
    public class Country
    {
        public short Id { get; set; } // Utilisez short pour correspondre à smallint

        [MaxLength(2)]
        public string AbrevTwo { get; set; }

        [MaxLength(3)]
        public string AbrevThree { get; set; }

        [MaxLength(70)]
        public string Name_en { get; set; }

        [MaxLength(70)]
        public string Name_fr { get; set; }

        [MaxLength(70)]
        public string Nationality { get; set; }
    }
}
