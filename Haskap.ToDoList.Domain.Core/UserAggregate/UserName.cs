using Ardalis.GuardClauses;
using Haskap.DddBase.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Domain.Core.UserAggregate;

public class UserName : ValueObject
{
    public string Value { get; private set; }

    private UserName()
    {
    }
    
    public UserName(string value)
    {
        Guard.Against.NullOrWhiteSpace(value, nameof(value));

        Value = value;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
