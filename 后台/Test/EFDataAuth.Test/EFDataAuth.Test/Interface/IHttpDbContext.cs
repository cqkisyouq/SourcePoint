using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EFDataAuth.Test.Interface
{
    public interface IHttpDbContext
    {
        IHttpContextAccessor httpContextAccessor { get; set; }
    }
}
