using Microsoft.AspNetCore.Mvc;
using TimeTracking.API.Mapping;
using TimeTracking.Application.Repositories;
using TimeTracking.Contracts.Requests;

namespace TimeTracking.API.Controllers;

[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IClientRepository _clientRepo;

    public ClientsController(IClientRepository clientRepo)
    {
        _clientRepo = clientRepo;
    }

    [HttpPost(ApiEndpoints.Clients.Create)]
    public async Task<IActionResult> Create([FromBody] CreateClientRequest request)
    {
        var client = request.MapToClient();
        await _clientRepo.CreateAsync(client);
        return CreatedAtAction(nameof(Get), new { idOrSlug = client.Id }, client.MapToResponse());
    }

    [HttpGet(ApiEndpoints.Clients.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug)
    {
        var client = Guid.TryParse(idOrSlug, out var guid)
            ? await _clientRepo.GetByIdAsync(guid)
            : await _clientRepo.GetBySlugAsync(idOrSlug);

        if (client is null)
        {
            return NotFound();
        }

        return Ok(client.MapToResponse());
    }

    [HttpGet(ApiEndpoints.Clients.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var clients = await _clientRepo.GetAllAsync();
        return Ok(clients.MapToResponse());
    }

    [HttpPut(ApiEndpoints.Clients.Update)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateClientRequest request)
    {
        var client = request.MapToClient(id);
        var updated = await _clientRepo.UpdateAsync(client);

        if (!updated)
        {
            return NotFound();
        }

        return Ok(client.MapToResponse());
    }

    [HttpDelete(ApiEndpoints.Clients.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _clientRepo.DeleteByIdAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}