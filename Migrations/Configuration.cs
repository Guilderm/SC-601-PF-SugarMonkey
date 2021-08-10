
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace SugarMonkey.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SugarMonkey.Models.GeneralPurposeDBEntities1>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
    } 
}