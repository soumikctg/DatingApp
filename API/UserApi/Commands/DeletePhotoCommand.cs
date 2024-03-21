using MediatR;

namespace UserAPI.Commands;

public class DeletePhotoCommand:IRequest
{
    public int PhotoId { get; set; }
}