using MediatR;
using api.Business.Data;
using api.Business.Commands;
using api.Business.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuestController : ControllerBase
{

    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GuestController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("create",Name = "CreateGuest")]
    public async Task<IActionResult> CreateGuest([FromBody] CreateGuest request)
    {
        try
        {
            var result = await _mediator.Send(request);
            
            return this.GetResponse(result);
        }
        catch (Exception ex)
        {
            return this.GetResponse(new BaseResponse()
            {
                Message = ex.Message,
                Success = false,
                ResponseCode = (int)HttpStatusCode.InternalServerError
            });
        }
    }

    [HttpPost("update", Name = "UpdateGuest")]
    public async Task<IActionResult> UpdateGuest([FromBody] UpdateGuest request)
    {
        try
        {
            var result = await _mediator.Send(request);

            return this.GetResponse(result);
        }
        catch (Exception ex)
        {
            return this.GetResponse(new BaseResponse()
            {
                Message = ex.Message,
                Success = false,
                ResponseCode = (int)HttpStatusCode.InternalServerError
            });
        }
    }

    [HttpPost("delete", Name = "DeleteGuest")]
    public async Task<IActionResult> DeleteGuest([FromBody] DeleteGuest request)
    {
        try
        {
            var result = await _mediator.Send(request);

            return this.GetResponse(result);
        }
        catch (Exception ex)
        {
            return this.GetResponse(new BaseResponse()
            {
                Message = ex.Message,
                Success = false,
                ResponseCode = (int)HttpStatusCode.InternalServerError
            });
        }
    }

    [HttpGet("{GuestId}")]
    public async Task<IActionResult> GetGuestById(string GuestId)
    {

        try
        {
            var result = await _mediator.Send(new GetGuestById()
            {
                Id = GuestId
            });

            return this.GetResponse(result);
        }
        catch (Exception ex)
        {
            return this.GetResponse(new BaseResponse()
            {
                Message = ex.Message,
                Success = false,
                ResponseCode = (int)HttpStatusCode.InternalServerError
            });
        }
    }
    [HttpGet("search/{SortType?}")]
    public async Task<IActionResult> GetGuests(string SortType = "")
    {
        try
        {
            var result = await _mediator.Send(new GetGuests()
            {
                SortType = SortType
            });

            return this.GetResponse(result);
        }
        catch (Exception ex)
        {
            return this.GetResponse(new BaseResponse()
            {
                Message = ex.Message,
                Success = false,
                ResponseCode = (int)HttpStatusCode.InternalServerError
            });
        }
    }
}
