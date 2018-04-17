using Microsoft.AspNetCore.Mvc;
using SourcePoint.Infrastructure.Authenticator.GoogleAuthenticator;

namespace Google.Controllers
{
    [Route("api/[controller]")]
    public class GoogleController : Controller
    {
        private readonly GoogleAuthenticator _GoogleAuth;
        
        public GoogleController(GoogleAuthenticator googleAuthenticator)
        {
            _GoogleAuth = googleAuthenticator;
        }

        /// <summary>
        /// 得到  base32 谷歌验证Key
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            var result = _GoogleAuth.GoogleKeyForRand();
            return Ok(result);
        }

        /// <summary>
        /// 拿谷歌验证Key生成 验证码 时间周期为60秒变一次
        /// </summary>
        /// <param name="key">得到 的 base32 谷歌验证Key</param>
        /// <returns></returns>
        [HttpGet("{key}")]
        public IActionResult GetCodeFor(string key)
        {
            var code = _GoogleAuth.VerificationCode(key);
            return Ok(code);
        }

        /// <summary>
        /// 校验 验证码是否正确
        /// </summary>
        /// <param name="code">通过Key得到的 谷歌验证码</param>
        /// <param name="key">得到 的 base32 谷歌验证Key</param>
        /// <returns></returns>
        [HttpGet("{code}/{key}")]
        public IActionResult Verification(string code,string key)
        {
            var result = _GoogleAuth.AuthenticationCode(code, key);
            return Ok(result);
        }
    }
}
