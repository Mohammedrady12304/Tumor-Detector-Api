using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TumorDetector.Core.Entities;
using TumorDetector.infrastructure.Data;
using System.Text;
// Add services to the container.

var builder = WebApplication.CreateBuilder(args);
//Ïí ÚÔÇä ÇáÛí Çá validator ÇáÇÝÊÑÇÖí æ ÇÚãáå ÇäÇ ÈäÝÓí
//builder.Services.AddControllers().ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = true);



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});




builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(
                  builder.Configuration.GetConnectionString("Remote")
                ));
//----------------------------------------------------------------------------
//åäÇ ÈäÓÌá Çá userManger
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//هنا أنا أغير الإعدادات الافتراضية عشان تشتغل على token (JWT) بدل الـ cookie
builder.Services.AddAuthentication(o =>//verified key
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//ÚÔÇä ÊÑÏ Úáíß È unauthorized ÈÏá note found
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(o =>
{
    o.SaveToken = true;
    o.RequireHttpsMetadata = false;
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = "http://localhost:5286/",
        ValidateAudience = true,
        ValidAudience = "http://localhost:4200/",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("asdfASDF1230!!jfkadjkdlaj@#$!#$5"))   //åÏíáå Çá key ÈÇá byte 

    };
}


);


//----------------------------------------------------------------------------
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
