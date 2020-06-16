using System;
using System.Threading.Tasks;
using BookApi.Persistence;
using MassTransit;
using MessageBus.Message;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Consumer
{
    public class AddAmountConsumer : IConsumer<AddAmountMessage>
    {
        private readonly BookContext _context;

        public AddAmountConsumer (BookContext context)
        {
            _context = context;
        }
        public async Task Consume(ConsumeContext<AddAmountMessage> context)
        {
            var data = context.Message;

            Console.WriteLine(data.BookId);

            var bookResource = await _context.Books.FirstOrDefaultAsync(b => b.BookId == data.BookId);

            if(bookResource != null)
            {
                if(data.isAdd == true)
                {
                    bookResource.Amount -= data.Amount;
                    await _context.SaveChangesAsync();
                }
                else 
                {
                    bookResource.Amount += data.Amount;
                    await _context.SaveChangesAsync();
                }
                
            }
        }
    }
}