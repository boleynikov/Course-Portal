using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enum
{
    public enum CourseStatus
    {
        Edited = 0,
        Unultered = 1,
        Deleted = 2
    }
}
