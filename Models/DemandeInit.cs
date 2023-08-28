using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ActeAdministratif.Models
{
    public class DemandeInit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? id { get; set; }

        public string? TypeActAdmin { get; set; }

        public float ? Montant { get; set; }

        public string? NumeroRecuPaiem { get; set; }

        public string? NombreCopie { get; set; }

        public DateTime DataRequet { get; set; } = DateTime.Now;

        // Clé étrangère pour le Document
        public string? EnregistrerId { get; set; }

        [ForeignKey("EnregistrerId")]
        public Enregistrer? Enregistrer { get; set; }

        public int status { get; set; } = 0;
        public DemandeInit()
        {
            // Vous pouvez également initialiser d'autres propriétés ici si nécessaire
        }

    }
}
