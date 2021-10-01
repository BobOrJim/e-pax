using Microsoft.AspNetCore.Builder;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDP
{
    public class LogUtils
    {

        public static void LogInMiddleware(IApplicationBuilder app, string project, string aboveInCode, string belowInCode)
        {
            app.Use(async (context, next) =>
            {
                Log.Information($"{project} : {aboveInCode}  -->  {belowInCode} ");
                await next.Invoke();
                Log.Information($"{project} : {aboveInCode}  <--  {belowInCode} ");
            });


        }

    }
}
