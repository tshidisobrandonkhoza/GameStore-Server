//configure services
using GameStore.Server.Data;
using GameStore.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;




var builder = WebApplication.CreateBuilder(args);
//using builder to configure services with CORS and allow it to accept options arg1
builder.Services.AddCors(Options => Options.AddDefaultPolicy(builder =>
{
   //build the additional origin outside of the REST API origin
   builder.WithOrigins("http://localhost:5198")
   .AllowAnyHeader()
   .AllowAnyMethod();
})
);

//connecting to database string - appsettings ConnectionString
var connString = builder.Configuration.GetConnectionString("GameStoreContext");

//pass in the connection string
builder.Services.AddSqlServer<GameStoreContext>(connString);

var app = builder.Build();

//allow the app to use the CORS SERVICES 
app.UseCors();

//GET METHOD | Index
app.MapGet("/", () => "Hello, we have succesfully reached the index, Happy!!");


//grouping routes for | Games - 

// var group = app.MapGroup("/games").WithParameterValidation();
var group = app.MapGroup("/games");
//GET METHOD | Games
//Receive an instance of the gamestore context
// Because Entity Frame work keeps track of the in and outs of data will use .AsTracking()
group.MapGet("/", async (GameStoreContext context) => await context.Games.AsNoTracking().ToListAsync());
//GET METHOD | A single | Game
group.MapGet("/{id}", async (int id, GameStoreContext context) =>
{
   //declare a variable with a Game DataType
   //  Game? game = games.Find(game => game.Id == id);
   Game? game = await context.Games.FindAsync(id);
   if (game is null)
   {
      return Results.NotFound();
   }
   return Results.Ok(game);
}).WithName("singleGame");
//POST METHOD | A single | Game
group.MapPost("/", async (Game game, GameStoreContext context) =>
{
   //  game.Id = games.Max(game => game.Id) + 1;
   //games.Add(game);
   context.Games.Add(game);
   await context.SaveChangesAsync();

   //args 1 - name of the route to retrieve a Game 
   //args 2 - define ID with an anonymouse object without an explicit class
   //args 3 - object that waas created 
   return Results.CreatedAtRoute("singleGame", new { id = game.Id }, game);
});

group.MapPut("/{id}", async (int id, Game updatedGame, GameStoreContext context) =>
{

   // Game? existingGame = games.Find(game => game.Id == id);
   //updated
   Game? existingGame = await context.Games.FindAsync(id);

   // if (existingGame is null)
   // {
   //    return Results.NotFound();
   // }

   // existingGame.Name = updatedGame.Name;
   // existingGame.Genre = updatedGame.Genre;
   // existingGame.Price = updatedGame.Price;
   // existingGame.ReleasedDate = updatedGame.ReleasedDate;
   // await context.SaveChangesAsync();

   // return Results.NoContent();
   var rowsAffected = await context.Games.Where(game => game.Id == id)
   .ExecuteUpdateAsync(update => (

      update.SetProperty(game => game.Name, updatedGame.Name)
      .SetProperty(game => game.Genre, updatedGame.Genre)
      .SetProperty(game => game.Price, updatedGame.Price)
      .SetProperty(game => game.ReleasedDate, updatedGame.ReleasedDate)

));

   return rowsAffected == 0 ? Results.NotFound() : Results.NoContent();
});

group.MapDelete("/{id}", async (int id, GameStoreContext context) =>
{
   // Game? game = games.Find(game => game.Id == id);

   // if (game is null)
   // {
   //    return Results.NoContent();
   // }
   // games.Remove(game);

   // return Results.NoContent();

   var rowsAffected = await context.Games.Where(game => game.Id == id)
   .ExecuteDeleteAsync();

   return Results.NoContent();
});
app.Run();