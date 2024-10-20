using _01_ApiSemRedis.src.config.App;
using _01_ApiSemRedis.src.config.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.AddDependencies();

var app = builder.Build();
app.AddDependencies();

app.Run();
