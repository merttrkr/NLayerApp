using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Seeds
{
    public class CategorySeed : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(new Category { Id = 1, Name = "kalemler" },
                new Category { Id = 2, Name = "kitaplar" },
                new Category { Id = 3, Name = "çantalar" },
                new Category { Id = 4, Name = "defterler" },
                new Category { Id = 5, Name = "kıyafetler" });
        }
    }
}
