using DatingApp.Shared.Queries;
using MassTransit;
using UserAPI.Interfaces;

namespace UserAPI.QueryHandlers
{
    public class MemberQueryConsumer : IConsumer<MemberQuery>
    {
        private readonly IUserRepository _userRepository;

        public MemberQueryConsumer(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Consume(ConsumeContext<MemberQuery> context)
        {
            var memberQuery = context.Message;

            var memberDto = await _userRepository.GetMemberAsync(memberQuery.MemberName);

            await context.RespondAsync(new MemberQueryResponse
            {
                Member = memberDto
            });
        }
    }
}
