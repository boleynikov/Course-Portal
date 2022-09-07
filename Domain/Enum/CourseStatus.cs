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
        InEditing = 0,
        CompletelyDone = 1,
        Deleted = 2
    }
}
