using FluentValidation;
using TimeTracking.Application.Models;
using TimeTracking.Application.Repositories;

namespace TimeTracking.Application.Validators;

public class ClientValidator : AbstractValidator<Client>
{
    private readonly IClientRepository _clientRepository;

    public ClientValidator(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;

        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Slug)
            .MustAsync(ValidateSlug)
            .WithMessage("Slug must be unique");
    }

    private async Task<bool> ValidateSlug(Client client, string slug, CancellationToken cancellationToken = default)
    {
        var existingClient = await _clientRepository.GetBySlugAsync(slug);

        if (existingClient is not null)
        {
            return existingClient.Id == client.Id;
        }

        return existingClient is null;
    }
}