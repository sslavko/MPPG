using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPPG
{
    public struct Float3Struct(float x, float y, float z)
    {
        public float X { get; internal set; } = x;
        public float Y { get; internal set; } = y;
        public float Z { get; internal set; } = z;
    }

    public struct Float4Struct(float x, float y, float z, float val)
    {
        public float X { get; internal set; } = x;
        public float Y { get; internal set; } = y;
        public float Z { get; internal set; } = z;
        public float Val { get; internal set; } = val;
    }
}
