using System;
using System.Collections.Generic;
using System.Text;

namespace AIAcademy.Core
{
    public class MathTeacherBot : RobotTeacher, IQuizCreator, IRechargeable
    {
        public MathTeacherBot(string name) : base(name)
        {
        }

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
            return $"{Name} solved 100 equations in 1 second and got confused by 2 + 2.";
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
    }
}
