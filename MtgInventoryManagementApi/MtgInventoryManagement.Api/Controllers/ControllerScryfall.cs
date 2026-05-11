using Microsoft.AspNetCore.Mvc;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Scryfall;

namespace MtgInventoryManagement.Api.Controller;

[ApiController]
[Route("api/v1/[controller]")]
public class ScryfallController : ControllerBase
{ 
	private readonly IDatabaseDownloader _databaseDownloader;

	public ScryfallController(IDatabaseDownloader databaseDownloader)
	{
		_databaseDownloader = databaseDownloader;
	}

	[HttpPost("download")]
	public async Task<IActionResult> DownloadCards(CancellationToken cancellationToken)
	{
		await _databaseDownloader.UpdateCardsFromScryfall(cancellationToken);

		return Ok();
	}
}
