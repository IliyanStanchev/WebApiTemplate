using WebApiTemplate.Application.Commands.DummyEntity.Commands.CreateDummyEntity;
using WebApiTemplate.Application.Commands.DummyEntity.Queries.GetDummyEntityWithPagination;
using WebApiTemplate.Application.Common.Models;

namespace WebApiTemplate.Web.Endpoints;

public class DummyEntity : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
           .RequireAuthorization()
           .MapGet(GetDummyEntitiesWithPagination)
           .MapPost(CreateDummyEntity);
    }

    public Task<PaginatedList<DummyEntityDto>> GetDummyEntitiesWithPagination(ISender sender, [AsParameters] GetDummyEntityWithPaginationQuery query)
    {
        return sender.Send(query);
    }

    public Task<int> CreateDummyEntity(ISender sender, CreateDummyEntityCommand command)
    {
        return sender.Send(command);
    }
}
