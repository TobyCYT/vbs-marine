using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VIREO_KIS.Properties;

namespace VIREO_KIS
{
    /// <summary>
    /// Interaction logic for ColorSearcher.xaml
    /// </summary>
    public partial class ColorSearcher : UserControl
    {
        AllSearchPanel containerPanel;

        bool isMin = false;

        double searchArea = 6;
        double pickArea = 2;

        // Color panel
        int numColorRow = 3;
        int numColorCol = 9;

        int lastQueryCol = -1;
        int lastQueryRow = -1;

        int lastColorCol = 0;
        int lastColorRow = 0;

        // Canvas
        static int NUM_ROW = Settings.Default.COLOR_GRID_NUM_ROW;
        static int NUM_COL = Settings.Default.COLOR_GRID_NUM_COL;

        Color[,] DrawingBoard = new Color[NUM_ROW, NUM_COL];
        Color[,] ColorBoard = new Color[3, 9];
        Color defaultColor = Colors.DarkTurquoise;

        byte[,] transparency = new byte[NUM_ROW, NUM_COL];

        int[,] iBoard = new int[NUM_ROW, NUM_COL];

        bool[,] cellSelected = new bool[NUM_ROW, NUM_COL];
        bool[,] colorSelected = new bool[3, 9];

        ColorSearchResult result;

        public bool HasResult()
        {
            return result.HasResult();
        }

        public void Clear()
        {
            for (int i = 0; i < NUM_ROW; i++)
            {
                for (int j = 0; j < NUM_COL; j++)
                {
                    DrawingBoard[i, j] = defaultColor;
                    cellSelected[i, j] = false;
                    transparency[i, j] = 255;
                }
            }
            this.InvalidateVisual();
            result.Clear();
        }

        public void UpdateParent(AllSearchPanel _mainPanel)
        {
            containerPanel = _mainPanel;
        }
        public ColorSearcher()
        {
            InitializeComponent();
            for (int i = 0; i < NUM_ROW; i++)
            {
                for (int j = 0; j < NUM_COL; j++)
                {
                    DrawingBoard[i, j] = defaultColor;
                    cellSelected[i, j] = false;
                    transparency[i, j] = 255;
                }
            }
            for (int i = 0; i < numColorRow; i++)
            {
                for (int j = 0; j < numColorCol; j++)
                {
                    int pos = numColorCol * i + j;
                    int r = pos / 9;
                    int g = (pos % 9) / 3;
                    int b = pos % 3;
                    byte red = 0;
                    if (r > 0)
                    {
                        red = Convert.ToByte(r * 128 - 1);
                    }
                    byte green = 0;
                    if (g > 0)
                    {
                        green = Convert.ToByte(g * 128 - 1);
                    }
                    byte blue = 0;
                    if (b > 0)
                    {
                        blue = Convert.ToByte(b * 128 - 1);
                    }
                    ColorBoard[i, j] = Color.FromRgb(red, green, blue);
                }
            }
            colorSelected[0, 0] = true;
            result = new ColorSearchResult();
        }

