using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.Server.Data.Configurations
{
    // Implemented IEntityTypeConfiguration and its interface
    public class GameConfigurations : IEntityTypeConfiguration<Game>
    {
        //methods to configure any properties of the game model 

        public void Configure(EntityTypeBuilder<Game> builder)
        {
            //What property you want to configure
            builder.Property(game => game.Price)
            .HasPrecision(18,2);
        }
    }
}