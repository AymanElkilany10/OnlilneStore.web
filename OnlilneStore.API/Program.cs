using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Services;
using Services.Abstraction;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Services.MappingProfiles;
using OnlineStore.API.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;
using OnlineStore.API.Extenstions;

namespace OnlilneStore.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.RegisterAllServices(builder.Configuration);

            var app = builder.Build();

            await app.ConfigureMiddlewares();

            app.Run();
        }
    }
}
