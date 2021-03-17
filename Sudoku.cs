﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    class Sudoku
    {
        public Cell[,] Board
        {get; set; }

        public readonly int SIMPLE = 26;
        public readonly int EASY   = 24;
        public readonly int MEDIUM = 22;
        public readonly int HARD = 20;

        private void Initialize()
        {
            Board = new Cell[9,9];
            for (int row = 0; row < 9; row++)
                for (int col = 0; col < 9; col++)
                {
                    Board[row, col] = new Cell
                    {
                        value = 0,
                        original = true
                    };
                }
        }

        private bool IsValid(Cell[,] grid, int row, int col, int value)
         {
            for (int i = 0; i < 9; i++)
            {
                //check row  
                if (grid[i, col].value != 0 && grid[i, col].value == value)
                    return false;
                //check column  
                if (grid[row, i].value != 0 && grid[row, i].value == value)
                    return false;
                //check 3*3 block  
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

        private void Remove(int level)
        {
            int row, col;
            for (int i = 81; i > level; i--)
            {
                do
                {
                    row = new Random().Next(9);
                    col = new Random().Next(9);
                } while (Board[row, col].value == 0);
                Board[row, col].value = 0;
                Board[row, col].original = false;
            }
        }

        public void Generate(int level)
        {
            Initialize();
            Solve(Board);
            Remove(level);
        }

        public bool IsSolved()
        {
            for (int row = 0; row < 9; row++)
                for (int col = 0; col < 9; col++)
                    if (Board[row, col].value == 0 || IsValid(Board, row, col, Board[row, col].value) == false)
                        return false;

            return true;
        }
    }
}