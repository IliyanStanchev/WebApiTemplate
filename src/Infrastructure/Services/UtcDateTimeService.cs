using WebApiTemplate.Application.Interfaces;

namespace WebApiTemplate.Infrastructure.Services;

public class UtcDateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}
