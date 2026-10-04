using System;
using System.Collections.Generic;
using System.Text;

namespace AIAcademy.Core
{
    public class LanguageTeacherBot : RobotTeacher, IQuizCreator, IRechargeable
    {
        public LanguageTeacherBot(string name) : base(name)
        {
        }

        public string CreateQuiz()
        {
            if (BatteryLevel < 5)
            {
                return $"{Name} has insufficient battery to create a quiz.";
            }

            BatteryLevel -= 5;

            return $"{Name} created a language quiz.";
        }

        public override string CrazyAction()
        {
            return $"{Name} translated one word into 20 languages and forgot the original.";
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

            return $"{Name} teaches English vocabulary.";
        }
    }

}
