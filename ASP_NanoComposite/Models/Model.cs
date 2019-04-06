using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CodeFirst
{
    public class Context : DbContext
    {
        public Context() : base("Test")
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<SubscriptionModel> SubModel { get; set; }
    }
    public class SubscriptionModel
    {
        public int SubscriptionModelID { get; set; }
        public int NumberOfProj { get; set; }
        public int NumberOfShared { get; set; }
    }
    public class User
    {
        public int UserID { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public SubscriptionModel SubModel { get; set; }
        public string APIKey { get; set; }
    }
}