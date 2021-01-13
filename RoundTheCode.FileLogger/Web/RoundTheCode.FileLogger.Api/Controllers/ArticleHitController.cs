using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoundTheCode.FileLogger.Api.Services;
using RoundTheCode.FileLogger.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace RoundTheCode.FileLogger.Api.Controllers
{
    [ApiController]
    [Route("api/article-hit")]
    public class ArticleHitController : Controller
    {
        protected readonly FileLoggerDbContext _fileLoggerDbContext;
        protected readonly IMostViewedArticleService _mostViewedArticleService;
        protected readonly ILogger<ArticleHitController> _logger;

        public ArticleHitController([NotNull] FileLoggerDbContext fileLoggerDbContext, [NotNull] IMostViewedArticleService mostViewedArticleService, [NotNull] ILogger<ArticleHitController> logger)
        {
            _fileLoggerDbContext = fileLoggerDbContext;
            _mostViewedArticleService = mostViewedArticleService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(ArticleHit entity)
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/article-hit", "POST");

            await _fileLoggerDbContext.Set<ArticleHit>().AddAsync(entity);
            await _fileLoggerDbContext.SaveChangesAsync();

            _logger.LogTrace("Added new ArticleHit entity with Id {id}", entity.Id);

            return Ok(entity);
        }

        [HttpGet("most-viewed")]
        public IActionResult GetMostViewed()
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/article-hit/most-viewed", "GET");

            return Ok(_mostViewedArticleService.MostViewedArticles);
        }


    }
}
