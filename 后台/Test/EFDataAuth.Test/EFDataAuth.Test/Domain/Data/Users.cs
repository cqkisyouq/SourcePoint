using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataAuth.Test.Domain.Data
{
    public class Users:BaseModel
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Account { get; set; }
    }
}
