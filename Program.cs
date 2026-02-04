var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Swagger（API確認用）
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Swagger（Dev環境のみ）
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Controller を有効化
app.MapControllers();

app.Run();

