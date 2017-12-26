using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataAuth.Test.Domain.Data
{
    public class Adress:BaseModel
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
    }
}
