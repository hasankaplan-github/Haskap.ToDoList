using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Domain.Core.ToDoListAggregate.GuardClauses;

public static partial class ToDoListGuardClauses
{
    public static string TooLongName(this IGuardClause guardClause, string name, string? parameterName = null, string? message = null)
    {
        if (name.Length > 250)
        {
            throw new ArgumentException(message ?? $"{parameterName ?? "Content"} is too long", parameterName);
        }

        return name;
    }
}
