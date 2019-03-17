using Lingsearcher.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lingsearcher
{
    public class DbContextIdentity<T> : IdentityDbContext
    {
        public DbContextIdentity() : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserApplication>()
                .Property(b => b.Id)
                .HasColumnType("VARCHAR")
                .HasMaxLength(50);

            modelBuilder.Entity<UserApplication>()
                .Property(b => b.FullName)
                .HasColumnType("VARCHAR")
                .HasMaxLength(50);

            modelBuilder.Entity<UserApplication>()
                .Property(b => b.Password)
                .HasColumnType("VARCHAR")
                .HasMaxLength(30);

            modelBuilder.Entity<UserApplication>()
                .Property(b => b.Email)
                .HasColumnType("VARCHAR")
                .HasMaxLength(70);

            modelBuilder.Entity<UserApplication>()
                .Property(b => b.PasswordHash)
                .HasColumnType("VARCHAR")
                .HasMaxLength(100);

            modelBuilder.Entity<UserApplication>()
                .Property(b => b.SecurityStamp)
                .HasColumnType("VARCHAR")
                .HasMaxLength(70);

            modelBuilder.Entity<UserApplication>()
                .Property(b => b.PhoneNumber)
                .HasColumnType("VARCHAR")
                .HasMaxLength(20);

            modelBuilder.Entity<UserApplication>()
                .Property(b => b.UserName)
                .HasColumnType("VARCHAR")
                .HasMaxLength(50);

        }

    }
}