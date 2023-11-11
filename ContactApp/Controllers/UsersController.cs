using ContactApp.Data;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ContactApp.Controllers;

[Authorize]
public class UsersController : Controller
{
    private readonly ContactAppDbContext _context;
    private readonly ILogger<UsersController> _logger;

    public UsersController(ContactAppDbContext context, ILogger<UsersController> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("List", "Contacts");
        }
        
        return View("~/Views/Users/Login.cshtml");
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(string username, string password, bool rememberMe)
    {
        var user = _context.Users.AsEnumerable().FirstOrDefault(u => u.Username == username);

        if (user != null)
        {
            var passwordVerificationResult = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (passwordVerificationResult)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = rememberMe
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("List", "Contacts");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View("~/Views/Users/Login.cshtml");
            }
        }
        
        ViewBag.ErrorMessage = "User not found!";
        return View("~/Views/Users/Login.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}