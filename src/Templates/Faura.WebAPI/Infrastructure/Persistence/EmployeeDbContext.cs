﻿using Faura.WebAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace YourNamespace.Data;

public class EmployeeDbContext : DbContext
{
    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
        : base(options) { }

    public DbSet<Employee> Employee { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>().Property(e => e.Id).ValueGeneratedOnAdd();
    }
}
