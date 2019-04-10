using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication1.Models
{
    public class User : IdentityUser
    {
        public virtual SubscriptionModel SubModel { get; set; }
        public string APIKey { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }
    public class SubscriptionModel //модель подписки
    {
        public int SubscriptionModelID { get; set; }
        public int NumberOfProj { get; set; }
        public int NumberOfShared { get; set; }
        public decimal SubCost { get; set; }
    }
    public class Project //проект
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public DateTime ProjectDate { get; set; }
        public User Owner { get; set; }//владелец
        public virtual ICollection<Share> SharedTo { get; set; }
        public virtual ICollection<Composite> UsedComposits { get; set; }
    }
    public class Share
    {
        public int ShareID { get; set; }
        public virtual Project ProjectToShare { get; set; } //TODO: Возможно стоит убрать
        public virtual User Shared { get; set; }
        public bool isRead { get; set; }
        public bool isWrite { get; set; }
    }
    public class Material
    {
        public int MaterialID { get; set; }
        public virtual User Owner { get; set; }
        public string Name { get; set; }
        public decimal Hardness { get; set; }
        public decimal Elasticity { get; set; }
        public decimal StrengthBeyond { get; set; }
        public decimal Density { get; set; }
        public decimal HeatCapacity { get; set; }
        public decimal ThermalConduct { get; set; }
        public decimal ThermalExpansion { get; set; }
        public decimal Percent { get; set; }
        public bool isMatrix { get; set; }
    }
    public class Composite
    {
        public int CompositeID { get; set; }
        public string Name { get; set; }
        public decimal Porosity { get; set; }
        public decimal Elasticity { get; set; }
        public decimal Hardness { get; set; }
        public decimal FactorKogezia { get; set; }
        public decimal Strength { get; set; }
        public decimal ThermalConduct { get; set; } 
        public virtual ICollection<Material> Materials { get; set; }
    }
    public class ApplicationDbContext : IdentityDbContext<User> 
    {
        public ApplicationDbContext()
            : base("Test")
        {
        }
        public DbSet<SubscriptionModel> SubModel { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Share> Shares { get; set; }
        public DbSet<Composite> Composits { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasMany(u => u.UsedComposits)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("UsedComposits");  //композит в проекте
                    m.MapLeftKey("ProjectID");
                    m.MapRightKey("CompositeID");
                });
            modelBuilder.Entity<Composite>()
                .HasMany(u => u.Materials)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("UsedMaterials"); //материалы в композите
                    m.MapLeftKey("CompositeID");
                    m.MapRightKey("MaterialID");
                });
            modelBuilder.Entity<Project>()
                .HasMany(u => u.SharedTo)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("SharedTo"); //кому даем доступ
                    m.MapLeftKey("ProjectID");
                    m.MapRightKey("ShareID");
                });
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}