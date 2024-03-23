using MediatR;
using UserAPI.Entities;

namespace UserAPI.Commands;

public class PhotoAddCommand:IRequest<Photo>
{
    public IFormFile File { get; set; }
}