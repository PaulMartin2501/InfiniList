using InfiniList.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace InfiniList.DAL
{
    public class InfiniBackEnd:DbContext
    {
        public InfiniBackEnd():base("DefaultConnection")
        {

        }

        public DbSet<Collection> Collections { get; set; }
        public DbSet<List> Lists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}