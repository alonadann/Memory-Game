using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class AI
    {
        private readonly int r_NumOfRows;
        private readonly int r_NumOfCols;
        private Dictionary<int, List<Position>> m_KnownPos;
        private List<int> m_KnownPairs;
        private Position m_NextUnknownPos;
        private bool m_IsFirstChoice;
        private int m_ChoiceIndex;

        // Constructor
        public AI(int i_numOfDifferentObjects, int i_Row, int i_Col)
        {
            m_KnownPairs = new List<int>();
            m_NextUnknownPos = new Position(0, 0);
            m_IsFirstChoice = true;
            r_NumOfRows = i_Row;
            r_NumOfCols = i_Col;
            m_ChoiceIndex = 0;
            m_KnownPos = new Dictionary<int, List<Position>>();

            for(int i = 0; i < i_numOfDifferentObjects; i++)
            {
                m_KnownPos.Add(i, new List<Position>());
            }
        }

        // Remember 2 positions for each item (using a Dictionary that has a List for the 2 positions as a value)
        public void RememberPos(Position i_Pos, int i_Val)
        {
            List<Position> currList = m_KnownPos[i_Val];

            if(!currList.Contains(i_Pos))
            {
                currList.Add(i_Pos);
            }

            if(currList.Count == 2 && !m_KnownPairs.Contains(i_Val))
            {
                m_KnownPairs.Add(i_Val);
            }
        }

        // Select cell: 
        // If there is a known pair choose a cell from the pair,
        // Otherwise randomly choose a valid cell by order starting at [0,0]
        public Position SelectCell()
        {
            Position chosenPos;

            if(m_KnownPairs.Count == 0)
            {
                chooseNextUnknownPos();
                chosenPos = m_NextUnknownPos.Clone();
            }
            else
            {
                int currVal = m_KnownPairs[0];
                if (m_IsFirstChoice)
                {
                    chosenPos = m_KnownPos[currVal][0];
                    m_ChoiceIndex = 1;
                }
                else
                {
                    chosenPos = m_KnownPos[currVal][m_ChoiceIndex];
                    m_ChoiceIndex = 0;
                }
            }

            m_IsFirstChoice = !m_IsFirstChoice;

            return chosenPos;
        }

        // Remove the item from KnownPairs (so that the computer won't choose it again)
        public void RemoveFromKnownPairs(int i_val)
        {
            m_KnownPairs.Remove(i_val);
        }

        // Randomly choose a valid and unknown cell by order starting at [0,0]
        private void chooseNextUnknownPos()
        {
            while(isInKnownPos(m_NextUnknownPos))
            {
                if(m_NextUnknownPos.Col < r_NumOfCols - 1)
                {
                    m_NextUnknownPos.Col += 1;
                }
                else
                {
                    m_NextUnknownPos.Row += 1;
                    m_NextUnknownPos.Col = 0;
                }
            }
        }

        // Check for the next unknown position (for the ramdon cell choise)
        private bool isInKnownPos(Position i_Pos)
        {
            bool found = false;
            int numOfObjects = m_KnownPos.Count;
            int val = 0;

            while(val < numOfObjects && !found)
            {
                foreach(Position pos in m_KnownPos[val])
                {
                    if(i_Pos.Equals(pos))
                    {
                        found = true;
                        break;
                    }
                }

                val++;
            }

            return found;
        }
    } 
}
