namespace View.Models
{
    public class CreateClassDto
    {
        public string ClassName { get; set; } = null!;
        public double Price { get; set; }
        public int ClassCapacity { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? SlotId { get; set; }
        //public string? Monday { get; set; }
        //public string? Tuesday { get; set; }
        //public string? Wednesday { get; set; }
        //public string? Thursday { get; set; }
        //public string? Friday { get; set; }
        //public string? Saturday { get; set; }
        //public string? Sunday { get; set; }
    }
}
