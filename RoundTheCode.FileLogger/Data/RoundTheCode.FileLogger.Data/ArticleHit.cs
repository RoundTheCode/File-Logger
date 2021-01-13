using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoundTheCode.FileLogger.Data
{
    public class ArticleHit : Base
    {
        public int ArticleId { get; set; }

        public Article Article { get; set; }

        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleHit>()
                .HasOne(prop => prop.Article)
                .WithMany()
                .HasPrincipalKey(article => article.Id)
                .HasForeignKey(articleHit => articleHit.ArticleId);
        }
    }
}
