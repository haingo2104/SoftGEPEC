namespace Server.Data.Entities{
    public class Employee{
        public required string Id { get; set; }
        public required string LastName { get; set; }
        public required string FirstName { get; set; }
        public required string Entry_date { get; set; }
        public required string Confirmation_date { get; set; }
        public required string Service_id { get; set; }
        public string? ServiceTitle { get; set; } 
        public required string Created_by { get; set; }
    }
}

