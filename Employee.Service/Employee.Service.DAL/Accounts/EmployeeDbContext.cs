using Employee.Service.DAL.Account;
using Employee.Service.DAL.Task;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Service.DAL
{
   public class EmployeeDbContext: DbContext
    {
        #region Public DbContext
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
           : base(options)
        {
        }
        #endregion
    
        #region DbSets
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<TaskDetails> taskDetails { get; set; }
        #endregion
        #region Entity Relationship
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskDetails>()
                .HasOne<UserInfo>(s => s.UserInfo)
                .WithMany(g => g.TaskDetails)
                .HasForeignKey(s => s.UserId);
        }
        #endregion
    }
}
