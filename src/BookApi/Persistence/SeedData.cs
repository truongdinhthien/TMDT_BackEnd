using System;
using System.Linq;
using BookApi.Core.Entity;
namespace BookApi.Persistence
{
    public class SeedData
    {
        public static async void Initialize ( BookContext context)
        {
            context.Database.EnsureCreated();
            
            if (!context.Books.Any())
            {
                //Add Category
                for (int i = 1;i<=10;i++)
                {
                    await context.Categorys.AddAsync(
                        new Category () {
                            Name = "Category Book " + i,
                            Slug = "slug-category-" + i,
                        }
                    );
                }
                await context.SaveChangesAsync();
                
                //Add Book
                for (int i = 1;i<=100;i++)
                {
                    await context.Books.AddAsync(
                        new Book () {
                            Name = "Book " + i,
                            Price = new Random().Next(1,10) * 10000,
                            Amount = new Random().Next(20,30),
                            Slug = "slug-book-" + i,
                            Content = "This is content " + i,
                            Author = "Author - " + new Random().Next(1,10),
                            Publisher = "Publisher - " + new Random().Next(1,10),
                            Rate1 = new Random().Next(1,2),
                            Rate2 = new Random().Next(1,3),
                            Rate3 = new Random().Next(1,4),
                            Rate4 = new Random().Next(1,8),
                            Rate5 = new Random().Next(1,15),
                            ImagePaths = {"Images/book1.jpg", "Images/book2.jpg", "Images/book3.jpg", "Images/book4.jpg", "Images/book5.jpg"},
                            CategoryId = new Random().Next(1,10)
                    }
                );
            }
                // context.Books.AddRange(
                //     new Book () {
                //         Name = "Book 1",
                //         Price = 50000,
                //         Rating = 5,
                //         Slug = "slug-1",
                //         Content = "This is content 1",
                //         ImagePath = "Images/book1.jpg"
                //     },
                //     new Book () {
                //         Name = "Book 2",
                //         Price = 70000,
                //         Rating = 4,
                //         Slug = "slug-2",
                //         Content = "This is content 2",
                //         ImagePath = "Images/book2.jpg"
                //     },
                //     new Book () {
                //         Name = "Book 3",
                //         Price = 90000,
                //         Rating = 3,
                //         Slug = "slug-3",
                //         Content = "This is content 3",
                //         ImagePath = "Images/book3.jpg"
                //     },
                //     new Book () {
                //         Name = "Book 4",
                //         Price = 45000,
                //         Rating = 4,
                //         Slug = "slug-4",
                //         Content = "This is content 4",
                //         ImagePath = "Images/book4.jpg"
                //     },
                //     new Book () {
                //         Name = "Book 5",
                //         Price = 35000,
                //         Rating = 5,
                //         Slug = "slug-5",
                //         Content = "This is content 5",
                //         ImagePath = "Images/book1.jpg"
                //     },
                //     new Book () {
                //         Name = "Book 6",
                //         Price = 90000,
                //         Rating = 5,
                //         Slug = "slug-6",
                //         Content = "This is content 6",
                //         ImagePath = "Images/book1.jpg"
                //     },
                //     new Book () {
                //         Name = "Book 7",
                //         Price = 110000,
                //         Rating = 4,
                //         Slug = "slug-7",
                //         Content = "This is content 7",
                //         ImagePath = "Images/book1.jpg"
                //     },
                //     new Book () {
                //         Name = "Book 8",
                //         Price = 49000,
                //         Rating = 2,
                //         Slug = "slug-8",
                //         Content = "This is content 8",
                //         ImagePath = "Images/book1.jpg"
                //     }
                // );
                await context.SaveChangesAsync();
            }
        }
    }
}