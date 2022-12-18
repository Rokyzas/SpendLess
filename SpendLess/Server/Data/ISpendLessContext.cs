using Microsoft.EntityFrameworkCore;
using SpendLess.Shared;

namespace SpendLess.Server.Models
{
    public interface ISpendLessContext
    {
        DbSet<Goal> Goals { get; set; }
        DbSet<Transactions> Transactions { get; set; }
        DbSet<User> Users { get; set; }
    }
}