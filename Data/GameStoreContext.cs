using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GameStore.Server.Models;
using Microsoft.EntityFrameworkCore;


namespace GameStore.Server.Data
{
    //Inherits the DbContext - provided by the MS-EFC
    public class GameStoreContext : DbContext
    {
        //Constructor takes in as parameter instance named option
        //Send the Options over to the Base Class - Details to connect to SQL Server
        public GameStoreContext(DbContextOptions<GameStoreContext> options) : base(options)
        {

        }
        //property that defines a collection of Game Entities
        //use to Query any data from SQL Sever database
        public DbSet<Game> Games => Set<Game>();

        //Registering GameConfiguration Class to be compact with the migration Generated
        //Overriding a method
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Applying any configurations defined accross the project
            //Assembly.GetExecutingAssembly() will help us find every single configuration 
            //and apply it as part of the migration
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}