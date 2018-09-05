using System;
using System.Collections.Generic;
using System.Text;

namespace SourcePoint.Core.Models
{
    public class SearchPaged
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool HasTime { get; set; }
        public bool HasPage { get; set; }
    }
}
