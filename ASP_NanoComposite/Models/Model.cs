using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ASP_NanoComposite.Models//CodeFirst //просто для лучшего понимания заменил на Models
{
    public class Context : DbContext
    {
        public Context() : base("Test")
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<SubscriptionModel> SubModel { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Share> Shares { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>()
                .HasMany(u => u.UsedMaterials)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("UsedMaterials");
                    m.MapLeftKey("ProjectID");
                    m.MapRightKey("MaterialID");
                });
            modelBuilder.Entity<Project>()
                .HasMany(u => u.SharedTo)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("SharedTo");
                    m.MapLeftKey("ProjectID");
                    m.MapRightKey("ShareID");
                });
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
        public virtual User Owner { get; set; }//владелец
        public virtual ICollection<Share> SharedTo { get; set; }
        public virtual ICollection<Material> UsedMaterials { get; set; }
    }
    public class User //пользователь
    {
        public int UserID { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual SubscriptionModel SubModel { get; set; }
        public string APIKey { get; set; }
        public bool isVerified { get; set; }
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
}