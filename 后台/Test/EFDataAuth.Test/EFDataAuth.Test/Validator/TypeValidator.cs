using System.Collections.Generic;

namespace EFDataAuth.Test.Validator
{
    public class TypeValidator
    {
        public static List<string> list = new List<string>()
        {
            "CreateTime",
            "UpdateTime"
        };
        //todo 这个类要怎么处理 1、想得到注入服务 2、取出数据权限缓存 每次查询的时候进行过滤
        public static bool IsValidat(string name)
        {
            return !(list.Contains(name));
        }
    }
}
