using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Contracts.Dtos;

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
