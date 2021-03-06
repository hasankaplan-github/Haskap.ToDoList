using AutoMapper;
using Haskap.DddBase.Application.UseCaseServices;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Guids;
using Haskap.ToDoList.Application.Contracts;
using Haskap.ToDoList.Application.Dtos;
using Haskap.ToDoList.Application.Dtos.DataTable;
using Haskap.ToDoList.Domain.Core;
using Haskap.ToDoList.Domain.Core.ToDoListAggregate;
using Haskap.ToDoList.Domain.Core.ToDoListAggregate.Exceptions;
using Haskap.ToDoList.Domain.Core.UserAggregate;
using Haskap.ToDoList.Infrastructure.Data.ToDoListDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        _ownerUserId=currentUserProvider.CurrentUser==null ? Guid.Empty : currentUserProvider.CurrentUser.Id;
    }

    public async Task AddToDoList(ToDoListInputDto toDoListInputDto)
    {
        var toDoList = new Domain.Core.ToDoListAggregate.ToDoList(
            GuidGenerator.CreateSimpleGuid(), 
            _ownerUserId, 
            toDoListInputDto.Name
            );
        await _appDbContext.ToDoList.AddAsync(toDoList);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task DeleteToDoList(Guid toDoListId)
    {
        var toDoList = await _appDbContext.ToDoList.FindAsync(toDoListId);
        if (toDoList.OwnerUserId != _ownerUserId)
        {
            throw new YouAreNotOwnerOfThisListException();
        }

        _appDbContext.ToDoList.Remove(toDoList);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task UpdateToDoList(Guid toDoListId, ToDoListInputDto toDoListInputDto)
    {
        var toDoList = await _appDbContext.ToDoList.FindAsync(toDoListId);
        if (toDoList.OwnerUserId != _ownerUserId)
        {
            throw new YouAreNotOwnerOfThisListException();
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
            throw new YouAreNotOwnerOfThisListException();
        }
        
        toDoList.MarkAsCompleted();
        
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<JqueryDataTableResult> GetToDoLists(JqueryDataTableParam jqueryDataTableParam)
    {
        var searchBy = jqueryDataTableParam.Search?.Value;
        var skip = jqueryDataTableParam.Start;
        var take = jqueryDataTableParam.Length;

        var toDoListsQuery = _appDbContext.ToDoList
            .Where(x => x.OwnerUserId == _ownerUserId);

        var totalCount = await toDoListsQuery.CountAsync();
        var filteredCount = totalCount;

        var outputDto = new List<ToDoListOutputDto>();
        if (skip > -1 && take > 0)
        {
            var toDoLists = await toDoListsQuery
            .Include(x => x.ToDoItems)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            outputDto = _mapper.Map<List<ToDoListOutputDto>>(toDoLists);
        }

        return new JqueryDataTableResult
        {
            // this is what datatables wants sending back
            draw = jqueryDataTableParam.Draw,
            recordsTotal = totalCount,
            recordsFiltered = filteredCount,
            data = outputDto
        };
    }
}
