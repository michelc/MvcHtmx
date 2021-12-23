using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MvcHtmx.Models
{
    public class SeedData
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            using var context = new MvcHtmxContext(scope.ServiceProvider.GetRequiredService<DbContextOptions<MvcHtmxContext>>());
            
            // Applique automatiquement les migrations
            context.Database.Migrate();

            // Rien à faire s'il existe déjà des données
            if (context.Movies.Any())
            {
                return;
            }

            // Ajoute quelques films pour démarrer
            var movies = new[] {
                    new Movie
                        {
                            Title = "When Harry Met Sally...",
                            ReleaseDate = DateTime.Parse("1989-7-14"),
                            Genre = "Romantic Comedy",
                            Price = 7.99M
                        },
                    new Movie
                        {
                            Title = "Ghostbusters",
                            ReleaseDate = DateTime.Parse("1984-6-8"),
                            Genre = "Comedy",
                            Price = 8.99M
                        },
                    new Movie
                        {
                            Title = "Ghostbusters II",
                            ReleaseDate = DateTime.Parse("1989-6-16"),
                            Genre = "Comedy",
                            Price = 9.99M
                        },
                    new Movie
                        {
                            Title = "Rio Bravo",
                            ReleaseDate = DateTime.Parse("1959-4-4"),
                            Genre = "Western",
                            Price = 3.99M
                        }
                };
            context.Movies.AddRange(movies);
            context.SaveChanges();
        }
    }
}
