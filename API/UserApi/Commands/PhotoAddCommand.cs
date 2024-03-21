using MediatR;
using Microsoft.AspNetCore.Mvc;
using User.Contracts.Dtos;
using UserAPI.Entities;

namespace UserAPI.Commands;

public class PhotoAddCommand:IRequest<Photo>
{
    public IFormFile File { get; set; }
}