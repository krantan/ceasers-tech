using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using api.Business.Dtos;
using api.Business.Queries;
using api;
using MediatR;

namespace GuestApp.Pages;

public class GuestEditModel : PageModel
{
    private readonly IMediator _mediator;
    public static GuestInfo guest = new GuestInfo(){};
    public GuestEditModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> OnGet()
    {
        string AccountId = HttpContext.Session.GetString("AccountId") ?? "";
        if (!string.IsNullOrEmpty(AccountId))
        {
            return Redirect("/");
        }
        string Id = RouteData.Values["Id"]?.ToString() ?? "";
        var result = await _mediator.Send(new GetGuestById()
        {
            Id = Id
        });
        
        guest = result.Guest;
        return new EmptyResult();
    }
}

