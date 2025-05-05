using Classes_Compartilhadas.Data.Configurations;
using Classes_Compartilhadas.Services;
using LojaVirtualApi.Configuration;
var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers();
builder.AddApiConfig()
        .AddSwaggerConfig()
        .AddDbContextConfig()
        .AddIdentityConfig();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<VendedorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//popula a base de dados na maquina 
app.UseDbMigrationHelper();

app.Run();
