using System;
using System.Data.Entity;

namespace CartoSys.Models {

    public class MovieDBContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Applications> Applications { get; set; }

        public DbSet<Flow> Flows { get; set; }

        public DbSet<FlowType> FlowTypes { get; set; }

    }

    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
    }
}