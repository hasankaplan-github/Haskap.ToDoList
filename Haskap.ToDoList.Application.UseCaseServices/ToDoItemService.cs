using AutoMapper;
using Haskap.DddBase.Application.UseCaseServices;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Guids;
using Haskap.ToDoList.Application.UseCaseServices.Contracts;
using Haskap.ToDoList.Application.UseCaseServices.Dtos;
using Haskap.ToDoList.Domain.Core;
using Haskap.ToDoList.Domain.Core.ToDoListAggregate;
using Haskap.ToDoList.Domain.Core.UserAggregate;
using Haskap.ToDoList.Infrastructure.Data.ToDoListDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Application.UseCaseServices;

public class ToDoItemService : UseCaseService, IToDoItemService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    private readonly Guid _ownerUserId;

    public ToDoItemService(AppDbContext appDbContext, IMapper mapper, CurrentUserProvider<User, Guid> currentUserProvider)
    {
        _appDbContext=appDbContext;
        _mapper=mapper;
        _ownerUserId=currentUserProvider.CurrentUser.Id;
    }
    
    public async Task AddToDoItem(ToDoItemInputDto toDoItemInputDto)
    {
        var toDoList = await _appDbContext.ToDoList.FindAsync(toDoItemInputDto.OwnerToDoListId);
        if (toDoList.OwnerUserId != _ownerUserId)
        {
            throw new GeneralException("You are not the owner of this list", System.Net.HttpStatusCode.BadRequest);
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

    public async Task DeleteToDoItem(Guid ownerToDoListId, Guid toDoItemId)
    {
        var toDoList = await _appDbContext.ToDoList
            .Where(x => x.Id == ownerToDoListId)
            .Include(x => x.ToDoItems.Where(y => y.Id == toDoItemId))
            .SingleAsync();

        if (toDoList!.OwnerUserId != _ownerUserId)
        {
            throw new GeneralException("You are not the owner of this list", System.Net.HttpStatusCode.BadRequest);
        }

        var toBeRemoved = toDoList.ToDoItems.Single(x => x.Id == toDoItemId);
        toDoList.RemoveToDoItem(toBeRemoved);

        await _appDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ToDoItemOutputDto>> GetToDoItems(Guid ownerToDoListId)
    {
        var toDoList = await _appDbContext.ToDoList
            .Include(x => x.ToDoItems)
            .Where(x => x.Id == ownerToDoListId)
            .SingleAsync();

        if (toDoList!.OwnerUserId != _ownerUserId)
        {
            throw new GeneralException("You are not the owner of this list", System.Net.HttpStatusCode.BadRequest);
        }

        var outputDto = _mapper.Map<IEnumerable<ToDoItemOutputDto>>(toDoList.ToDoItems);

        return outputDto;
    }

    public async Task MarkToDoItemAsCompleted(MarkAsCompleted_ToDoItemInputDto toDoItemInputDto)
    {
        var toDoList = await _appDbContext.ToDoList
            .Where(x => x.Id == toDoItemInputDto.OwnerToDoListId)
            .Include(x => x.ToDoItems.Where(y => y.Id == toDoItemInputDto.ToDoItemId))
            .SingleAsync();
        if (toDoList!.OwnerUserId != _ownerUserId)
        {
            throw new GeneralException("You are not the owner of this list", System.Net.HttpStatusCode.BadRequest);
        }

        var toDoItem = toDoList.ToDoItems.Single(x => x.Id == toDoItemInputDto.ToDoItemId);
        toDoItem.MarkAsCompleted();

        await _appDbContext.SaveChangesAsync();
    }

    public async Task MarkToDoItemAsNotCompleted(MarkAsNotCompleted_ToDoItemInputDto toDoItemInputDto)
    {
        var toDoList = await _appDbContext.ToDoList
            .Where(x => x.Id == toDoItemInputDto.OwnerToDoListId)
            .Include(x => x.ToDoItems.Where(y => y.Id == toDoItemInputDto.ToDoItemId))
            .SingleAsync();
        if (toDoList!.OwnerUserId != _ownerUserId)
        {
            throw new GeneralException("You are not the owner of this list", System.Net.HttpStatusCode.BadRequest);
        }

        var toDoItem = toDoList.ToDoItems.Single(x => x.Id == toDoItemInputDto.ToDoItemId);
        toDoItem.MarkAsNotCompleted();

        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateToDoItem(Guid toDoItemId, ToDoItemInputDto toDoItemInputDto)
    {
        var toDoList = await _appDbContext.ToDoList
            .Include(x => x.ToDoItems.Where(y => y.Id == toDoItemId))
            .Where(x => x.Id == toDoItemInputDto.OwnerToDoListId)
            .SingleAsync();
        if (toDoList.OwnerUserId != _ownerUserId)
        {
            throw new GeneralException("You are not the owner of this list", System.Net.HttpStatusCode.BadRequest);
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
}
