using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing;
using ODataWeb.Models;
using SourcePoint.Infrastructure.Extensions.MVCExtension.ControllerExtensions;
using SourcePoint.Infrastructure.Extensions.ODataExtension.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ODataWeb.Controllers
{
    [ODataRoute("Actions")]
    [OdataEntity(typeof(ActionOutPut))]
    [Route("[controller]/[action]")]
    [Produces("application/json")]
    public class ActionsController : Controller
    {
        /// <summary>
        /// 获取操作定义数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(IQueryable<ActionOutPut>), 200)]
        public IQueryable<ActionOutPut> Get()
        {
            #region 返回一个 Iqueryable 对象 用作OData 自己对查询操作处理
            //var result = _actionService.Get(query => query).Select(x => new ActionOutput()
            //{
            //    Id = x.Id,
            //    Key = x.Key,
            //    Name = x.Name,
            //    Description = x.Description,
            //    DisplayOrder = x.DisplayOrder,
            //    Enabled = x.Enabled,
            //    CreatedOnUtc = x.CreatedOnUtc,
            //    UpdatedOnUtc = x.UpdatedOnUtc,
            //    Modules = x.Features.Where(d => d.Deleted == false && d.Module.Deleted == false)
            //   .Select(d => new ModuleOutput()
            //   {
            //       Id = d.Module.Id,
            //       Key = d.Module.Key,
            //       ApplicationId = d.Module.ApplicationId,
            //       Name = d.Module.Name,
            //       Description = d.Module.Description,
            //       Enabled = d.Module.Enabled,
            //       CreatedOnUtc = d.Module.CreatedOnUtc,
            //       UpdatedOnUtc = d.Module.UpdatedOnUtc
            //   }).ToList()
            //});
            #endregion
            var result =new  List<ActionOutPut>();
            return result.AsQueryable();
        }

        /// <summary>
        /// 添加操作信息
        /// </summary>
        /// <param name="input">操作信息</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201)]
        public IActionResult Post([FromBody] ActionOutPut input)
        {
            return CreatedAtAction(nameof(Get), input);
        }

        /// <summary>
        /// 修改操作信息
        /// </summary>
        /// <param name="id">应用ID</param>
        /// <param name="value">更新的应用信息</param>
        /// <returns></returns>
        [HttpPut]
        [ODataRoute("({id})")]
        [ProducesResponseType(204)]
        public IActionResult Put(Guid id, [FromBody] ActionOutPut value)
        {
            if (id == null) return this.ModelStateError(nameof(id), "请填写int");
            
            return NoContent();
        }

        /// <summary>
        /// 增量更新
        /// </summary>
        /// <param name="id">操作ID</param>
        /// <param name="value">更新的对象</param>
        /// <returns></returns>
        [HttpPatch]
        [ODataRoute("({id})")]
        [ProducesResponseType(204)]
        public IActionResult Patch(Guid id, [FromBody]ActionOutPut value)
        {
            if (id == null) return this.ModelStateError(nameof(id), "请填写int");
            var entity = new List<ActionOutPut>().Where(x=>x.ID==value.ID);

            if (entity == null)
            {
                return NotFound("没有数据");
            }
            
            Delta<ActionOutPut> deresult = new Delta<ActionOutPut>(value);
            deresult.Patch(entity,this.Services< IHttpContextAccessor>().HttpContext);
            
            if (deresult.HasSourceProperty(nameof(value.Name)))
            {
              
            }
            

            return NoContent();
        }

        /// <summary>
        /// 根据应用ID删除操作信息
        /// </summary>
        /// <param name="id">应用ID</param>
        /// <returns></returns>
        [HttpDelete]
        [ODataRoute("({id})")]
        [ProducesResponseType(204)]
        public IActionResult Delete(Guid id)
        {
            if (id == null) return this.ModelStateError(nameof(id), "请填写int");
            
            return NoContent();
        }
    }
}
