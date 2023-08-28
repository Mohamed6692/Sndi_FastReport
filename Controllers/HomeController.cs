using ActeAdministratif.Models;
using FastReport.Export.PdfSimple;
using FastReport;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ActeAdministratif.Data;
using Microsoft.EntityFrameworkCore;
using ActeAdministratif.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;



namespace ActeAdministratif.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly SNDIContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<SNDIUser> _userManager;

        public HomeController(SNDIContext context, ILogger<HomeController> logger, UserManager<SNDIUser> userManager)
        {
            _logger = logger;
            this._userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["UserID"] = _userManager.GetUserId(this.User);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
        ////int id
        //public FileResult Generate()
        //{
        //    FastReport.Utils.Config.WebMode = true;
        //    Report report = new();
        //    string path = Path.Combine(Directory.GetCurrentDirectory(), "documents.frx");
        //    //var pathServer = Path.Combine(path, "Templates");
        //    //path = Path.Combine(pathServer, "liste_ordonnance.frx");
        //    report.Load(path);
        //    //report.SetParameterValue("OnerID0",Id)

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
    }
}