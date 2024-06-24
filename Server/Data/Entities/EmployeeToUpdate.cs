namespace Server.Data.Entities{
    public class EmployeeToUpdate{
        public required string LastName { get; set; }
        public required string FirstName { get; set; }
        public required string Entry_date { get; set; }
        public required string Confirmation_date { get; set; }
        public required string Service_id { get; set; }
    }
}