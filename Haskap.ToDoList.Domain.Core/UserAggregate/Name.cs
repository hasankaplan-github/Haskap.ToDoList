using Ardalis.GuardClauses;
using Haskap.DddBase.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Domain.Core.UserAggregate;

public class Name : ValueObject
{
    public string FirstName { get; private set; }
    public string? MiddleName { get; private set; }
    public string LastName { get; private set; }

    private Name()
    {
    }
    
    public Name(string firstName, string? middleName, string lastName)
    {
        Guard.Against.NullOrWhiteSpace(firstName, nameof(firstName));
        Guard.Against.NullOrWhiteSpace(lastName, nameof(lastName));
        
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return MiddleName;
        yield return LastName;
    }
}
