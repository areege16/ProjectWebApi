
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using WebApiProject.Helpers;
//using WebApiProject.Hubs;
//using WebApiProject.Interfaces;
//using WebApiProject.Models;
//using WebApiProject.Service;
//using Microsoft.AspNetCore.Authentication.Google;
//using Microsoft.AspNetCore.Authentication.Cookies;


//namespace WebApiProject
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.

//            builder.Services.AddControllers();

//            builder.Services.AddHttpContextAccessor();


//            //connectionString
//            builder.Services.AddDbContext<LostFoundContext>(option =>
//            {
//                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//            });

//          //  for authentication
//            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//                .AddEntityFrameworkStores<LostFoundContext>();

//            //setting authentication middleware check using JWT Token
//            builder.Services.AddAuthentication(options =>
//            {
//                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

//                //options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;

//            })
//                //.AddGoogle(options =>
//                //{
//                //    options.ClientId = builder.Configuration["Google:ClientId"]; 
//                //    options.ClientSecret = builder.Configuration["Google:ClientSecret"]; 
//                //})

//                .AddJwtBearer(options =>
//                {
//                    options.SaveToken = true;
//                    options.RequireHttpsMetadata = false;
//                    options.TokenValidationParameters = new()
//                    {
//                        ValidateIssuer = true,
//                        ValidIssuer = builder.Configuration["JWT:Iss"],
//                        ValidateAudience = true,
//                        ValidAudience = builder.Configuration["JWT:Aud"],
//                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
//                    };
//                });


//            //// Add Identity
//            //builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//            //    .AddEntityFrameworkStores<LostFoundContext>()
//            //    .AddDefaultTokenProviders();


//            //builder.Services.AddAuthentication(options =>
//            //{
//            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; 
//            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//            //})
//            //.AddCookie() 
//            //.AddJwtBearer(options =>
//            //{
//            //    options.SaveToken = true;
//            //    options.RequireHttpsMetadata = false;
//            //    options.TokenValidationParameters = new TokenValidationParameters
//            //    {
//            //        ValidateIssuer = true,
//            //        ValidIssuer = builder.Configuration["JWT:Iss"],
//            //        ValidateAudience = true,
//            //        ValidAudience = builder.Configuration["JWT:Aud"],
//            //        IssuerSigningKey = new SymmetricSecurityKey(
//            //            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
//            //    };
//            //})
//            //.AddGoogle(options =>
//            //{
//            //    options.ClientId = builder.Configuration["Google:ClientId"];
//            //    options.ClientSecret = builder.Configuration["Google:ClientSecret"];
//            //    options.CallbackPath = "/signin-google"; 
//            //});





//            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();

//            //chatFound
//            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
//            builder.Services.AddScoped<IChatFoundService, ChatFoundService>();
//            builder.Services.AddSignalR();

//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseRouting();

//            app.UseCookiePolicy();

//            builder.Services.AddSession();
//            app.UseSession();

//            app.UseAuthentication();
//            app.UseAuthorization();

//            app.MapHub<ChatFoundHub>("/chatFoundHub");
//            app.MapControllers();

//            app.Run();
//        }
//    }
//}


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApiProject.Hubs;
using WebApiProject.Interfaces;
using WebApiProject.Helpers;
using WebApiProject.Hubs;
using WebApiProject.Interfaces;
using WebApiProject.Models;
using WebApiProject.Repository;
using WebApiProject.Service;

namespace WebApiProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            //connectionString
            builder.Services.AddDbContext<LostFoundContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IFoundItemRepository, FoundItemRepository>();


            //for authentication
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<LostFoundContext>();

            //setting authentication middleware check using JWT Token
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidIssuer = builder.Configuration["JWT:Iss"],
                        ValidateAudience = false,
                        ValidAudience = builder.Configuration["JWT:Aud"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    };
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            //Inject for  Lost Item Chat 
            builder.Services.AddScoped<ILostItemCurrentUserRepo, LostItemCurrentUserRepo>();
            builder.Services.AddScoped<ILostItemMsgRepo, LostItemMsgRepo>();
            builder.Services.AddScoped<ILostItemRepo, LostItemRepo>();
            builder.Services.AddScoped<ILostCommentRepo, LostCommentRepo>();
            builder.Services.AddSignalR();
            builder.Services.AddHttpContextAccessor();


            //chatFound
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<IChatFoundService, ChatFoundService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<ChatFoundHub>("/chatFoundHub");
            app.MapHub<HLostItemChatHub>("/HLostItemChatHub");
            app.MapControllers();

            app.Run();
        }
    }
}