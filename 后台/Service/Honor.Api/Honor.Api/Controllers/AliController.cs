using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Honor.Controllers
{
    /// <summary>
    /// 调用阿里接口
    /// </summary>
    public class AliController : Controller
    {



        /// <summary>
        /// 获取芝麻信用接口
        /// </summary>
        /// <returns></returns>
        public IActionResult ZhimaCredit()
        {
            return NoContent();
        }

    }
}
