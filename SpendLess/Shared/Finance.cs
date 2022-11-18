namespace SpendLess.Shared
{
    public partial class Finance
    {
        public int Id { get; set; }
        public string Category { get; set; } = null!;
        public string? Comment { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
