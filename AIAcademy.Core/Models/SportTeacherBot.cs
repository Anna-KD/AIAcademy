using AIAcademy.Core.Base;
using AIAcademy.Core.Interfaces;

namespace AIAcademy.Core.Models
{
    public class SportTeacherBot : RobotTeacher, IQuizCreator, IRechargeable
    {
        public SportTeacherBot(string name) : base(name)
        {
        }

        public override string TypeName => "Sport Teacher";

        public override string Teach()
        {
            if (BatteryLevel < 10)
            {
                return $"{Name} has insufficient battery to teach.";
            }

            BatteryLevel -= 10;

            return $"{Name} teaches fitness and proper exercise technique.";
        }

        public string CreateQuiz()
        {
            if (BatteryLevel < 5)
            {
                return $"{Name} has insufficient battery to create a quiz.";
            }

            BatteryLevel -= 5;

            return $"{Name} created a sport quiz.";
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

            OnCrazyActionPerformed();

            return $"{Name} organised a full marathon through the academy corridors.";
        }

        public override string ReactToCrazyAction(RobotTeacher robot)
        {
            BatteryLevel -= 5;

            return $"{Name} starts warming up to join {robot.Name}'s challenge.";
        }
    }
}