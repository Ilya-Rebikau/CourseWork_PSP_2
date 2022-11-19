using CourseWork.ComputingAPI.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAPIServices();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
