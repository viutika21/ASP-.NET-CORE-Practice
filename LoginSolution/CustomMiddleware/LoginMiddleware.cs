using Microsoft.Extensions.Primitives;


namespace LoginSolution.CustomMiddleware
{
    public class LoginMiddleware
    {
        private readonly RequestDelegate _next;

        public LoginMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == "POST" && context.Request.Path == "/")
            {
                StreamReader reader = new StreamReader(context.Request.Body);
                string body = await reader.ReadToEndAsync();

                Dictionary<string, StringValues> dict = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);

                string email = string.Empty, password = string.Empty;
                if (dict.ContainsKey("email"))
                {
                    email = Convert.ToString(dict["email"][0]);
                }
                else
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid input for 'email'\n");
                }

                if (dict.ContainsKey("password"))
                {
                    password = Convert.ToString(dict["password"][0]);
                }
                else
                {
                    if (context.Response.StatusCode == 200)
                        context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid input for 'password'\n");
                }
                if (string.IsNullOrEmpty(email) == false && string.IsNullOrEmpty(password) == false)
                {
                    string validEmail = "admin@example.com", validPass = "admin1234";
                    bool isValidLogin;

                    if (email == validEmail && password == validPass)
                    {
                        isValidLogin = true;
                    }
                    else
                    {
                        isValidLogin = false;
                    }

                    if (isValidLogin)
                    {
                        await context.Response.WriteAsync("Successfull Login \n");
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync("Invalid Login \n");
                    }

                }
            }

            else
            {
                await _next(context);
            }
        }
    }

        public static class LoginMiddlewareExtensions
        {
            public static IApplicationBuilder UseLoginMiddleware(this IApplicationBuilder applicationBuilder)
            {
                return applicationBuilder.UseMiddleware<LoginMiddleware>();
            }
        }

    }

