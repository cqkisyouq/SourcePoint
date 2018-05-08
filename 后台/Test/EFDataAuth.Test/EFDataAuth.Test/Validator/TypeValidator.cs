using System.Collections.Generic;
using System.Linq;
namespace EFDataAuth.Test.Validator
{
    public class TypeValidator
    {
        public static List<string> list = new List<string>()
        {
            //"CreateTime",
            //"UpdateTime"
            "Account"
        };
        //todo 这个类要怎么处理 1、想得到注入服务 2、取出数据权限缓存 每次查询的时候进行过滤
        public static bool IsValidat(string name)
        {
           return list.Any(x => string.Equals(x, name, System.StringComparison.OrdinalIgnoreCase))==false;
        }
    }
}
