namespace Server.Data.Entities{
    public class Skills{
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required string Skills_group_id { get; set; }
        public required string Created_by { get; set; }
        public string? SkillsGroupTitle { get; set; } 
    }
}