using System.Web;

namespace SqlChangeDataLog.Web
{
    public class SelectTables : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            var rezult = @"[{""id"":1,""Name"":""Vehicle""},{""id"":2,""Name"":""Request""}]";

            context.Response.Write(rezult);
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}