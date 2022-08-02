using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class CourseProgress
    {
        public State State;

        public float Percentage;

    }
    public enum State
    {
        NotCompleted = 0,
        Completed = 1
    }
}
