using System;
using System.Collections.Generic;
using System.Text;

namespace SourcePoint.Core.Options
{
    public class IdentityAuthOptions
    {
        public string BaseUrl { get; set; }
        public string Name { get; set; }
        public string Secret { get; set; }
        public string Scope { get; set; }
    }
    public class AuthClient : IdentityAuthOptions
    {

    }
    public class AuthToken : IdentityAuthOptions
    {

    }
}
