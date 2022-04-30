using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Domain.Core.ToDoListAggregate.GuardClauses;

public static partial class ToDoItemGuardClauses
{
    public static string TooLongContent(this IGuardClause guardClause, string content, string? parameterName = null, string? message = null)
    {
        if (content.Length > 1000)
        {
            throw new ArgumentException(message ?? $"{parameterName ?? "Content"} is too long", parameterName);
        }

        return content;
    }
}
