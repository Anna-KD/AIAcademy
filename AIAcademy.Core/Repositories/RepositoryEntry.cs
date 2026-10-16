using System;
using System.Collections.Generic;
using AIAcademy.Core.Base;

namespace AIAcademy.Core.Repositories
{
    public interface IRepositoryEntry
    {
        string Name { get; }                      // "Math", "Language"...
        IEnumerable<RobotTeacher> Robots { get; }
        event Action<string>? Logged;
        void Create(string robotName);
        void Remove(RobotTeacher robot);
    }

    public class RepositoryEntry<T> : IRepositoryEntry where T : RobotTeacher
    {
        private readonly Repository<T> _repo = new();
        private readonly Func<string, T> _factory;

        public string Name { get; }
        public IEnumerable<RobotTeacher> Robots => _repo.GetAll();

        public event Action<string>? Logged
        {
            add => _repo.Logged += value;
            remove => _repo.Logged -= value;
        }

        public RepositoryEntry(string name, Func<string, T> factory)
        {
            Name = name;
            _factory = factory;
        }

        public void Create(string robotName) => _repo.Add(_factory(robotName));

        public void Remove(RobotTeacher robot)
        {
            if (robot is T typed) _repo.Remove(typed);
        }
    }
}
