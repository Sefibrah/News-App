using System;
using System.Threading.Tasks;
using api.Data;
using api.DTOs;
using api.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers {
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseApiController {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public AdminController(DataContext context, IMapper mapper) {
            _mapper = mapper;
            _context = context;
        }

        [HttpPost("article/create")]
        public async Task<ActionResult<Article>> CreateArticle(ArticleDto articleDto) {
            var article = _mapper.Map<Article>(articleDto);
            article.PostDate = DateTime.Now;
            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();
            return article;
        }

        [HttpPut("article/update")]
        public async Task<ActionResult> UpdateArticle(ArticleUpdateDto article) {
            var articleInDb = await _context.Articles.SingleOrDefaultAsync(a => a.Id == article.Id);
            if (articleInDb == null) return BadRequest("Article doesn't exist");
            var articleUpdated = _mapper.Map(article, articleInDb);
            _context.Articles.Update(articleUpdated);
            await _context.SaveChangesAsync();
            return Ok("Update successful.");
        }

        [HttpDelete("article/delete/{id}")]
        public async Task<ActionResult> DeleteArticle(int id) {
            var articleInDb = await _context.Articles.SingleOrDefaultAsync(a => a.Id == id);
            if (articleInDb == null) return BadRequest("Article doesn't exist");
            _context.Articles.Remove(articleInDb);
            await _context.SaveChangesAsync();
            return Ok("Deletion successful.");
        }
    }
}