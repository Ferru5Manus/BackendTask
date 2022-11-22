using Microsoft.AspNetCore.Http.Features;
using Org.BouncyCastle.Asn1.X509;

namespace BackendTask
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FormOptions>(options => { 
                options.MemoryBufferThreshold= int.MaxValue;
            });
            services.AddSingleton<UserDatabaseRepository>();
            services.AddSingleton<UserManager>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

           
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapPost("/getUser", async context =>
                {
                    var um = app.ApplicationServices.GetService<UserManager>();
                  
                    try{
                        var query = await context.Request.ReadFromJsonAsync<ValidatedUserDto>();
                        if (query.id > 0)
                        {
                            string isId = um.IsId(query.id);
                            if(query.username!= null) {
                                string isUserName = um.IsUserName(query.username);
                                if (isId == "" && isUserName == "")
                                {
                                    var res = um.GetUser(query.id);
                                    if (res != null)
                                    {
                                        await context.Response.WriteAsJsonAsync(res);
                                    }
                                    else
                                    {
                                        await context.Response.WriteAsync("No user with such id!");
                                    }
                                }
                                else
                                {
                                    await context.Response.WriteAsync(isId + System.Environment.NewLine + isUserName);
                                }
                            }
                            else
                            {
                                if (isId == "")
                                {
                                    var res = um.GetUser(query.id);
                                    if (res != null)
                                    {
                                        await context.Response.WriteAsJsonAsync(res);
                                    }
                                    else
                                    {
                                        await context.Response.WriteAsync("No user with such id!");
                                    }
                                }
                                else
                                {
                                    await context.Response.WriteAsync(isId + System.Environment.NewLine);
                                }
                            }

                        }
                        else if(query.username!= null)
                        {
                            string isUserName = um.IsUserName(query.username);
                            if (isUserName == "")
                            {
                                var res = um.GetUser(query.username);
                                if (res != null)
                                {
                                    await context.Response.WriteAsJsonAsync(res);
                                }
                                else
                                {
                                    await context.Response.WriteAsync("No user with such username!");
                                }
                            }
                            else
                            {
                                await context.Response.WriteAsync(isUserName);
                            }
                        }
                        else
                        {
                            await context.Response.WriteAsync("Invad id!");
                        }
                    }
                    catch(Exception ex)
                    {
                        await context.Response.WriteAsync(ex.Message);
                    }
                });
              
                endpoints.MapPut("/createUser", async context => 
                {

                    var um = app.ApplicationServices.GetService<UserManager>();
                    try
                    {
                        var query = await context.Request.ReadFromJsonAsync<ValidatedUserDto>();
                        await context.Response.WriteAsync(um.AddUser(new ValidatedUserDto() { username = query.username, password = query.password }));
                    }
                    catch(Exception ex)
                    {
                        await context.Response.WriteAsync(ex.Message);
                    }
                });
                endpoints.MapDelete("/deleteUser", async context =>
                {
                    var um = app.ApplicationServices.GetService<UserManager>();
                   
                    try
                    {
                        var query = await context.Request.ReadFromJsonAsync<ValidatedUserDto>();
                        string isId = um.IsId(query.id);
                        if (isId == "")
                        {
                            await context.Response.WriteAsync(um.DeleteUser(query.id));
                        }
                        else
                        {
                            await context.Response.WriteAsync(isId);
                        }
                    }
                    catch(Exception ex)
                    {
                        await context.Response.WriteAsync(ex.Message);
                    }
                });
                endpoints.MapPost("/updateUser", async context =>
                {
                    var um = app.ApplicationServices.GetService<UserManager>();
                   
                    try
                    {
                        var query = await context.Request.ReadFromJsonAsync<ValidatedUserDto>();
                        await context.Response.WriteAsync(um.UpdateUser(query));
                    }
                    catch(Exception ex)
                    {
                        await context.Response.WriteAsync(ex.Message);
                    }
                });

            });
            app.ApplicationServices.GetService<UserDatabaseRepository>().ConfigureDb();
        }
        
    }
}
