using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;

namespace ActeAdministratif.Models
{
    public class Enregistrer
    {
        [Key] // Attribut pour définir cette propriété comme clé primaire
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }
        // Clé étrangère pour le Document
        public string? DocumentId { get; set; }

        [ForeignKey("DocumentId")]
        public Document? Document { get; set; }
        // Clé étrangère pour le Filiation
        public string? FiliationId { get; set; }

        [ForeignKey("FiliationId")]
        public Filiation? Filiation { get; set; }
    }
}
