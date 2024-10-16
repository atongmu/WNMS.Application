using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WNMS.Application.Utility;
using WNMS.Application.Utility.Filters;
using WNMS.Utility;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using WNMS.Application.Utility.SignalRChat.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using WNMS.Application.Utility.MediaServer;

namespace WNMS.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StaticConstraint.Init(s => configuration[s]);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region 鉴权
            JWTTokenOptions jwtTokenOptions = new JWTTokenOptions();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {

                    //options.ExpireTimeSpan = TimeSpan.FromSeconds(2);
                    //认证失败，会自动跳转到这个地址
                    options.LoginPath = "/Login/Index";
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = jwtTokenOptions.Key,

                        ValidateIssuer = true,
                        ValidIssuer = jwtTokenOptions.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtTokenOptions.Audience,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(120)//过期时间
                    };
                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                //options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //services.AddMvc(options =>
            //{ 
            //    options.Filters.Add(new RequireHttpsAttribute());//所有请求都使用HTTPS
            //}).SetCompatibilityVersion(CompatibilityVersion.Latest);

            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(new RequireHttpsAttribute());//所有请求都使用HTTPS

            //}).SetCompatibilityVersion(CompatibilityVersion.Latest)


            //.AddJsonOptions(options => {
            //    options.JsonSerializerOptions.PropertyNamingPolicy = null;
            //    options.ContractResolver = new DefaultContractResolver();
            //} 
            //   );
            ;

            //services.AddHttpsRedirection(opt =>
            //{
            //    opt.RedirectStatusCode = StatusCodes.Status301MovedPermanently;
            //    //opt.HttpsPort = 10014;
            //    opt.HttpsPort = 5000;
            //    //opt.HttpsPort = 81;

            //});

            //services.AddHsts(options =>
            //{
            //    options.Preload = true;
            //    options.IncludeSubDomains = true;
            //    options.MaxAge = TimeSpan.FromDays(30);
            //    //options.ExcludedHosts.Add("47.104.187.1:10014");
            //    options.ExcludedHosts.Add("localhost:5000");
            //    //options.ExcludedHosts.Add("127.0.0.1:81");
            //});
            #endregion



            //services.AddMvc().AddRazorRuntimeCompilation();
            services.AddDistributedMemoryCache(o =>
            //services.AddMemoryCache(o =>
            {
                //最大缓存空间大小限制为 1024
                o.SizeLimit = 1024;
                //缓存策略设置为缓存压缩比为 2%
                o.CompactionPercentage = 0.02d;
                //每 5 分钟进行一次过期缓存的扫描
                o.ExpirationScanFrequency = TimeSpan.FromMinutes(5);

            });

            //services.AddScoped<Utility.Cache.ICache, Utility.Cache.MemoryCacheExtensions>();

            services.AddSession();

            services.AddControllersWithViews(o =>
            {
                o.Filters.Add(typeof(GlobalExceptionFilter));
                o.Filters.Add(typeof(GlobalActionFilter));
            }).AddRazorRuntimeCompilation();

            services.AddRazorPages();

            // cookies
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder.WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS").AllowAnyOrigin();
                });
            });

            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "WMNS_API", Version = "v1" });

                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "WNMS.Application.xml");
                c.IncludeXmlComments(xmlPath, true);

                #region 启用swagger验证功能
                //添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称一致即可，WNMS.Application。
                //var security = new Dictionary<string, IEnumerable<string>> { { "WNMS.Application", new string[] { } }, };
                //Microsoft.OpenApi.Models.OpenApiSecurityRequirement security = null;
                c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                 {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "WNMS.Application"
                            }
                        },
                        new string[] { }
                    }
                });
                c.AddSecurityDefinition("WNMS.Application", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 在下方输入Bearer {token} 即可，注意两者之间有空格",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
                #endregion

            });
            //services.AddControllers()
            // .AddNewtonsoftJson();
            services.AddSignalR();
            services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(@"D:\temp-keys\"));
                     //.PersistKeysToFileSystem(new DirectoryInfo(@"e:\temp-keys\"));

            services.AddHttpClient();

            services.AddSingleton<IHostedService, BackCloseStreams>();

        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<CustomAutofacModule>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

            }
            app.UseHsts();
           // app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            #region Nlog记日志
            //           //将日志记录到数据库                 config/NLog.config
            //NLog.LogManager.LoadConfiguration("Config/Nlog.config").GetCurrentClassLogger();
            //NLog.LogManager.Configuration.Variables["connectionString"] = Configuration.GetConnectionString("WNMSConnectionString"); 
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  //避免日志中的中文输出乱码

            #endregion


            //cookies
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("any");

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WMNS_API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                  name: "areas",
                  areaName: "Sys",
                     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                      name: "areas",
                      areaName: "Sws",
                      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(
                      name: "areas",
                      areaName: "Wos",
                      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(
                     name: "areas",
                     areaName: "Com",
                     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(
                     name: "areas",
                     areaName: "Wo",
                     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Login}/{action=Index}/{id?}");


                endpoints.MapHub<ChatHub>("/api/chatHub");
                endpoints.MapRazorPages();
            });
        }
    }
}
