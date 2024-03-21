using MediatR;

namespace LikeAPI.Commands;

public class AddLikeCommand : IRequest
{
    public string TargetUserName { get; set; }
}