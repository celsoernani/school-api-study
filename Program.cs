using Microsoft.EntityFrameworkCore;
using SchoolApi.Data;

var builder = WebApplication.CreateBuilder(args);

// EF Core SQL Server
builder.Services.AddDbContext<SchoolApi.Data.SchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

// Add services to the container.
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

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// Seed database (runs only if empty)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SchoolDbContext>();
    await DbSeeder.SeedAsync(db);
}

// Institutions CRUD
app.MapGet("/institutions", async (SchoolApi.Data.SchoolDbContext db) =>
    await db.Institutions.AsNoTracking().ToListAsync());

app.MapGet("/institutions/{id:int}", async (int id, SchoolApi.Data.SchoolDbContext db) =>
    await db.Institutions.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id) is { } institution
        ? Results.Ok(institution)
        : Results.NotFound());

app.MapPost("/institutions", async (SchoolApi.Models.Institution institution, SchoolApi.Data.SchoolDbContext db) =>
{
    db.Institutions.Add(institution);
    await db.SaveChangesAsync();
    return Results.Created($"/institutions/{institution.Id}", institution);
});

app.MapPut("/institutions/{id:int}", async (int id, SchoolApi.Models.Institution input, SchoolApi.Data.SchoolDbContext db) =>
{
    var entity = await db.Institutions.FindAsync(id);
    if (entity is null) return Results.NotFound();
    entity.Name = input.Name;
    entity.Address = input.Address;
    entity.City = input.City;
    entity.State = input.State;
    entity.PostalCode = input.PostalCode;
    entity.Phone = input.Phone;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/institutions/{id:int}", async (int id, SchoolApi.Data.SchoolDbContext db) =>
{
    var entity = await db.Institutions.FindAsync(id);
    if (entity is null) return Results.NotFound();
    db.Institutions.Remove(entity);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Students CRUD
app.MapGet("/students", async (SchoolApi.Data.SchoolDbContext db) =>
    await db.Students.AsNoTracking().ToListAsync());

app.MapGet("/students/{id:int}", async (int id, SchoolApi.Data.SchoolDbContext db) =>
    await db.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id) is { } student
        ? Results.Ok(student)
        : Results.NotFound());

app.MapPost("/students", async (SchoolApi.Models.Student student, SchoolApi.Data.SchoolDbContext db) =>
{
    db.Students.Add(student);
    await db.SaveChangesAsync();
    return Results.Created($"/students/{student.Id}", student);
});

app.MapPut("/students/{id:int}", async (int id, SchoolApi.Models.Student input, SchoolApi.Data.SchoolDbContext db) =>
{
    var entity = await db.Students.FindAsync(id);
    if (entity is null) return Results.NotFound();
    entity.FirstName = input.FirstName;
    entity.LastName = input.LastName;
    entity.DateOfBirth = input.DateOfBirth;
    entity.PeopleInHousehold = input.PeopleInHousehold;
    entity.Notes = input.Notes;
    entity.InstitutionId = input.InstitutionId;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/students/{id:int}", async (int id, SchoolApi.Data.SchoolDbContext db) =>
{
    var entity = await db.Students.FindAsync(id);
    if (entity is null) return Results.NotFound();
    db.Students.Remove(entity);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
