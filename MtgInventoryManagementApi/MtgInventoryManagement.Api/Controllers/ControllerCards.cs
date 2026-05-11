using Microsoft.AspNetCore.Mvc;
using MtgInventoryManagementApi.MtgInventoryManagement.Service.Scryfall;

namespace MtgInventoryManagement.Api.Controller;

[ApiController]
[Route("api/v1/[controller]")]
public class CardsController : ControllerBase
{
    private readonly IDatabaseDownloader _databaseDownloader;

    public CardsController(IDatabaseDownloader databaseDownloader)
    {
        _databaseDownloader = databaseDownloader;
    }

    [HttpPost("update-from-scryfall")]
    public async Task<IActionResult> UpdateAllCardsFromScryfall(CancellationToken cancellationToken)
    {
        await _databaseDownloader.UpdateCardsFromScryfall(cancellationToken);

        return Ok();
    }
}
