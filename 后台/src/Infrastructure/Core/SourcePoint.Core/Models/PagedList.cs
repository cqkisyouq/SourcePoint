using System;
using System.Collections.Generic;
using System.Text;

namespace SourcePoint.Core.Models
{
    public class PagedList<T>
    {
        public int Total { get; set; }
        public int CurrentPage { get; set; }
        public List<T> Data { get; set; } = new List<T>();
    }
}
