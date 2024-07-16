using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace GuestApp.Pages;

public class IndexModel : PageModel
{
    public IndexModel()
    {
    }

    public void OnGet()
    {
        ViewData["AccountId"] = HttpContext.Session.GetString("AccountId") ?? "";
    }
}
