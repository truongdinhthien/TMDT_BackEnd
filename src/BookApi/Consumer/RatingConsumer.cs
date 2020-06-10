using System;
using System.Threading.Tasks;
using BookApi.Persistence;
using MassTransit;
using MessageBus.Message;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Consumer
{
    public class RatingConsumer : IConsumer<RatingMessage>
    {
        private readonly BookContext _context;

        public RatingConsumer (BookContext context)
        {
            _context = context;
        }
        public async Task Consume(ConsumeContext<RatingMessage> context)
        {
            var data = context.Message;

            Console.WriteLine("go");

            var bookResource = await _context.Books.FirstOrDefaultAsync(b => b.BookId == data.BookId);
            
            if (bookResource != null)
            {
                switch (data.Rating)
                {
                    case 1 : bookResource.Rate1++; break;
                    case 2 : bookResource.Rate2++; break;
                    case 3 : bookResource.Rate3++; break;
                    case 4 : bookResource.Rate4++; break;
                    case 5 : bookResource.Rate5++; break;
                    default:  break;
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}