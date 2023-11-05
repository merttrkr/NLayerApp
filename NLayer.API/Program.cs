using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Filters;
using NLayer.API.Middlewares;
using NLayer.API.Modules;
using NLayer.Repository;
using NLayer.Service.Mapping;
using NLayer.Service.Validations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>  options.Filters.Add(new ValidateFilterAttribute()));

//Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ProductDtoValidator>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

// because NotFoundFilter has DI we are adding it to the services
builder.Services.AddScoped(typeof(NotFoundFilter<,>));

//model to modelDto mapping definition
builder.Services.AddAutoMapper(typeof(MapProfile));



builder.Services.AddDbContext<AppDbContext>(x =>
{   
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {   //retrieves the name of the assembly where the AppDbContext class is defined and uses it as the assembly for migrations.
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
    //x.UseNpgsql(builder.Configuration.GetConnectionString("NpgSqlConnection"));
});


builder.Host.UseServiceProviderFactory
    (new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>
    (containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// custom exception class
app.UseCustomException();

app.UseAuthorization();

app.MapControllers();

app.Run();
