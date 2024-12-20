﻿using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete
{
    public class Context : IdentityDbContext<User, Role, int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("server=77.245.159.27\\MSSQLSERVER2019; database=CoreBlogDb; user=coreblogdb; password=HV8plhmA&z?9d6za");
            optionsBuilder.UseMySQL("Server=daily-dev-hoangvh230823-7d81.i.aivencloud.com;Port=21159;Database=dailydev;Uid=avnadmin;Pwd=AVNS_Mc1eYyxgr4e8E-pu3MG;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasOne(m => m.SenderUser)
                .WithMany(m => m.WriterSender)
                .HasForeignKey(m => m.SenderID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.ReceiverUser)
                .WithMany(m => m.WriterReceiver)
                .HasForeignKey(m => m.ReceiverID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<About> Abouts { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Writer> Writers { get; set; }

        public DbSet<NewsLetter> NewsLetters { get; set; }

        public DbSet<BlogRating> BlogRatings { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Visitor> Visitors { get; set; }
    }
}