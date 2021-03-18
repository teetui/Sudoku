using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Sudoku
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private TextBlock currentBlock = null;
        private Sudoku sudoku = new Sudoku();

        private Cell getCell(TextBlock textBlock)
        {
            int row = textBlock.Name[5] - 49;
            int col = textBlock.Name[6] - 49;
            return sudoku.Board[row, col];
        }

        private void FillBoard()
        {
            for (int row = 0; row < 9; row++)
                for (int col = 0; col < 9; col++)
                {
                    TextBlock textBlock = (TextBlock)this.FindName("Block" + (row + 1).ToString() + (col + 1).ToString());
                    if (sudoku.Board[row, col].value != 0)
                        textBlock.Text = sudoku.Board[row, col].value.ToString();
                    else
                        textBlock.Text = "";

                    if (sudoku.Board[row, col].original)
                        textBlock.FontWeight = Windows.UI.Text.FontWeights.Bold;

                    Border border = (Border)this.FindName("Border" + (row + 1).ToString() + (col + 1).ToString());
                    border.Tapped += new TappedEventHandler(BorderTapped);
                }
        }

        public MainPage()
        {
            this.InitializeComponent();
            sudoku.Generate(Difficulty.SIMPLE);
            FillBoard();
        }

        private async void NewBoard()
        {
            NewBoardDialog dialog = new NewBoardDialog();
            await dialog.ShowAsync();
            if (dialog.newGame)
            {
                sudoku.Generate(dialog.level);
                FillBoard();
            }
        }

        private void BlockTapped(object sender, TappedRoutedEventArgs e)
        {
            TextBlock textBlock = (TextBlock)sender;

            if (currentBlock != null)
            {
                if (PencilMarks.IsChecked == true)
                    currentBlock.FontSize = 12;
                else
                    currentBlock.FontSize = 24;

                if (textBlock.Text.Equals("X"))
                    currentBlock.Text = "";
                else
                    currentBlock.Text = textBlock.Text;
            }

            if (sudoku.IsSolved())
                NewBoard();
        }

        private void BorderTapped(object sender, TappedRoutedEventArgs e)
        {
            Border border = (Border)sender;
            TextBlock textBlock = (TextBlock)border.Child;

            if (getCell(textBlock).original)
                return;

            // highlight the tapped game block 
            textBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Yellow);
            textBlock.FontWeight = Windows.UI.Text.FontWeights.Normal;
            border.BorderThickness = new Thickness(2);
            border.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Yellow);

            if (currentBlock != null && currentBlock != textBlock)
            {
                // unhighlight the previous block
                currentBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                ((Border)currentBlock.Parent).BorderThickness = new Thickness(1);
                ((Border)currentBlock.Parent).BorderBrush = new SolidColorBrush(Windows.UI.Colors.White);
            }

            currentBlock = textBlock;
        }

        private void SolveButton(object sender, TappedRoutedEventArgs e)
        {
            for (int row = 0; row < 9; row++)
                for (int col = 0; col < 9; col++)
                {
                    TextBlock textBlock = (TextBlock)this.FindName("Block" + (row + 1).ToString() + (col + 1).ToString());
                    if (textBlock.FontSize == 12)
                    {
                        textBlock.Text = "";
                        textBlock.FontSize = 24;
                    }
                }
            
            sudoku.Solve(sudoku.Board);
            FillBoard();
        }

        private void NewButton(object sender, TappedRoutedEventArgs e)
        {
            NewBoard();
        }
    }
}