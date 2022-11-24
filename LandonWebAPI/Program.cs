using LandonWebAPI.DataAccess;
using LandonWebAPI.Filters;
using LandonWebAPI.Infrastructure.Mapper;
using LandonWebAPI.Models.DTOs;
using LandonWebAPI.Models.Generic;
using LandonWebAPI.Models.Options;
using LandonWebAPI.Services.Abstract;
using LandonWebAPI.Services.Concretes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<HotelInfo>(
    builder.Configuration.GetSection("Info"));
builder.Services.Configure<HotelOptions>(builder.Configuration);
builder.Services.Configure<PagingOptions>(builder.Configuration.GetSection("DefaultPagingOptions"));

builder.Services.AddScoped<IRoomService, DefaultRoomService>();
builder.Services.AddScoped<IOpeningService, DefaultOpeningService>();
builder.Services.AddScoped<IBookingService, DefaultBookingService>();
builder.Services.AddScoped<IDateLogicService, DefaultDateLogicService>();

builder.Services.AddDbContext<HotelApiDbContext>(options =>
{
    options.UseSqlServer("Server=EPTRANKW0038\\SQLEXPRESS;Database=HotelAPI;Trusted_Connection=True;");
});

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(options =>
    {
        options.Filters.Add<JsonExceptionFilter>();
        options.Filters.Add<RequireHttpsOrCloseAttribute>();
        options.Filters.Add<LinkRewritingFilter>();
    })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
        options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
        options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;

    });

builder.Services.AddRouting(options =>
    options.LowercaseUrls = true);

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader
        = new MediaTypeApiVersionReader();
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionSelector
         = new CurrentImplementationApiVersionSelector(options);
});

builder.Services.AddAutoMapper(
    options => options.AddProfile<MappingProfile>());

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errorResponse = new ApiError(context.ModelState);
        return new BadRequestObjectResult(errorResponse);
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
