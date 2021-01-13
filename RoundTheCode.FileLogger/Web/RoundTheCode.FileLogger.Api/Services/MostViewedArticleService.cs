using RoundTheCode.FileLogger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoundTheCode.FileLogger.Api.Services
{
    public class MostViewedArticleService : IMostViewedArticleService
    {
        public IEnumerable<MostViewedArticleView> MostViewedArticles { get; set; }

    }
}
