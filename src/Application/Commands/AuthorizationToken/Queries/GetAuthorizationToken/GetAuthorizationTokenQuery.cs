using WebApiTemplate.Application.Interfaces;

namespace WebApiTemplate.Application.Commands.AuthorizationToken.Queries.GetAuthorizationToken;

public record GetAuthorizationTokenQuery : IRequest<string>;

public class GetAuthorizationTokenQueryHandler(IAuthorizationProvider authorizationProvider) : IRequestHandler<GetAuthorizationTokenQuery, string>
{
    public async Task<string> Handle(GetAuthorizationTokenQuery request, CancellationToken cancellationToken)
    {
        return await authorizationProvider.GenerateTokenAsync();
    }
}
