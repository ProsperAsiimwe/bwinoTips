using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwinoTips.Domain.Enums
{

    public enum Status : int
    {
        Pending = 1,
        Correct = 2,
        Wrong = 3
       
    }

    public enum RegStatus : int
    {
        Pending = 1,
        Registered = 2      
    }

    public enum TicketType : int
    {
        Exclusive = 1,
        Free = 2
    }

    public enum Plan : int
    {
        Trial = 1,
        Weekly = 2,
        Monthly = 3,
        Yearly = 4
    } 

    public enum ReferenceType : byte
    {
        Education = 1,
        Employment = 2
    }

    public enum EmploymentTerminated : byte
    {
        Resignation = 1,
        Redundancy = 2,
        Dismissal = 3,
        Voluntary = 4,
        Compulsory = 5
    }

    public enum PerformanceScale : byte
    {
        Yes = 1,
        No = 2,
        N_A = 99
    }

    public enum ReferenceSection : int
    {
        RefereeDetails = 1,
        EducationOrEmployment = 2,
        Performance = 3,
        OtherInfo = 4
    }

    public enum ReferenceStatus : int
    {
        Pending = 1,
        OptedOut = 2,
        Started = 3,
        Submitted = 99
    }
}
