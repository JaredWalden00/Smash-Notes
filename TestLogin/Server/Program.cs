global using TestLogin.Shared;
global using Microsoft.EntityFrameworkCore;
global using AutoMapper;
global using TestLogin.Server.BlogControlService;
using Microsoft.AspNetCore.ResponseCompression;
using TestLogin.Server.Data;
using TestLogin.Client.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using TestLogin.Server.CharacterControlService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSwaggerGen();
string ConnectionString = @"Server=smashnotes.database.windows.net; Authentication=Active Directory Interactive; Database=testlogin";
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(
        ConnectionString));
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen(c => {
	c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
	{
		Description = "Standard Authorization header using the Bearer scheme, e.g. \"bearer {token} \"",
		In = ParameterLocation.Header,
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey
	});

	c.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddRazorPages();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBlogControlService, BlogControlService>();
builder.Services.AddScoped<ICharacterControlService, CharacterControlService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options => {
		options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
			ValidateIssuer = false,
			ValidateAudience = false
		};
	});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blazor API V1");
});
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
