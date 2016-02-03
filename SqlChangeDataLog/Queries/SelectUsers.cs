using System.Collections.Generic;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using SqlChangeDataLog.Dtos;
using SqlChangeDataLog.Extensions;
using SqlChangeDataLog.QueryObjects;

namespace SqlChangeDataLog.Queries
{
    public class SelectUsers:AbstractQuery<IEnumerable<UserDto>>
    {
        public SelectUsers(IDbConnection connection, string tableName) : base(connection)
        {
            TableName = tableName;
        }

        public string TableName { get; private set; }

        public override IEnumerable<UserDto> Query()
        {
            var users = Connection.Query<string>(new SelectLogUsers().All(TableName));
            return users.Select(login =>
            {
                PrincipalContext context;

                if ( login.Length>=7 && login.Substring(0, 7).ToLower() == "polymir")
                    context = new PrincipalContext(ContextType.Domain, "POLYMIR.NET");
                else
                    context = new PrincipalContext(ContextType.Domain, "lan.naftan.by");

                var principal = UserPrincipal.FindByIdentity(context, login);

                var user = new UserDto {Login = login};

                if (principal != null)
                {
                    user.Name = principal.DisplayName;
                    user.Phone = principal.VoiceTelephoneNumber;
                    user.Email = principal.EmailAddress;
                   
                }

                return user;

            });
        }
    }
}
