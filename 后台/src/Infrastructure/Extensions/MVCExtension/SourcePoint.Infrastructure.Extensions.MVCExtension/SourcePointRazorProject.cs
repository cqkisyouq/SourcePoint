using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;

namespace SourcePoint.Infrastructure.Extensions.MVCExtension
{
    public class SourcePointRazorProject : FileProviderRazorProjectFileSystem
    {
        private List<PhysicalFileProvider> _provider=new List<PhysicalFileProvider>();
        private IRazorProjectConfiguration _ProjectConfiguration;
        public SourcePointRazorProject(
             IRazorViewEngineFileProviderAccessor accessor
            , IHostingEnvironment hostingEnvironment
            , IRazorProjectConfiguration razorProjectConfiguration
            ):base(accessor,hostingEnvironment)
        {
            _ProjectConfiguration = razorProjectConfiguration;
            Init();

        }
        public override RazorProjectItem GetItem(string path)
        {
            var projectItem= base.GetItem(path);
            if (projectItem.Exists == false)
            {
                foreach (var item in _provider)
                {
                    var fileInfo = item.GetFileInfo(path);
                    if (fileInfo.Exists)
                    {
                        return new FileProviderRazorProjectItem(fileInfo, string.Empty,path,item.Root);
                    }
                }
            }
            return projectItem;
        }

        private void Init()
        {
            foreach (var item in _ProjectConfiguration.Paths)
            {
                _provider.Add(new PhysicalFileProvider(item));
            }
        }
    }
}
