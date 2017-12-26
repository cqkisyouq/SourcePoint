using EFDataAuth.Test.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SourcePoint.Infrastructure.Extensions.ExpressionExtension;
using System.Linq.Expressions;
using EFDataAuth.Test.Domain.Data;
using System;

namespace EFDataAuth.Test.Controllers
{

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly MyTestDbContext _db;
        public ValuesController(MyTestDbContext myTestDbContext)
        {
            _db = myTestDbContext;
        }
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
           // var ds = _db.Users.WhereIf(true, x => x.Name.Length > 10).ToList();
            Expression<Func<Users, bool>> expression = query => query.Account.Length > 2;
            var ts = _db.Users.Where(expression.WhereIf(d => d.Name.Length > 10,
                d=>d.Phone=="123123",
                d=>d.Name=="dddd"
                )).ToList();
            var result = _db.Users.Where(x => x.Name.Length > 4).FirstOrDefault(x => x.Name.Length > 5);
            return Ok(result);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _db.Users.AsQueryable().Join(_db.Adress.AsQueryable(),
                user => user.Id,
                adress => adress.UserId,
                (user, adress) => new {
                   UserName=user.Name,
                   Adress=adress.Name,
                   Phone=user.Phone,
                   CreateTime=user.CreateTime
                }).ToList();

            return Ok(result);
        }
    }
}
