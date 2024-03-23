using AutoMapper;
using MediatR;
using UserAPI.Commands;
using UserAPI.Entities;
using UserAPI.Helpers;
using UserAPI.Interfaces;

namespace UserAPI.CommadHandlers;

public class PhotoAddCommandHandler : IRequestHandler<PhotoAddCommand, Photo>
{
    private readonly IUserRepository _userRepository;
    private readonly IPhotoService _photoService;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public PhotoAddCommandHandler(IUserRepository userRepository, IPhotoService photoService, IUnitOfWork uow, IMapper mapper)
    {
        _mapper = mapper;
        _uow = uow;
        _photoService = photoService;
        _userRepository = userRepository;
    }

    public async Task<Photo> Handle(PhotoAddCommand request, CancellationToken cancellationToken)
    {
        var username = UserInfoProvider.CurrentUserName();
        var user = await _userRepository.GetUserByUserNameAsync(username);
        if (user == null) throw new Exception("user not found");

        var result = await _photoService.AddPhotoAsync(request.File);

        if (result.Error != null) throw new Exception(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        if (user.Photos.Count == 0) photo.IsMain = true;

        user.Photos.Add(photo);

        if (await _uow.SaveChangesAsync() == 0) throw new Exception("Photo not uploaded");
        return photo;
    }
}