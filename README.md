Entity Framework üzembe helyezés
1-n és n-m kapcsolat létrehozása


NuGet: 
Microsoft.EntityFrameworkCore (8.0.23)
Microsoft.EntityFrameworkCore.Design (8.0.23)
Microsoft.EntityFrameworkCore.Tools (8.0.23)

MS SQL server esetén:
Microsoft.EntityFrameworkCore.SqlServer

SQLite esetében az SqlServer helyett:
Microsoft.EntityFrameworkCore.Sqlite

MySQL esetén:
MySql.EntityFrameworkCore (9.0.6)

Models osztályok kiegészítése (1-n):
 
namespace POOLCAR.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string LicencePlate { get; set; }
        public int NumberOfSeats { get; set; }
        public DateTime Validity { get; set; }
        public string EngineNumber { get; set; }
        [JsonIgnore]
        public List<PoolCarRegister>? PoolCarRegisters { get; set; } // Navigation property
    }
}
 
using System.ComponentModel.DataAnnotations.Schema;
 
namespace POOLCAR.Models
{
    public class PoolCarRegister
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int KmStart { get; set; }
        public int KmEnd { get; set; }
        public string DriverName { get; set; }
        
        public int? CarId { get; set; }
        
        [ForeignKey("CarId")]
        [JsonIgnore]
        public Car? Car { get; set; }
    }
}

Ha kötelező megadni az idegen kulcsot, akkor így kell használni:
 public int CarId { get; set; }
        	
       	 [ForeignKey("CarId")]
[JsonIgnore]
        	public Car Car { get; set; } = null!;
 

Context osztály létrehozása:
 
using Microsoft.EntityFrameworkCore;
using POOLCAR.Models;
 
namespace POOLCAR.Entity
{
    public class PoolCarContext: DbContext
    {
        public PoolCarContext(DbContextOptions<PoolCarContext> options) : base(options)
        {
        }
 
        public DbSet<Car> Cars { get; set; }
        public DbSet<PoolCarRegister> PoolCarRegisters { get; set; }
    }
}

 
appsettings.json:
 
 "ConnectionStrings": {
   "PoolCarContext": "Data Source=.;Initial Catalog=PoolCar; Integrated Security=true;TrustServerCertificate=True;"

Ha SQLite, akkor:
" PoolCarContext ": "Data Source=SQLiteTodo.db"  
 },

Ha MySQL:
"ConnectionStrings": {
  " PoolCarContext ": "server=localhost;port=3306;user=root;password=;database= PoolCar "
},
 
Program.cs fájban:
 
builder.Services.AddDbContext<PoolCarContext>(db => db.UseSqlServer(
    builder.Configuration.GetConnectionString("PoolCarContext")));

Ha SQLite, akkor:
builder.Services.AddDbContext<PoolCarContext>( 
    db => db.UseSqlite(builder.Configuration.GetConnectionString("PoolCarContext")));

Ha mySQL:
builder.Services.AddDbContext<PoolCarContext>(options => {
    options.UseMySQL(builder.Configuration.GetConnectionString("PoolCarContext"));
});
 

Adatbázis létrehozás:
Add-Migration Init
Update-Database
 
 

n-m kapcsolat létrehozása
TodoItem pélada osztály:

 public class TodoItem
 {
     public long Id { get; set; }
     [Required]
     public string Name { get; set; }
     public bool IsComplete { get; set; }
     public int PriorityId { get; set; }

     [JsonIgnore]
     public List<Category>? Categories { get; set; }

     [ForeignKey("PriorityId")]
     [JsonIgnore]
     public Priority? Priority { get; set; }

     [JsonIgnore]
     [InverseProperty("TodoItems")]
     public List<TodoItemCategory>? TodoItemCategories { get; set; }
 }

Category példa osztály

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }

    [JsonIgnore]
    public List<TodoItem>? TodoItems { get; set; }

    [JsonIgnore]
    [InverseProperty("Categories")]
    public List<TodoItemCategory>? TodoItemCategories { get; set; }
}

Osztáy az n-m kapcsolathoz:

public class TodoItemCategory
{
    [Key]
    public int Id { get; set; }

    public long TodoItemId { get; set; }
    [ForeignKey(nameof(TodoItemId))]
    public TodoItem TodoItem { get; set; }

    public int CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public Category Categories { get; set; }
}

Context osztály:

namespace TodoWebAPI.Models
{
    public class TodoContext : IdentityDbContext<IdentityUser>
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; } = null!;
        public DbSet<Priority> Priorites { get; set; } = null!;
        public DbSet<TodoItemCategory> TodoItemCategories { get; set; } = null!;
        public DbSet<TodoWebAPI.Models.Category> Categories { get; set; } = default!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
	    modelBuilder.Entity<TodoItem>()
	        .HasMany(e => e.Categories)
	        .WithMany(e => e.TodoItems)
	        .UsingEntity<TodoItemCategory>();
	}
	
    }
}


Adatbázishoz kapcsolódó műveletek:

Add-Migration N_NRelationExample

Update-Database
