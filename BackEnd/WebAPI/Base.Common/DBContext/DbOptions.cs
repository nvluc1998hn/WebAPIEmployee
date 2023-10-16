using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Common.DBContext
{
    public class DbOptions
    {
        public bool Enabled { get; set; }

        public bool EnabledDapper { get; set; }

        public bool EnabledSqlTableDependency { get; set; }

        public string ConnString { get; set; }

        public string ConnString1 { get; set; }

        public string ConnString2 { get; set; }

        public string ConnString3 { get; set; }

        public string ConnString4 { get; set; }

        public string ConnString5 { get; set; }

        public string ConnString6 { get; set; }

        public string ConnString100 { get; set; }
    }
}
