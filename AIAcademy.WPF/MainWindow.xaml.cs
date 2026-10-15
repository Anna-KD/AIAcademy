using AIAcademy.Core;
using AIAcademy.Core.Base;
using AIAcademy.Core.Interfaces;
using AIAcademy.Core.Models;
using AIAcademy.Core.Repositories;
using AIAcademy.WPF.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AIAcademy.WPF
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<RobotTeacher> Robots { get; set; }

        private Repository<MathTeacherBot> _mathRepository;
        private Repository<LanguageTeacherBot> _languageRepository;
        private Repository<ScienceTeacherBot> _scienceRepository;
        private Repository<HistoryTeacherBot> _historyRepository;
        private MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            //Robots = new ObservableCollection<RobotTeacher>();

            _mathRepository = new Repository<MathTeacherBot>();
            _languageRepository = new Repository<LanguageTeacherBot>();
            _scienceRepository = new Repository<ScienceTeacherBot>();
            _historyRepository = new Repository<HistoryTeacherBot>();

            var albert = new MathTeacherBot("Albert");
            var luna = new LanguageTeacherBot("Luna");
            var nova = new ScienceTeacherBot("Nova");
            var marcus = new HistoryTeacherBot("Marcus");

            _mathRepository.Add(albert);
            _languageRepository.Add(luna);
            _scienceRepository.Add(nova);
            _historyRepository.Add(marcus);

            //Robots.Add(albert);
            //Robots.Add(luna);
            //Robots.Add(nova);

            _viewModel = new MainViewModel();
            DataContext = _viewModel;

            RobotListBox.SelectionChanged += RobotListBox_SelectionChanged;
        }

        private void RobotListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RobotListBox.SelectedItem is RobotTeacher robot)
            {
                NameTextBlock.Text = $"Name: {robot.Name}";

                if (robot is MathTeacherBot)
                {
                    TypeTextBlock.Text = "Type: Math Teacher";
                }
                else if (robot is LanguageTeacherBot)
                {
                    TypeTextBlock.Text = "Type: Language Teacher";
                }
                else if (robot is ScienceTeacherBot)
                {
                    TypeTextBlock.Text = "Type: Science Teacher";
                }
                else if (robot is HistoryTeacherBot)
                {
                    TypeTextBlock.Text = "Type: History Teacher";
                }

                BatteryTextBlock.Text = $"Battery: {robot.BatteryLevel}%";
                BatteryProgressBar.Value = robot.BatteryLevel;
            }
        }
        private void CrazyActionButton_Click(object sender, RoutedEventArgs e)
        {
            if (RobotListBox.SelectedItem is RobotTeacher robot)
            {
                string message = robot.CrazyAction();

                LogTextBox.AppendText(message + Environment.NewLine);
                if (robot is MathTeacherBot)
                {
                    foreach (var existingRobot in _mathRepository.GetAll())
                    {
                        if (existingRobot != robot)
                        {
                            LogTextBox.AppendText(
                                existingRobot.ReactToCrazyAction(robot) + Environment.NewLine);
                        }
                    }
                }
                else if (robot is LanguageTeacherBot)
                {
                    foreach (var existingRobot in _languageRepository.GetAll())
                    {
                        if (existingRobot != robot)
                        {
                            LogTextBox.AppendText(
                                existingRobot.ReactToCrazyAction(robot) + Environment.NewLine);
                        }
                    }
                }
                else if (robot is ScienceTeacherBot)
                {
                    foreach (var existingRobot in _scienceRepository.GetAll())
                    {
                        if (existingRobot != robot)
                        {
                            LogTextBox.AppendText(
                                existingRobot.ReactToCrazyAction(robot) + Environment.NewLine);
                        }
                    }
                }
                else if (robot is HistoryTeacherBot)
                {
                    foreach (var existingRobot in _historyRepository.GetAll())
                    {
                        if (existingRobot != robot)
                        {
                            LogTextBox.AppendText(
                                existingRobot.ReactToCrazyAction(robot) + Environment.NewLine);
                        }
                    }
                }
                BatteryTextBlock.Text = $"Battery: {robot.BatteryLevel}%";
                BatteryProgressBar.Value = robot.BatteryLevel;
            }
            else
            {
                MessageBox.Show("Select a robot first.");
            }
        }
        private void TeachButton_Click(object sender, RoutedEventArgs e)
        {
            if (RobotListBox.SelectedItem is RobotTeacher robot)
            {
                string message = robot.Teach();

                LogTextBox.AppendText(message + Environment.NewLine);

                BatteryTextBlock.Text = $"Battery: {robot.BatteryLevel}%";
                BatteryProgressBar.Value = robot.BatteryLevel;
            }
            else
            {
                MessageBox.Show("Select a robot first.");
            }
        }
        private void QuizButton_Click(object sender, RoutedEventArgs e)
        {
            if (RobotListBox.SelectedItem is IQuizCreator quizCreator)
            {
                string message = quizCreator.CreateQuiz();

                LogTextBox.AppendText(message + Environment.NewLine);

                if (RobotListBox.SelectedItem is RobotTeacher robot)
                {
                    BatteryTextBlock.Text = $"Battery: {robot.BatteryLevel}%";
                    BatteryProgressBar.Value = robot.BatteryLevel;
                }
            }
        }

        private void RechargeButton_Click(object sender, RoutedEventArgs e)
        {
            if (RobotListBox.SelectedItem is IRechargeable rechargeable)
            {
                string message = rechargeable.Recharge();

                LogTextBox.AppendText(message + Environment.NewLine);

                if (RobotListBox.SelectedItem is RobotTeacher robot)
                {
                    BatteryTextBlock.Text = $"Battery: {robot.BatteryLevel}%";
                    BatteryProgressBar.Value = robot.BatteryLevel;
                }
            }
            else
            {
                MessageBox.Show("This robot cannot recharge.");
            }
        }
        private void AddMathRobotButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RobotNameTextBox.Text))
            {
                MessageBox.Show("Enter a robot name.");
                return;
            }
            if (RobotNameTextBox.Text.All(char.IsDigit))
            {
                MessageBox.Show("Name cannot contain only numbers.");
                return;
            }

            var robot = new MathTeacherBot(RobotNameTextBox.Text);
            foreach (var existingRobot in _mathRepository.GetAll())
            {
                LogTextBox.AppendText(
                    existingRobot.Welcome(robot) + Environment.NewLine);
            }

            _mathRepository.Add(robot);
            _viewModel.Robots.Add(robot);

            LogTextBox.AppendText($"Math robot {RobotNameTextBox.Text} added.{Environment.NewLine}");

            RobotNameTextBox.Clear();
        }
        private void AddLanguageRobotButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RobotNameTextBox.Text))
            {
                MessageBox.Show("Enter a robot name.");
                return;
            }
            if (RobotNameTextBox.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Robot name must contain only letters.");
                return;
            }

            var robot = new LanguageTeacherBot(RobotNameTextBox.Text);
            foreach (var existingRobot in _languageRepository.GetAll())
            {
                LogTextBox.AppendText(
                    existingRobot.Welcome(robot) + Environment.NewLine);
            }

            _languageRepository.Add(robot);
            _viewModel.Robots.Add(robot);

            LogTextBox.AppendText($"Language robot {RobotNameTextBox.Text} added.{Environment.NewLine}");

            RobotNameTextBox.Clear();
        }
        private void AddScienceRobotButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RobotNameTextBox.Text))
            {
                MessageBox.Show("Enter a robot name.");
                return;
            }
            if (RobotNameTextBox.Text.All(char.IsDigit))
            {
                MessageBox.Show("Name cannot contain only numbers.");
                return;
            }

            var robot = new ScienceTeacherBot(RobotNameTextBox.Text);
            foreach (var existingRobot in _scienceRepository.GetAll())
            {
                LogTextBox.AppendText(
                    existingRobot.Welcome(robot) + Environment.NewLine);
            }

            _scienceRepository.Add(robot);
            _viewModel.Robots.Add(robot);

            LogTextBox.AppendText($"Science robot {RobotNameTextBox.Text} added.{Environment.NewLine}");

            RobotNameTextBox.Clear();
        }

        // teacher bot
        private void AddHistoryRobotButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RobotNameTextBox.Text))
            {
                MessageBox.Show("Enter a robot name.");
                return;
            }

            if (RobotNameTextBox.Text.All(char.IsDigit))
            {
                MessageBox.Show("Name cannot contain only numbers.");
                return;
            }

            var robot = new HistoryTeacherBot(RobotNameTextBox.Text);
            foreach (var existingRobot in _historyRepository.GetAll())
            {
                LogTextBox.AppendText(
                    existingRobot.Welcome(robot) + Environment.NewLine);
            }

            _historyRepository.Add(robot);
            _viewModel.Robots.Add(robot);

            LogTextBox.AppendText(
                $"History robot {RobotNameTextBox.Text} added.{Environment.NewLine}");

            RobotNameTextBox.Clear();
        }

        private void DeleteRobotButton_Click(object sender, RoutedEventArgs e)
        {
            if (RobotListBox.SelectedItem is RobotTeacher robot)
            {
                if (robot is MathTeacherBot)
                {
                    foreach (var existingRobot in _mathRepository.GetAll())
                    {
                        if (existingRobot != robot)
                        {
                            LogTextBox.AppendText(
                                existingRobot.Goodbye(robot) + Environment.NewLine);
                        }
                    }

                    _mathRepository.Remove((MathTeacherBot)robot);
                }
                else if (robot is LanguageTeacherBot)
                {
                    foreach (var existingRobot in _languageRepository.GetAll())
                    {
                        if (existingRobot != robot)
                        {
                            LogTextBox.AppendText(
                                existingRobot.Goodbye(robot) + Environment.NewLine);
                        }
                    }

                    _languageRepository.Remove((LanguageTeacherBot)robot);
                }
                else if (robot is ScienceTeacherBot)
                {
                    foreach (var existingRobot in _scienceRepository.GetAll())
                    {
                        if (existingRobot != robot)
                        {
                            LogTextBox.AppendText(
                                existingRobot.Goodbye(robot) + Environment.NewLine);
                        }
                    }

                    _scienceRepository.Remove((ScienceTeacherBot)robot);
                }
                else if (robot is HistoryTeacherBot)
                {
                    foreach (var existingRobot in _historyRepository.GetAll())
                    {
                        if (existingRobot != robot)
                        {
                            LogTextBox.AppendText(
                                existingRobot.Goodbye(robot) + Environment.NewLine);
                        }
                    }

                    _historyRepository.Remove((HistoryTeacherBot)robot);
                }

                _viewModel.Robots.Remove(robot);

                LogTextBox.AppendText(
                    $"{robot.Name} removed from AI Academy." + Environment.NewLine);
            }
            else
            {
                MessageBox.Show("Select a robot first.");
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var results = _viewModel.Robots
                .Where(r => r.Name.Contains(SearchTextBox.Text))
                .ToList();

            LogTextBox.AppendText(
                Environment.NewLine + "Search results:" + Environment.NewLine);

            foreach (var robot in results)
            {
                LogTextBox.AppendText(robot.Name + Environment.NewLine);
            }
        }
        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            var sorted = _viewModel.Robots
                .OrderBy(r => r.Name)
                .ToList();

            _viewModel.Robots.Clear();

            foreach (var robot in sorted)
            {
                _viewModel.Robots.Add(robot);
            }

            LogTextBox.AppendText(
                Environment.NewLine + "Robots sorted A-Z." + Environment.NewLine);
        }  
    }
}