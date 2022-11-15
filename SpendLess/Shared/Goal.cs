namespace SpendLess.Server.Models
{
    public class Goal
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Name { get; set; }
        public double Amount { get; set; }
        public DateTime endDate { get; set; }
    }
}
