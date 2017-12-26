using System;
using System.Text;
using System.Linq;


namespace MapPen.Data
{
    public class DbConnectionConfig
    {

        public string Server { get; set; }
        public string DbName { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string ProviderName { get; set; }
        public bool Encrypt { get; set; } = true;


        public string ConnectionString => GetConntionStr();

        string GetConntionStr(ProviderType dbType = ProviderType.MsSql)
        {
            switch (dbType)
            {
                case ProviderType.MsSql:
                    return $"Server={Server};Initial Catalog={DbName};Persist Security Info=False;User ID={UserName};Password={Password};MultipleActiveResultSets=False;Encrypt={Encrypt};TrustServerCertificate=False;Connection Timeout=30;";
                case ProviderType.MySql:
                    return $"Server={Server};Initial Catalog={DbName};Persist Security Info=False;User ID={UserName};Password={Password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                default:
                    return $"Server={Server};Initial Catalog={DbName};Persist Security Info=False;User ID={UserName};Password={Password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            }
        }
        internal ProviderType ProviderType
        {
            get
            {
                //TODO:DataProvider
                if (ProviderName.Equals("System.Data.SqlClient", StringComparison.CurrentCultureIgnoreCase))
                    return ProviderType.MsSql;

                return ProviderType.MsSql;

            }
        }
    }

    public enum ProviderType
    {
        MsSql,
        MySql
    }

}
