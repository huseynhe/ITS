using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITS.UI.Enums
{
    public enum TypeEnum
    {
        GenderType = 1,
        ResultType = 4,
        CareType = 8,
        PlanedCareType = 11,
        CareTeamType = 15


    }
    public enum StructureType 
    {

        ForeignMinister=5,
        Embassy=6,
        Council=7,
        HonoraryConsulate=8,
        ForeignPolicyAffairs=9


    }
    public enum OutForm
    {
        Person = 27,  //1019,
        Organization = 28, //1020

    }

    public enum InstructionType
    {
        Muellif = 1041,  //1019,
        Icraci = 1042, //1020
        MuellifIcraci = 1043
    }

    public enum DocumentSourceType
    {
        Daxili = 1060,  //,
        Diger = 1061, //
    }
}