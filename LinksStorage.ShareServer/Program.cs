using LinksStorage.ShareServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(o => o.AddDefaultPolicy(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials()));
builder.Services.AddSignalR();
builder.Services.AddSingleton<Cache>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors();

app.MapHub<ShareDataHub>("/share-data");

app.Run();
