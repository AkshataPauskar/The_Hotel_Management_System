namespace Hotel_Management_System.Models
{
    public class HotelManagementDatabaseSettings : IHotelManagementDatabaseSettings
    {
        public string CollectionName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }
}
