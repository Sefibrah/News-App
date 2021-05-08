using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers {
    public class ArticlesController : BaseApiController {
        private readonly DataContext _context;
        public ArticlesController(DataContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles() {
            return await _context.Articles.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id) {
            return await _context.Articles.SingleOrDefaultAsync(u => u.Id == id);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Article>>> SearchArticles([FromQuery] string query) {
            return await _context.Articles
                .Where(a => a.Title.ToLower().Contains(query.ToLower()))
                .Where(a => a.Content.ToLower().Contains(query.ToLower()))
                .ToListAsync();
        }
    }
}