var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(c => c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin()));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

<<<<<<< HEAD


=======
>>>>>>> 1c3de640dea512340f26cc236ec7141f7e0ae131
app.UseCors(p => p
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

<<<<<<< HEAD
app.UseHttpLogging();

app.Run();
=======

app.Run();
>>>>>>> 1c3de640dea512340f26cc236ec7141f7e0ae131
