using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using AIAcademy.Core.Events;

namespace AIAcademy.Core.Base
{
    public abstract class RobotTeacher : INotifyPropertyChanged
    {
        public string Name { get; }


        public abstract string TypeName { get; }


        private int _batteryLevel;
        public int BatteryLevel
        {
            get => _batteryLevel;
            protected set => SetProperty(ref _batteryLevel, Math.Clamp(value, 0, 100));
        }


        private int _reactionCount;
        public int ReactionCount
        {
            get => _reactionCount;
            private set => SetProperty(ref _reactionCount, value);
        }

        public RobotTeacher(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty.");
            }

            Name = name;
            BatteryLevel = 100;
        }

        public virtual string Teach()
        {
            if (BatteryLevel < 10)
            {
                return $"{Name} has insufficient battery to teach.";
            }

            BatteryLevel -= 10;
            return $"{Name} teaches a lesson.";
        }

        public abstract string CrazyAction();
        public virtual string Welcome(RobotTeacher newRobot)
        {
            return $"{Name} welcomes {newRobot.Name} to AI Academy.";
        }
        public virtual string Goodbye(RobotTeacher robot)
        {
            return $"{Name} says goodbye to {robot.Name}.";
        }


        public virtual string ReactToCrazyAction(RobotTeacher robot)
        {

            BatteryLevel -= 2;
            return $"{Name} reacts to {robot.Name}'s crazy action.";
        }

        public event EventHandler<RobotEventArgs>? CrazyActionPerformed;
        protected void OnCrazyActionPerformed()
        {
            CrazyActionPerformed?.Invoke(
                this,
                new RobotEventArgs(this));
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (Equals(field, value)) return;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}