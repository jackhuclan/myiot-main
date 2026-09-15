// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using NLog.Web;
using VgCNCServer.DAO;

namespace VgCNCWeb;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.


        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContextFactory<VegaContext>(optionsBuilder =>
        {
            optionsBuilder.EnableSensitiveDataLogging(true);
            string? connection = builder.Configuration.GetSection("ConnectionSetting:ConnectionString").Value;
            if (!string.IsNullOrWhiteSpace(connection))
            {
                optionsBuilder.UseMySQL(connection, providerOptions => providerOptions.CommandTimeout(60));
            }
        });

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Host.UseNLog();

        builder.Services.AddWindowsService();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        app.UseSwagger();
        app.UseSwaggerUI();
        //}



        app.UseStaticFiles();
        app.MapControllers();




        app.Run();
    }
}
