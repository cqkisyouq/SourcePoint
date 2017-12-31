using SourcePoint.Data.BaseData.Interface;
using System;
namespace SourcePoint.Data.BaseData.Bases
{
    public partial class BaseEntity: IBaseEntity<Guid>
    {
        public Guid Id { get; set; }
        public bool Deleted { get; set; }
        public bool Enabled { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