        public void UpdateRecommendation(double[,] _toUpdate)
        {
            double min = Double.MaxValue;
            double max = Double.MinValue;
            for (int i = 0; i < NUM_ROW; i++)
            {
                for (int j = 0; j < NUM_COL; j++)
                {
                    if ((_toUpdate[i, j] < min) && (_toUpdate[i, j] != -1))
                    {
                        min = _toUpdate[i, j];
                    }
                    if (_toUpdate[i, j] > max)
                    {
                        max = _toUpdate[i, j];
                    }
                }
            }
            for (int i = 0; i < NUM_ROW; i++)
            {
                for (int j = 0; j < NUM_COL; j++)
                {
                    if (_toUpdate[i, j] == -1)
                    {
                        transparency[i, j] = 255;
                    }
                    else
                    {
                        transparency[i, j] = Convert.ToByte(63 + ((_toUpdate[i, j] - min) / (max - min)) * 192);
                    }
                }
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            // Draw query area
            double cellWidth = this.ActualWidth / NUM_COL;
            double cellHeight = this.ActualHeight * (searchArea / (searchArea + pickArea)) / NUM_ROW;

            for (int i = 0; i < NUM_ROW; i++)
            {
                for (int j = 0; j < NUM_COL; j++)
                {
                    Rect bounds = new Rect(j * cellWidth, i * cellHeight, cellWidth - 1, cellHeight - 1);
                    if (cellSelected[i, j])
                    {
                        drawingContext.DrawRectangle(new SolidColorBrush(DrawingBoard[i, j]), new Pen(Brushes.Black, 2), bounds);
                    }
                    else
                    {
                        Color temp = DrawingBoard[i, j];
                        temp.A = transparency[i, j];
                        drawingContext.DrawRectangle(new SolidColorBrush(temp), null, bounds);
                    }
                }
            }

            // Draw color picker area
            cellWidth = this.ActualWidth / numColorCol;
            cellHeight = this.ActualHeight * (pickArea / (searchArea + pickArea)) / numColorRow;

            for (int i = 0; i < numColorRow; i++)
            {
                for (int j = 0; j < numColorCol; j++)
                {
                    Rect bounds = new Rect(j * cellWidth, this.ActualHeight * (searchArea / (searchArea + pickArea)) + i * cellHeight, cellWidth - 1, cellHeight - 1);
                    if (!colorSelected[i, j])
                    {
                        drawingContext.DrawRectangle(new SolidColorBrush(ColorBoard[i, j]), null, bounds);
                    }
                    else
                    {
                        drawingContext.DrawRectangle(new SolidColorBrush(ColorBoard[i, j]), new Pen(Brushes.Green, 2), bounds);
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Click on query area
                if (e.GetPosition(this).Y < this.ActualHeight * (searchArea / (searchArea + pickArea)))
                {
                    double cellWidth = this.ActualWidth / NUM_COL;
                    double cellHeight = this.ActualHeight * (searchArea / (searchArea + pickArea)) / NUM_ROW;
                    lastQueryCol = Convert.ToInt16(e.GetPosition(this).X) / Convert.ToInt16(cellWidth);
                    lastQueryRow = Convert.ToInt16(e.GetPosition(this).Y) / Convert.ToInt16(cellHeight);

                    // Click on new cell 
                    if (DrawingBoard[lastQueryRow, lastQueryCol] == defaultColor)
                    {
                        DrawingBoard[lastQueryRow, lastQueryCol] = ColorBoard[lastColorRow, lastColorCol];
                        cellSelected[lastQueryRow, lastQueryCol] = true;

                        ColorCellQuery qr;
                        qr.Pos = Convert.ToByte(lastQueryRow * NUM_COL + lastQueryCol);
                        qr.R = DrawingBoard[lastQueryRow, lastQueryCol].R;
                        qr.G = DrawingBoard[lastQueryRow, lastQueryCol].G;
                        qr.B = DrawingBoard[lastQueryRow, lastQueryCol].B;
                        result.AddQuery(qr);

                        string logging = "add," + lastQueryRow.ToString() + "-" + lastQueryCol.ToString() + "," + ColorBoard[lastColorRow, lastColorCol].ToString();
                        containerPanel.UpdateResult(result.Get(isMin), RESULT_UPDATE.FROM_COLOR, result.HasResult(), logging);   
                                             
                    }
                    // Override
                    else
                    {
                        DrawingBoard[lastQueryRow, lastQueryCol] = ColorBoard[lastColorRow, lastColorCol];

                        ColorCellQuery qr;
                        qr.Pos = Convert.ToByte(lastQueryRow * NUM_COL + lastQueryCol);
                        qr.R = DrawingBoard[lastQueryRow, lastQueryCol].R;
                        qr.G = DrawingBoard[lastQueryRow, lastQueryCol].G;
                        qr.B = DrawingBoard[lastQueryRow, lastQueryCol].B;
                        result.RemoveQuery(qr);

                        qr.R = DrawingBoard[lastQueryRow, lastQueryCol].R;
                        qr.G = DrawingBoard[lastQueryRow, lastQueryCol].G;
                        qr.B = DrawingBoard[lastQueryRow, lastQueryCol].B;
                        result.AddQuery(qr);

                        string logging = "update," + lastQueryRow.ToString() + "-" + lastQueryCol.ToString() + "," + ColorBoard[lastColorRow, lastColorCol].ToString();
                        containerPanel.UpdateResult(result.Get(isMin), RESULT_UPDATE.FROM_COLOR, result.HasResult(), logging);
                    }
                }
                // Click on color picker area
                else
                {
                    double cellWidth = this.ActualWidth / numColorCol;
                    double cellHeight = this.ActualHeight * (pickArea / (searchArea + pickArea)) / numColorRow;
                    int cellCol = Convert.ToInt16(e.GetPosition(this).X) / Convert.ToInt16(cellWidth);
                    int cellRow = Convert.ToInt16(e.GetPosition(this).Y - this.ActualHeight * (searchArea / (searchArea + pickArea))) / Convert.ToInt16(cellHeight);

                    for (int i = 0; i < numColorRow; i++)
                    {
                        for (int j = 0; j < numColorCol; j++)
                        {
                            colorSelected[i, j] = false;
                        }
                    }
                    colorSelected[cellRow, cellCol] = true;
                    lastColorCol = cellCol;
                    lastColorRow = cellRow;
                }
                this.InvalidateVisual();
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                // Click on query area
                if (e.GetPosition(this).Y < this.ActualHeight * (searchArea / (searchArea + pickArea)))
                {
                    double cellWidth = this.ActualWidth / NUM_COL;
                    double cellHeight = this.ActualHeight * (searchArea / (searchArea + pickArea)) / NUM_ROW;
                    int cellCol = Convert.ToInt16(e.GetPosition(this).X) / Convert.ToInt16(cellWidth);
                    int cellRow = Convert.ToInt16(e.GetPosition(this).Y) / Convert.ToInt16(cellHeight);

                    if (cellSelected[cellRow, cellCol])
                    {
                        ColorCellQuery qr;
                        qr.Pos = Convert.ToByte(cellRow * NUM_COL + cellCol);
                        qr.R = DrawingBoard[cellRow, cellCol].R;
                        qr.G = DrawingBoard[cellRow, cellCol].G;
                        qr.B = DrawingBoard[cellRow, cellCol].B;

                        result.RemoveQuery(qr);

                        string logging = "remove," + cellRow.ToString() + "-" + cellCol.ToString() + "," + ColorBoard[lastColorRow, lastColorCol].ToString();
                        containerPanel.UpdateResult(result.Get(isMin), RESULT_UPDATE.FROM_COLOR, result.HasResult(), logging);                        

                        DrawingBoard[cellRow, cellCol] = defaultColor;
                        cellSelected[cellRow, cellCol] = false;
                    }
                }
                this.InvalidateVisual();
            }
        }
    }
}
