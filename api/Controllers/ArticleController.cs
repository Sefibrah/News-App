using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers {
    [AllowAnonymous]
    public class ArticleController : BaseApiController {
        private readonly DataContext _context;
        public ArticleController(DataContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles() {
            return await _context.Articles.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id) {
            return await _context.Articles.SingleOrDefaultAsync(a => a.Id == id);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Article>>> SearchArticle([FromQuery] string query) {
            return await _context.Articles.Where(a => a.Content.ToLower().Contains(query) || a.Title.ToLower().Contains(query)).ToListAsync();
        }
    }
}