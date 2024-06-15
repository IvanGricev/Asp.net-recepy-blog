using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Asp.net_recepy_blog.Pages.Services;
using Asp.net_recepy_blog.Pages.Dbcontrol;
public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {

        try
        {
            var dbcreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            if (dbcreator != null)
            {
                if (!dbcreator.CanConnect())
                {
                    dbcreator.Create();
                }
                if (!dbcreator.HasTables())
                {
                    dbcreator.CreateTables();
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Posts> Posts { get; set; }

    public MyDbContext(DbContextOptions<DbContext> options) : base(options)
    {
    }
}



