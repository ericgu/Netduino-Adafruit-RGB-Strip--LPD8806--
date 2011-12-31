using System;
using Microsoft.SPOT;

namespace MyApplication
{
    class Random
    {
        uint m_w;
        uint m_z;

        public Random()
        {
            m_z = (uint) DateTime.Now.Ticks;
            m_w = (uint) DateTime.Now.Ticks;
        }

        public uint GetUint()
        {
            m_z = 36969 * (m_z & 65535) + (m_z >> 16);
            m_w = 18000 * (m_w & 65535) + (m_w >> 16);
            return (m_z << 16) + m_w;
        }

        public int GetInt(int maximum)
        {
            return (int)(GetUint() % maximum);
        }

        public byte GetByte(int maximum)
        {
            return (byte)(GetUint() % maximum);
        }
    }
}
