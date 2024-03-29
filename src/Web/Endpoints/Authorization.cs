using WebApiTemplate.Application.Commands.AuthorizationToken.Queries.GetAuthorizationToken;

namespace WebApiTemplate.Web.Endpoints;

public class Authorization : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GenerateAuthorizationToken);
    }

    public async Task<string> GenerateAuthorizationToken(ISender sender)
    {
        return await sender.Send(new GetAuthorizationTokenQuery());
    }
}
