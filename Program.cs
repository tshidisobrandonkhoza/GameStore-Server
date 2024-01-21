//configure services
using GameStore.Server.Models;
using Microsoft.Extensions.Options;



//Just a list of games
List<Game> games = new(){
    new Game(){
     Id= 1,
     Name = "Street Fighter",
     Genre = "Fighting",
     Price = 19.99M,
     ReleasedDate = new DateTime(2000,2,1)
    },  new Game(){
     Id= 2,
     Name = "Final Fantasy",
     Genre = "Playing",
     Price = 29.99M,
     ReleasedDate = new DateTime(2007,7,16)
    },  new Game(){
     Id= 3,
     Name = "Fifa",
     Genre = "Soccer",
     Price = 49.99M,
     ReleasedDate = new DateTime(2020,4,12)
    }
   };



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
var app = builder.Build();

//allow the app to use the CORS SERVICES
app.UseCors();

//GET METHOD | Index
app.MapGet("/", () => "Hello, we have succesfully reached the index, Happy!!");


//grouping routes for | Games - 

// var group = app.MapGroup("/games").WithParameterValidation();
var group = app.MapGroup("/games");
//GET METHOD | Games
group.MapGet("/", () => games);
//GET METHOD | A single | Game
group.MapGet("/{id}", (int id) =>
{
   //declare a variable with a Game DataType
   Game? game = games.Find(game => game.Id == id);

   if (game is null)
   {
      return Results.NotFound();
   }
   return Results.Ok(game);
}).WithName("singleGame");
//POST METHOD | A single | Game
group.MapPost("/", (Game game) =>
{
   game.Id = games.Max(game => game.Id) + 1;
   games.Add(game);

   //args 1 - name of the route to retrieve a Game 
   //args 2 - define ID with an anonymouse object without an explicit class
   //args 3 - object that waas created 
   return Results.CreatedAtRoute("singleGame", new { id = game.Id }, game);
});

group.MapPut("/{id}", (int id, Game updatedGame) =>
{

   Game? existingGame = games.Find(game => game.Id == id);


   if (existingGame is null)
   {
      updatedGame.Id = id;
      games.Add(updatedGame);
      return Results.CreatedAtRoute("singleGame", new { id = updatedGame.Id }, updatedGame);
   }

   existingGame.Name = updatedGame.Name;
   existingGame.Genre = updatedGame.Genre;
   existingGame.Price = updatedGame.Price;
   existingGame.ReleasedDate = updatedGame.ReleasedDate;

   return Results.NoContent();

});

group.MapDelete("/{id}", (int id) =>
{
   Game? game = games.Find(game => game.Id == id);

   if (game is null)
   {
      return Results.NoContent();
   }
   games.Remove(game);

   return Results.NoContent();

});
app.Run();