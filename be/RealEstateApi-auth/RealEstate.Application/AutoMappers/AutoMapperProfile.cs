using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RealEstate.Application.Models;

namespace RealEstate.Application.AutoMappers
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Post, PostDto>()
                .ForMember(des => des.ImageUrls, act => act.MapFrom(src => src.Images.Select(x => x.ImageUrl)))
                .ForMember(des => des.CreatorName, act => act.MapFrom(src => src.Creator.Name))
                .ForMember(des => des.CreatorPhone, act => act.MapFrom(src => src.Creator.PhoneNumber))
                .ForMember(des => des.CategoryName, act => act.MapFrom(src => src.Category.Name))
                .ForMember(des => des.PostTypeName, act => act.MapFrom(src => src.PostType.Name));

            CreateMap<Report, ReportResponse>()
                .ForMember(des => des.Name, act => act.MapFrom(src => src.User.Name))
                .ForMember(des => des.UserName, act => act.MapFrom(src => src.User.UserName));

            CreateMap<ReportProcessing, ReportProcessingDto>()
                .ForMember(des => des.Title, act => act.MapFrom(src => src.Post.Title));

            CreateMap<Category, CategoryDto>();

            CreateMap<NewsDto, News>();
            CreateMap<News, NewsDto>()
                .ForMember(des => des.CreatorName, act => act.MapFrom(src => src.Creator.Name));

            CreateMap<PostDto, Post>();
            CreateMap<UserDto, User>();
            CreateMap<UserRequest, User>();
            CreateMap<User, UserDto>()
                .ForMember(des => des.Lock, act => act.MapFrom(src => (src.LockoutEnd == null || src.LockoutEnd < DateTime.Now) ? false : true));
            CreateMap<User, Info>();
            CreateMap<User, UserFollow>();

            CreateMap<RegisterRequest, User>();
            CreateMap<LoginRequest, User>();
        }
    }
}
