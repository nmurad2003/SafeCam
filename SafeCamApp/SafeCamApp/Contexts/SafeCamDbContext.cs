﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SafeCamApp.Models;

namespace SafeCamApp.Contexts;

public class SafeCamDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Designation> Designations { get; set; }
    public DbSet<Member> Members { get; set; }

    public SafeCamDbContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
