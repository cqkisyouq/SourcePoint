﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.Client;
using IdentityModel;
using SourcePoint.Core.Options;
using Microsoft.Extensions.Options;
using IdentityServer4.AccessTokenValidation;
using SourcePoint.Core.Models;
using SourcePoint.Core.Helpers.Common;

namespace Identity.API.Test.Controllers
{
    
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly AuthClient authClient;
        public ValuesController(IOptions<AuthClient> authOption)
        {
            authClient = authOption.Value;
        }
        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var url = "http://139.217.11.253:8010";
            url = "http://localhost:8010";
            var discoClient = new DiscoveryClient(url) { Policy = { RequireHttps = false } };

            var disco = await discoClient.GetAsync();

            //disco.IsError  服务有错 需要进行判断再执行下一步
            

            //字符串转成base64   这是 IdentityService4 提供的辅助方法
             var base64=Base64Url.Encode(System.Text.UTF8Encoding.UTF8.GetBytes($"{123}:{3123}"));
            //base64转成字符串
             var orgString=Convert.ToBase64String(Base64Url.Decode(base64));
            

            var tokenClient = new TokenClient(
               disco.TokenEndpoint,
              // "zhou.test",
              "pen.client",//ClientID
              "passwordq123q" //加密码盐值   Secret
                //"hsL7J6nACUol1916h1yiPygPhxnGFiwA"
               );
            //offline_access:允许刷新Token 如果在Client表中没有设置允许请不要在这里加上
            //如果不加上   RefreshToken 就是 null
            var response =
                tokenClient.RequestResourceOwnerPasswordAsync("18983039906", "123123", "penApi offline_access").Result;
            //tokenClient.RequestResourceOwnerPasswordAsync("18983039906", "123456", "api1 offline_access").Result;


            return new string[] { response.AccessToken, response.RefreshToken };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public string Get(int id)
        {
            var isAuth = HttpContext.User.Identity.IsAuthenticated;
            if (isAuth)
            {
                var userid=HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            }
            return isAuth.ToString();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account">用户帐号</param>
        /// <returns>成功登录后的 Token</returns>
        [HttpPost]
        public async Task<IActionResult> Login(string passWord,string name)
        {
            var discoClient = new DiscoveryClient(authClient.BaseUrl) { Policy = { RequireHttps = false } };

            var disco = await discoClient.GetAsync();
            if (disco.IsError) return BadRequest("服务异常");
            //字符串转成base64   这是 IdentityService4 提供的辅助方法
            //var base64 = Base64Url.Encode(System.Text.UTF8Encoding.UTF8.GetBytes($"{123}:{3123}"));
            //base64转成字符串
            //var orgString = Convert.ToBase64String(Base64Url.Decode(base64));

            var tokenClient = new TokenClient(
               disco.TokenEndpoint,
             authClient.Name,//ClientID
             authClient.Secret //  Secret
               );

            passWord = MD5Helper.GetStringHash(passWord);
            //offline_access:允许刷新Token 如果在Client表中没有设置允许请不要在这里加上
            //如果不加上   RefreshToken 就是 null
            var response = await tokenClient.RequestResourceOwnerPasswordAsync(name, passWord, authClient.Scope);

            return Ok(new { response.IsError, response.Error, response.AccessToken, response.RefreshToken });
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        /// <summary>
        /// 用户退出->注销用户登录使用的 Token
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(200)]
        public async Task<IActionResult> LoginOut()
        {
            var disco = await DiscoveryClient.GetAsync(authClient.BaseUrl);
            if (disco.IsError) return StatusCode(500, new ErrorResult("授权服务异常，请稍后再试"));
            var header = HttpContext.Request.Headers["Authorization"].ToString();
            var token = header.Split(IdentityServerAuthenticationDefaults.AuthenticationScheme)[1];

            var revocationClient = new TokenRevocationClient(disco.RevocationEndpoint, authClient.Name, authClient.Secret);
            var response = await revocationClient.RevokeAccessTokenAsync(token);
            if (response.IsError) return StatusCode(500, new ErrorResult("注销用户失败，请稍后再试"));

            return NoContent();
        }
    }
}
