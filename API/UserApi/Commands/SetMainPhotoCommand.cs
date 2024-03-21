using MediatR;

namespace UserAPI.Commands;

public class SetMainPhotoCommand: IRequest
{
    public int PhotoId { get; set; }
}