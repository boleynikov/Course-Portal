using System;

namespace Domain
{
    [Serializable]
    public class CourseProgress
    {
        public State State;

        public float Percentage;

    }
    [Serializable]
    public enum State
    {
        NotCompleted = 0,
        Completed = 1
    }
}
