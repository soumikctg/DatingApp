using AutoMapper;
using MessageAPI.DTOs;
using MessageAPI.Models;


namespace MessageAPI.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Message, MessageDto>();

        CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        CreateMap<DateTime?, DateTime?>().ConvertUsing(d =>
            d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);

    }
}