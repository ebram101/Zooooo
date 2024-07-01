using Microsoft.EntityFrameworkCore;
using Bogus;
using System.Collections.Generic;
using Zoo.Enums;
using Zoo.Models;

namespace Zoo.Data
{
    public static class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Mammals" },
                new Category { Id = 2, Name = "Reptiles" }
            };

            modelBuilder.Entity<Category>().HasData(categories);

            var enclosures = new List<Enclosure>
            {
                new Enclosure { Id = 1, Name = "Savannah Habitat", Climate = Climate.Tropical, HabitatType = HabitatType.Grassland, SecurityLevel = SecurityLevel.Medium, Size = 1000.0 },
                new Enclosure { Id = 2, Name = "Desert Habitat", Climate = Climate.Arid, HabitatType = HabitatType.Desert, SecurityLevel = SecurityLevel.High, Size = 500.0 }
            };

            modelBuilder.Entity<Enclosure>().HasData(enclosures);

            var faker = new Faker<Animals>()
                .RuleFor(a => a.Id, f => f.IndexFaker + 1)
                .RuleFor(a => a.Name, f => f.Name.FirstName())
                .RuleFor(a => a.Species, f => f.Random.Word())
                .RuleFor(a => a.CategoryId, f => f.PickRandom(categories).Id)
                .RuleFor(a => a.Size, f => f.PickRandom<CustomSize>())
                .RuleFor(a => a.DietaryClass, f => f.PickRandom<DietaryClass>())
                .RuleFor(a => a.ActivityPattern, f => f.PickRandom<ActivityPattern>())
                .RuleFor(a => a.Prey, f => f.Random.Word())
                .RuleFor(a => a.EnclosureId, f => f.PickRandom(enclosures).Id)
                .RuleFor(a => a.SpaceRequirement, f => f.Random.Double(50, 200))
                .RuleFor(a => a.SecurityRequirement, f => f.PickRandom<SecurityLevel>());

            var animals = faker.Generate(10); // Generate 10 random animals

            modelBuilder.Entity<Animals>().HasData(animals);
        }
    }
}
