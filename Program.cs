using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MediatR;
using FluentValidation;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Service.Mapper;
using Infrastructure.Services;
using Infrastructure.Repositories;
using Application.Command;
using Application.Command.Handler;
using Application.Common.Behaviour;
using Application.Interfaces;
using Application.Service;
using Application.Query;
using Application.Common.Dto;
using Application.Query.Handler;
using Application.Common.Validation;
using Application.Common;
using Microsoft.OpenApi.Models;
using Application.Common.Mapping;
using static Infrastructure.Services.MailService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// For authentication
var _key = builder.Configuration["Jwt:Key"];
var _issuer = builder.Configuration["Jwt:Issuer"];
var _audience = builder.Configuration["Jwt:Audience"];
var _expirtyMinutes = builder.Configuration["Jwt:ExpiryMinutes"];

//services cors
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

// Configuration for token
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = _audience,
        ValidIssuer = _issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
        ClockSkew = TimeSpan.FromMinutes(Convert.ToDouble(_expirtyMinutes))

    };
});
builder.Services.AddSingleton<ITokenService>(new TokenService(_key, _issuer, _audience, _expirtyMinutes));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});


builder.Services.AddDbContext<YGCContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("YGC")
));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IBaseRepository<>),typeof(BaseRepository<>));
builder.Services.AddScoped<IRequestHandler<AuthCommand, BaseResponse<AuthResponseDto>>, AuthHandler>();
builder.Services.AddScoped<IRequestHandler<EditProfileCommand, BaseResponse<UserDto>>, EditProfileHandler>();
builder.Services.AddScoped<IRequestHandler<EditUserCommand, BaseResponse<UserDto>>, EditUserHandler>();
builder.Services.AddScoped<IRequestHandler<GetUsersQuery, BaseResponse<List<UserDto>>>, GetUsersHandler>();
builder.Services.AddScoped<IRequestHandler<DisableUserCommand, BaseResponse<UserDto>>, DisableUserHandler>();
builder.Services.AddScoped<IRequestHandler<ClassNotificationQuery, BaseResponse<ClassNotificationDto>>, ClassNotificationHandler>();
builder.Services.AddScoped<IRequestHandler<AvailableDateQuery, BaseResponse<IEnumerable<AvailableDateDto>>>, AvailableDateHandler>();
builder.Services.AddScoped<IRequestHandler<GetClassByIdQuery, BaseResponse<ClassDto>>, GetClassByIdHandler>();
builder.Services.AddScoped<IRequestHandler<GetClassesQuery, BaseResponse<PaginatedResult<ClassDto>>>, GetClassesHandler>();
builder.Services.AddScoped<IRequestHandler<CreateNotificationCommand,BaseResponse<ClassNotificationDto>>, CreateNotificationHandler>();
builder.Services.AddScoped<IRequestHandler<CreateClassCommand,BaseResponse<ClassDto>>, CreateClassHandler>();
builder.Services.AddScoped<IRequestHandler<CreateStudySlotCommand,BaseResponse<StudySlotDto>>, CreateStudySlotHandler>();
builder.Services.AddScoped<IRequestHandler<AddAvailableDateCommand,BaseResponse<IEnumerable<AvailableDateDto>>>, AddAvailableDateHandler>();
builder.Services.AddScoped<IRequestHandler<SignUpCommand,BaseResponse<UserDto>>, SignUpHandler>();
builder.Services.AddScoped<IRequestHandler<VerifyEmailCommand,BaseResponse<bool>>, VerifyEmailHandler>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

//Service
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddTransient<IMailService, MailService>();
// Validator
builder.Services.AddScoped<IValidator<AuthCommand>, AuthCommandValidator>();
builder.Services.AddScoped<IValidator<CreateNotificationCommand>, CreateNotificationCommandValidator>();

//Behaviour registration
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

//config mapper
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<UserMapper>();
    cfg.AddProfile<UserProfile>();
    cfg.AddProfile<ScheduleMapper>();
    cfg.AddProfile<ScheduleProfile>();
    cfg.AddProfile<StudySlotMapper>();
    cfg.AddProfile<StudySlotProfile>();
    cfg.AddProfile<AvailableDateMapper>();
    cfg.AddProfile<AvailableDateProfile>();
    cfg.AddProfile<ClassMapper>();
    cfg.AddProfile<ClassProfile>();
});
var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("corsapp");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
