using DBOperation.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace DBOperation.Data
{
    /// <summary>
    /// Web is the name of the database in server
    /// </summary>
    public class WebContext:DbContext
    {
        private object MemoryLock = new object();

        public static IConfiguration Configuration { private get; set; }

        public WebContext()
        {

        }

        public WebContext(DbContextOptions options):base(options)
        {

        }

        /// <summary>
        /// Data set mapped with server database
        /// </summary>
        public DbSet<MediaView> MediaView { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {                
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MediaView>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever().IsRequired();
                entity.Property(e => e.ViewName).HasColumnName("ViewName").HasMaxLength(25).IsUnicode(false);
                entity.Property(e => e.DeviceIds).HasColumnName("DeviceIds").HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.TotalDevices).HasColumnName("TotalDevices").IsRequired(true);
            });
        }

        #region VIEW DETAILS
        /// <summary>
        /// Get view details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal MediaView GetView(int id)
        {
            MediaView view = null;
            if(MediaView==null || MediaView.Count()<1)
            {
                return view;
            }
            lock(MemoryLock)
            {
                foreach(MediaView mediaView in MediaView)
                {
                    if(mediaView.Id==id)
                    {
                        view = mediaView;
                        break;
                    }
                }
            }
            return view;
        }

        /// <summary>
        /// Delete view by id
        /// </summary>
        /// <param name="id"></param>
        internal void DeleteView(int id)
        {
            MediaView mView = MediaView.Where(v => v.Id == id).Single();
            MediaView.Remove(mView);
        }

        /// <summary>
        /// Update view details in the dataset
        /// </summary>
        /// <param name="mediaView"></param>
        internal void UpdateView(ref MediaView mediaView)
        {
            int mediaViewId = mediaView.Id;
            MediaView mView = MediaView.Where(v => v.Id == mediaViewId).Single();
            mView.ViewName = mediaView.ViewName;
            mView.DeviceIds = mediaView.DeviceIds;
            mView.TotalDevices = mediaView.TotalDevices;
            MediaView.Update(mView);
        }

        /// <summary>
        /// Add a new view inside dataset
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        internal int AddView(ref MediaView view)
        {
            MediaView.Add(view);
            int id = MediaView.Count();
            return id;
        }
        #endregion
    }
}
