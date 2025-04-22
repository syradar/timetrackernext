using FluentValidation;
using TimeTracking.Application.Models;
using TimeTracking.Application.Repositories;

namespace TimeTracking.Application.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly IValidator<Client> _clientValidator;

    public ClientService(IClientRepository clientRepository, IValidator<Client> clientValidator)
    {
        _clientRepository = clientRepository;
        _clientValidator = clientValidator;
    }

    public async Task<bool> CreateAsync(Client client)
    {
        await _clientValidator.ValidateAndThrowAsync(client);
        return await _clientRepository.CreateAsync(client);
    }

    public Task<Client?> GetByIdAsync(Guid id)
    {
        return _clientRepository.GetByIdAsync(id);
    }

    public Task<Client?> GetBySlugAsync(string slug)
    {
        return _clientRepository.GetBySlugAsync(slug);
    }

    public Task<IEnumerable<Client>> GetAllAsync()
    {
        return _clientRepository.GetAllAsync();
    }

    public async Task<Client?> UpdateAsync(Client client)
    {
        await _clientValidator.ValidateAndThrowAsync(client);
        var clientExists = await _clientRepository.ExistsByIdAsync(client.Id);

        if (!clientExists)
        {
            return null;
        }

        return client;
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        return _clientRepository.DeleteByIdAsync(id);
    }
}