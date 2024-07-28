namespace Hotel_Management_System.Models
{
    public interface IHotelManagementDatabaseSettings
    {
        string CollectionName { get; set; }
        string ConnectionString { get; set;}
        string DatabaseName { get; set;}
    }
}
