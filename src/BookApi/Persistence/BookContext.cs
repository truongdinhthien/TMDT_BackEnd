using Microsoft.EntityFrameworkCore;
using BookApi.Core.Entity;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace BookApi.Persistence
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categorys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Write Fluent API configurations here

            //Property Configurations

            modelBuilder.Entity<Book>()
                        .Property(e => e.ImagePaths)
                        .HasConversion(
                            v => JsonConvert.SerializeObject(v),
                            v => JsonConvert.DeserializeObject<List<string>>(v));
        }

        public override int SaveChanges()
        {
            // var entries = ChangeTracker
            //     .Entries()
            //     .Where(e => e.Entity is Book && (
            //             e.State == EntityState.Added
            //             || e.State == EntityState.Modified));

            // foreach (var entityEntry in entries)
            // {
            //     ((Book)entityEntry.Entity).RateCount =  ((Book)entityEntry.Entity).Rate1 +
            //                                             ((Book)entityEntry.Entity).Rate2 +
            //                                             ((Book)entityEntry.Entity).Rate3 + 
            //                                             ((Book)entityEntry.Entity).Rate4 +
            //                                             ((Book)entityEntry.Entity).Rate5 ;

            //     if (entityEntry.State == EntityState.Added)
            //     {
            //         ((Book)entityEntry.Entity).Rating = ( ((Book)entityEntry.Entity).Rate1 ) * 1 +
            //                                             ( ((Book)entityEntry.Entity).Rate2 ) * 2 +
            //                                             ( ((Book)entityEntry.Entity).Rate3 ) * 3 + 
            //                                             ( ((Book)entityEntry.Entity).Rate4 ) * 4 +
            //                                             ( ((Book)entityEntry.Entity).Rate5 ) * 5;
            //     }
            // }

         return base.SaveChanges();
        }
    }
}