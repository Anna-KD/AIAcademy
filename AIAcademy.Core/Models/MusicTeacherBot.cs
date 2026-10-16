using AIAcademy.Core.Base;
using AIAcademy.Core.Interfaces;

namespace AIAcademy.Core.Models
{
    public class MusicTeacherBot : RobotTeacher, IQuizCreator, IRechargeable
    {
        public MusicTeacherBot(string name) : base(name)
        {
        }

        public override string TypeName => "Music Teacher";

        public override string Teach()
        {
            if (BatteryLevel < 10)
            {
                return $"{Name} has insufficient battery to teach.";
            }

            BatteryLevel -= 10;

            return $"{Name} teaches rhythm and music theory.";
        }

        public string CreateQuiz()
        {
            if (BatteryLevel < 5)
            {
                return $"{Name} has insufficient battery to create a quiz.";
            }

            BatteryLevel -= 5;

            return $"{Name} created a music quiz.";
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

            return $"{Name} played a dramatic symphony on a broken piano.";
        }

        public override string ReactToCrazyAction(RobotTeacher robot)
        {
            BatteryLevel -= 5;

            return $"{Name} joins {robot.Name} with an improvised drum solo.";
        }
    }
}