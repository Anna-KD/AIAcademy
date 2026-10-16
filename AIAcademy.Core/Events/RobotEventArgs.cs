using System;
using AIAcademy.Core.Base;

namespace AIAcademy.Core.Events
{
    public class RobotEventArgs : EventArgs
    {
        public RobotTeacher Robot { get; }

        public RobotEventArgs(RobotTeacher robot)
        {
            Robot = robot;
        }
    }
}
