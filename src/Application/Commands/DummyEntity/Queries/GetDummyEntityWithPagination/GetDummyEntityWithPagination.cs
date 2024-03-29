using WebApiTemplate.Application.Common.Models;

namespace WebApiTemplate.Application.Commands.DummyEntity.Queries.GetDummyEntityWithPagination;

public record GetDummyEntityWithPaginationQuery : IRequest<PaginatedList<DummyEntityDto>>
{
    public int ListId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetDummyEntityWithPaginationQueryHandler(IMapper mapper) : IRequestHandler<GetDummyEntityWithPaginationQuery, PaginatedList<DummyEntityDto>>
{
    public async Task<PaginatedList<DummyEntityDto>> Handle(GetDummyEntityWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var dummyEntity = new Domain.Entities.DummyEntity { Id = 1, ListId = 1, Title = "Do this thing" };
        var dummyEntityDto = mapper.Map<DummyEntityDto>(dummyEntity);

        var result = await Task.FromResult(new List<DummyEntityDto>
        {
            dummyEntityDto
        });

        return new PaginatedList<DummyEntityDto>(result, result.Count, request.PageNumber, request.PageSize);
    }
}
