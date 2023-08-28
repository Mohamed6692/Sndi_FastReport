using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActeAdministratif.Models;
using FastReport.Export.PdfSimple;
using FastReport;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActeAdministratif.Data;
using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace ActeAdministratif.Controllers
{

    public class DemandeInitController : Controller
    {
        private readonly SNDIContext _context;
        public DemandeInitController(SNDIContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var informationsDemandes = _context.DemandeInit
                .Include(d => d.Enregistrer) // Charge l'objet Enregistrer associé à la DemandeInit
                    .ThenInclude(e => e.Document) // Charge l'objet Document associé à l'Enregistrement
                .Include(d => d.Enregistrer) // Charge l'objet Enregistrer associé à la DemandeInit
                    .ThenInclude(e => e.Filiation) // Charge l'objet Filiation associé à l'Enregistrement
                .ToList();

            return View(informationsDemandes);
        }


        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.DemandeInit == null)
            {
                return NotFound();
            }

            var demandeInit = await _context.DemandeInit
                .FirstOrDefaultAsync(m => m.id == id);
            if (demandeInit == null)
            {
                return NotFound();
            }

            return View(demandeInit);
        }

        //GET:DemandeInit
        public IActionResult Create(string enregistrerId)
        {
            var model = new DemandeInit
            {
                EnregistrerId = enregistrerId
            };
            return View(model);
        }

        // POST: DemandeInit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,TypeActAdmin,Montant,NumeroRecuPaiem,NombreCopie,DataRequet,EnregistrerId")] DemandeInit demandeInit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(demandeInit);
                await _context.SaveChangesAsync();

                // Generate the URL for the Details action with the created document ID
                var detailsUrl = Url.Action("Details", new { id = demandeInit.id });

                // Redirect to the Details action
                return Redirect("/DemandeInit/Index");
            }

            // If ModelState is not valid, return to the Create view
            return View(demandeInit);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.DemandeInit == null)
            {
                return NotFound();
            }

            var demandeInit = await _context.DemandeInit.FindAsync(id);
            if (demandeInit == null)
            {
                return NotFound();
            }
            ViewData["EnregistrerId"] = new SelectList(_context.DemandeInit, "id", "id", demandeInit.EnregistrerId);
            return View(demandeInit);
        }


        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id,TypeActAdmin,NumeroRecuPaiem,NombreCopie,DataRequet,enregistrerId")] DemandeInit demandeInit)
        {
            if (id != demandeInit.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(demandeInit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DemandeInitExists(demandeInit.id))
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
            ViewData["enregistrerId"] = new SelectList(_context.DemandeInit, "id", "id", demandeInit.EnregistrerId);
            return View(demandeInit);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.DemandeInit == null)
            {
                return NotFound();
            }

            var demandeInit = await _context.DemandeInit
                .FirstOrDefaultAsync(m => m.id == id);
            if (demandeInit == null)
            {
                return NotFound();
            }

            return View(demandeInit);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.DemandeInit == null)
            {
                return Problem("Entity set 'SNDIContext.DemandeInit'  is null.");
            }
            var demandeInit = await _context.DemandeInit.FindAsync(id);
            if (demandeInit != null)
            {
                _context.DemandeInit.Remove(demandeInit);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Generer Casier jurdicaire

        public FileResult Generate(string id)
        {
            FastReport.Utils.Config.WebMode = true;
            Report report = new();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "documents.frx");

            // Charge la demande à partir de la base de données en fonction de l'ID
            var demande = _context.DemandeInit.FirstOrDefault(d => d.id == id);

            if (demande != null)
            {
                report.Load(path);
                report.SetParameterValue("DemandeId", id);

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

                    // Modifiez le statut de la demande ici
                    demande.status = 1;
                    // Enregistrez les modifications dans la base de données
                    _context.SaveChanges();

                    return File(ms, "application/pdf", "myreport.pdf");
                }
            }

            // Si la demande n'existe pas ou si la génération du rapport échoue, retournez null ou gérez l'erreur selon vos besoins.
            return null;
        }


        //Generer Certificat de Nationnalié
        //string id
        //public FileResult Generate1()
        //{
        //    FastReport.Utils.Config.WebMode = true;
        //    Report report = new();
        //    string path = Path.Combine(Directory.GetCurrentDirectory(), "Untitled1.frx");
        //    //var pathServer = Path.Combine(path, "Templates");
        //    //path = Path.Combine(pathServer, "liste_ordonnance.frx");
        //    report.Load(path);
        //    //report.SetParameterValue("DemandeId", id);

        //    if (report.Report.Prepare())
        //    {
        //        PDFSimpleExport pdfExport = new()
        //        {
        //            ShowProgress = false,
        //            Subject = "Subject Test",
        //            Title = "Report Title"
        //        };
        //        MemoryStream ms = new();
        //        report.Report.Export(pdfExport, ms);
        //        report.Dispose();
        //        pdfExport.Dispose();
        //        ms.Position = 0;
        //        return File(ms, "application/pdf", "myreport.pdf");
        //    }

        //    else
        //        return null;
        //}


        //Generer Certificat de Nationnalié
        public FileResult Generate1(string id)
        {
            FastReport.Utils.Config.WebMode = true;
            Report report = new();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Untitled1.frx");

            // Charge la demande à partir de la base de données en fonction de l'ID
            var demande = _context.DemandeInit.FirstOrDefault(d => d.id == id);

            if (demande != null)
            {
                // Modifiez le statut de la demande ici
                demande.status = 1;
                // Enregistrez les modifications dans la base de données
                _context.SaveChanges();

                report.Load(path);
                report.SetParameterValue("DemandeId", id);
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
            }

            // Si la demande n'existe pas ou si la génération du rapport échoue, retournez null ou gérez l'erreur selon vos besoins.
            return null;
        }




        private bool DemandeInitExists(string id)
        {
            return (_context.DemandeInit?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
