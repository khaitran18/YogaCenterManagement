namespace View.Models
{
    public class DisableUserDto
    {
        public int UserId { get; set; }
        public string Reason { get; set; } = null!;
    }
}
