using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.DebuggerVisualizers;
using XenoGears.DebuggerVisualizers.Hierarchy;
using XenoGears.Traits.Hierarchy;
using XenoGears.Assertions;

[assembly: DebuggerVisualizer(typeof(HierarchyVisualizer), typeof(HierarchySource), Target = typeof(IImmutableHierarchy), Description = "Hierarchy Visualizer")]

namespace XenoGears.DebuggerVisualizers.Hierarchy
{
    public class HierarchyVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var model = objectProvider.GetObject().AssertCast<HierarchyModel>();
            MessageBox.Show("Deserialized " + model.Root.Family().Count() + " objects");
        }
    }
}