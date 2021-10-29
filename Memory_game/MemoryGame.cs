using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class MemoryGame
    {
        // Indicates current player 
        public enum eCurrentPlayer
        {
            Player1 = -1,
            Player2 = 1
        }

        // Indicates choice to end game or continue 
        public enum eRoundChoice
        {
            Countine = 1,
            End
        }

        // Const values 
        public struct Constants
        {
            public const int k_MaxDimVal = 6, k_MinDimVal = 4;
            public const int k_ObjectShows = 2;
        }

        // MemoryGame members
        private readonly string r_Player1Name; 
        private readonly string r_Player2Name;
        private readonly bool r_IsAgainstComp;
        private int m_PointsPlayer1;
        private int m_PointsPlayer2;
        private eCurrentPlayer m_currPlayer;
        private Board m_GameBoard;
        private AI m_CompAI;

        // Properties
        public string Player1Name
        {
            get { return r_Player1Name; }
        }

        public string Player2Name
        {
            get { return r_Player2Name; }
        }

        public int PointsPlayer1
        {
            get { return m_PointsPlayer1; }
            set { m_PointsPlayer1 = value; }
        }

        public int PointsPlayer2
        {
            get { return m_PointsPlayer2; }
            set { m_PointsPlayer2 = value; }
        }

        public bool IsAgainstComp
        {
            get { return r_IsAgainstComp; }
        }

        public eCurrentPlayer currPlayer
        {
            get { return m_currPlayer; }
            set { m_currPlayer = value; }
        }

        // Constructor
        public MemoryGame(string i_Player1Name, string i_Player2Name)
        {
            r_Player1Name = i_Player1Name;
            r_Player2Name = i_Player2Name;
            r_IsAgainstComp = i_Player2Name == null;
        }

        // Start a new round with the same players - initialize everothing but the players names
        public void NewRound(int i_Rows, int i_Cols, int i_numOfDifferentObjects)
        {
            m_PointsPlayer1 = 0;
            m_PointsPlayer2 = 0;
            m_GameBoard = new Board(i_Rows, i_Cols);
            m_currPlayer = eCurrentPlayer.Player1;
            InitialGameMatrixValues(i_numOfDifferentObjects);
            if(r_IsAgainstComp)
            {
                m_CompAI = new AI(i_numOfDifferentObjects, i_Rows, i_Cols);
            }
        }

        // Randomly initialize tha game board with the numbers {0,1,2.......i_numOfDifferentObjects-1} 
        // Each number represent an element in the memory game      
        public void InitialGameMatrixValues(int i_numOfDifferentObjects)
        {
            Random rnd = new Random();
            int row, col;

            for(int i = 0; i < i_numOfDifferentObjects; i++)
            {
                for(int j = 0; j < Constants.k_ObjectShows; j++)
                {
                    do
                    {
                        row = rnd.Next(0, m_GameBoard.Rows);
                        col = rnd.Next(0, m_GameBoard.Cols);
                    }
                    while(m_GameBoard[row, col] != null);

                    m_GameBoard[row, col] = new Cell(i);
                }
            }
        }

        // Switch turns
        public void NextPlayer()
        {
            m_currPlayer = (eCurrentPlayer)((int)m_currPlayer * (-1));
        }

        // Reveal the cell on the board
        // If playing against computer - remember the chosen cell
        public void ShowCell(Position cellPos)
        {
            int cellRow = cellPos.Row;
            int cellCol = cellPos.Col;

            m_GameBoard[cellRow, cellCol].IsShown = true;
            if(r_IsAgainstComp)
            {
                m_CompAI.RememberPos(cellPos, m_GameBoard[cellRow, cellCol].Value);
            }
        }

        // Get name of player that his turn is on
        public string GetCurrPlayerName()
        {
            string name;

            if(IsComputerTurn())
            {
                name = "Computer";
            }
            else
            {
                if(m_currPlayer == eCurrentPlayer.Player1)
                {
                    name = Player1Name;
                }
                else
                {
                    name = Player2Name;
                }
            }

            return name;
        }

        // Check if the chosen cell's bounds are valid
        public bool IsPosInBounds(int i_Row, int i_Col)
        {
            return i_Row >= 0 && i_Row < m_GameBoard.Rows && i_Col >= 0 && i_Col < m_GameBoard.Cols;
        }

        // Check if the chosen cell has already been chosen (is revealed on board)
        public bool IsPosHiddenCell(int i_Row, int i_Col)
        {
            return !m_GameBoard[i_Row, i_Col].IsShown;
        }

        // Get the value in the chosen cell
        public int GetCellValue(int i_Row, int i_Col)
        {
            return m_GameBoard[i_Row, i_Col].Value;
        }

        // Get number of board's rows
        public int GetBoardRows()
        {
            return m_GameBoard.Rows;
        }

        // Get number of board's cols
        public int GetBoardCols()
        {
            return m_GameBoard.Cols;
        }

        // Check if cell value is shown and return the value's index in both cases
        public bool TryGetShownCellValue(int i_Row, int i_Col, out int io_Value)
        {
            Cell cell;

            cell = m_GameBoard[i_Row, i_Col];
            io_Value = cell.Value;

            return cell.IsShown;
        }

        // Get computer's cell choise
        public Position GetComputerCell()
        {
            return m_CompAI.SelectCell();
        }

        // Check if 2 cells are a pair (have the same value)
        public bool IsCellsHaveSameValues(Position i_Cell1Pos, Position i_Cell2Pos)
        {
            return m_GameBoard[i_Cell1Pos.Row, i_Cell1Pos.Col].Value.Equals(m_GameBoard[i_Cell2Pos.Row, i_Cell2Pos.Col].Value);
        }

        // If 2 cells aren't a pair - hide them from board
        public void HideCells(Position i_Cell1Pos, Position i_Cell2Pos)
        {
            m_GameBoard[i_Cell1Pos.Row, i_Cell1Pos.Col].IsShown = false;
            m_GameBoard[i_Cell2Pos.Row, i_Cell2Pos.Col].IsShown = false;
        }

        // If a pair is found (same valu for 2 cells):
        // Add points to the player who found the pair
        // If playing against computer - remove the value from the KnowPair List
        public void PairFound(Position i_Pos)
        {
            if(m_currPlayer == eCurrentPlayer.Player1)
            {
                m_PointsPlayer1 += 1;
            }
            else
            {
                m_PointsPlayer2 += 1;
            }

            if (r_IsAgainstComp)
            {
                int val = m_GameBoard[i_Pos.Row, i_Pos.Col].Value;
                m_CompAI.RemoveFromKnownPairs(val);
            }
        }

        // If all pairs reveald - end round
        public bool IsRoundEnded()
        {
            return PointsPlayer1 + PointsPlayer2 == (m_GameBoard.Cols * m_GameBoard.Rows) / 2;
        }

        // Check if player1 won (has more points)
        public bool IsPlayer1Won()
        {
            return PointsPlayer1 > PointsPlayer2;
        }

        // Check if it's a tie between both players (same amount of points)
        public bool IsTie()
        {
            return PointsPlayer1 == PointsPlayer2;
        }

        // Check is the chosen board dimentions are valid - min=4, max=6
        public bool IsDimensionInBounds(int i_Dim)
        {
            return i_Dim >= Constants.k_MinDimVal && i_Dim <= Constants.k_MaxDimVal;
        }

        // Check is the chosen board dimentions are valid - so that the game can make pairs (even number of cells)
        public bool IsValidBoardDimensions(int i_Rows, int i_Cols)
        {
            return (i_Rows * i_Cols) % 2 == 0;
        }

        // Check if it's computers turn
        public bool IsComputerTurn()
        {
            return r_IsAgainstComp && (m_currPlayer == eCurrentPlayer.Player2);
        }
    }
}