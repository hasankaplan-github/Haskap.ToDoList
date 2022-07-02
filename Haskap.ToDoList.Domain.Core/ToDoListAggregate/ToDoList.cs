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
    private List<ToDoItem> _toDoItems;
    public IReadOnlyCollection<ToDoItem> ToDoItems => _toDoItems.AsReadOnly();
    public Guid OwnerUserId { get; private set; }
    public bool IsCompleted 
    { 
        get 
        {
            return _toDoItems.All(x => x.IsCompleted);
        } 
    }


    private ToDoList()
    {
    }

    public ToDoList(Guid id, Guid ownerUserId, string name)
        : base(id)
    {
        _toDoItems = new List<ToDoItem>();

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
        _toDoItems.Add(toDoItem);
    }

    public void RemoveToDoItem(ToDoItem toDoItem)
    {
        Guard.Against.Null(toDoItem);
        _toDoItems.Remove(toDoItem);
    }

    public void MarkAsCompleted()
    {
        foreach (var toDoItem in _toDoItems)
        {
            toDoItem.MarkAsCompleted();            
        } 
    }
}
