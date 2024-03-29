using System.Reflection;
using WebApiTemplate.Application.Common.Security;

namespace WebApiTemplate.Application.Common.Behaviors;

public class AuthorizationBehaviour<TRequest, TResponse>()
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        return await next();
    }
}
