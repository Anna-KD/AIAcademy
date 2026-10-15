using AIAcademy.Core.Base;
using AIAcademy.Core.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace AIAcademy.WPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<RobotTeacher> Robots { get; }
        public RelayCommand TestCommand { get; }
        public RelayCommand TestCanExecuteCommand { get; }
        public RelayCommand SummaryCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }

        public MainViewModel()
        {
            Robots = new ObservableCollection<RobotTeacher>();

            Robots.Add(new MathTeacherBot("Albert"));
            Robots.Add(new LanguageTeacherBot("Luna"));
            Robots.Add(new ScienceTeacherBot("Nova"));
            Robots.Add(new HistoryTeacherBot("Marcus"));

            TestCommand = new RelayCommand(
                _ =>
                {
                    System.Windows.MessageBox.Show("MVVM works!");
                });

            TestCanExecuteCommand = new RelayCommand(
                _ =>
                {
                    System.Windows.MessageBox.Show("Command executed!");
                },
                _ => Robots.Count > 0);
            SummaryCommand = new RelayCommand(
    _ =>
    {
        var summary = Robots
            .GroupBy(r => r.GetType().Name)
            .Select(g => $"{g.Key}: {g.Count()}");

        System.Windows.MessageBox.Show(
            string.Join("\n", summary),
            "Robot Summary");
    }); 
        }
    }
}
