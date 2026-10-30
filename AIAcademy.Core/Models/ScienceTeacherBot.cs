using AIAcademy.Core.Base;
using AIAcademy.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AIAcademy.Core.Models
{
    public class ScienceTeacherBot : RobotTeacher, IQuizCreator, IRechargeable, IHeavyWorker
    {
        public ScienceTeacherBot(string name) : base(name)
        {
        }
        public override string TypeName => "Science Teacher";

        public string CreateQuiz()
        {
            if (BatteryLevel < 5)
            {
                return $"{Name} has insufficient battery to create a quiz.";
            }

            BatteryLevel -= 5;

            return $"{Name} created a science quiz.";
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
                return $"{Name} has insufficient battery for an experiment.";
            }

            BatteryLevel -= 20;
            OnCrazyActionPerformed();

            return $"{Name} created glowing tea during an experiment.";
        }
        public override string Teach()
        {
            if (BatteryLevel < 10)
            {
                return $"{Name} has insufficient battery to teach.";
            }

            BatteryLevel -= 10;

            return $"{Name} teaches physics.";
        }
        public override string Welcome(RobotTeacher newRobot)
        {
            return $"{Name} welcomes {newRobot.Name} to the Science Department.";
        }
        public override string ReactToCrazyAction(RobotTeacher robot)
        {
            BatteryLevel -= 5;
            return $"{Name} starts analysing {robot.Name}'s experiment.";
        }

        public Task<WorkResult> DoHeavyWorkAsync(
    RobotSnapshot snapshot, int workload, CancellationToken token)
    => HeavyWorkRunner.RunAsync(snapshot, () =>
    {
        // Count perfect squares (1, 4, 9, 16...) among numbers up to workload
        long squares = 0;
        for (long i = 1; i <= workload; i++)
        {
            token.ThrowIfCancellationRequested();
            long root = (long)Math.Sqrt(i);
            if (root * root == i)
                squares++;
        }
        return squares;
    });
    }
}