using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ActeAdministratif.Data;
using ActeAdministratif.Models;

namespace ActeAdministratif.Controllers
{
    public class FiliationsController : Controller
    {
        private readonly SNDIContext _context;

        public FiliationsController(SNDIContext context)
        {
            _context = context;
        }

        // GET: Filiations
        public async Task<IActionResult> Index()
        {
            var sNDIContext = _context.Filiation.Include(f => f.Document);
            return View(await sNDIContext.ToListAsync());
        }

        // GET: Filiations/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Filiation == null)
            {
                return NotFound();
            }

            var filiation = await _context.Filiation
                .Include(f => f.Document)
                .FirstOrDefaultAsync(m => m.id == id);
            if (filiation == null)
            {
                return NotFound();
            }

            return View(filiation);
        }

        // GET: Filiations/Create
        public IActionResult Create(string documentId)
        {
            var model = new Filiation
            {
                DocumentId = documentId
            };
            return View(model);
        }

        // POST: Filiations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,NomPere,PrenomPere,DateDenaissancePere,LieuxNaissancePere,TypeDePiecesPere,NumeroPiecePere,DateeditionPere,LieuxEditionPere,NomMere,PrenomMere,DateDenaissanceMere,LieuxNaissanceMere,TypeDePiecesMere,NumeroPieceMere,DateeditionMere,LieuxEditionMere,DocumentId")] Filiation filiation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(filiation);
                await _context.SaveChangesAsync();
                // Generate the URL for the Details action with the created document ID
                var detailsUrl = Url.Action("Details", new { id = filiation.id });

                // Redirect to the Details action
                return Redirect(detailsUrl);
            }
            return View(filiation);
        }

        // GET: Filiations/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Filiation == null)
            {
                return NotFound();
            }

            var filiation = await _context.Filiation.FindAsync(id);
            if (filiation == null)
            {
                return NotFound();
            }
            ViewData["DocumentId"] = new SelectList(_context.Document, "id", "id", filiation.DocumentId);
            return View(filiation);
        }

        // POST: Filiations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id,NomPere,PrenomPere,DateDenaissancePere,LieuxNaissancePere,NomMere,PrenomMere,DateDenaissanceMere,LieuxNaissanceMere,DocumentId")] Filiation filiation)
        {
            if (id != filiation.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filiation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FiliationExists(filiation.id))
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
            ViewData["DocumentId"] = new SelectList(_context.Document, "id", "id", filiation.DocumentId);
            return View(filiation);
        }

        // GET: Filiations/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Filiation == null)
            {
                return NotFound();
            }

            var filiation = await _context.Filiation
                .Include(f => f.Document)
                .FirstOrDefaultAsync(m => m.id == id);
            if (filiation == null)
            {
                return NotFound();
            }

            return View(filiation);
        }

        // POST: Filiations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Filiation == null)
            {
                return Problem("Entity set 'SNDIContext.Filiation'  is null.");
            }
            var filiation = await _context.Filiation.FindAsync(id);
            if (filiation != null)
            {
                _context.Filiation.Remove(filiation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Filiations/Regroupe/5
        public async Task<IActionResult> Regroupe(string id)
        {
            // Recherchez la filiation correspondante à l'ID
            var filiation = await _context.Filiation.FindAsync(id);

            if (filiation == null)
            {
                return NotFound();
            }

            // Recherchez le document correspondant à DocumentId dans la filiation
            var document = await _context.Document.FindAsync(filiation.DocumentId);

            if (document == null)
            {
                return NotFound();
            }

            // Comparer l'Id du Document dans la filiation avec l'Id du Document de l'élément sélectionné
            if (document.id == filiation.DocumentId)
            {
                // Créez un modèle pour les informations regroupées
                var regroupeModel = new RegroupeViewModel
                {
                    Filiation = filiation,
                    Document = document
                };

                return View(regroupeModel);
            }
            else
            {
                return NotFound();
            }
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Regroupe(string FiliationId, string DocumentId)
        {
            if (ModelState.IsValid)
            {
                // Créez une nouvelle instance de Enregistrer avec les IDs fournis
                var enregistrer = new Enregistrer
                {
                    FiliationId = FiliationId,
                    DocumentId = DocumentId
                };

                _context.Enregistrer.Add(enregistrer);
                await _context.SaveChangesAsync();

                // Redirigez vers l'action Details appropriée en utilisant les IDs
                var detailsUrl = Url.Action("Details", new { id = enregistrer.Id });
                return Redirect("/");
            }

            // Si le modèle n'est pas valide, affichez le formulaire avec les erreurs
            return View();
        }


        private bool FiliationExists(string id)
        {
          return (_context.Filiation?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
