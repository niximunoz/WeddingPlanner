using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers;

public class WeddingController : Controller
{
    private readonly ILogger<WeddingController> _logger;
    private MyContext _context;

    public WeddingController(ILogger<WeddingController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    /*METODOS GET*/
    [HttpGet("Weddings")]
    [SessionCheck]
    public IActionResult Weddings()
    {
        List<Wedding> AllWeddings = _context.Weddings.Include(at => at.Assistants).ThenInclude(at => at.User).ToList();

        return View("AllWeddings", AllWeddings);
    }

    [HttpGet("weddings/new")]
    [SessionCheck]
    public IActionResult NewWedding()
    {
        return View("FormWedding");
    }


    [HttpGet("weddings/{id_wedding}")]
    [SessionCheck]
    public IActionResult ViewWedding(int id_wedding)
    {
        Wedding? viewWedding = _context.Weddings
        .Include(w => w.Creator)
        .FirstOrDefault(w => w.WeddingId == id_wedding);
        var confirmedUsers = _context.Assistances
        .Where(c => c.WeddingId == id_wedding)
        .Include(c => c.User)
        .Select(c => c.User)
        .ToList();

        var viewModel = new WeddingUser
        {
            Wedding = viewWedding,
            Creator = viewWedding.Creator,
            ListAsist = confirmedUsers
        };
        return View("ViewWedding", viewModel);
    }

    [HttpGet]
    [SessionCheck]
    [Route("wedding/assistance")]
    public IActionResult AssistanceWedding(int id_wedding)
    {
        var assitance = _context.Assistances.FirstOrDefault(w => w.WeddingId == id_wedding && w.UserId == HttpContext.Session.GetInt32("UserId"));

        if (assitance != null)
        {
            _context.Assistances.Remove(assitance);
            _context.SaveChanges();
        }
        else
        {
            var newAssist = new Attendance
            {
                UserId = (int)HttpContext.Session.GetInt32("UserId"),
                WeddingId = id_wedding
            };
            _context.Assistances.Add(newAssist);
            _context.SaveChanges();
        }
        return RedirectToAction("Weddings");
    }

    [HttpGet]
    [SessionCheck]
    [Route("wedding/delete/{id_wedding}")]
    public IActionResult RemoveWedding(int id_wedding)
    {
        Wedding? wedding = _context.Weddings.FirstOrDefault(w => w.WeddingId == id_wedding);
        if (wedding != null)
        {
            // Asistentes de la boda
            List<Attendance> assistances = _context.Assistances.Where(a => a.WeddingId == id_wedding).ToList();

            _context.Weddings.Remove(wedding);
            // Eliminar los asistentes
            foreach (var assistance in assistances)
            {
                _context.Assistances.Remove(assistance);
            }
            _context.SaveChanges();
            return RedirectToAction("Weddings");
        }
        return RedirectToAction("Weddings");
    }
    /*Metodos POST*/
    [HttpPost]
    [Route("wedding/add")]
    public IActionResult AddWedding(Wedding newWedding)
    {
        if (ModelState.IsValid)
        {
            _context.Weddings.Add(newWedding);

            _context.SaveChanges();
            int id_wedding = newWedding.WeddingId;
            return RedirectToAction("ViewWedding", new { id_wedding });
        }
        return View("NewWedding");
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    public class SessionCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string? email = context.HttpContext.Session.GetString("UserEmail");
            if (email == null)
            {
                context.Result = new RedirectToActionResult("Index", "Authentication", null);
            }
        }
    }
}
