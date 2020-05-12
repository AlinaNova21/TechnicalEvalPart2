using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalEvalPart2.Models;

namespace TechnicalEvalPart2
{
    public class AddressBookContext : DbContext
    {
        public AddressBookContext(DbContextOptions options) : base(options) {
        }
        public DbSet<Contact> Contacts { get; set; }
    }
}
