

namespace ZooWeb.Data;

public class ZooContext : DbContext
{
    public ZooContext(DbContextOptions<ZooContext> options) : base(options)
    {
    }

    public DbSet<Category>? Categories { get; set; }
    public DbSet<Animal>? Animals { get; set; }
    public DbSet<Comment>? Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Birds" },
            new Category { CategoryId = 2, Name = "Reptiles" },
            new Category { CategoryId = 3, Name = "Mammals" },
            new Category { CategoryId = 4, Name = "Fish" },
            new Category { CategoryId = 5, Name = "Insects" }
        );

        int animalId = 0;

        modelBuilder.Entity<Animal>().HasData(
            new Animal { AnimalId = ++animalId, Name = "Sparrow", Age = 5, ImageName = "sparrow.jpg", Description = "Description for Sparrow", CategoryId = 1 },
            new Animal { AnimalId = ++animalId, Name = "Eagle", Age = 6, ImageName = "eagle.jpg", Description = "Description for Eagle", CategoryId = 1 },
            new Animal { AnimalId = ++animalId, Name = "Ostrich", Age = 7, ImageName = "ostrich.jpg", Description = "Description for Ostrich", CategoryId = 1 },
            new Animal { AnimalId = ++animalId, Name = "Snake", Age = 8, ImageName = "snake.jpg", Description = "Description for Snake", CategoryId = 2 },
            new Animal { AnimalId = ++animalId, Name = "Turtle", Age = 9, ImageName = "turtle.jpg", Description = "Description for Turtle", CategoryId = 2 },
            new Animal { AnimalId = ++animalId, Name = "Lizard", Age = 10, ImageName = "lizard.jpg", Description = "Description for Lizard", CategoryId = 2 },
            new Animal { AnimalId = ++animalId, Name = "Lion", Age = 11, ImageName = "lion.jpg", Description = "Description for Lion", CategoryId = 3 },
            new Animal { AnimalId = ++animalId, Name = "Tiger", Age = 12, ImageName = "tiger.jpg", Description = "Description for Tiger", CategoryId = 3 },
            new Animal { AnimalId = ++animalId, Name = "Elephant", Age = 13, ImageName = "elephant.jpg", Description = "Description for Elephant", CategoryId = 3 },
            new Animal { AnimalId = ++animalId, Name = "Goldfish", Age = 14, ImageName = "goldfish.jpg", Description = "Description for Goldfish", CategoryId = 4 },
            new Animal { AnimalId = ++animalId, Name = "Clownfish", Age = 15, ImageName = "clownfish.jpg", Description = "Description for Clownfish", CategoryId = 4 },
            new Animal { AnimalId = ++animalId, Name = "Shark", Age = 16, ImageName = "shark.jpg", Description = "Description for Shark", CategoryId = 4 },
            new Animal { AnimalId = ++animalId, Name = "Bees", Age = 17, ImageName = "Bee.jpg", Description = "Description for Bees", CategoryId = 5 },
            new Animal { AnimalId = ++animalId, Name = "Ants", Age = 18, ImageName = "Ant.jpg", Description = "Description for Ants", CategoryId = 5 },
            new Animal { AnimalId = ++animalId, Name = "Butterflies", Age = 19, ImageName = "Butterfly.jpg", Description = "Description for Butterflies", CategoryId = 5 }
        );

        var faker = new Faker();
        int commentId = 0;
        var comments = new List<Comment>();
        comments.Add(
                new Comment { CommentId = ++commentId, Text = faker.Lorem.Sentence(), AnimalId = animalId });
        comments.Add(new Comment { CommentId = ++commentId, Text = faker.Lorem.Sentence(), AnimalId = 1 });
        for (int i = 1; i <= animalId; i++)
        {
            comments.Add(
                new Comment { CommentId = ++commentId, Text = faker.Lorem.Sentence(), AnimalId = i });
            comments.Add(new Comment { CommentId = ++commentId, Text = faker.Lorem.Sentence(), AnimalId = i });
        }
        modelBuilder.Entity<Comment>().HasData(comments);

    }
}
