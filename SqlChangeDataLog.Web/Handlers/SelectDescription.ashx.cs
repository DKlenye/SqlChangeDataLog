using System.Data;
using SqlChangeDataLog.Dtos;
using SqlChangeDataLog.Extensions;
using SqlChangeDataLog.Web.Application;

namespace SqlChangeDataLog.Web.Handlers
{
    public class SelectDescription : Handler<Context>
    {
        protected override object Process(Context context, IDbConnection connection)
        {
            var data =  connection.Query<DescriptionDto>(new QueryObjects.SelectDescription().All());
            return new DataSerializer(data);
        }
    }
}