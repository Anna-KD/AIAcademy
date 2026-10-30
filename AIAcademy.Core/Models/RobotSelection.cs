using AIAcademy.Core.Base;

namespace AIAcademy.WPF.Models
{
    public class RobotSelection
    {
        public RobotTeacher Robot { get; set; } = null!;

        public bool IsSelected { get; set; } = true;

        public string Name => Robot.Name;

        public string Type => Robot.TypeName;
    }
}
