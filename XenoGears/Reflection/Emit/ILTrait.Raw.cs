using System;
using System.Reflection.Emit;
using XenoGears.Functional;
using XenoGears.Reflection.Typed;

namespace XenoGears.Reflection.Emit
{
    // todo. this doesn't take into account all the intricacies of ILGenerator
    // e.g. ain't update maxstack, tokenfixups and God knows what else

    public static partial class ILTrait
    {
        public static ILGenerator raw(this ILGenerator il, OpCode opcode, byte[] operand)
        {
            var ensureCapacity = il.GetAction<int>("EnsureCapacity");
            var internalEmit = il.GetAction<OpCode>("InternalEmit");
            Action<byte> writeByte = b =>
            {
                var m_ILStream = il.GetSlot<byte[]>("m_ILStream");
                var m_length = il.GetSlot<int>("m_length");
                m_ILStream.Value[m_length.Value++] = b;
            };

            ensureCapacity(opcode.Size + operand.Length);
            internalEmit(opcode);
            operand.ForEach(writeByte);

            return il;
        }
    }
}
