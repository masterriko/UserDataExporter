using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace UserDataExporter
{
    // the name of csv columns are based on the property
    public class User
    {
        [Name("ID")]
        public string CustomID { get; set; }

        [Name("Ime")]
        public string FirstName { get; set; }

        [Name("Priimek")]
        public string LastName { get; set; }

        [Name("Mobi")]
        public string Mobile { get; set; }
    }
}
