using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class Board
    {
        private int m_Rows;
        private int m_Cols;
        private Cell[,] m_GameMatrix;

        // Properties
        public int Rows
        {
            get { return m_Rows; }
            set { m_Rows = value; }
        }

        public int Cols
        {
            get { return m_Cols; }
            set { m_Cols = value; }
        }

        public Cell[,] GameMatrix
        {
            get { return m_GameMatrix; }
            set { m_GameMatrix = value; }
        }

        // using indexers
        public Cell this[int i, int j]
        {
            get { return m_GameMatrix[i, j]; }
            set { m_GameMatrix[i, j] = value; }
        }

        // Constructor
        public Board(int i_Rows, int i_Cols)
        {
            this.m_Rows = i_Rows;
            this.m_Cols = i_Cols;
            m_GameMatrix = new Cell[m_Rows, m_Cols];
        }
    }
}
