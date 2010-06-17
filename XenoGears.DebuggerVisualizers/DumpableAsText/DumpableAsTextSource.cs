using System;
using System.IO;
using Microsoft.VisualStudio.DebuggerVisualizers;

namespace XenoGears.DebuggerVisualizers.DumpableAsText
{
    public class DumpableAsTextSource : VisualizerObjectSource
    {
        public override void GetData(Object target, Stream outgoingData)
        {
            throw new NotImplementedException();
        }

        public override Object CreateReplacementObject(Object target, Stream incomingData)
        {
            throw new NotImplementedException();
        }

        public override void TransferData(Object target, Stream incomingData, Stream outgoingData)
        {
            throw new NotImplementedException();
        }
    }
}