using Microsoft.AspNetCore.Mvc;

namespace MtgInventoryManagement.Api.Controller;

[ApiController]
[Route("api/magic")]
public class MagicController : ControllerBase
{
	[HttpGet("types")]
	public IActionResult GetTypes()
	{
		return Ok(new[]
		{
			"Sorcery", "Creature", "Land", "Instant"
		});
	}
}
