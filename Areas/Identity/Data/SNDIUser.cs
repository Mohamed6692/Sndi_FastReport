using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ActeAdministratif.Areas.Identity.Data;

// Add profile data for application users by adding properties to the SNDIUser class
public class SNDIUser : IdentityUser   
{
    [PersonalData]
    [Column(TypeName="nvarchar(100)")]
    public string? Nom { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string? Prenom { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string? PassGenerate { get; set; }
}

