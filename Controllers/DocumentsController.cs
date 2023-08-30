using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using FastReport;
using FastReport.Export.PdfSimple;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActeAdministratif.Data;
using ActeAdministratif.Models;

namespace ActeAdministratif.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly SNDIContext _context;

        public DocumentsController(SNDIContext context)
        {
            _context = context;
        }
        // Generate File
        public FileResult Generate()
        {
            FastReport.Utils.Config.WebMode = true;
            Report report = new();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "document.frx");
            //var pathServer = Path.Combine(path, "Templates");
            //path = Path.Combine(pathServer, "liste_ordonnance.frx");
            report.Load(path);

            if (report.Report.Prepare())
            {
                PDFSimpleExport pdfExport = new()
                {
                    ShowProgress = false,
                    Subject = "Subject Test",
                    Title = "Report Title"
                };
                MemoryStream ms = new();
                report.Report.Export(pdfExport, ms);
                report.Dispose();
                pdfExport.Dispose();
                ms.Position = 0;
                return File(ms, "application/pdf", "myreport.pdf");
            }

            else
                return null;
        }

     
        



        // GET: Documents
        //public async Task<IActionResult> Index()
        //{
        //      return _context.Document != null ? 
        //                  View(await _context.Document.ToListAsync()) :
        //                  Problem("Entity set 'SNDIContext.Document'  is null.");
        //}

        // GET: Documents second
        public IActionResult Index()
        {
            var enregistrements = _context.Enregistrer
                .Include(e => e.Document) // Charge l'objet Document associé
                .Include(e => e.Filiation) // Charge l'objet Filiation associé
                .ToList();
            return View(enregistrements);
        }

        //Recherche
        public IActionResult Search(string searchNumero)
        {
            var enregistrements = _context.Enregistrer
                .Include(e => e.Document) // Charge l'objet Document associé
                .Include(e => e.Filiation) // Charge l'objet Filiation associé
                .Where(e => e.Document.Numero.Contains(searchNumero))  // Utilisez "==" pour une correspondance exacte
                .ToList();
            return View(enregistrements);
        }


        // GET: Documents/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Document == null)
            {
                return NotFound();
            }

            var document = await _context.Document
                .FirstOrDefaultAsync(m => m.id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        //Recuperer donner de la Bd:
        public IActionResult Create()
        {
            // Récupérez les données des pays depuis votre base de données
            List<Country> countries = _context.T_CONF_COUNTRY.ToList();

            // Vous pouvez trier, filtrer ou manipuler les données ici si nécessaire
            ViewBag.Countries = countries;
            // Transmettez les données des pays à la vue
            ViewBag.Countries = countries;

            return View();
        }

        // POST: Filiations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Numero,Date,Circonscription,LieuxEtablissement,NomDeFamille,NomDeJeuneFille,Prenom,Pseudo,Sexe,SituationMatrimoniale,NombreEnfant,DateDenaissance,LieuxNaissance,PaysNaissance,Nationnalite,CommuneDeNaissance,NumeroDeTelephone,Profession,Domicile,TypeDepiece,NumeroDePiece,DateEdition")] Document document)
        {
            if (ModelState.IsValid)
            {
                _context.Add(document);
                await _context.SaveChangesAsync();

                // Generate the URL for the Details action with the created document ID
                var detailsUrl = Url.Action("Details", new { id = document.id });

                // Redirect to the Details action
                return Redirect(detailsUrl);
            }

            // If ModelState is not valid, return to the Create view
            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Document == null)
            {
                return NotFound();
            }

            var document = await _context.Document.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            List<Country> countries = _context.T_CONF_COUNTRY.ToList();

            // Vous pouvez trier, filtrer ou manipuler les données ici si nécessaire
            ViewBag.Countries = countries;
            // Transmettez les données des pays à la vue
            ViewBag.Countries = countries;

            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id,Numero,Date,Circonscription,LieuxEtablissement,NomDeFamille,NomDeJeuneFille,Prenom,Pseudo,Sexe,SituationMatrimoniale,NombreEnfant,DateDenaissance,LieuxNaissance,PaysNaissance,Nationnalite,CommuneDeNaissance,NumeroDeTelephone,Profession,Domicile,TypeDepiece,NumeroDePiece,DateEdition")] Document document)
        {
            if (id != document.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(document);
        }

        // Edit page for 2
        public async Task<IActionResult> Editcmp(string id)
        {
            var enregistrement = await _context.Enregistrer
        .Include(e => e.Document)
        .Include(e => e.Filiation)
        .FirstOrDefaultAsync(e => e.Id == id);

            if (enregistrement == null)
            {
                return NotFound(); // Gérez le cas où l'enregistrement n'est pas trouvé
            }

            return View(enregistrement);
        }

        // POST: Filiations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Editcmp(string id, [Bind("id,NomPere,PrenomPere,DateDenaissancePere,LieuxNaissancePere,NomMere,PrenomMere,DateDenaissanceMere,LieuxNaissanceMere,DocumentId")] Filiation filiation)
        //{
            
        //}

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Document == null)
            {
                return NotFound();
            }

            var document = await _context.Document
                .FirstOrDefaultAsync(m => m.id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Document == null)
            {
                return Problem("Entity set 'SNDIContext.Document'  is null.");
            }
            var document = await _context.Document.FindAsync(id);
            if (document != null)
            {
                _context.Document.Remove(document);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(string id)
        {
          return (_context.Document?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
