﻿using Microsoft.AspNetCore.Mvc;
using SpendLess.Shared;

namespace SpendLess.Client.Services
{
    public interface ITransactionService
    {
        public List<Transaction> Transactions { get; set; }
        Task<List<Transaction>> GetTransactions();

        Task AddTransaction(Transaction transaction);
    }
}
