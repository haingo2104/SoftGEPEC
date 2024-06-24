namespace Server.Data.Entities{
    public class Service{
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required string Department_id { get; set; }
        public required string Created_by { get; set; }
        public string? DepartmentTitle { get; set; } 
    }
}