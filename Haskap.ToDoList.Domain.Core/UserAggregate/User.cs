namespace Haskap.ToDoList.Domain.Core.UserAggregate;

public class User : AggregateRoot
{
    public Name Name { get; set; }
    public UserName UserName { get; set; }
    public Password Password { get; set; }

    public User(Guid id)
        : base(id)
    {

    }
}
