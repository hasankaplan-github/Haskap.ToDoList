using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Application.UseCaseServices.Dtos.Mappings
{
    internal class ToDoItemProfile : Profile
    {
        public ToDoItemProfile()
        {
            CreateMap<Domain.Core.ToDoListAggregate.ToDoItem, ToDoItemOutputDto>();
        }
    }
}
