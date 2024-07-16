using MediatR;
using api.Business.Data;
using api.Business.Commands;
using api.Business.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{

    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("create",Name = "CreateAccount")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccount request)
    {
        try
        {
            var result = await _mediator.Send(new CreateAccount()
            {
                Username = request.Username,
                Password = request.Password
            });

            if (string.IsNullOrEmpty(result.Id))
            {
                throw new BadHttpRequestException("Invalid Registration");
            }

            _httpContextAccessor?.HttpContext?.Session.SetString("AccountId", result.Id);

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
    [HttpPost("",Name = "GetAccount")]
    public async Task<IActionResult> GetAccount([FromBody] GetAccount request)
    {
        try
        {
            var result = await _mediator.Send(new GetAccount()
            {
                Username = request.Username,
                Password = request.Password
            });

            if (string.IsNullOrEmpty(result.Id))
            {
                throw new BadHttpRequestException("Invalid Account");
            }

            _httpContextAccessor?.HttpContext?.Session.SetString("AccountId", result.Id);

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
