using System.Text.Json;
using System.Text.Json.Serialization;
using Server.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
    options.AddPolicy("SoftGEPEC", policy => {
        policy.WithOrigins("*", "*")
            .AllowAnyHeader()
            .AllowAnyMethod().
            SetIsOriginAllowed(origin => true)
            .AllowCredentials();
    });
});

builder.Services.AddAuthentication(options => { 
        options.DefaultScheme = "Auth_cookie"; 
    }).AddCookie("Auth_cookie", options => {
        options.Cookie.Name = "fhq3rhcDG";
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        options.Cookie.HttpOnly = true;
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<DepartmentRepository>();
builder.Services.AddScoped<ServiceRepository>();
builder.Services.AddScoped<PasswordRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddScoped<SkillsGroupRepository>();
builder.Services.AddScoped<SkillsRepository>();
var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseCors("SoftGEPEC");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


