using System;
using System.Collections.Generic;
using System.Linq;
using AIAcademy.Core.Base;
using AIAcademy.Core.Events;

namespace AIAcademy.Core.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : RobotTeacher
    {
        private readonly List<T> _items = new();

        public event EventHandler<RobotEventArgs>? RobotAdded;
        public event EventHandler<RobotEventArgs>? RobotRemoved;
        public event Action<string>? Logged;

        public void Add(T item)
        {
            ArgumentNullException.ThrowIfNull(item);
            if (_items.Contains(item)) return;

            foreach (var existing in _items.ToList())
                Logged?.Invoke(existing.Welcome(item));

            _items.Add(item);
            item.CrazyActionPerformed += OnCrazyActionPerformed;

            RobotAdded?.Invoke(this, new RobotEventArgs(item));
        }

        public void Remove(T item)
        {
            ArgumentNullException.ThrowIfNull(item);

            if (!_items.Remove(item)) return;
            item.CrazyActionPerformed -= OnCrazyActionPerformed;

            foreach (var other in _items.ToList())
                Logged?.Invoke(other.Goodbye(item));

            RobotRemoved?.Invoke(this, new RobotEventArgs(item));
        }

        private void OnCrazyActionPerformed(object? sender, RobotEventArgs e)
        {
            if (sender is not T actor) return;

            foreach (var other in _items.Where(x => !ReferenceEquals(x, actor)).ToList())
                Logged?.Invoke(other.ReactToCrazyAction(actor));
        }

        public IEnumerable<T> GetAll() => _items.ToList();

        public T? Find(Func<T, bool> predicate) => _items.FirstOrDefault(predicate);
    }
}