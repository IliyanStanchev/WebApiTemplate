namespace WebApiTemplate.Application.Commands.DummyEntity.Queries.GetDummyEntityWithPagination;

public class DummyEntityDto
{
    public int Id { get; init; }

    public int ListId { get; init; }

    public string? Title { get; init; }

    public bool Done { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.DummyEntity, DummyEntityDto>();
        }
    }
}
