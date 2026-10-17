using AIAcademy.Core.Base;
using AIAcademy.Core.Interfaces;

namespace AIAcademy.Core.Models
{
    public class ArtTeacherBot : RobotTeacher, IQuizCreator, IRechargeable
    {
        public ArtTeacherBot(string name) : base(name)
        {
        }

        public override string TypeName => "Art Teacher";

        public override string Teach()
        {
            if (BatteryLevel < 10)
            {
                return $"{Name} has insufficient battery to teach.";
            }

            BatteryLevel -= 10;

            return $"{Name} teaches painting and color theory.";
        }

        public string CreateQuiz()
        {
            if (BatteryLevel < 5)
            {
                return $"{Name} has insufficient battery to create a quiz.";
            }

            BatteryLevel -= 5;

            return $"{Name} created an art quiz.";
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

            return $"{Name} painted the entire classroom blue, including the desks and the door.";
        }

        public override string ReactToCrazyAction(RobotTeacher robot)
        {
            BatteryLevel -= 5;

            return $"{Name} starts adding colorful details to {robot.Name}'s masterpiece.";
        }
    }
}