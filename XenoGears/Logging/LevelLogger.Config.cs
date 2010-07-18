namespace XenoGears.Logging
{
    public partial class LevelLogger
    {
        public bool IsEnabled { get; set; }
        public void Enable() { IsEnabled = true; }
        public void Disable() { IsEnabled = false; }

        private bool IsMuted()
        {
            if (!IsEnabled) return true;
            if (Logger.Level > Level) return true;

            return false;
        }
    }
}