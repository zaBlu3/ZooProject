using System.Xml.Linq;
using Zoo.Models.Zoo;

namespace Zoo.DataAccess.Data;

public class ZooContext : DbContext
{
	public ZooContext(DbContextOptions<ZooContext> options) : base(options)
	{
	}

	public DbSet<Category>? Categories { get; set; }
	public DbSet<Animal>? Animals { get; set; }
	public DbSet<Comment>? Comments { get; set; }
	public DbSet<Image<Animal>>? AnimalImages { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Category>()
			.HasIndex(c => c.Name)
			.IsUnique();
		modelBuilder.Entity<Animal>()
			.HasIndex(a => a.Name)
			.IsUnique();
		modelBuilder.Entity<Comment>()
		.Property(c => c.Date)
		.HasDefaultValueSql("getdate()");
		

		modelBuilder.Entity<Category>().HasData(
			new Category { CategoryId = 1, Name = "Birds" },
			new Category { CategoryId = 2, Name = "Reptiles" },
			new Category { CategoryId = 3, Name = "Mammals" },
			new Category { CategoryId = 4, Name = "Fish" },
			new Category { CategoryId = 5, Name = "Insects" }
		);

		int animalId = 0;
		var faker = new Faker();
		var animals = new List<Animal>()
		{
						new Animal
			{
				AnimalId = ++animalId,
				Name = "Sparrow",
				Age = 5,
				Description = faker.Lorem.Sentence(20),
				CategoryId = 1
			},
	new Animal
	{
		AnimalId = ++animalId,
		Name = "Eagle",
		Age = 6,
		Description = faker.Lorem.Sentence(10),
		CategoryId = 1
	},
	new Animal
	{
		AnimalId = ++animalId,
		Name = "Ostrich",
		Age = 7,
		Description = faker.Lorem.Sentence(15),
		CategoryId = 1
	},
	new Animal
	{
		AnimalId = ++animalId,
		Name = "Snake",
		Age = 8,
		Description = faker.Lorem.Sentence(25),
		CategoryId = 2
	},
	new Animal
	{
		AnimalId = ++animalId,
		Name = "Turtle",
		Age = 9,
		Description = faker.Lorem.Sentence(14),
		CategoryId = 2
	},
	new Animal
	{
		AnimalId = ++animalId,
		Name = "Lizard",
		Age = 10,
		Description = faker.Lorem.Sentence(10),
		CategoryId = 2
	},
	new Animal
	{
		AnimalId = ++animalId,
		Name = "Lion",
		Age = 11,
		Description = faker.Lorem.Sentence(),
		CategoryId = 3
	},
	new Animal
	{
		AnimalId = ++animalId,
		Name = "Tiger",
		Age = 12,
		Description = faker.Lorem.Sentence(20),
		CategoryId = 3
	},
	new Animal
	{
		AnimalId = ++animalId,
		Name = "Elephant",
		Age = 13,
		Description = faker.Lorem.Sentence(27),
		CategoryId = 3
	},
	new Animal
	{
		AnimalId = ++animalId,
		Name = "Goldfish",
		Age = 14,
		Description = faker.Lorem.Sentence(5),
		CategoryId = 4
	},
	new Animal
	{
		AnimalId = ++animalId,
		Name = "Clownfish",
		Age = 15,
		Description = faker.Lorem.Sentence(20),
		CategoryId = 4
	},
	new Animal
	{
		AnimalId = ++animalId,
		Name = "Shark",
		Age = 16,
		Description = faker.Lorem.Sentence(15),
		CategoryId = 4
	},
	new Animal
	{
		AnimalId = ++animalId,
		Name = "Bee",
		Age = 17,
		Description = faker.Lorem.Sentence(16),
		CategoryId = 5
	},
	new Animal
	{
		AnimalId = ++animalId,
		Name = "Ant",
		Age = 18,
		Description = faker.Lorem.Sentence(21),
		CategoryId = 5
	},
	new Animal
	{
		AnimalId = ++animalId,
		Name = "Butterfly",
		Age = 19,
		Description = faker.Lorem.Sentence(24),
		CategoryId = 5
	}
	};
		var images = new List<Image<Animal>>();
		int imageId = 0;
		foreach (var animal in animals)
		{
			for (int i = 1; i <= 2; i++) 
			{
				images.Add(new Image<Animal>
				{
					ImageId = ++imageId,
					EntityID = animal.AnimalId,
					ImageURI = $"/images/animals/animal-{animal.AnimalId}/{animal.Name!.ToLower()}{i}.jpg"
				});
			}
		}

		int commentId = 0;
		var comments = new List<Comment>();
		comments.Add(
				new Comment
				{
					Username="admin",
					CommentId = ++commentId,
					Text = faker.Lorem.Sentence(),
					AnimalId = animalId
				});
		comments.Add(
			new Comment
			{
				Username = "admin",
				CommentId = ++commentId,
				Text = faker.Lorem.Sentence(),
				AnimalId = 1
			});
		for (int i = 1; i <= animalId; i++)
		{
			comments.Add(
				new Comment
				{
					Username = "admin",
					CommentId = ++commentId,
					Text = faker.Lorem.Sentence(),
					AnimalId = i
				});
			comments.Add(
				new Comment
				{
					Username = "admin",
					CommentId = ++commentId,
					Text = faker.Lorem.Sentence(),
					AnimalId = i
				});
		}
		modelBuilder.Entity<Animal>().HasData(animals);
		modelBuilder.Entity<Comment>().HasData(comments);
		modelBuilder.Entity<Image<Animal>>().HasData(images);

	}


}
