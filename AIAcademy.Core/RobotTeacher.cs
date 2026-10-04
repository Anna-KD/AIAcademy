using System;
using System.Collections.Generic;
using System.Text;

namespace AIAcademy.Core
{
    public abstract class RobotTeacher
    {
        public string Name { get; }

        public int BatteryLevel { get; protected set; }

        public RobotTeacher(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty.");
            }

            Name = name;
            BatteryLevel = 100;
        }

        public virtual string Teach()
        {
            if (BatteryLevel < 10)
            {
                return $"{Name} has insufficient battery to teach.";
            }

            BatteryLevel -= 10;
            return $"{Name} teaches a lesson.";
        }

        public abstract string CrazyAction();
    }
}