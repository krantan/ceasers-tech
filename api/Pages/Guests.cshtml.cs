using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using api.Business.Dtos;
using api.Business.Queries;
using api;
using MediatR;

namespace GuestApp.Pages;

public class GuestsModel : PageModel
{

    private readonly IMediator _mediator;

    public static List<GuestInfo> guests = new List<GuestInfo>();

    public GuestsModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> OnGet()
    {
        string AccountId = HttpContext.Session.GetString("AccountId") ?? "";

        if (string.IsNullOrEmpty(AccountId))
        {
            return Redirect("/");
        }
        var result = await _mediator.Send(new GetGuests()
        {
            SortType = ""
        });
        if (result.Guests.Count > 0)
        {
            guests = result.Guests;
        }

        return Page();
    }
}

