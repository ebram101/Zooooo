using Bogus;
using Zoo.Data;
using Zoo.Enums;
using Zoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Zoo.Data
{
    // Seeder class to seed the database with data
    public class ZooSeeder
    {
        private readonly ZooContext _context;

        // Reference to the dbcontext
        public ZooSeeder(ZooContext context)
        {
            _context = context;
        }

        public void DataSeeder()
        {
            // Create the database if it doesn't exist
            _context.Database.EnsureCreated();

            if (!_context.Animals.Any())
            {
                try
                {
                    // Create a list of categories
                    var categoryNames = new[] { "Mammals", "Birds", "Reptiles", "Fish", "Amphibians", "Insects", "Arachnids" };
                    var categories = categoryNames.Select(name => new Category { Name = name }).ToList();

                    // Add the categories to the database
                    _context.Category.AddRange(categories);
                    _context.SaveChanges();

                    // Create a list of enclosures
                    var enclosureFaker = new Faker<Enclosure>()
                        .RuleFor(e => e.Name, f => f.Lorem.Word())
                        .RuleFor(e => e.Climate, f => f.PickRandom<Climate>())
                        .RuleFor(e => e.HabitatType, f => f.PickRandom<HabitatType>())
                        .RuleFor(e => e.SecurityLevel, f => f.PickRandom<SecurityLevel>())
                        .RuleFor(e => e.Size, f => f.Random.Double(20, 200));

                    // Generate the enclosures
                    var enclosures = enclosureFaker.Generate(5);
                    _context.Enclosure.AddRange(enclosures);
                    _context.SaveChanges();

                    // Verify that categories and enclosures are successfully added
                    var allCategories = categories.Select(c => c.Id).ToList();
                    var allEnclosures = enclosures.Select(c => c.Id).ToList();

                    // Create a list of animals
                    var animalFaker = new Faker<Animals>()
                        .RuleFor(a => a.Name, f => f.Name.FirstName())
                        .RuleFor(a => a.Species, f => f.Lorem.Word())
                        .RuleFor(a => a.Size, f => f.PickRandom<CustomSize>())
                        .RuleFor(a => a.DietaryClass, f => f.PickRandom<DietaryClass>())
                        .RuleFor(a => a.ActivityPattern, f => f.PickRandom<ActivityPattern>())
                        .RuleFor(a => a.Prey, f => f.Lorem.Word())
                        .RuleFor(a => a.CategoryId, f => f.PickRandom(allCategories))
                        .RuleFor(a => a.EnclosureId, f => f.PickRandom(allEnclosures))
                        .RuleFor(a => a.SpaceRequirement, f => f.Random.Double(1, 20))
                        .RuleFor(a => a.SecurityRequirement, f => f.PickRandom<SecurityLevel>());

                    var animals = animalFaker.Generate(20);
                    _context.Animals.AddRange(animals);
                    _context.SaveChanges();

                    // Assign a random prey to each animal
                    var random = new Random();
                    foreach (var animal in animals)
                    {
                        var potentialPrey = animals.Where(a => a.Id != animal.Id).ToList();
                        if (potentialPrey.Any())
                        {
                            int randomIndex = random.Next(potentialPrey.Count);
                            animal.Prey = potentialPrey[randomIndex].Name;
                        }
                    }

                    // Update the Animals
                    _context.Animals.UpdateRange(animals);
                    _context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine("An error occurred while updating the database: " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }
    }
}
