using MediatR;
using System.Security.Claims;
using UserAPI.Commands;
using UserAPI.Entities;
using UserAPI.Helpers;
using UserAPI.Interfaces;

namespace UserAPI.CommadHandlers;

public class DeletePhotoCommandHandler : IRequestHandler<DeletePhotoCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IPhotoService _photoService;
    private readonly IUnitOfWork _uow;

    public DeletePhotoCommandHandler(IUserRepository userRepository, IPhotoService photoService, IUnitOfWork uow)
    {
        _uow = uow;
        _photoService = photoService;
        _userRepository = userRepository;
    }

    public async Task Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
    {
        var username = UserInfoProvider.CurrentUserName();
        var user = await _userRepository.GetUserByUserNameAsync(username);

        var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);

        if (photo == null) throw new Exception("Photo not found");

        if (photo.IsMain) throw new Exception("You cannot delete your main photo");

        if (photo.PublicId != null)
        {
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) throw new Exception(result.Error.Message);
        }

        user.Photos.Remove(photo);

        if (await _uow.SaveChangesAsync() == 0) throw new Exception("Problem Deleting Photo");

    }
}