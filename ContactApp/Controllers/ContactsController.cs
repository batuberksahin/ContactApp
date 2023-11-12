using System.Security.Claims;
using ContactApp.Data;
using ContactApp.Models;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> List()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var contactList = await _context.Contacts
            .Where(c => c.OwnerId == userId)
            .ToListAsync();

        return View("~/Views/Contacts/List.cshtml", contactList);
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(Guid contactId)
    {
        var contact = await _context.Contacts.FindAsync(contactId);
        
        if (contact == null)
        {
            return NotFound();
        }

        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException());
        
        if (contact.OwnerId != userId)
        {
            return Forbid();
        }

        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();

        return RedirectToAction("List");
    }

    [HttpPost]
    public async Task<IActionResult> Add(string contactName, string phoneNumber)
    {
        if (string.IsNullOrEmpty(contactName) || string.IsNullOrEmpty(phoneNumber))
        {
            ViewBag.ErrorMessage = "Name or number is empty!";
            return RedirectToAction("List");
        }

        if (!IsPhoneNumberValid(phoneNumber))
        {
            ViewBag.ErrorMessage = "Invalid phone number!";
            return RedirectToAction("List");
        }

        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException());

        var newContact = new Contact
        {
            Id = Guid.NewGuid(),
            OwnerId = userId,
            ContactName = contactName,
            PhoneNumber = phoneNumber,
            CreatedAt = DateTime.UtcNow
        };

        _context.Contacts.Add(newContact);
        await _context.SaveChangesAsync();

        return RedirectToAction("List");
    }
    
    private bool IsPhoneNumberValid(string phoneNumber)
    {
        return phoneNumber.Length == 10 && phoneNumber.All(char.IsDigit);
    }


}