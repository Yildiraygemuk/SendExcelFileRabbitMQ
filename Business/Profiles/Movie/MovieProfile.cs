using AutoMapper;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Vms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookVm>()
                  .ForMember(dest => dest.CategoryName,
                    act => act.MapFrom(src => src.Category.CategoryName));
            CreateMap<BookVm, Book>();
            CreateMap<BookDto, Book>();
            CreateMap<Book, BookDto>();
            CreateMap<Book, BookPutDto>();
            CreateMap<BookPutDto, Book>();
        }
    }
}
