using Haskap.DddBase.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Haskap.ToDoList.Domain.Core;

public abstract class Entity : Entity<Guid>, IEntity
{
    protected Entity()
    {

    }

    protected Entity(Guid id)
        : base(id)
    {

    }
}
