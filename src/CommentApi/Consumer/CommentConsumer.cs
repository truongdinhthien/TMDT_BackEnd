using System;
using System.Threading.Tasks;
using CommentApi.Data;
using CommentApi.Models;
using MassTransit;
using MessageBus.Message;
using Microsoft.EntityFrameworkCore;

namespace CommentApi.Consumer
{
    public class CommentConsumer : IConsumer<CommentMessage>
    {
        private readonly CommentContext _context;

        public CommentConsumer (CommentContext context)
        {
            _context = context;
        }

        public async  Task Consume(ConsumeContext<CommentMessage> context)
        {
            var data = context.Message;
            Console.WriteLine($"{data.Content}");

            var comment = new Comment() {
                CommentId = data.CommentId,
                BuyerId = data.BuyerId,
                UserId = data.UserId,
                BookId = data.BookId,
                Rating = data.Rating,
                Content = data.Content,
                CreatedDate = new DateTime()
            };
            await _context.AddAsync(comment);
            await _context.SaveChangesAsync();
        }
    }
}