using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class Position
    {
        // Position's  members
        private int m_Row; 
        private int m_Col;

        // Properties
        public int Row
        {
            get { return m_Row; }
            set { m_Row = value; }
        }

        public int Col
        {
            get { return m_Col; }
            set { m_Col = value; }
        }

        // Constructor 
        public Position(int i_Row, int i_Col)
        {
            m_Row = i_Row;
            m_Col = i_Col;
        }

        // Check if 2 given positions are equals 
        public static bool operator ==(Position i_Pos1, Position i_Pos2)
        {
            return i_Pos1.Equals(i_Pos2);
        }

        // Check if 2 given positions are equals 
        public static bool operator !=(Position i_Pos1, Position i_Pos2)
        {
            return !(i_Pos1 == i_Pos2);
        }

        // Clone the current position
        public Position Clone()
        {
            return this.MemberwiseClone() as Position;
        }

        // Cverride the Equals function check if the current position is equals the given object
        public override bool Equals(object i_Obj)
        {
            return i_Obj is Position Pos ? (object.ReferenceEquals(this, Pos) || (Col == Pos.Col && Row == Pos.Row)) : false;
        }

        // Cverrides GetHashCode function 
        public override int GetHashCode()
        {
            int hash = 17;

            hash = (hash * 23) + m_Row.GetHashCode();
            hash = (hash * 23) + m_Col.GetHashCode();

            return hash;
        }
    }
}
