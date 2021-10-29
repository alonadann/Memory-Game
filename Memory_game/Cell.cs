using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class Cell
    {
        private int m_Value;
        private bool m_IsShown;

        // Properties
        public int Value
        {
            get { return m_Value; }
            set { m_Value = value; }
        }

        public bool IsShown
        {
            get { return m_IsShown; }
            set { m_IsShown = value; }
        }

        // Constructor
        public Cell(int i_Value)
        {
            m_IsShown = false;
            m_Value = i_Value;
        }
    }
}
