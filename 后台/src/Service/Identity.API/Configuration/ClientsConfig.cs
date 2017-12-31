using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SourcePoint.Service.Identity.API.Configuration
{
    public class ClientsConfig
    {
        public static List<Client> GetClients()
        {
            List<Client> list = new List<Client>();
            Client client = new Client();
            client.ClientName = "用户密码";
            client.ClientId = "pen.client";
            client.ClientSecrets = new List<Secret>()
            {
                new Secret("passwordq123q".Sha256())
            };
            //此属性 默认为 jwt(Token 根据数据进行加密的 很长)
            //AccessTokenType.Reference：为此Token的引用 很短  保存在数据库中此表  PersistedGrants
            //如果在获取Token的时候  返回了Token 但还是报错  有可能是PersistedGrants 表在数据迁移中没有生成到数据库中
            client.AccessTokenType = AccessTokenType.Reference;
            //这里开启离线验证  true:允许刷新Token  false:不允许刷新Token的
            client.AllowOfflineAccess = true;
            client.AllowedGrantTypes = GrantTypes.ResourceOwnerPassword;
            client.AllowedScopes = new List<string>() { "penApi", IdentityServerConstants.StandardScopes.OpenId };
            list.Add(client);
            return list;
        }
    }
}
