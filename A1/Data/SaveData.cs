using A1.Models;
using Microsoft.EntityFrameworkCore;

namespace A1.Data
{
    public class SaveData : DbContext
    {
        public DbSet<Product> ProductTable { get; set; }
        public DbSet<User> UserTable { get; set; }
        public DbSet<Cart> CartTable { get; set; }
        public DbSet<Tr> TrTable { get; set; }
        public DbSet<Master> MasterTable { get; set; }
        public DbSet<Comment> CommentTable { get; set; }
        public DbSet<Video> VideoTable { get; set; }

        public SaveData(DbContextOptions<SaveData> options) : base(options)
        {

        }
    }
}
