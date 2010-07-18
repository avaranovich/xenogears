namespace XenoGears.Logging
{
    public partial class LevelLogger
    {
        public bool IsEnabled { get; set; }
        public void Enable() { IsEnabled = true; }
        public void Disable() { IsEnabled = false; }

        private bool IsMuted()
        {
            if (!Logger.IsEnabled) return true;
            if (!IsEnabled) return true;
            if (Logger.MinLevel > Level) return true;

            return false;
        }
    }
}