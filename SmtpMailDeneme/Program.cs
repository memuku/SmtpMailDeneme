﻿using MailKit;
using Microsoft.Extensions.Configuration;
using SmtpMailDeneme.Configuration;
using SmtpMailDeneme.Data;
using SmtpMailDeneme.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection(nameof(EmailSettings)));
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddSingleton(typeof(MongoDBService<VerificationCode>));
builder.Services.AddTransient<IVerificationCodeService, VerificationCodeService>();
builder.Services.AddHostedService<VerificationCodeCleanupService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

