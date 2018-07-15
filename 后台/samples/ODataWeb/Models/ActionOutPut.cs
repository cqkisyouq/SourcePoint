using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODataWeb.Models
{
    public class ActionOutPut
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public TesetModel tesetModel { get; set; }
    }

    public class TesetModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
}
