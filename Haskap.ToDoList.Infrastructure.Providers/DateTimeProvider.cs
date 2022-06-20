using Haskap.ToDoList.Domain.Providers;

namespace Haskap.ToDoList.Infrastructure.Providers;
public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
