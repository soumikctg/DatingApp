using DatingApp.Shared.Dtos;
using DatingApp.Shared.Queries;
using MassTransit;
using MediatR;
using MessageAPI.Queries;

namespace MessageAPI.QueryHandlers
{
    public class GetUserByNameQueryHandler:IRequestHandler<GetUserByNameQuery, MemberDto>
    {
        private readonly IScopedClientFactory _scopedClientFactory;
        public GetUserByNameQueryHandler(IScopedClientFactory scopedClientFactory)
        {
            _scopedClientFactory = scopedClientFactory;
        }

        public async Task<MemberDto> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
        {
            var requestClient = _scopedClientFactory.CreateRequestClient<MemberQuery>();

            var memberQueryResponse = await requestClient.GetResponse<MemberQueryResponse>(new MemberQuery
            {
                MemberName = request.UserName

            });

            var user = memberQueryResponse.Message.Member;
            return user;
        }
    }
}
