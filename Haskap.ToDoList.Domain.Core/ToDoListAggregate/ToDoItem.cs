using Ardalis.GuardClauses;
using Haskap.DddBase.Domain.Core;
using Haskap.ToDoList.Domain.Core.ToDoListAggregate.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Domain.Core.ToDoListAggregate;

public class ToDoItem : Entity
{
    public string Content { get; private set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; private set; }
    public Guid OwnerToDoListId { get; private set; }
    public bool IsOverDue => DueDate.HasValue && DueDate.Value > DateTime.Now;
    public DateTime DateAdded { get; private set; }


    private ToDoItem()
    {
    }
    
    public ToDoItem(Guid id, string content, DateTime? dueDate, bool isCompleted)
        : base(id)
    {
        SetContent(content);

        DateAdded = DateTime.Now;
        DueDate=dueDate;
        IsCompleted =isCompleted;
    }

    public void SetContent(string content)
    {
        Guard.Against.NullOrWhiteSpace(content);
        Guard.Against.TooLongContent(content);
        Content = content;
    }

    public void MarkAsCompleted()
    {
        if (IsCompleted == false)
        {
            IsCompleted = true;
            // MediatorWrapper.Publish(new ToDoItemMarkedAsCompletedDomainEvent()).GetAwaiter().GetResult();
        }
    }

    public void MarkAsNotCompleted()
    {
        if (IsCompleted == true)
        {
            IsCompleted = false;
            // MediatorWrapper.Publish(new ToDoItemMarkedAsNotCompletedDomainEvent()).GetAwaiter().GetResult();            
        }
    }
}
