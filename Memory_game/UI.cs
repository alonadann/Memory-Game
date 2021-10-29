using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class UI
    {
        // Indicates player's type 
        public enum ePlayerChoice
        {
            Computer = 1,
            Player2
        }

        // class members 
        private MemoryGame m_Game;
        private char[] m_ObjectsToShowOnBoard;

        // The function runs the memoey game
        public void RunGame()
        {
            GetPlayersData();
            do
            {
                StartNewRound();
            }
            while(IsBeginAnotherRound());
        }

        // The function get data of the players 
        public void GetPlayersData()
        {
            bool success;
            string player1Name, player2Name = null;

            player1Name = GetName();
            Console.WriteLine("Press 1 to play against computer , press 2 to play with other player");
            success = int.TryParse(Console.ReadLine(), out int choice);
            while(!success || !IsValidPlayer2Choice(choice))
            {
                Console.WriteLine("Wrong input , please try again ");
                success = int.TryParse(Console.ReadLine(), out choice);
            }

            if(choice == (int)ePlayerChoice.Player2)
            {
                player2Name = GetName();
            }

            m_Game = new MemoryGame(player1Name, player2Name);
        }

        // Check if the given string is valid name
        public bool IsValidUserName(string i_Name)
        {
            return i_Name != string.Empty;
        }

        // The function gets a valid name 
        public string GetName()
        {
            string name;

            Console.WriteLine("Please enter your name");
            name = Console.ReadLine();
            while(!IsValidUserName(name))
            {
                Console.WriteLine("Invalid name, try again");
                name = Console.ReadLine();
            }

            return name;
        }

        // The function get the board dimensions, initialize the values of the game board and start the fisrstplayer turn 
        public void StartNewRound()
        {
            int numOfDifferentChars;

            GetDimensions(out int rows, out int cols);
            numOfDifferentChars = (rows * cols) / 2;
            m_ObjectsToShowOnBoard = GenerateInitialGameMatrixValues(numOfDifferentChars);
            m_Game.NewRound(rows, cols, numOfDifferentChars);
            Play();
        }

        // Gets the board dimensions
        public void GetDimensions(out int io_Rows, out int io_Cols)
        {
            Console.WriteLine("Enter number of coloumns(number between 4 to 6)");
            GetOneDimension(out io_Cols);
            Console.WriteLine("Enter number of rows(number between 4 to 6)");
            GetOneDimension(out io_Rows);
            while(!m_Game.IsValidBoardDimensions(io_Rows, io_Cols))
            {
                Console.WriteLine("wrong input! number of cells in board should be even , please try again");
                GetDimensions(out io_Rows, out io_Cols);
            }
        }

        // Gets one dimension of the board 
        public void GetOneDimension(out int o_dim)
        {
            bool success = int.TryParse(Console.ReadLine(), out o_dim);
            while(!success || !m_Game.IsDimensionInBounds(o_dim))
            {
                Console.WriteLine("Wrong input please enter again");
                success = int.TryParse(Console.ReadLine(), out o_dim);
            }
        }

        // Check if the choice of player type is valid 
        public bool IsValidPlayer2Choice(int i_Choice)
        {
            return (ePlayerChoice)i_Choice == ePlayerChoice.Computer || (ePlayerChoice)i_Choice == ePlayerChoice.Player2;
        }

        // Print the game board 
        public void PrintMatrix()
        {
            char colName = 'A';
            char value = ' ';
            StringBuilder row = new StringBuilder("    ");
            StringBuilder separator = new StringBuilder("  =");

            for (int j = 0; j < m_Game.GetBoardCols(); j++)
            {
                row.Append(colName + "   ");
                colName = (char)((int)colName + 1);
                separator.Append("====");
            }

            Console.WriteLine(row);
            Console.WriteLine(separator);
            for (int i = 0; i < m_Game.GetBoardRows(); i++)
            {
                row = new StringBuilder((i + 1) + " |");
                for (int j = 0; j < m_Game.GetBoardCols(); j++)
                {
                    value = ' ';
                    if (!m_Game.IsPosHiddenCell(i, j))
                    {
                        // Get the cell value in the board (numbers {0,1,2.......i_numOfDifferentObjects-1})
                        // Convert each number to the element it represents in the game
                        value = m_ObjectsToShowOnBoard[m_Game.GetCellValue(i, j)];
                    }

                    row.Append(" " + value + " |");
                }

                Console.WriteLine(row);
                Console.WriteLine(separator);
            }
        }

        // select a cell in the game board
        public Position GetCell()
        {
            Position cellPos;

            if(m_Game.IsComputerTurn())
            {
                cellPos = m_Game.GetComputerCell();
            }
            else
            {
                do
                {
                    cellPos = GetUserCell();
                }
                while(!IsValidChosenPos(cellPos.Row, cellPos.Col));
            }

            return cellPos;
        }

        // Request for cell position from the user, return the position of the wanted cell, check validity , and exit the game if 'Q' is given  
        public Position GetUserCell()
        {
            string cellSring;
            int colNum, rowNum;

            Console.WriteLine("Please enter cell");
            cellSring = Console.ReadLine();
            while(!IsInputRepresentCell(cellSring))
            {
                if(cellSring == "Q")
                {
                    ExitGame();
                }
                else
                {
                    Console.WriteLine("Wrong input ,please try again");
                    cellSring = Console.ReadLine();
                }
            }

            colNum = cellSring[0] - 'A';
            rowNum = cellSring[1] - '1';

            return new Position(rowNum, colNum);
        }

        // Generate and initialize the values of the game board
        public char[] GenerateInitialGameMatrixValues(int i_NumOfDifferentChars)
        {
            char[] values = new char[i_NumOfDifferentChars];
            char currVal = 'A';

            for(int i = 0; i < i_NumOfDifferentChars; i++)
            {
                values[i] = currVal;
                currVal = (char)((int)currVal + 1);
            }

            return values;
        }

        // check if the given string represent cell position in the game board
        public bool IsInputRepresentCell(string i_StringCell)
        {
            return i_StringCell.Length == 2 && char.IsUpper(i_StringCell[0]) && char.IsDigit(i_StringCell[1]);
        }

        // check if the given input is valid cell to select
        public bool IsValidChosenPos(int i_Row, int i_Col)
        {
            bool isValid = false;

            if(!m_Game.IsPosInBounds(i_Row, i_Col))
            {
                Console.WriteLine("The chosen cell is not in board bounds");
            }
            else
            {
                if(!m_Game.IsPosHiddenCell(i_Row, i_Col))
                {
                    Console.WriteLine("The chosen cell is not an hidden cell");
                }
                else
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        // runs one round and summarize the result of the round .
        public void Play()
        {
            Position cell1Pos, cell2Pos;

            while (!m_Game.IsRoundEnded())
            {
                PrintMatrix();
                Console.WriteLine(m_Game.GetCurrPlayerName() + "'s turn :");
                cell1Pos = GetCell();
                m_Game.ShowCell(cell1Pos);
                Ex02.ConsoleUtils.Screen.Clear();
                PrintMatrix();
                cell2Pos = GetCell();
                m_Game.ShowCell(cell2Pos);
                Ex02.ConsoleUtils.Screen.Clear();
                PrintMatrix();
                if(!m_Game.IsCellsHaveSameValues(cell1Pos, cell2Pos))
                {
                    m_Game.HideCells(cell1Pos, cell2Pos);
                    System.Threading.Thread.Sleep(2000);
                    m_Game.NextPlayer();
                }
                else
                {
                    m_Game.PairFound(cell1Pos);
                }

                Ex02.ConsoleUtils.Screen.Clear();
            }

            SummarizeRound();
        }

        // Print the winner , print number of points of each player 
        public void SummarizeRound()
        {
            StringBuilder msg = new StringBuilder(string.Empty);
            string player2Name = null;

            PrintMatrix();
            if(m_Game.IsTie())
            {
                msg.Append("It is a tie!");
            }
            else
            {
                if(m_Game.IsPlayer1Won())
                {
                    msg.Append(m_Game.Player1Name + " has won ");
                }
                else
                {
                    if(m_Game.IsAgainstComp)
                    {
                        player2Name = "The Computer";
                    }
                    else
                    {
                        player2Name = m_Game.Player2Name;
                    }

                    msg.Append(player2Name + " has won " + Environment.NewLine);
                }
            }

            msg.AppendFormat(
@"{0}'s points:{1} 
{2}'s points:{3} ",
            m_Game.Player1Name,
            m_Game.PointsPlayer1,
            player2Name,
            m_Game.PointsPlayer2);
            Console.WriteLine(msg);
        }

        // Starts new round or exit the program according to the user wish
        public bool IsBeginAnotherRound()
        {
            MemoryGame.eRoundChoice choice;
            bool success;

            Console.WriteLine("To begin another round press 1 , to end game press 2");
            success = int.TryParse(Console.ReadLine(), out int intRoundChoice);
            while(!success || !IsValidRoundChoice((MemoryGame.eRoundChoice)intRoundChoice))
            {
                Console.WriteLine("Wrong input , please try again");
                success = int.TryParse(Console.ReadLine(), out intRoundChoice);
            }

            choice = (MemoryGame.eRoundChoice)intRoundChoice;
            if (choice == MemoryGame.eRoundChoice.End)
            {
                ExitGame();
            }

            return choice == MemoryGame.eRoundChoice.Countine;
        }

        // check if the given round choice is valid
        public bool IsValidRoundChoice(MemoryGame.eRoundChoice roundChoice)
        {
            return (roundChoice != MemoryGame.eRoundChoice.End) || (roundChoice != MemoryGame.eRoundChoice.Countine);
        }
        
        // exit from program 
        public void ExitGame()
        {
            Console.WriteLine("Goodbye!");
            System.Threading.Thread.Sleep(2000);
            Environment.Exit(0);
        }
    }
}