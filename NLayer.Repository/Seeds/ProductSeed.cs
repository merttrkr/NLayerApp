using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Seeds
{
    public class ProductSeed : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                new Product
                {
                    Id = 1,
                    CategoryId = 1,
                    CreatedDate = DateTime.Now,
                    Price = 100,
                    Stock= 10,
                    Name = "kalem 1",

                },
                 new Product
                 {
                     Id = 2,
                     CategoryId = 1,
                     CreatedDate = DateTime.Now,
                     Price = 200,
                     Stock = 20,
                     Name = "kalem 2",

                 },
                  new Product
                  {
                      Id = 3,
                      CategoryId = 1,
                      CreatedDate = DateTime.Now,
                      Price = 200,
                      Stock = 30,
                      Name = "kalem 3",
                  },
                  new Product
                  {
                      Id = 4,
                      CategoryId = 2,
                      CreatedDate = DateTime.Now,
                      Price = 200,
                      Stock = 30,
                      Name = "kitap 1",
                  },
                  new Product
                  {
                      Id = 5,
                      CategoryId = 2,
                      CreatedDate = DateTime.Now,
                      Price = 200,
                      Stock = 30,
                      Name = "kitap 2",
                  }
               );
        }
    }
}
