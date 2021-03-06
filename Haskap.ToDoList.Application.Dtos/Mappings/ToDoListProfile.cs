using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haskap.ToDoList.Application.Dtos.Mappings
{
    public class ToDoListProfile : Profile
    {
        public ToDoListProfile()
        {
            CreateMap<Domain.Core.ToDoListAggregate.ToDoList, ToDoListOutputDto>()
                .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.ToDoItems.Count));
        }
    }
}
