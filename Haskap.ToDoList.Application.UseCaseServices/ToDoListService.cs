using AutoMapper;
using Haskap.DddBase.Application.UseCaseServices;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Guids;
using Haskap.ToDoList.Application.UseCaseServices.Contracts;
using Haskap.ToDoList.Application.UseCaseServices.Dtos;
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

public class ToDoListService : UseCaseService, IToDoListService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;
    private readonly Guid _ownerUserId;

    public ToDoListService(AppDbContext appDbContext, IMapper mapper, CurrentUserProvider<User, Guid> currentUserProvider)
    {
        _appDbContext=appDbContext;
        _mapper=mapper;
        _ownerUserId=currentUserProvider.CurrentUser.Id;
    }

    public async Task AddToDoList(ToDoListInputDto toDoListInputDto)
    {
        var toDoList = new Domain.Core.ToDoListAggregate.ToDoList(_ownerUserId, toDoListInputDto.Name);
        await _appDbContext.ToDoList.AddAsync(toDoList);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task DeleteToDoList(Guid toDoListId)
    {
        var toDoList = await _appDbContext.ToDoList.FindAsync(toDoListId);
        if (toDoList.OwnerUserId != _ownerUserId)
        {
            throw new Exception("You are not the owner of this list");
        }

        _appDbContext.ToDoList.Remove(toDoList);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateToDoList(Guid toDoListId, ToDoListInputDto toDoListInputDto)
    {
        var toDoList = await _appDbContext.ToDoList.FindAsync(toDoListId);
        if (toDoList.OwnerUserId != _ownerUserId)
        {
            throw new Exception("You are not the owner of this list");
        }

        toDoList.SetName(toDoListInputDto.Name);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task MarkToDoListAsCompleted(MarkAsCompleted_ToDoListInputDto toDoListInputDto)
    {
        var toDoList = await _appDbContext.ToDoList
            .Where(x => x.Id == toDoListInputDto.ToDoListId)
            .Include(x => x.ToDoItems)
            .SingleAsync();
        if (toDoList!.OwnerUserId != _ownerUserId)
        {
            throw new Exception("You are not the owner of this list");
        }

        foreach (var toDoItem in toDoList.ToDoItems)
        {
            toDoItem.MarkAsCompleted();
        }
        
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ToDoListOutputDto>> GetToDoLists()
    {
        var toDoLists = await _appDbContext.ToDoList
            .Include(x => x.ToDoItems)
            .Where(x => x.OwnerUserId == _ownerUserId)
            .ToListAsync();

        var outputDto = _mapper.Map<IEnumerable<ToDoListOutputDto>>(toDoLists);

        return outputDto;
    }

    
}
