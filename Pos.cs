using System;

namespace GameOfLife
{
    public struct Pos : IEquatable<Pos> //A coordinate type
    {
        public Pos(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }
        public int x;
        public int y;
        public static bool operator ==(Pos pos1, Pos pos2) => (pos1.x == pos2.x) && (pos1.y == pos2.y);
        public static bool operator !=(Pos pos1, Pos pos2) => (pos1.x != pos2.x) || (pos1.y != pos2.y);
        public override string ToString()
        {
            return x + ", " + y;
        }
        //Code from Microsoft Docs; I don't really understand it, but it should reduce the amount of hash code collisions
        public int ShiftAndWrap(int val, int pos)
        {
            pos = pos & 0x1F;
            uint number = BitConverter.ToUInt32(BitConverter.GetBytes(val), 0);
            uint wrapped = number >> (32 - pos);
            return BitConverter.ToInt32(BitConverter.GetBytes((number << pos | wrapped)), 0);
        }
        public override int GetHashCode()
        {
            return ShiftAndWrap(x, 2) ^ y;
        }
        // override object.Equals
        public override bool Equals(object obj)
        {
            if (!(obj is Pos)) return false;
            Pos pos = (Pos) obj;
            return x == pos.x && y == pos.y;
        }
        //the implementation of IEquatable
        public bool Equals(Pos pos)
        {
            return x == pos.x && y == pos.y;
        }
    }
}