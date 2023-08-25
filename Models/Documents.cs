using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ActeAdministratif.Models
{
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? id { get; set; }

        [Required]
        [MaxLength(255)] // Vous pouvez ajuster la longueur maximale si nécessaire
         // Indique que la propriété doit être unique
        public string? Numero { get; set; }

        public DateTime Date { get; set; }
        public string? Circonscription { get; set; }
         
        public string? LieuxEtablissement { get; set; }

        public string? NomDeFamille { get; set; }

        public string? NomDeJeuneFille { get; set; }

        public string? Prenom { get; set; }

        public string? Pseudo { get; set; }

        public string? Sexe { get; set; }

        public string? SituationMatrimoniale { get; set; }

        public int? NombreEnfant { get; set; }

        public DateTime? DateDenaissance { get; set; }

        public string? LieuxNaissance { get; set; }

        public string? PaysNaissance { get; set; }

        public string? Nationnalite { get; set; }

        public string? CommuneDeNaissance { get; set; }

        public string? NumeroDeTelephone { get; set; }

        public string? Profession { get; set; }

        public string? Domicile { get; set; }

        public string? TypeDepiece { get; set; }

        public string? NumeroDePiece { get; set; }

        public DateTime? DateEdition { get; set; }
    }
}
