using System.ComponentModel.DataAnnotations;

namespace DBOperation.Model
{
    public class MediaView
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ViewName { get; set; }
        public int TotalDevices { get; set; }
        public string DeviceIds { get; set; }
        /// <summary>
        /// Use to show message in the client side whether the operation successful or failed.
        /// </summary>
        public static string Message{get;set;}
    }
}
