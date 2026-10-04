using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using AIAcademy.Core;
using System.Linq;

namespace AIAcademy.WPF
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<RobotTeacher> Robots { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Robots = new ObservableCollection<RobotTeacher>();

            Robots.Add(new MathTeacherBot("Albert"));
            Robots.Add(new LanguageTeacherBot("Luna"));
            Robots.Add(new ScienceTeacherBot("Nova"));

            DataContext = this;

            RobotListBox.SelectionChanged += RobotListBox_SelectionChanged;
        }

        private void RobotListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RobotListBox.SelectedItem is RobotTeacher robot)
            {
                NameTextBlock.Text = robot.Name;
                TypeTextBlock.Text = robot.GetType().Name;
                BatteryTextBlock.Text = robot.BatteryLevel.ToString();
                BatteryProgressBar.Value = robot.BatteryLevel;
            }
        }
        private void CrazyActionButton_Click(object sender, RoutedEventArgs e)
        {
            if (RobotListBox.SelectedItem is RobotTeacher robot)
            {
                string message = robot.CrazyAction();

                LogTextBox.AppendText(message + Environment.NewLine);
                BatteryTextBlock.Text = robot.BatteryLevel.ToString();
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

                BatteryTextBlock.Text = robot.BatteryLevel.ToString();
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
                    BatteryTextBlock.Text = $"{robot.BatteryLevel}%";
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
                    BatteryTextBlock.Text = robot.BatteryLevel.ToString();
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

            Robots.Add(new MathTeacherBot(RobotNameTextBox.Text));

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

            Robots.Add(new LanguageTeacherBot(RobotNameTextBox.Text));

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

            Robots.Add(new ScienceTeacherBot(RobotNameTextBox.Text));

            LogTextBox.AppendText($"Science robot {RobotNameTextBox.Text} added.{Environment.NewLine}");

            RobotNameTextBox.Clear();
        }
        private void DeleteRobotButton_Click(object sender, RoutedEventArgs e)
        {
            if (RobotListBox.SelectedItem is RobotTeacher robot)
            {
                Robots.Remove(robot);

                LogTextBox.AppendText($"{robot.Name} removed from AI Academy.{Environment.NewLine}");
            }
            else
            {
                MessageBox.Show("Select a robot first.");
            }
        }
    }
}