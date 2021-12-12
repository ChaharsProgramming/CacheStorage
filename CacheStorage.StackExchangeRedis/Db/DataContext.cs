using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CacheStorage.StackExchangeRedis.Db
{
    public class DataContext
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public long TenantId { get; set; }


        public string CreatedBy { get; set; }


        public DateTime CreationTime { get; set; }


        public DateTime ModificationTime { get; set; }


        public string Properties { get; set; }


    }

    public class ResponseModel
    {
        public object DataContexts { get; set; }

        public string Location { get; set; }


    }
}
