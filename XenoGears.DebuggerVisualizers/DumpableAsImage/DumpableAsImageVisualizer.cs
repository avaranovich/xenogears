using System;
using System.Diagnostics;
using Microsoft.VisualStudio.DebuggerVisualizers;
using XenoGears.DebuggerVisualizers.DumpableAsImage;
using XenoGears.Traits.Dumpable;

[assembly: DebuggerVisualizer(typeof(DumpableAsImageVisualizer), typeof(DumpableAsImageSource), Target = typeof(IDumpableAsImage), Description = "Image Visualizer")]

namespace XenoGears.DebuggerVisualizers.DumpableAsImage
{
    public class DumpableAsImageVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            throw new NotImplementedException();
        }
    }
}