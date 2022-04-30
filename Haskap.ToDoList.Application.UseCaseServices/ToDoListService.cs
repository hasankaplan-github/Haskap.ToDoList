﻿using AutoMapper;
using Haskap.DddBase.Application.UseCaseServices;
using Haskap.DddBase.Utilities.Guids;
using Haskap.ToDoList.Application.UseCaseServices.Contracts;
using Haskap.ToDoList.Application.UseCaseServices.Dtos;
using Haskap.ToDoList.Domain.Core.ToDoListAggregate;
using Haskap.ToDoList.Infrastructure.Data.ToDoListDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Application.UseCaseServices;

public class ToDoListService : UseCaseService, IToDoListService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public ToDoListService(AppDbContext appDbContext, IMapper mapper)
    {
        _appDbContext=appDbContext;
        _mapper=mapper;
    }

    public async Task AddToDoItem(Guid ownerUserId, Guid toDoListId, ToDoItemInputDto toDoItemInputDto)
    {
        var toDoList = await _appDbContext.ToDoList.FindAsync(toDoListId);
        if (toDoList.OwnerUserId != ownerUserId)
        {
            throw new Exception("You are not the owner of this list");
        }

        var toDoItem = new ToDoItem(
            GuidGenerator.CreateSimpleGuid(), 
            toDoItemInputDto.Content, 
            toDoItemInputDto.DueDate,
            toDoItemInputDto.IsCompleted
            );

        toDoList.AddToDoItem(toDoItem);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateToDoItem(Guid ownerUserId, Guid toDoListId, Guid toDoItemId, ToDoItemInputDto toDoItemInputDto)
    {
        var toDoList = await _appDbContext.ToDoList.FindAsync(toDoListId);
        if (toDoList.OwnerUserId != ownerUserId)
        {
            throw new Exception("You are not the owner of this list");
        }

        var toDoItem = toDoList.ToDoItems.Single(x => x.Id == toDoItemId);
        toDoItem.SetContent(toDoItemInputDto.Content);
        toDoItem.DueDate = toDoItemInputDto.DueDate;
        if (toDoItemInputDto.IsCompleted && toDoItem.IsCompleted == false)
        {
            toDoItem.MarkAsCompleted();
        }
        else if (!toDoItemInputDto.IsCompleted && toDoItem.IsCompleted == true)
        {
            toDoItem.MarkAsNotCompleted();
        }

        await _appDbContext.SaveChangesAsync();
    }

    public async Task AddToDoList(Guid ownerUserId, string name)
    {
        var toDoList = new Domain.Core.ToDoListAggregate.ToDoList(ownerUserId, name);
        await _appDbContext.ToDoList.AddAsync(toDoList);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task DeleteToDoItem(Guid ownerUserId, Guid toDoListId, Guid toDoItemId)
    {
        var toDoList = await _appDbContext.ToDoList
            .Where(x => x.Id == toDoListId)
            .Include(x => x.ToDoItems.Where(y => y.Id == toDoItemId))
            .SingleAsync();
        
        if (toDoList!.OwnerUserId != ownerUserId)
        {
            throw new Exception("You are not the owner of this list");
        }

        var toBeRemoved = toDoList.ToDoItems.Single(x => x.Id == toDoItemId);
        toDoList.RemoveToDoItem(toBeRemoved);
        
        await _appDbContext.SaveChangesAsync();
    }

    public async Task DeleteToDoList(Guid ownerUserId, Guid toDoListId)
    {
        var toDoList = await _appDbContext.ToDoList.FindAsync(toDoListId);
        if (toDoList.OwnerUserId != ownerUserId)
        {
            throw new Exception("You are not the owner of this list");
        }

        _appDbContext.ToDoList.Remove(toDoList);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateToDoList(Guid ownerUserId, Guid toDoListId, string name)
    {
        var toDoList = await _appDbContext.ToDoList.FindAsync(toDoListId);
        if (toDoList.OwnerUserId != ownerUserId)
        {
            throw new Exception("You are not the owner of this list");
        }

        toDoList.SetName(name);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task MarkToDoListAsCompleted(Guid ownerUserId, Guid toDoListId)
    {
        var toDoList = await _appDbContext.ToDoList
            .Where(x => x.Id == toDoListId)
            .Include(x => x.ToDoItems)
            .SingleAsync();
        if (toDoList!.OwnerUserId != ownerUserId)
        {
            throw new Exception("You are not the owner of this list");
        }

        foreach (var toDoItem in toDoList.ToDoItems)
        {
            toDoItem.MarkAsCompleted();
        }
        
        await _appDbContext.SaveChangesAsync();
    }

    public async Task MarkToDoItemAsCompleted(Guid ownerUserId, Guid toDoListId, Guid toDoItemId)
    {
        var toDoList = await _appDbContext.ToDoList
            .Where(x => x.Id == toDoListId)
            .Include(x => x.ToDoItems.Where(y => y.Id == toDoItemId))
            .SingleAsync();
        if (toDoList!.OwnerUserId != ownerUserId)
        {
            throw new Exception("You are not the owner of this list");
        }

        var toDoItem = toDoList.ToDoItems.Single(x => x.Id == toDoItemId);
        toDoItem.MarkAsCompleted();

        await _appDbContext.SaveChangesAsync();
    }

    public async Task MarkToDoItemAsNotCompleted(Guid ownerUserId, Guid toDoListId, Guid toDoItemId)
    {
        var toDoList = await _appDbContext.ToDoList
            .Where(x => x.Id == toDoListId)
            .Include(x => x.ToDoItems.Where(y => y.Id == toDoItemId))
            .SingleAsync();
        if (toDoList!.OwnerUserId != ownerUserId)
        {
            throw new Exception("You are not the owner of this list");
        }

        var toDoItem = toDoList.ToDoItems.Single(x => x.Id == toDoItemId);
        toDoItem.MarkAsNotCompleted();

        await _appDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<GetToDoLists_ToDoListOutputDto>> GetToDoLists(Guid ownerUserId)
    {
        var toDoLists = await _appDbContext.ToDoList
            .Where(x => x.OwnerUserId == ownerUserId)
            .ToListAsync();

        var outputDto = _mapper.Map<IEnumerable<GetToDoLists_ToDoListOutputDto>>(toDoLists);

        return outputDto;
    }

    public async Task<IEnumerable<GetToDoItems_ToDoItemOutputDto>> GetToDoItems(Guid ownerUserId, Guid toDoListId)
    {
        var toDoList = await _appDbContext.ToDoList
            .Include(x => x.ToDoItems)
            .Where(x => x.Id == toDoListId)
            .SingleAsync();

        if (toDoList!.OwnerUserId != ownerUserId)
        {
            throw new Exception("You are not the owner of this list");
        }

        var outputDto = _mapper.Map<IEnumerable<GetToDoItems_ToDoItemOutputDto>>(toDoList.ToDoItems);

        return outputDto;
    }
}