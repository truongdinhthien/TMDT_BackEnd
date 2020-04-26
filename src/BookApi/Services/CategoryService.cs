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
using System.Threading.Tasks;

namespace BookApi.Services
{
    public class CategoryService : ControllerBase
    {
        private readonly BookContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;

        public CategoryService (BookContext context,IWebHostEnvironment environment,IMapper mapper)
        {
            _context = context;
            _environment = environment;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetCategoryAsync (FillterCategory fillterCategory)
        {
            var categorySoruce = await _context.Categorys.ToListAsync();
            var categorys = _mapper.Map<IEnumerable<Category>,IEnumerable<CategoryDTO>>(categorySoruce);

            if(fillterCategory.Name != "")
            {
                categorys = categorys.Where(c => c.Name.ToLower().Contains(fillterCategory.Name.ToLower())).ToList();
            }
            return Ok(new {success = true, data = categorys});
        }
        public async Task<IActionResult> GetCategoryByIdAsync (int id)
        {
            var categorysSoruce = await _context.Categorys.Where(c => c.CategoryId == id).ToListAsync();
            var categorySource = categorysSoruce.SingleOrDefault();
            
            var category = _mapper.Map<Category, CategoryDTO>(categorySource);
            if(category != null)
            {
                return Ok(new {success = true, data = category});
            }
            return BadRequest(new {success = false, message = "Không tìm thấy theo Id"});
        }
        
    }
}