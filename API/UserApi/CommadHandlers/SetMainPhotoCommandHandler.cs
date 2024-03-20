using MediatR;
using System.Security.Claims;
using UserAPI.Commands;
using UserAPI.Entities;
using UserAPI.Helpers;
using UserAPI.Interfaces;

namespace UserAPI.CommadHandlers
{
    public class SetMainPhotoCommandHandler : IRequestHandler<SetMainPhotoCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _uow;

        public SetMainPhotoCommandHandler(IUserRepository userRepository, IPhotoService photoService, IUnitOfWork uow)
        {
            _uow = uow;
            _photoService = photoService;
            _userRepository = userRepository;
        }

        public async Task Handle(SetMainPhotoCommand request, CancellationToken cancellationToken)
        {
            var username = UserInfoProvider.CurrentUserName();
            var user = await _userRepository.GetUserByUserNameAsync(username);

            if (user == null) throw new Exception("User Not found");

            var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);
            if (photo == null) throw new Exception("Photo Not found");

            if (photo.IsMain) throw new Exception("this is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _uow.SaveChangesAsync() == 0) throw new Exception("Problem setting the main photo");


        }
    }
}
