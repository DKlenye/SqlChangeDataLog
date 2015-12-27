using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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