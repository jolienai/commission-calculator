using System.Net;
using FCamara.Commission.Api.Extensions;
using FCamara.Commission.Application.Sales.Commission;
using FCamara.Commission.Application.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FCamara.Commission.Api.Sales.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CommissionController (IMediator mediator) : ControllerBase
{
    [ProducesResponseType(typeof(CommissionCalculationResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(CommissionCalculationResponse), (int)HttpStatusCode.BadRequest)]
    [HttpPost]
    public async Task<IResult> Calculate(CommissionCalculationRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.SendAsync(request, cancellationToken);
        return response.ToHttpResult();
    }
}