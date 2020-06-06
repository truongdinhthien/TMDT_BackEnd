using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using BookApi.Persistence;
using BookApi.Core.Fillter;
using BookApi.Core;
using BookApi.Core.Entity;
using BookApi.Core.DTOs;
using Microsoft.AspNetCore.Http;
using System.IO;
using AutoMapper;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookApi.Services
{
    public class BookService : ControllerBase
    {
        private readonly BookContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;

        public BookService(BookContext context, IWebHostEnvironment environment, IMapper mapper)
        {
            _context = context;
            _environment = environment;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetBookAsync(Pagination pagination, FillterBook fillterBook)
        {
            //Mapper
            var booksSoruce = await _context.Books.Include(b => b.Category).ToListAsync();
            var books = _mapper.Map<IEnumerable<Book>, IEnumerable<BookDTO>>(booksSoruce);

            //Fill by UserId
            if(fillterBook.UserId != "")
            {
                books = books.Where(b => b.UserId == fillterBook.UserId).ToList();
            }

            //Fill by Category
            if (fillterBook.CategoryId != -1)
            {
                books = books.Where(b => b.Category.CategoryId == fillterBook.CategoryId).ToList();
            }
            //Fill by name
            if (fillterBook.Name != "")
            {
                books = books.Where(b => b.Name.ToLower().Contains(fillterBook.Name.ToLower())).ToList();
            }
            //Fill by price
            if (fillterBook.PriceFrom != -1 || fillterBook.PriceTo != int.MaxValue)
            {
                books = books.Where(b => b.Price >= fillterBook.PriceFrom && b.Price <= fillterBook.PriceTo).ToList();
            }
            //Sort asc
            if (fillterBook.sortAsc != "")
            {
                try
                {
                    books = books.OrderBy(b => b.GetType().GetProperty(fillterBook.sortAsc).GetValue(b)).ToList();
                }
                catch (NullReferenceException)
                {
                    return BadRequest(new { success = false, message = fillterBook.sortAsc + " Không hợp lệ " });
                }
            }
            //Sort desc
            if (fillterBook.sortDesc != "")
            {
                try
                {
                    books = books.OrderByDescending(b => b.GetType().GetProperty(fillterBook.sortDesc).GetValue(b)).ToList();
                }
                catch (NullReferenceException)
                {
                    return BadRequest(new { success = false, message = fillterBook.sortDesc + " Không hợp lệ " });
                }
            }
            return Ok(PaginatedList<BookDTO>.Create(books, pagination.current, pagination.pageSize));
        }
        public async Task<IActionResult> GetBookByIdAsync(int id)
        {
            var booksSoruce = await _context.Books.Include(b => b.Category).Where(b => b.BookId == id).ToListAsync();

            var bookSoruce = booksSoruce.SingleOrDefault();
            var book = _mapper.Map<Book, BookDTO>(bookSoruce);
            if (book != null)
            {
                return Ok(new { success = true, data = book });
            }
            return BadRequest(new { success = false, message = "Không tìm thấy theo Id" });
        }
        public async Task<IActionResult> PostBookAsync(Book book)
        {
            //Check images
            if (book.Images == null && book.Images.Count == 0)
            {
                if (book.Images.Count < 6)
                {
                    //Check  is  image
                    foreach (IFormFile image in book.Images)
                    {
                        if (!isImage(image))
                            return BadRequest(new { success = false, message = "Ảnh phải nhỏ hơn 2 MB và đúng định dạng .jpg .png .jpeg .gif" });
                    }
                }
                else return BadRequest(new { success = false, message = "Chỉ tối đa 5 ảnh" });

                return BadRequest(new { success = false, message = "Vui lòng thêm hình ảnh" });
            }
            //Check Slug 
            var slug = await _context.Books.Where(b => b.Slug == book.Slug).ToListAsync();

            if(slug.Count() != 0)
            {
                return BadRequest(new { success = false, message = "slug bị trùng" });
            }

            // if (book.CategoryId == 0) return BadRequest (new {success = false, message = "Thêm danh mục sản phẩm"});

            var ImagePaths = convertImageToPath(book.Images);
            if (ImagePaths.Count == 0)
                return BadRequest(new { success = false, message = "Lỗi Sever" });

            book.ImagePaths = ImagePaths;
            await _context.AddAsync(book);
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Thêm thành công" });
        }

        public async Task<IActionResult> RateBookAsync(int id, int rate)
        {
            var bookResource = await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);
            
            if (bookResource != null)
            {
                if ( rate > 5 || rate < 1) return BadRequest(new {success = false, message ="Rate is range 1 to 5"});
                switch (rate)
                {
                    case 1 : bookResource.Rate1++; break;
                    case 2 : bookResource.Rate2++; break;
                    case 3 : bookResource.Rate3++; break;
                    case 4 : bookResource.Rate4++; break;
                    case 5 : bookResource.Rate5++; break;
                    default:  break;
                }
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(new { success = false, message = "Cant not find id"});
        }

        public async Task<IActionResult> PutBookAsync(int id, Book book)
        {
            var bookResource = await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);

            if(bookResource != null)
            {
                bookResource.Content = book.Content;
                await _context.SaveChangesAsync();
            }

            
            return Ok();
        }
        public List<String> convertImageToPath(List<IFormFile> file)
        {
            var ImagePaths = new List<string>();
            try
            {
                if (file != null && file.Count > 0)
                {

                    //Tạo folder Images
                    if (!Directory.Exists(_environment.WebRootPath + "\\Images\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\Images\\");
                    }
                    foreach (IFormFile Image in file)
                    {
                        string uploadsFolder = Path.Combine(_environment.WebRootPath, "Images");

                        //Tạo file name
                        var fileName = Image.FileName.Trim();
                        string name = Guid.NewGuid().ToString() + "_" + fileName;

                        //Tạo image PAth lưu db
                        var imagePath = "Images/" + name;
                        ImagePaths.Add(imagePath);

                        //Lưu ảnh vào wwwroot
                        string filePath = Path.Combine(uploadsFolder, name);
                        Image.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                    return ImagePaths;
                }
                else
                {
                    return ImagePaths;
                }
            }
            catch (Exception)
            {
                return ImagePaths;
            }

        }

        public bool isImage(IFormFile postedFile)
        {
            int ImageMinimumBytes = 2048;
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                    postedFile.ContentType.ToLower() != "image/jpeg" &&
                    postedFile.ContentType.ToLower() != "image/pjpeg" &&
                    postedFile.ContentType.ToLower() != "image/gif" &&
                    postedFile.ContentType.ToLower() != "image/x-png" &&
                    postedFile.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.OpenReadStream().CanRead)
                {
                    return false;
                }
                //------------------------------------------
                //check whether the image size exceeding the limit or not
                //------------------------------------------ 
                if (postedFile.Length < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                postedFile.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            try
            {
                using (var bitmap = new System.Drawing.Bitmap(postedFile.OpenReadStream()))
                {
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                postedFile.OpenReadStream().Position = 0;
            }

            return true;
        }
    }
}