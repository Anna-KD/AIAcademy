using AIAcademy.Core.Base;
using AIAcademy.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AIAcademy.Core.Models
{
    public class MathTeacherBot : RobotTeacher, IQuizCreator, IRechargeable
    {
        public MathTeacherBot(string name) : base(name)
        {
        }
        public override string TypeName => "Math Teacher";

        public string CreateQuiz()
        {
            if (BatteryLevel < 5)
            {
                return $"{Name} has insufficient battery to create a quiz.";
            }

            BatteryLevel -= 5;

            return $"{Name} created a math quiz.";
        }

        public override string CrazyAction()
        {
            if (BatteryLevel < 20)
            {
                return $"{Name} has insufficient battery.";
            }

            BatteryLevel -= 20;

            OnCrazyActionPerformed();

            return $"{Name} solved 100 equations in one second and got confused by 2 + 2.";
        }
        public string Recharge()
        {
            BatteryLevel = 100;
            return $"{Name} is fully recharged.";
        }
        public override string Teach()
        {
            if (BatteryLevel < 10)
            {
                return $"{Name} has insufficient battery to teach.";
            }

            BatteryLevel -= 10;

            return $"{Name} teaches algebra.";
        }
        public override string Welcome(RobotTeacher newRobot)
        {
            return $"{Name} welcomes {newRobot.Name} to the Math Department.";
        }
        public override string ReactToCrazyAction(RobotTeacher robot)
        {
            BatteryLevel -= 5;
            return $"{Name} is impressed by {robot.Name}'s calculations.";
        }
    }
}
