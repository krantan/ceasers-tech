using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GuestApp.Pages;

public class GuestNewModel : PageModel
{
    public GuestNewModel()
    {
    }

    public IActionResult OnGet()
    {
        string AccountId = HttpContext.Session.GetString("AccountId") ?? "";
        if (!string.IsNullOrEmpty(AccountId))
        {
            return Redirect("/");
        }
        return new EmptyResult();
    }
}

