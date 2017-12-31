using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourcePoint.Infrastructure.Extensions.SwaggerExtension.Models
{
    /// <summary>
    /// OData注册EdmModel管理
    /// </summary>
    public class ODataEdmModelManager
    {
        private List<ODataRouterEdmModel>  edmModelList = new List<ODataRouterEdmModel>();

        public List<ODataRouterEdmModel>  GetItems
        {
            get => edmModelList;
        }

        public ODataRouterEdmModel AddEdmModel(string key,IEdmModel edmModel)
        {
            var model = CheckErrorAddEdmModel(key, edmModel);
            if (model==null) throw new Exception($"{nameof(key)}:{key} 键已存在 不能重复添加");

            return model;
        }


        public IEdmModel FindEdmModel(string key)
        {
            return edmModelList.FirstOrDefault(x=>x.Key==key)?.EdmModel;
        }
        /// <summary>
        /// 查询添加模型数据只能添加没有前缀相同的数据
        /// </summary>
        /// <param name="key">api请求前缀</param>
        /// <param name="edmModel">模型数据</param>
        /// <returns></returns>
        private ODataRouterEdmModel CheckErrorAddEdmModel(string key,IEdmModel edmModel)
        {
            var isError=edmModelList.Any(x => x.Key == key);
            if (isError) return null;

            var model = new ODataRouterEdmModel(key, edmModel);
            edmModelList.Add(model);
            return model;
        }
    }


}
