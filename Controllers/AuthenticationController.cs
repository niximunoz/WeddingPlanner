using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers;

public class AuthenticationController : Controller
{
    private readonly ILogger<AuthenticationController> _logger;
    private MyContext _context;

    public AuthenticationController(ILogger<AuthenticationController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }
    /*METODOS GET*/
    [HttpGet("")]
    public IActionResult Index()
    {
        HttpContext.Session.Clear();
        return View();
    }

    [HttpGet("logout")]
    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    /*METODOS POST*/
    [HttpPost]
    [Route("procesa/registro")]
    public IActionResult ProcesaRegistro(User newUser)
    {
        if (ModelState.IsValid)
        {
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
            _context.Users.Add(newUser);
            _context.SaveChanges();
            HttpContext.Session.SetString("UserEmail", newUser.Email);
            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            return RedirectToAction("Weddings", "Wedding");
        }
        return View("Index");
    }


    [HttpPost]
    [Route("procesa/login")]
    public IActionResult ProcesaLogin(LoginUser loginUser)
    {
        if (ModelState.IsValid)
        {
            User? user = _context.Users.FirstOrDefault(us => us.Email == loginUser.EmailLogin);

            if (user != null)
            {
                PasswordHasher<LoginUser> Hasher = new PasswordHasher<LoginUser>();
                var result = Hasher.VerifyHashedPassword(loginUser, user.Password, loginUser.PasswordLogin);

                if (result != 0)
                {
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    return RedirectToAction("Weddings", "Wedding");
                }
                ModelState.AddModelError("PasswordLogin", "Credenciales incorrectas");
                return View("Index");

            }
            ModelState.AddModelError("EmailLogin", "El campo Email no existe en la base de datos.");
            return View("Index");
        }
        return View("Index");
    }

    public IActionResult Privacy()
    {
        return View("../Home/Privacy");
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
