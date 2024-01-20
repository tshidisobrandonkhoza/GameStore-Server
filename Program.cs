using GameStore.Server.Models;

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
var app = builder.Build();
//GET METHOD | Index
app.MapGet("/", () => "Hello, we have succesfully reached the index, Happy!!");

//GET METHOD | Games
app.MapGet("/games", () => games);

//GET METHOD | A single Game
app.MapGet("/games/{id}", (int id) =>
{
   //declare a variable with a Game DataType
   Game game = games.Find(game => game.Id == id);

   if (game is null)
   {
      return Results.NotFound();
   }
   return Results.Ok(game);
});

app.Run();