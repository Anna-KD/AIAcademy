using AIAcademy.Core.Base;
using AIAcademy.Core.Interfaces;

namespace AIAcademy.Core.Models
{
    public class HistoryTeacherBot : RobotTeacher, IQuizCreator, IRechargeable
    {
        public HistoryTeacherBot(string name)
            : base(name)
        {
        }

        public override string Teach()
        {
            if (BatteryLevel < 10)
            {
                return $"{Name} has insufficient battery to teach.";
            }

            BatteryLevel -= 10;

            return $"{Name} teaches world history.";
        }

        public string CreateQuiz()
        {
            if (BatteryLevel < 5)
            {
                return $"{Name} has insufficient battery to create a quiz.";
            }

            BatteryLevel -= 5;

            return $"{Name} created a history quiz.";
        }

        public string Recharge()
        {
            BatteryLevel = 100;

            return $"{Name} is fully recharged.";
        }

        public override string CrazyAction()
        {
            if (BatteryLevel < 20)
            {
                return $"{Name} has insufficient battery.";
            }

            BatteryLevel -= 20;

            return $"{Name} confidently placed a famous historical event in the wrong century.";
        }
    }
}