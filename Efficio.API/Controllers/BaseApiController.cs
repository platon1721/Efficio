using Microsoft.AspNetCore.Mvc;

namespace Efficio.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    // Ühised meetodid kõigile kontrolleritele võib siia lisada
}