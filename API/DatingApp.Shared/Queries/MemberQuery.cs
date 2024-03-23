using DatingApp.Shared.Dtos;

namespace DatingApp.Shared.Queries
{
    public class MemberQuery
    {
        public string MemberName { get; set; }
    }

    public class MemberQueryResponse
    {
        public MemberDto Member { get; set; }

    }
}
