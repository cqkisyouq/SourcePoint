using System;
using System.Collections.Generic;
using System.Text;

namespace SourcePoint.Infrastructure.Extensions.MVCExtension
{
    public interface IRazorProjectConfiguration
    {
        string[] Paths { get; set; }
    }
    public class RazorProjectConfiguration:IRazorProjectConfiguration
    {
        public string[] Paths { get; set; } = new string[1];
        public RazorProjectConfiguration() { }
        public RazorProjectConfiguration(params string[] paths)
        {
            if (paths.Length > 0)
            {
                Paths = paths;
            }
        }
    }
}
