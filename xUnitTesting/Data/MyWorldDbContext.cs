using xUnitTesting.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace xUnitTesting.Data;


public class MyWorldDbContext : DbContext
{
    public MyWorldDbContext(DbContextOptions<MyWorldDbContext> options) : base(options)
    {
 
    }
    public DbSet<Todo> Todo { get; set; }
}