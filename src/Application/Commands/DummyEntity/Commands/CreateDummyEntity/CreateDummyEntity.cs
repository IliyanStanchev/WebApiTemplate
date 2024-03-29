using Microsoft.Extensions.Logging;
using WebApiTemplate.Application.Repositories;
using WebApiTemplate.Domain.Events;

namespace WebApiTemplate.Application.Commands.DummyEntity.Commands.CreateDummyEntity;

public record CreateDummyEntityCommand : IRequest<int>
{
    public int Id { get; init; }

    public string? Title { get; init; }
}

public class CreateDummyEntityCommandHandler(IDummyEntityRepository dummyEntityRepository, ILogger<CreateDummyEntityCommand> logger) : IRequestHandler<CreateDummyEntityCommand, int>
{
    public async Task<int> Handle(CreateDummyEntityCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = new Domain.Entities.DummyEntity() { ListId = request.Id, Title = request.Title, };

            entity.AddDomainEvent(new DummyEntityCreatedEvent(entity));

            var result = await dummyEntityRepository.AddAsync(entity, cancellationToken);

            return result.Id;
        }
        catch (Exception ex)
        {
            logger.LogError($"CreateDummyEntityCommandHandler: {ex.Message}");
            return 0;
        }
    }
}
