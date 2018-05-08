using EFDataAuth.Test.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EFDataAuth.Test.Validator
{
    public static class DbContextExtension
    {
        public static HttpContext HttpContext(this IDbContextServices dbContextServices)=> dbContextServices.HttpDbContext()?.HttpContext;

        public static IHttpContextAccessor HttpDbContext(this IDbContextServices dbContextServices)
        {
            return dbContextServices.CurrentContext.Context?.GetService<IHttpContextAccessor>();
        }

        //todo 这个类要怎么处理 1、想得到注入服务 2、取出数据权限缓存 每次查询的时候进行过滤
        public static bool IsValidat(string name)
        {
            return true;
        }
    }
}
