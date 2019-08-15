using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Users.DBModel
{
    public class UsersContext : DbContext
    {
        public UsersContext([NotNull] DbContextOptions<UsersContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Token> Tokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = -1,
                Username = "meep",
                Password = "arf",
            });
        }
    }

    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public ICollection<Token> Tokens { get; set; }
    }

    public class Token
    {
        public int TokenId { get; set; }
        public ulong TokenValue { get; set; }
        [Required]
        public User User { get; set; }
    }
}
