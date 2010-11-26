namespace XenoGears.Logging
{
    public partial class Logger
    {
        public Level MinLevel { get; set; }

        public bool IsEnabled { get; set; }
        public void Enable() { IsEnabled = true; }
        public void Disable() { IsEnabled = false; }
    }
}
