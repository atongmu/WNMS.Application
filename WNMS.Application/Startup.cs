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

            #region ��Ȩ
            JWTTokenOptions jwtTokenOptions = new JWTTokenOptions();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {

                    //options.ExpireTimeSpan = TimeSpan.FromSeconds(2);
                    //��֤ʧ�ܣ����Զ���ת�������ַ
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
                        ClockSkew = TimeSpan.FromMinutes(120)//����ʱ��
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
            //    options.Filters.Add(new RequireHttpsAttribute());//��������ʹ��HTTPS
            //}).SetCompatibilityVersion(CompatibilityVersion.Latest);

            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(new RequireHttpsAttribute());//��������ʹ��HTTPS

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
                //��󻺴�ռ��С����Ϊ 1024
                o.SizeLimit = 1024;
                //�����������Ϊ����ѹ����Ϊ 2%
                o.CompactionPercentage = 0.02d;
                //ÿ 5 ���ӽ���һ�ι��ڻ����ɨ��
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

            //ע��Swagger������������һ���Ͷ��Swagger �ĵ�
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "WMNS_API", Version = "v1" });

                // Ϊ Swagger JSON and UI����xml�ĵ�ע��·��
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//��ȡӦ�ó�������Ŀ¼�����ԣ����ܹ���Ŀ¼Ӱ�죬������ô˷�����ȡ·����
                var xmlPath = Path.Combine(basePath, "WNMS.Application.xml");
                c.IncludeXmlComments(xmlPath, true);

                #region ����swagger��֤����
                //���һ�������ȫ�ְ�ȫ��Ϣ����AddSecurityDefinition����ָ���ķ�������һ�¼��ɣ�WNMS.Application��
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
                    Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ���·�����Bearer {token} ���ɣ�ע������֮���пո�",
                    Name = "Authorization",//jwtĬ�ϵĲ�������
                    In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
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

            #region Nlog����־
            //           //����־��¼�����ݿ�                 config/NLog.config
            //NLog.LogManager.LoadConfiguration("Config/Nlog.config").GetCurrentClassLogger();
            //NLog.LogManager.Configuration.Variables["connectionString"] = Configuration.GetConnectionString("WNMSConnectionString"); 
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  //������־�е������������

            #endregion


            //cookies
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("any");

            //�����м����������Swagger��ΪJSON�ս��
            app.UseSwagger();
            //�����м�������swagger-ui��ָ��Swagger JSON�ս��
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
