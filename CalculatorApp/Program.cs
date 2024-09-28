var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run(async (HttpContext context) =>
{
    if(context.Request.Method == "GET" && context.Request.Path == "/")
    {
        int first = 0, second = 0;
        string? operation = null;
        long? result = null;

        if (context.Request.Query.ContainsKey("first"))
        {
            string fNum = context.Request.Query["first"][0];
            if(!string.IsNullOrEmpty(fNum))
            {
                first = Convert.ToInt32(fNum);
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid input for first Number");
            }
        }
        else
        {
            if (context.Response.StatusCode == 200)
                context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Invalid input for first Number");
        }
        if(context.Request.Query.ContainsKey("second"))
        {
            string secNum = context.Request.Query["second"][0];
            if(!string.IsNullOrEmpty(secNum))
            {
                second= Convert.ToInt32(secNum);
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Inavalid input for second number");
            }
        }
        else
        {
            if (context.Response.StatusCode == 200)
                context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Inavalid input for second number");
        }

        if (context.Request.Query.ContainsKey("operation"))
        {
            operation = Convert.ToString(context.Request.Query["operation"][0]);

            switch(operation)
            {
                case "add": result = first + second; break;
                case "sub": result = first - second; break;
                case "mul": result = (second!=0)?  first * second : 0; break;
                case "div": result = (second != 0) ? first / second : 0 ; break;                    
            }

            if (result.HasValue)
            {
               await context.Response.WriteAsync(result.Value.ToString());
            }
            else
            {
                if (context.Response.StatusCode == 200)
                    context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Inavalid input for operation");
            }
        }
        else
        {
            if (context.Response.StatusCode == 200)
                context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Inavalid input for second number");
        }
    }
   
});

app.Run();
