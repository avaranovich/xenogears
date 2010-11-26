using System;
using System.Diagnostics;
using Microsoft.VisualStudio.DebuggerVisualizers;
using XenoGears.DebuggerVisualizers.DumpableAsText;
using XenoGears.Traits.Dumpable;

[assembly: DebuggerVisualizer(typeof(DumpableAsTextVisualizer), typeof(DumpableAsTextSource), Target = typeof(IDumpableAsText), Description = "Text Visualizer")]

namespace XenoGears.DebuggerVisualizers.DumpableAsText
{
    public class DumpableAsTextVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            throw new NotImplementedException();
        }
    }
}