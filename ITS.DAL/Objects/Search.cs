﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.DAL.Objects
{
    public class Search
    {
        public int? page { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }

        public Int64 Id { get; set; }
        public int ParentId { get; set; }
        public Int64 TypeId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public bool isCount { get; set; }
        public Int64 PId { get; set; }


        public int PersonId { get; set; }
        public int StructureId { get; set; }
        public bool isPhoto { get; set; }

        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public string N_Name { get; set; }
        public string SurName { get; set; }
        public string FatherName { get; set; }

        public string BusinessCenterName { get; set; }
        public string MachineGroupName { get; set; }
        public string MachineName { get; set; }
        public int Description { get; set; }

    }
}
