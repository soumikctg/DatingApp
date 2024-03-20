using MediatR;
using Microsoft.AspNetCore.Mvc;
using User.Contracts.Dtos;

namespace UserAPI.Commands
{
    public class PhotoAddCommand:IRequest
    {
        public IFormFile File { get; set; }
    }
}
