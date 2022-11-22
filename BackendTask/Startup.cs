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
                
                endpoints.MapGet("/getUser", async context =>
                {
                    var um = app.ApplicationServices.GetService<UserManager>();
                  
                    try{
                        var query = await context.Request.ReadFromJsonAsync<ValidatedUserDto>();
                        if (query.id != 0)
                        {
                            if (um.IsId(query.id) == "")
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
                                await context.Response.WriteAsJsonAsync(um.IsId(query.id));
                            }

                        }
                        else
                        {
                            if (um.IsUserName(query.username) == "")
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
                                await context.Response.WriteAsJsonAsync(um.IsUserName(query.username));
                            }
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
                        await context.Response.WriteAsync(um.DeleteUser(query.id));
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
        }
        
    }
}
