using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.DAL.DTO
{
    public class TypeDTO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public string ParentTypeName { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public string Level { get; set; }
        public int Count { get; set; }
    }
}
