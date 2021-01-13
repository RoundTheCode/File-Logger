using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RoundTheCode.FileLogger.Api.Services;
using RoundTheCode.FileLogger.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RoundTheCode.FileLogger.Api.HostedServices
{
    public class MostViewedArticleHostedService : IHostedService
    {
        protected IServiceProvider _serviceProvider;
        protected IMostViewedArticleService _mostViewedArticleService;
        protected ILogger<MostViewedArticleHostedService> _logger;

        public MostViewedArticleHostedService([NotNull] IServiceProvider serviceProvider, [NotNull] IMostViewedArticleService mostViewedArticleService,
            [NotNull] ILogger<MostViewedArticleHostedService> logger
            )
        {
            _serviceProvider = serviceProvider;
            _mostViewedArticleService = mostViewedArticleService;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await UpdateMostViewedArticlesAsync();
                    await Task.Delay(new TimeSpan(0, 1, 0), cancellationToken);
                }
            });

            return Task.CompletedTask;
        }

        protected async Task UpdateMostViewedArticlesAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var fileLoggerDbContext = (FileLoggerDbContext)scope.ServiceProvider.GetRequiredService(typeof(FileLoggerDbContext));
                var timeFrom = DateTimeOffset.Now.AddSeconds(-60);

                _logger.LogInformation("Run background task");

                _mostViewedArticleService.MostViewedArticles = await fileLoggerDbContext.Set<ArticleHit>()
                   .Join(
                       fileLoggerDbContext.Set<Article>(),
                       articleHit => articleHit.ArticleId,
                       article => article.Id,
                       (articleHit, article) => new { ArticleHit = articleHit, Article = article }
                   )
                   .Where(g => g.ArticleHit.Created >= timeFrom)
                   .GroupBy(g => g.Article.Id)
                   .Select(g => new MostViewedArticleView { ArticleId = g.Key, Title = g.Min(t => t.Article.Title), Hits = g.Count() })
                   .OrderByDescending(g => g.Hits)
                   .ToListAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
