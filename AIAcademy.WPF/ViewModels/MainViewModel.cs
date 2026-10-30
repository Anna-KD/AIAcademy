using AIAcademy.Core;
using AIAcademy.Core.Base;
using AIAcademy.Core.Interfaces;
using AIAcademy.Core.Models;
using AIAcademy.Core.Repositories;
using AIAcademy.WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AIAcademy.WPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // ===== Storage units (one per type) =====
        public IReadOnlyList<IRepositoryEntry> Repositories { get; }

        // ===== List that is visible in ListBox =====
        public ObservableCollection<RobotTeacher> Robots { get; } = new();

        // ===== Vol.3: robots that can be selected for calculation =====
        public ObservableCollection<RobotSelection> RobotSelections { get; } = new();

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

        private int _workload = 5000000;
        public int Workload
        {
            get => _workload;
            set => SetProperty(ref _workload, value);
        }

        private bool _isWorking;
        public bool IsWorking
        {
            get => _isWorking;
            set
            {
                if (SetProperty(ref _isWorking, value))
                    CommandManager.InvalidateRequerySuggested(); // refresh CanExecute of the buttons
            }
        }

        // temporarily accumulates reactions from neighbors while CrazyAction is underway
        private List<string>? _buffer;

        // one shared token source for all calculations of the current run
        private CancellationTokenSource? _cts;

        // ===== Vol.3: data for comparing the two modes =====
        private record Job(IHeavyWorker Worker, RobotSnapshot Snapshot);
        private record RunInfo(string Input, long Ms, List<WorkResult> Results);

        private RunInfo? _seqRun;
        private RunInfo? _parRun;

        // ===== Commands =====
        public RelayCommand AddCommand { get; }
        public RelayCommand RemoveCommand { get; }
        public RelayCommand CrazyActionCommand { get; }
        public RelayCommand TeachCommand { get; }
        public RelayCommand QuizCommand { get; }
        public RelayCommand RechargeCommand { get; }
        public RelayCommand SortByNameCommand { get; }
        public RelayCommand StartSequentialCommand { get; }
        public RelayCommand StartParallelCommand { get; }
        public RelayCommand CancelCommand { get; }

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

            RefreshRobotSelections();

            AddCommand = new RelayCommand(
                _ =>
                {
                    string name = NewRobotName.Trim();
                    SelectedRepository!.Create(name);
                    AppendLog($"Robot {name} added to {SelectedRepository.Name}.");
                    NewRobotName = "";
                    RefreshRobots();
                    RefreshRobotSelections();
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
                    RefreshRobotSelections();
                },
                _ => SelectedRepository != null && SelectedRobot != null);

            CrazyActionCommand = new RelayCommand(
                _ =>
                {
                    _buffer = new List<string>();
                    string action = SelectedRobot!.CrazyAction();   // neighbors' reactions go to the buffer
                    var reactions = _buffer;
                    _buffer = null;

                    AppendLog(action);                              // the action itself first
                    foreach (var line in reactions)                 // then the reactions
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

                // LINQ: sort by name (OrderBy), once per button press
                var sorted = Robots.OrderBy(r => r.Name).ToList();

                Robots.Clear();
                foreach (var robot in sorted)
                    Robots.Add(robot);

                SelectedRobot = keep;
            });

            // ===== Vol.3 =====
            CancelCommand = new RelayCommand(
                _ => _cts?.Cancel(),
                _ => IsWorking);

            StartSequentialCommand = new RelayCommand(
                async _ => await RunAsync(parallel: false),
                _ => !IsWorking);

            StartParallelCommand = new RelayCommand(
                async _ => await RunAsync(parallel: true),
                _ => !IsWorking);

            SelectedRepository = Repositories[0];
        }

        // ===================== Vol.3: heavy work =====================

        // Snapshots are taken on the UI thread at the moment the button is pressed.
        // The background calculations work only with these copies.
        private List<Job> GetJobs() =>
            RobotSelections
                .Where(x => x.IsSelected && x.Robot is IHeavyWorker)
                .Select(x => new Job(
                    (IHeavyWorker)x.Robot,
                    new RobotSnapshot
                    {
                        Name = x.Robot.Name,
                        TypeName = x.Robot.TypeName,
                        BatteryLevel = x.Robot.BatteryLevel
                    }))
                .ToList();

        private async Task RunAsync(bool parallel)
        {
            IsWorking = true;
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            try
            {
                int workload = Workload;
                if (workload <= 0)
                {
                    AppendLog("Workload must be greater than 0.");
                    return;
                }

                var jobs = GetJobs();
                if (jobs.Count == 0)
                {
                    AppendLog("Select at least one robot.");
                    return;
                }

                string mode = parallel ? "PARALLEL" : "SEQUENTIAL";
                AppendLog($"--- {mode} | {jobs.Count} robots | workload {workload} ---");

                var results = new List<WorkResult>();
                var total = Stopwatch.StartNew();

                if (parallel)
                {
                    // start all calculations at once, then wait for all of them
                    var tasks = jobs
                        .Select(j => j.Worker.DoHeavyWorkAsync(j.Snapshot, workload, token))
                        .ToList();

                    results = (await Task.WhenAll(tasks)).ToList();
                    total.Stop();

                    foreach (var r in results)
                        LogResult(r);
                }
                else
                {
                    foreach (var j in jobs)
                    {
                        // wait for one robot to finish before the next one starts
                        var r = await j.Worker.DoHeavyWorkAsync(j.Snapshot, workload, token);
                        results.Add(r);
                        LogResult(r);
                    }
                    total.Stop();
                }

                AppendLog($"{mode} TOTAL: {total.ElapsedMilliseconds} ms");

                // for the comparison keep only runs that were not cancelled
                bool cancelled = results.Any(r => r.Status == "Cancelled");
                string key = $"{workload}|" +
                             string.Join(",", jobs.Select(j => $"{j.Snapshot.Name}:{j.Snapshot.BatteryLevel}"));

                var run = cancelled ? null : new RunInfo(key, total.ElapsedMilliseconds, results);
                if (parallel) _parRun = run; else _seqRun = run;

                CompareRuns();
            }
            finally
            {
                _cts?.Dispose();
                _cts = null;
                IsWorking = false;
            }
        }

        private void LogResult(WorkResult r)
        {
            string info = r.Status switch
            {
                "Success" => $"result = {r.Result}",
                "Error" => $"error: {r.ErrorMessage}",
                _ => "stopped by user"
            };

            AppendLog($"{r.RobotName} | {r.Status} | {info} | {r.DurationMs} ms");
        }

        // Compares the last sequential and parallel runs if they had the same input
        private void CompareRuns()
        {
            if (_seqRun is null || _parRun is null || _seqRun.Input != _parRun.Input)
                return;

            string Sig(WorkResult r) => $"{r.RobotName}|{r.Status}|{r.Result}";

            bool same = _seqRun.Results.Select(Sig)
                .SequenceEqual(_parRun.Results.Select(Sig));

            AppendLog($"COMPARE: sequential {_seqRun.Ms} ms vs parallel {_parRun.Ms} ms | same results: {same}");
        }

        // Rebuilds the checkbox list; keeps the checked state of robots that already existed
        private void RefreshRobotSelections()
        {
            var oldState = RobotSelections.ToDictionary(x => x.Robot, x => x.IsSelected);
            RobotSelections.Clear();

            foreach (var robot in Repositories
                         .SelectMany(r => r.Robots)
                         .Where(r => r is IHeavyWorker))
            {
                RobotSelections.Add(new RobotSelection
                {
                    Robot = robot,
                    IsSelected = oldState.TryGetValue(robot, out var selected) ? selected : true
                });
            }
        }

        // ===================== Robots list =====================

        // Selected repository's robots + LINQ: Where
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

            // LINQ: searching by name (Where)
            if (!string.IsNullOrWhiteSpace(SearchText))
                query = query.Where(r =>
                    r.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            foreach (var robot in query)
                Robots.Add(robot);

            SelectedRobot = keep != null && Robots.Contains(keep) ? keep : null;

            RefreshSummary();
        }

        // LINQ: How many robots of each type (GroupBy), from all repositories
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