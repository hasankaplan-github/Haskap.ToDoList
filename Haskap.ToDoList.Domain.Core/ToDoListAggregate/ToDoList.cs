using Ardalis.GuardClauses;
using Haskap.DddBase.Domain.Core;
using Haskap.ToDoList.Domain.Core.ToDoListAggregate.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Domain.Core.ToDoListAggregate;

public class ToDoList : AggregateRoot
{
    public string Name { get; private set; }
    private List<ToDoItem> toDoItems;
    public IReadOnlyCollection<ToDoItem> ToDoItems => toDoItems.AsReadOnly();
    public Guid OwnerUserId { get; private set; }
    public bool IsCompleted 
    { 
        get 
        {
            return ToDoItems.All(x => x.IsCompleted);
        } 
    }


    private ToDoList()
    {
        toDoItems = new List<ToDoItem>();
    }

    public ToDoList(Guid ownerUserId, string name)
        : this()
    {
        SetName(name);
        OwnerUserId = ownerUserId;
    }

    public void SetName(string name)
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.TooLongName(name);

        Name = name;
    }

    public void AddToDoItem(ToDoItem toDoItem)
    {
        Guard.Against.Null(toDoItem);
        toDoItems.Add(toDoItem);
    }

    public void RemoveToDoItem(ToDoItem toDoItem)
    {
        Guard.Against.Null(toDoItem);
        toDoItems.Remove(toDoItem);
    }
}
