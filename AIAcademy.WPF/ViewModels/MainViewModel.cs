using AIAcademy.Core;
using AIAcademy.Core.Base;
using AIAcademy.Core.Interfaces;
using AIAcademy.Core.Models;
using AIAcademy.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AIAcademy.WPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // ===== Storage units (one per type) =====
        public IReadOnlyList<IRepositoryEntry> Repositories { get; }

        // ===== List that is visible in ListBox =====
        public ObservableCollection<RobotTeacher> Robots { get; } = new();

        private IRepositoryEntry? _selectedRepository;
        public IRepositoryEntry? SelectedRepository
        {
            get => _selectedRepository;
            set
            {
                if (SetProperty(ref _selectedRepository, value))
                    RefreshRobots();
            }
        }

        private RobotTeacher? _selectedRobot;
        public RobotTeacher? SelectedRobot
        {
            get => _selectedRobot;
            set => SetProperty(ref _selectedRobot, value);
        }

        private string _newRobotName = "";
        public string NewRobotName
        {
            get => _newRobotName;
            set => SetProperty(ref _newRobotName, value);
        }

        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    RefreshRobots();
            }
        }

        private string _logText = "";
        public string LogText
        {
            get => _logText;
            private set => SetProperty(ref _logText, value);
        }

        private string _summaryText = "";
        public string SummaryText
        {
            get => _summaryText;
            private set => SetProperty(ref _summaryText, value);
        }

        // temporarily accumulates reactions from neighbors while CrazyAction is underway
        private List<string>? _buffer;

        // ===== Commands =====
        public RelayCommand AddCommand { get; }
        public RelayCommand RemoveCommand { get; }
        public RelayCommand CrazyActionCommand { get; }
        public RelayCommand TeachCommand { get; }
        public RelayCommand QuizCommand { get; }
        public RelayCommand RechargeCommand { get; }
        public RelayCommand SortByNameCommand { get; }
        public MainViewModel()
        {
            Repositories = new List<IRepositoryEntry>
            {
                new RepositoryEntry<MathTeacherBot>("Math", n => new MathTeacherBot(n)),
                new RepositoryEntry<LanguageTeacherBot>("Language", n => new LanguageTeacherBot(n)),
                new RepositoryEntry<ScienceTeacherBot>("Science", n => new ScienceTeacherBot(n)),
                new RepositoryEntry<HistoryTeacherBot>("History", n => new HistoryTeacherBot(n)),
                new RepositoryEntry<ArtTeacherBot>("Art", n => new ArtTeacherBot(n)),
                new RepositoryEntry<MusicTeacherBot>("Music", n => new MusicTeacherBot(n)),
                new RepositoryEntry<SportTeacherBot>("Sport", n => new SportTeacherBot(n)),

            };

            // The repository log (greetings, farewells, reactions) ends up in our log.
            foreach (var repo in Repositories)
                repo.Logged += AppendLog;

            // Starting robots from Vol.1
            Repositories[0].Create("Albert");
            Repositories[1].Create("Luna");
            Repositories[2].Create("Nova");
            Repositories[3].Create("Marcus");
            Repositories[4].Create("Picasso");
            Repositories[5].Create("Mozart");
            Repositories[6].Create("Bolt");

            AddCommand = new RelayCommand(
                _ =>
                {
                    string name = NewRobotName.Trim();
                    SelectedRepository!.Create(name);
                    AppendLog($"Robot {name} added to {SelectedRepository.Name}.");
                    NewRobotName = "";
                    RefreshRobots();
                },
                _ => SelectedRepository != null
                     && !string.IsNullOrWhiteSpace(NewRobotName)
                     && !NewRobotName.Trim().All(char.IsDigit));

            RemoveCommand = new RelayCommand(
                _ =>
                {
                    var robot = SelectedRobot!;
                    SelectedRepository!.Remove(robot);
                    AppendLog($"{robot.Name} removed from AI Academy.");
                    RefreshRobots();
                },
                _ => SelectedRepository != null && SelectedRobot != null);

            CrazyActionCommand = new RelayCommand(
                _ =>
                {
                    _buffer = new List<string>();
                    string action = SelectedRobot!.CrazyAction();   // реакции соседей уходят в буфер
                    var reactions = _buffer;
                    _buffer = null;

                    AppendLog(action);                              // сначала само действие
                    foreach (var line in reactions)                 // потом реакции
                        AppendLog(line);
                },
                _ => SelectedRobot != null);

            TeachCommand = new RelayCommand(
                _ => AppendLog(SelectedRobot!.Teach()),
                _ => SelectedRobot != null);

            QuizCommand = new RelayCommand(
                _ => AppendLog(((IQuizCreator)SelectedRobot!).CreateQuiz()),
                _ => SelectedRobot is IQuizCreator);

            RechargeCommand = new RelayCommand(
                _ => AppendLog(((IRechargeable)SelectedRobot!).Recharge()),
                _ => SelectedRobot is IRechargeable);

            SortByNameCommand = new RelayCommand(_ =>
            {
                var keep = SelectedRobot;

                // LINQ 2: сортировка по имени (OrderBy), один раз по кнопке
                var sorted = Robots.OrderBy(r => r.Name).ToList();

                Robots.Clear();
                foreach (var robot in sorted)
                    Robots.Add(robot);

                SelectedRobot = keep;
            });

            SelectedRepository = Repositories[0];
        }

        // Selected repository's robots + LINQ: Where and OrderBy
        private void RefreshRobots()
        {
            var keep = SelectedRobot;

            Robots.Clear();
            if (SelectedRepository == null)
            {
                RefreshSummary();
                return;
            }

            IEnumerable<RobotTeacher> query = SelectedRepository.Robots;

            // LINQ 1: searching by name (Where)
            if (!string.IsNullOrWhiteSpace(SearchText))
                query = query.Where(r =>
                    r.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            foreach (var robot in query)
                Robots.Add(robot);

            SelectedRobot = keep != null && Robots.Contains(keep) ? keep : null;

            RefreshSummary();
        }

        // LINQ 2: How many robots of each type (GroupBy), from all repositories
        private void RefreshSummary()
        {
            var lines = Repositories
                .SelectMany(r => r.Robots)
                .GroupBy(robot => robot.TypeName)
                .Select(g => $"{g.Key}: {g.Count()} ({string.Join(", ", g.Select(r => r.Name))})");

            SummaryText = string.Join(Environment.NewLine, lines);
        }

        private void AppendLog(string message)
        {
            if (_buffer != null)
            {
                _buffer.Add(message);     
                return;
            }

            LogText += message + Environment.NewLine;
            RefreshSummary();
        }

        // ===== INotifyPropertyChanged =====
        public event PropertyChangedEventHandler? PropertyChanged;

        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            return true;
        }
    }
}