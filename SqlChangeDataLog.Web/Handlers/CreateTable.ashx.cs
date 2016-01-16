using System.Data;
using System.Linq;
using SqlChangeDataLog.Extensions;
using SqlChangeDataLog.QueryObjects;
using SqlChangeDataLog.Web.Application;

namespace SqlChangeDataLog.Web.Handlers
{
    public class CreateTable : Handler<Context>
    {
        protected override object Process(Context context, IDbConnection connection)
        {
            var exists = connection.Query<int>(new ExistsChangeLogTable().CheckExist(context.LogTable)).Single();
            if (exists == 0)
            {
                connection.Execute(new CreateChangeLogTable().Create(context.LogTable));
            }

            return true;
        }
    }
}