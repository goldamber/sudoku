using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Task_11
{
    public partial class MainWindow : Window
    {
        Sudoku game = new Sudoku();

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SudokuGenerate();
        }

        void SudokuGenerate()
        {            
            Random rnd = new Random();
            bool rev = (rnd.Next(0, 2) == 0) ? false : true; 
            grBody.Children.Clear();

            game.ArrData.Clear();
            int[] arr = new int[] { 1, 4, 7, 2, 5, 8, 3, 6, 9 };
            for (int i = 0; i < 9; i++)
            {
                game.Generate(arr[i] - 1);
            }
            int first = 0;
            int inc = 0;
            int second = 0;
            
            for (int i = 0; i < rnd.Next(6, 21); i++)
            {
                first = rnd.Next(0, 3);
                second = (first == 1) ? first + (rnd.Next(-1, 2)) : ((first > 1) ? first - (rnd.Next(1, 3)) : first + (rnd.Next(1, 3)));
                inc = rnd.Next(0, 3) * 3;
                game.SortCol(first + inc, second + inc);
            }
            for (int i = 0; i < rnd.Next(6, 21); i++)
            {
                first = rnd.Next(0, 3);
                second = (first == 1) ? first + (rnd.Next(-1, 2)) : ((first > 1) ? first - (rnd.Next(1, 3)) : first + (rnd.Next(1, 3)));
                inc = rnd.Next(0, 3) * 3;
                game.SortRow(first + inc, second + inc);
            }
            if (rev)
                game.ReverseRow();

            game.NullGrid();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Border br = new Border { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1.5) };
                    Grid tmp = new Grid();
                    tmp.RowDefinitions.Add(new RowDefinition());
                    tmp.RowDefinitions.Add(new RowDefinition());
                    tmp.RowDefinitions.Add(new RowDefinition());
                    tmp.ColumnDefinitions.Add(new ColumnDefinition());
                    tmp.ColumnDefinitions.Add(new ColumnDefinition());
                    tmp.ColumnDefinitions.Add(new ColumnDefinition());
                    Grid.SetColumn(br, j);
                    Grid.SetRow(br, i);

                    for (int a = 0; a < 3; a++)
                    {
                        for (int b = 0; b < 3; b++)
                        {
                            TextBox lb = new TextBox { FontWeight = FontWeights.Bold, IsReadOnly = true, FontSize = 14, VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Center, BorderBrush = Brushes.Silver, BorderThickness = new Thickness(1) };
                            Grid.SetColumn(lb, a);
                            Grid.SetRow(lb, b);
                            lb.MouseEnter += Lb_MouseEnter;
                            lb.MouseLeave += Lb_MouseLeave;

                            tmp.Children.Add(lb);
                        }
                    }
                    
                    br.Child = tmp;
                    grBody.Children.Add(br);
                }
            }
            
            foreach (Border item in grBody.Children)
            {                
                foreach (TextBox val in (item.Child as Grid).Children)
                {
                    if (game.ArrData[Grid.GetRow(val) + Grid.GetRow((val.Parent as Grid).Parent as Border) * 3][Grid.GetColumn(val) + Grid.GetColumn((val.Parent as Grid).Parent as Border) * 3] == 0)
                    {
                        val.Text = "";
                        val.IsReadOnly = false;
                        val.Opacity = 0.4;
                        val.TextChanged += Val_TextChanged;
                    }
                    else
                        val.Text = Convert.ToString(game.ArrData[Grid.GetRow(val) + Grid.GetRow((val.Parent as Grid).Parent as Border)*3][Grid.GetColumn(val) + Grid.GetColumn((val.Parent as Grid).Parent as Border) * 3]);
                }
            }
        }

        private void Val_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex reg = new Regex("^[1-9]$");
            if (!reg.IsMatch((sender as TextBox).Text))
            {
                (sender as TextBox).BorderBrush = Brushes.Red;
                (sender as TextBox).BorderThickness = new Thickness(1);
                (sender as TextBox).Text = "";
            }
            else
            {
                (sender as TextBox).BorderBrush = Brushes.Silver;
                (sender as TextBox).BorderThickness = new Thickness(1);
            }

            if (Check())
            {
                if (MessageBoxResult.OK == MessageBox.Show("You win!", "Right", MessageBoxButton.OK))
                {
                    // Application.Current.Shutdown();
                    dpSudokuGame.Visibility = Visibility.Collapsed;
                    grStart.Visibility = Visibility.Visible;
                }
            }
        }

        bool CheckNum (string num, Grid parent, int col, int row, Brush fColor, Brush bColor)
        {
            if (num == "")
                return false;

            bool single = true;

            foreach (Border item in grBody.Children)
            {
                if (Grid.GetColumn(item) == Grid.GetColumn((parent.Parent as Border)))
                {
                    foreach (TextBox val in (item.Child as Grid).Children)
                    {
                        if (col != Grid.GetColumn(val) && Grid.GetRow(item) == Grid.GetRow((parent.Parent as Border)) || row != Grid.GetRow(val) && Grid.GetRow(item) == Grid.GetRow((parent.Parent as Border)) || Grid.GetRow(item) != Grid.GetRow((parent.Parent as Border)))
                        {
                            if (Convert.ToString(val.Text) == num && col == Grid.GetColumn(val))
                            {
                                single = false;
                                val.Foreground = fColor;
                                val.Background = bColor;
                            }
                        }
                    }
                }
            }
            foreach (Border item in grBody.Children)
            {
                if (Grid.GetRow(item) == Grid.GetRow((parent.Parent as Border)))
                {
                    foreach (TextBox val in (item.Child as Grid).Children)
                    {
                        if (row != Grid.GetRow(val) && Grid.GetColumn(item) == Grid.GetColumn((parent.Parent as Border)) || col != Grid.GetColumn(val) && Grid.GetColumn(item) == Grid.GetColumn((parent.Parent as Border)) || Grid.GetColumn(item) != Grid.GetColumn((parent.Parent as Border)))
                        {
                            if (Convert.ToString(val.Text) == num && row == Grid.GetRow(val))
                            {
                                single = false;
                                val.Foreground = fColor;
                                val.Background = bColor;
                            }
                        }
                    }
                }
            }

            return single;
        }
        bool Check()
        {
            bool unique = true;

            foreach (Border item in grBody.Children)
            {
                foreach (TextBox val in (item.Child as Grid).Children)
                {
                    if (!CheckNum(Convert.ToString(val.Text), (val.Parent as Grid), Grid.GetColumn(val), Grid.GetRow(val), val.Foreground, val.Background))
                    {
                        unique = false;
                        break;
                    }
                }
            }

            return unique;
        }

        private void Lb_MouseLeave(object sender, MouseEventArgs e)
        {
            int row = Grid.GetRow((sender as TextBox));
            int col = Grid.GetColumn((sender as TextBox));
            Grid gr = ((sender as TextBox).Parent as Grid);
            foreach (Border item in grBody.Children)
            {
                if (Grid.GetRow(item) == Grid.GetRow((gr.Parent as Border)))
                {
                    foreach (dynamic val in (item.Child as Grid).Children)
                    {
                        if (Grid.GetRow(val) == row && val is TextBox)
                            (val as TextBox).Background = Brushes.White;
                    }
                }
            }
            foreach (Border item in grBody.Children)
            {
                if (Grid.GetColumn(item) == Grid.GetColumn((gr.Parent as Border)))
                {
                    foreach (dynamic val in (item.Child as Grid).Children)
                    {
                        if (Grid.GetColumn(val) == col && val is TextBox)
                            (val as TextBox).Background = Brushes.White;
                    }
                }
            }

            try
            {
                (sender as TextBox).Background = Brushes.White;
                CheckNum(Convert.ToString((sender as TextBox).Text), ((sender as TextBox).Parent as Grid), Grid.GetColumn((sender as TextBox)), Grid.GetRow((sender as TextBox)), Brushes.Black, Brushes.White);
            }
            catch { }
        }
        private void Lb_MouseEnter(object sender, MouseEventArgs e)
        {
            int row = Grid.GetRow((sender as TextBox));
            int col = Grid.GetColumn((sender as TextBox));
            Grid gr = ((sender as TextBox).Parent as Grid);
            foreach (Border item in grBody.Children)
            {
                if (Grid.GetRow(item) == Grid.GetRow((gr.Parent as Border)))
                {
                    foreach (dynamic val in (item.Child as Grid).Children)
                    {
                        if (Grid.GetRow(val) == row && val is TextBox)
                            (val as TextBox).Background = Brushes.LightGray;
                    }
                }
            }
            foreach (Border item in grBody.Children)
            {
                if (Grid.GetColumn(item) == Grid.GetColumn((gr.Parent as Border)))
                {
                    foreach (dynamic val in (item.Child as Grid).Children)
                    {
                        if (Grid.GetColumn(val) == col && val is TextBox)
                            (val as TextBox).Background = Brushes.LightGray;
                    }
                }
            }

            try
            {
                (sender as TextBox).Background = Brushes.SkyBlue;
                CheckNum(Convert.ToString((sender as TextBox).Text), ((sender as TextBox).Parent as Grid), Grid.GetColumn((sender as TextBox)), Grid.GetRow((sender as TextBox)), Brushes.Red, Brushes.LightPink);
            }
            catch { }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SudokuGenerate();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            dpSudokuGame.Visibility = Visibility.Collapsed;
            grStart.Visibility = Visibility.Visible;
        }

        private void LevelItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (MenuItem item in Level.Items)
            {
                item.IsChecked = false;
            }

            (sender as MenuItem).IsChecked = true;
            game.GameLevel = (Level)Enum.Parse(typeof(Level), Convert.ToString((sender as MenuItem).Header));
            SudokuGenerate();
        }

        private void Sudoku_MouseDown(object sender, MouseButtonEventArgs e)
        {
            grStart.Visibility = Visibility.Collapsed;
            dpSudokuGame.Visibility = Visibility.Visible;
            SudokuGenerate();
        }
        private void Cross_MouseDown(object sender, MouseButtonEventArgs e)
        {
            grStart.Visibility = Visibility.Collapsed;
        }
    }
}