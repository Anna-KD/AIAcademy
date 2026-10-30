namespace AIAcademy.Core
{
    public class WorkResult
    {
        public string RobotName { get; set; } = "";

        public string Status { get; set; } = "";

        public string Result { get; set; } = "";

        public string ErrorMessage { get; set; } = "";

        public long DurationMs { get; set; }
    }
}