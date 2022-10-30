using System;
using System.Collections.Generic;

namespace SpendLess.Server.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Name { get; set; }
        public int? InitialBalance { get; set; }
    }
}
