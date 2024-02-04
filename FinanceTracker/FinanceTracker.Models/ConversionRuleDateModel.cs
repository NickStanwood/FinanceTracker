﻿using SQLite;
using SQLiteNetExtensions.Attributes;

namespace FinanceTracker.Models
{
    public class ConversionRuleDateModel
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        [ForeignKey(typeof(AccountModel))]
        public Guid AccountId { get; set; }
        public int Column { get; set; }
        public string DateFormat { get; set; }

        public bool UseAdvanced { get; set; }
        public string AdvancedScript { get; set; }
    }
}
