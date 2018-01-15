using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;

namespace SourcePoint.Infrastructure.Extensions.MVCExtension
{
    public class SourcePointRazorProject : FileProviderRazorProject
    {
        private List<PhysicalFileProvider> _provider=new List<PhysicalFileProvider>();
        private IRazorProjectConfiguration _ProjectConfiguration;
        public SourcePointRazorProject(
             IRazorViewEngineFileProviderAccessor accessor
            , IRazorProjectConfiguration razorProjectConfiguration
            ) : base(accessor)
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
                        return new FileProviderRazorProjectItem(fileInfo, basePath: string.Empty, path: path);
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
