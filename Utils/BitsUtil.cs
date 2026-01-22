using System;
using System.Runtime.CompilerServices;

namespace DevKit.Utils
{
    public static class BitsUtil
    {
        public static bool IsValidFloatBitIndex(int bitIndex)
        {
            return bitIndex >= 0 && bitIndex < 32;
        }

        public static bool IsValidIntBitIndex(int bitIndex)
        {
            return bitIndex >= 0 && bitIndex < 32;
        }

        public static bool IsValidUShortBitIndex(int bitIndex)
        {
            return bitIndex >= 0 && bitIndex < 16;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetBitByIndex(int value, int bitIndex)
        {
            return (value & (1 << bitIndex)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetBitByIndex(ushort value, int bitIndex)
        {
            return (value & (1u << bitIndex)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetBitByIndex(float value, int bitIndex)
        {
            return (BitConverter.SingleToInt32Bits(value) & (1 << bitIndex)) != 0;
        }
    }
}