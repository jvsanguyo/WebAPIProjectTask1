using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Data
{
    public class Summary
    {
        public Guid ID { get; set; }
        public string Message { get; set; }
        public virtual Global Global { get; set; }
        public virtual IList<Country> Countries { get; set; }
        public DateTime Date { get; set; }
    }
    public class Global
    {
        public int Id { get; set; }
        public int NewConfirmed { get; set; }
        public int TotalConfirmed { get; set; }
        public int NewDeaths { get; set; }
        public int TotalDeaths { get; set; }
        public int NewRecovered { get; set; }
        public int TotalRecovered { get; set; }
        public DateTime Date { get; set; }
    }
    public class Country
    {
        public Guid ID { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string Slug { get; set; }
        public int NewConfirmed { get; set; }
        public int TotalConfirmed { get; set; }
        public int NewDeaths { get; set; }
        public int TotalDeaths { get; set; }
        public int NewRecovered { get; set; }
        public int TotalRecovered { get; set; }
        public DateTime Date { get; set; }
    }
}
