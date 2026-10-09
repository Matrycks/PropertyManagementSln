using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Client.Models;

namespace Client.Controllers;

/// <summary>
/// Controller for handling account-related actions such as login and registration.
/// </summary>
public class AccountController : Controller
{
    // GET: /Account/AccountDetails
    public IActionResult AccountDetails()
    {
        return RedirectToPage("/Account/Details", new { area = "Identity" });
    }
}
