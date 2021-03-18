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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Sudoku
{
    public sealed partial class NewBoardDialog : ContentDialog
    {
        public Difficulty level;
        public bool newGame = false;

        public NewBoardDialog()
        {
            this.InitializeComponent();
        }

        private void NewBoardDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            newGame = true;

            switch (LevelComboBox.Text)
            {
                case "Simple":
                    level = Difficulty.SIMPLE;
                    break;
                case "Easy":
                    level = Difficulty.EASY;
                    break;
                case "Medium":
                    level = Difficulty.MEDIUM;
                    break;
                case "Hard":
                    level = Difficulty.HARD;
                    break;
            }
        }

        private void NewBoardDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
