using Microsoft.AspNetCore.Mvc;
using TimeTracking.API.Mapping;
using TimeTracking.Application.Services;
using TimeTracking.Contracts.Requests;

namespace TimeTracking.API.Controllers;

[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpPost(ApiEndpoints.Clients.Create)]
    public async Task<IActionResult> Create([FromBody] CreateClientRequest request, CancellationToken token)
    {
        var client = request.MapToClient();
        await _clientService.CreateAsync(client, token);
        return CreatedAtAction(nameof(Get), new { idOrSlug = client.Id }, client.MapToResponse());
    }

    [HttpGet(ApiEndpoints.Clients.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken token)
    {
        var client = Guid.TryParse(idOrSlug, out var guid)
            ? await _clientService.GetByIdAsync(guid, token)
            : await _clientService.GetBySlugAsync(idOrSlug, token);

        if (client is null)
        {
            return NotFound();
        }

        return Ok(client.MapToResponse());
    }

    [HttpGet(ApiEndpoints.Clients.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token = default)
    {
        var clients = await _clientService.GetAllAsync(token);
        return Ok(clients.MapToResponse());
    }

    [HttpPut(ApiEndpoints.Clients.Update)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateClientRequest request, CancellationToken token)
    {
        var client = request.MapToClient(id);
        var updatedClient = await _clientService.UpdateAsync(client, token);

        if (updatedClient is null)
        {
            return NotFound();
        }

        return Ok(client.MapToResponse());
    }

    [HttpDelete(ApiEndpoints.Clients.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        var deleted = await _clientService.DeleteByIdAsync(id, token);

        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}