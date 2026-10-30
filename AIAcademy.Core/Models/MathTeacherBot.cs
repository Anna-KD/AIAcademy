using AIAcademy.Core.Base;
using AIAcademy.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AIAcademy.Core.Models
{
    public class MathTeacherBot : RobotTeacher, IQuizCreator, IRechargeable, IHeavyWorker
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
        public Task<WorkResult> DoHeavyWorkAsync(
    RobotSnapshot snapshot, int workload, CancellationToken token)
    => HeavyWorkRunner.RunAsync(snapshot, () =>
    {
        // Sum of all numbers up to workload that are divisible by 3 or 5
        long sum = 0;
        for (long i = 1; i <= workload; i++)
        {
            token.ThrowIfCancellationRequested();
            if (i % 3 == 0 || i % 5 == 0)
                sum += i;
        }
        return sum;
    });
    }
}
