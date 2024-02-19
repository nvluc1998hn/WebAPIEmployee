using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Mongo
{
    public class MongoDbOptions
    {
        public bool Enabled { get; set; }

        public string ConnectionString { get; set; }

        public string Database { get; set; }

        public bool Seed { get; set; }
    }
}
