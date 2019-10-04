using News_Portal.Models.RSSFeed;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace News_Portal.Models.EntityData
{
    public partial class SystemDB : DbContext
    {
        public SystemDB()
        {
            string con = ConfigurationManager.ConnectionStrings["con"].ToString();
            Database.SetInitializer<SystemDB>(null);
            Database.Connection.ConnectionString = con;
            Database.CommandTimeout = int.MaxValue;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<News_Portal.Models.RSSFeed.RSSFeed> RSSFeed { get; set; }
        public DbSet<RSSFeedMaster> RSSFeedMaster { get; set; }
        public DbSet<UserCount> UserCount { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<ePaper> ePaper { get; set; }
    }
}