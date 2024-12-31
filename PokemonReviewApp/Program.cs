using Microsoft.EntityFrameworkCore;
using PokemonReviewApp;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repository;
using PokemonReviewApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddTransient<Seed>(); //DependencyInjection: This provides Seed as a service to Program.cs
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());//Register our auto mapper as a service for our app
builder.Services.AddScoped<IPokemonRepository, PokemonRepository>(); //Register our repo interface as an injectable service, and provides the actual repo for injection
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewerRepository, ReviewerRepository>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CountryService>();
builder.Services.AddScoped<OwnerService>();
builder.Services.AddScoped<PokemonService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register our DbContext class in our DI Container (builder.services), configure it to use SqlServer, and provide Connection string from config file.
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata") //Args refers to arguments passed when running program.cs, dotnet run args
    SeedData(app);

void SeedData(IHost app) //Method to actually seed our database with data 
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        //Injecting our Seed service via GetService, and using its SeedDataContext to save its data to the given context
        var service = scope.ServiceProvider.GetService<Seed>(); //Grab Seed service provided earlier
        service.SeedDataContext(); //Execute seed's SeedDataContext method, which actually seeds DB with data.
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
