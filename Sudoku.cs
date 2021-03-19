using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public enum Difficulty
    {
        SIMPLE,
        EASY,
        MEDIUM,
        HARD
    }

    class Sudoku
    {
        public Cell[,] Board
        { get; set; }

        private void Initialize()
        {
            Board = new Cell[9,9];
            for (int row = 0; row < 9; row++)
                for (int col = 0; col < 9; col++)
                {
                    Board[row, col] = new Cell
                    {
                        value = 0,
                        original = true,
                        pencilmark = false
                    };
                }
        }

        public bool IsSolved()
        {
            for (int i = 0; i < 9; i++)
            {
                List<int> rows = new List<int>();
                List<int> cols = new List<int>();
                List<int> subgrids = new List<int>();

                for (int j = 0; j < 9; j++)
                {
                    // check for zeroes and pencil marks
                    if (Board[i, j].value == 0 || Board[i, j].pencilmark)
                        return false;

                    // check rows for non-unique values
                    if (rows.Contains(Board[i, j].value))
                        return false;
                    else
                        rows.Add(Board[i, j].value);

                    // check cols for non-unique values
                    if (cols.Contains(Board[j, i].value))
                        return false;
                    else
                        cols.Add(Board[j, i].value);

                    // check subgrids for non-unique values
                    if (subgrids.Contains(Board[3 * (i / 3) + j / 3, 3 * (i / 3) + j % 3].value))
                        return false;
                    else
                        subgrids.Add(Board[3 * (i / 3) + j / 3, 3 * (i / 3) + j % 3].value);
                }
            }

            return true;
        }

        private bool IsValid(Cell[,] grid, int row, int col, int value)
        {
            for (int i = 0; i < 9; i++)
            {
                // check rows
                if (grid[i, col].value != 0 && grid[i, col].value == value)
                    return false;

                // check columns
                if (grid[row, i].value != 0 && grid[row, i].value == value)
                    return false;

                // check subgrids
                if (grid[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3].value != 0 && 
                    grid[3 * (row / 3) + i / 3, 3 * (col / 3) + i % 3].value == value)
                    return false;
            }
            return true;
        }

        public bool Solve(Cell[,] grid)
        {
            for (int row = 0; row < 9; row++)
                for (int col = 0; col < 9; col++)
                    if (grid[row, col].value == 0)
                    {
                        Random r = new Random();
                        foreach (int value in Enumerable.Range(1, 9).OrderBy(x => r.Next()))
                        {
                            if (IsValid(grid, row, col, value))
                            {
                                grid[row, col].value = value;
                                if (Solve(grid))
                                    return true;
                                else
                                    grid[row, col].value = 0;
                            }
                        }
                        return false;
                    }

            return true;
        }

        private void Remove(Difficulty d)
        {
            int row, col, level;

            switch (d)
            {
                case Difficulty.EASY:
                    level = 24;
                    break;
                case Difficulty.MEDIUM:
                    level = 22;
                    break;
                case Difficulty.HARD:
                    level = 20;
                    break;
                default:
                    level = 26;
                    break;
            }

            for (int i = 81; i > level; i--)
            {
                do
                {
                    row = new Random().Next(9);
                    col = new Random().Next(9);
                } 
                while (Board[row, col].value == 0);

                Board[row, col].value = 0;
                Board[row, col].original = false;
            }
        }

        public void Generate(Difficulty d)
        {
            Initialize();
            Solve(Board);
            Remove(d);
        }
    }
}
