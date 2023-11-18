using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using System.Text.Json.Serialization;
using TalkHubAPI.Data;
using TalkHubAPI.Helper;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Interfaces.ForumInterfaces;
using TalkHubAPI.Interfaces.MessengerInterfaces;
using TalkHubAPI.Interfaces.PhotosManagerInterfaces;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Repositories;
using TalkHubAPI.Repositories.ForumRepositories;
using TalkHubAPI.Repositories.MessengerRepositories;
using TalkHubAPI.Repositories.PhotosManagerRepositories;
using TalkHubAPI.Repositories.VideoPlayerRepositories;
using TalkHubAPI.Hubs;
using Microsoft.AspNetCore.Http.Features;
using FFMpegCore;
using TalkHubAPI.BackgroundTasks;
using TalkHubAPI.Middlewarres;
using TalkHubAPI.Interfaces.ServiceInterfaces;
using TalkHubAPI.Services;
using Microsoft.Extensions.DependencyInjection;
using TalkHubAPI.Models.ConfigurationModels;
using Microsoft.Extensions.Options;

namespace TalkHubAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddTransient<Seed>();
            builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddMemoryCache();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IPhotoCategoryRepository, PhotoCategoryRepository>();
            builder.Services.AddScoped<IPhotoRepository, PhotoRepository>();
            builder.Services.AddScoped<IFileProcessingService, FileProcessingService>();
            builder.Services.AddScoped<IForumMessageRepository, ForumMessageRepository>();
            builder.Services.AddScoped<IForumThreadRepository, ForumThreadRepository>();
            builder.Services.AddScoped<IUserUpvoteRepository, UserUpvoteRepository>();
            builder.Services.AddScoped<IMessageRoomRepository, MessageRoomRepository>();
            builder.Services.AddScoped<IMessengerMessageRepository, MessengerMessageRepository>();
            builder.Services.AddScoped<IUserMessageRoomRepository, UserMessageRoomRepository>();
            builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
            builder.Services.AddScoped<IVideoCommentRepository, VideoCommentRepository>();
            builder.Services.AddScoped<IVideoCommentsLikeRepository, VideoCommentsLikeRepository>();
            builder.Services.AddScoped<IVideoPlaylistRepository, VideoPlaylistRepository>();
            builder.Services.AddScoped<IVideoRepository, VideoRepository>();
            builder.Services.AddScoped<IVideoTagRepository, VideoTagRepository>();
            builder.Services.AddScoped<IVideoUserLikeRepository, VideoUserLikeRepository>();
            builder.Services.AddHostedService<QueueService>();
            builder.Services.AddSingleton<IBackgroundQueue, BackgroundQueue>();

            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
            builder.Services.Configure<JwtTokenSettings>(builder.Configuration.GetSection("JwtTokenSettings"));
            builder.Services.Configure<FFMpegConfig>(builder.Configuration.GetSection("FFMpegConfig"));

            builder.Services.AddTransient<IMailService, MailService>();
            builder.Services.AddSignalR();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            JwtTokenSettings? jwtTokenSettings = builder.Configuration
                .GetSection("JwtTokenSettings")
                .Get<JwtTokenSettings>();

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddCookie(x =>
            {
                x.Cookie.Name = "token";

            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSettings.Token)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }
                        else
                        {
                            context.Token = context.Request.Cookies["jwtToken"];
                        }

                        return Task.CompletedTask;
                    }
                };

            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", builder =>
                {
                    builder.WithOrigins("http://localhost:5173")
                           .AllowAnyHeader()
                           .AllowCredentials()
                           .AllowAnyMethod();
                });
            });

            builder.Services.AddDbContext<TalkHubContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SDR"));
            });

            FFMpegConfig? ffmpegConfig = builder.Configuration
                .GetSection("FFMpegConfig")
                .Get<FFMpegConfig>();

            GlobalFFOptions.Configure(new FFOptions
            {
                BinaryFolder = ffmpegConfig.FFMpegBinaryDirectory,
                TemporaryFilesFolder = ffmpegConfig.TemporaryFilesDirectory
            });

            var app = builder.Build();

            if (args.Length == 1 && args[0].ToLower() == "seeddata")
            {
                SeedData(app);
            }

            void SeedData(IHost app)
            {
                var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

                using (var scope = scopedFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetService<Seed>();
                    service.SeedTalkHubContext();
                }
            }


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowOrigin");

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.MapControllers();

            app.MapHub<ChatHub>("/chatHub");

            app.Run();
        }
    }
}