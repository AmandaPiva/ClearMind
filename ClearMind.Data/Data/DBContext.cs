using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClearMind.ClearMind.Data.Models;
using ClearMind.ClearMind.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ClearMind.ClearMind.Api.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        //MODELOS 
        public DbSet<Pessoa> Pessoa { get; set; }
        public DbSet<Emocao> Emocao { get; set; }
        public DbSet<Anotacoes> Anotacoes{ get; set; }

    }
}