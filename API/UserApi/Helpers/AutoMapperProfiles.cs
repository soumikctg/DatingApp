﻿using AutoMapper;
using DatingApp.Shared.Dtos;
using UserAPI.DTOs;
using UserAPI.Entities;
using UserAPI.Extensions;

namespace UserAPI.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberDto>()
            .ForMember(dest => dest.PhotoUrl,
                opt => opt.MapFrom(
                    src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.Age,
                opt => opt.MapFrom(
                    src => src.DateOfBirth.CalculateAge()));
        CreateMap<Photo, PhotoDto>();
        CreateMap<MemberUpdateDto, AppUser>();
        CreateMap<RegisterDto, AppUser>();


        CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        CreateMap<DateTime?, DateTime?>().ConvertUsing(d =>
            d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);

    }
}