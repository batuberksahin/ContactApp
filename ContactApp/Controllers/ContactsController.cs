using ContactApp.Data;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;

namespace ContactApp.Controllers;

[Authorize]
public class ContactsController : Controller
{
    private readonly ContactAppDbContext _context;
    private readonly ILogger<UsersController> _logger;

    public ContactsController(ContactAppDbContext context, ILogger<UsersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult List()
    {
        return View("~/Views/Contacts/List.cshtml");
    }
}