using api.DTOs;
using api.Entities;
using AutoMapper;

namespace api.Middleware {
    public class AutoMapperProfiles : Profile {
        public AutoMapperProfiles() {
            CreateMap<AppUser, UserDto>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<ArticleDto, Article>();
            CreateMap<ArticleUpdateDto, Article>();
        }
    }
}