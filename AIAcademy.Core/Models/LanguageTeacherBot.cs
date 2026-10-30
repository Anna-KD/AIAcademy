using AIAcademy.Core.Base;
using AIAcademy.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace AIAcademy.Core.Models
{
    public class LanguageTeacherBot : RobotTeacher, IQuizCreator, IRechargeable, IHeavyWorker
    {
        public LanguageTeacherBot(string name) : base(name)
        {
        }
        public override string TypeName => "Language Teacher";

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
            if (BatteryLevel < 20)
            {
                return $"{Name} has insufficient battery.";
            }

            BatteryLevel -= 20;

            OnCrazyActionPerformed();

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
        public override string Welcome(RobotTeacher newRobot)
        {
            return $"{Name} welcomes {newRobot.Name} to the Language Department.";
        }
        public override string ReactToCrazyAction(RobotTeacher robot)
        {
            BatteryLevel -= 5;
            return $"{Name} is trying to translate {robot.Name}'s explanation.";
        }
        public Task<WorkResult> DoHeavyWorkAsync(
    RobotSnapshot snapshot, int workload, CancellationToken token)
    => HeavyWorkRunner.RunAsync(snapshot, () =>
    {
        // Vol.3: simulated failure for error-handling demo
        if (snapshot.BatteryLevel < 30)
            throw new InvalidOperationException("Language database error: battery too low");

        // Total number of letters in all words (word length is from 3 to 10)
        long letters = 0;
        for (long i = 0; i < workload; i++)
        {
            token.ThrowIfCancellationRequested();
            letters += 3 + i % 8;
        }
        return letters;
    });
    }

}
