using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XenoGears.Functional;
using XenoGears.Traits.Disposable;
using XenoGears.Assertions;

namespace XenoGears.Logging
{
    public partial class LogWriter
    {
        public readonly Guid Id = Guid.NewGuid();
        public Level Level { get; set; }

        public bool IsEnabled { get; set; }
        public void Enable() { IsEnabled = true; }
        public void Disable() { IsEnabled = false; }

        public LogWriter()
        {
#if DEBUG
            Level = Level.Debug;
#else
            Level = Level.Info;
#endif

            IsEnabled = true;
        }

        public LogWriter(TextWriter medium)
            : this()
        {
            Medium = medium;
        }

        private readonly HashSet<Logger> _muted = new HashSet<Logger>();
        public IDisposable Mute(params Logger[] loggers) { return Mute((IEnumerable<Logger>)loggers); }
        public IDisposable Mute(IEnumerable<Logger> loggers) { loggers.ForEach(logger => _muted.Add(logger)); return new DisposableAction(() => loggers.ForEach(logger => _muted.Remove(logger))); }
        public IDisposable Unmute(params Logger[] loggers) { return Unmute((IEnumerable<Logger>)loggers); }
        public IDisposable Unmute(IEnumerable<Logger> loggers) { loggers.ForEach(logger => _muted.Remove(logger)); return new DisposableAction(() => loggers.ForEach(logger => _muted.Add(logger))); }

        private readonly HashSet<String> _mutedNames = new HashSet<String>();
        public IDisposable Mute(params String[] loggers) { return Mute((IEnumerable<String>)loggers); }
        public IDisposable Mute(IEnumerable<String> loggers) { loggers.ForEach(logger => _mutedNames.Add(logger)); return new DisposableAction(() => loggers.ForEach(logger => _mutedNames.Remove(logger))); }
        public IDisposable Unmute(params String[] loggers) { return Unmute((IEnumerable<String>)loggers); }
        public IDisposable Unmute(IEnumerable<String> loggers) { loggers.ForEach(logger => _mutedNames.Remove(logger)); return new DisposableAction(() => loggers.ForEach(logger => _mutedNames.Add(logger))); }

        private readonly HashSet<Func<Logger, bool>> _muteFilters = new HashSet<Func<Logger, bool>>();
        public IDisposable Mute(params Func<Logger, bool>[] loggers) { return Mute((IEnumerable<Func<Logger, bool>>)loggers); }
        public IDisposable Mute(IEnumerable<Func<Logger, bool>> loggers) { loggers.ForEach(logger => _muteFilters.Add(logger)); return new DisposableAction(() => loggers.ForEach(logger => _muteFilters.Remove(logger))); }
        public IDisposable Unmute(params Func<Logger, bool>[] loggers) { return Unmute((IEnumerable<Func<Logger, bool>>)loggers); }
        public IDisposable Unmute(IEnumerable<Func<Logger, bool>> loggers) { loggers.ForEach(logger => _muteFilters.Remove(logger)); return new DisposableAction(() => loggers.ForEach(logger => _muteFilters.Add(logger))); }

        private bool IsMuted(Logger logger, Level level)
        {
            if (!IsEnabled) return true;
            if (Level > level) return true;

            logger.AssertNotNull();
            if (_muted.Contains(logger)) return true;
            if (_mutedNames.Contains(logger.Name)) return true;
            if (_muteFilters.Any(f => f(logger))) return true;

            return false;
        }
    }
}
