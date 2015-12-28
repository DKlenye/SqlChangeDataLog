using System.Data;
using SqlChangeDataLog.Queries;
using SqlChangeDataLog.Web.Application;

namespace SqlChangeDataLog.Web.Handlers
{
    
    public class SelectTableList : Handler<Context>
    {
        protected override object Process(Context parameters)
        {
            using (IDbConnection dbConnection = Connect())
            {
                return  new SelectTableListDto(dbConnection).Query();
            }
        }
    }
}