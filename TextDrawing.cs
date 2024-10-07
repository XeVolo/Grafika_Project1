using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Project1
{
    internal class TextDrawing
    {
        private MainWindow _mainWindow;

        public TextDrawing(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }
        public void NewLine()
        {


            if (double.TryParse(_mainWindow.X1CoordinateTextBox.Text, out double x1) &&
                    double.TryParse(_mainWindow.Y1CoordinateTextBox.Text, out double y1) &&
                    double.TryParse(_mainWindow.X2CoordinateTextBox.Text, out double x2) &&
                    double.TryParse(_mainWindow.Y2CoordinateTextBox.Text, out double y2) &&
                    double.TryParse(_mainWindow.LineThicknessTextBox.Text, out double thickness))
            {
                _mainWindow._currentLine = new Line
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = thickness,
                    X1 = x1,
                    Y1 = y1,
                    X2 = x2,
                    Y2 = y2
                };
                _mainWindow._currentLine.MouseLeftButtonDown += _mainWindow.ChangeLineProperties;
                _mainWindow.DrawCanvas.Children.Add(_mainWindow._currentLine);
            }
        }
        public void EditLine()
        {

            if (double.TryParse(_mainWindow.X1CoordinateTextBox.Text, out double x1) &&
                double.TryParse(_mainWindow.Y1CoordinateTextBox.Text, out double y1) &&
                double.TryParse(_mainWindow.X2CoordinateTextBox.Text, out double x2) &&
                double.TryParse(_mainWindow.Y2CoordinateTextBox.Text, out double y2) &&
                double.TryParse(_mainWindow.LineThicknessTextBox.Text, out double thickness))
            {
                _mainWindow._currentLine.X1 = x1;
                _mainWindow._currentLine.Y1 = y1;
                _mainWindow._currentLine.X2 = x2;
                _mainWindow._currentLine.Y2 = y2;
                _mainWindow._currentLine.StrokeThickness = thickness;
            }
        }
        public void NewRectangle()
        {
            if (double.TryParse(_mainWindow.X1CoordinateTextBox.Text, out double x1) &&
                    double.TryParse(_mainWindow.Y1CoordinateTextBox.Text, out double y1) &&
                    double.TryParse(_mainWindow.X2CoordinateTextBox.Text, out double x2) &&
                    double.TryParse(_mainWindow.Y2CoordinateTextBox.Text, out double y2) &&
                    double.TryParse(_mainWindow.LineThicknessTextBox.Text, out double thickness))
            {
                _mainWindow._currentRectangle = new Rectangle
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = thickness,
                    Fill = Brushes.Transparent
                };

                Canvas.SetLeft(_mainWindow._currentRectangle, x1);
                Canvas.SetTop(_mainWindow._currentRectangle, y1);

                double width = x2 - x1;
                double height = y2 - y1;

                _mainWindow._currentRectangle.Width = Math.Abs(width);
                _mainWindow._currentRectangle.Height = Math.Abs(height);

                if (width < 0)
                    Canvas.SetLeft(_mainWindow._currentRectangle, x2);
                if (height < 0)
                    Canvas.SetTop(_mainWindow._currentRectangle, y2);

                _mainWindow._currentRectangle.MouseLeftButtonDown += _mainWindow.ChangeRectangleProperties;
                _mainWindow.DrawCanvas.Children.Add(_mainWindow._currentRectangle);

            }
        }
        public void EditRectangle()
        {
            if (double.TryParse(_mainWindow.X1CoordinateTextBox.Text, out double x1) &&
                    double.TryParse(_mainWindow.Y1CoordinateTextBox.Text, out double y1) &&
                    double.TryParse(_mainWindow.X2CoordinateTextBox.Text, out double x2) &&
                    double.TryParse(_mainWindow.Y2CoordinateTextBox.Text, out double y2) &&
                    double.TryParse(_mainWindow.LineThicknessTextBox.Text, out double thickness))
            {
                _mainWindow._currentRectangle.StrokeThickness = thickness;
                Canvas.SetLeft(_mainWindow._currentRectangle, x1);
                Canvas.SetTop(_mainWindow._currentRectangle, y1);

                double width = x2 - x1;
                double height = y2 - y1;

                _mainWindow._currentRectangle.Width = Math.Abs(width);
                _mainWindow._currentRectangle.Height = Math.Abs(height);

                if (width < 0)
                    Canvas.SetLeft(_mainWindow._currentRectangle, x2);
                if (height < 0)
                    Canvas.SetTop(_mainWindow._currentRectangle, y2);
            }
        }
        public void NewCircle()
        {
            if (double.TryParse(_mainWindow.X1CircleCoordinateTextBox.Text, out double x1) &&
                    double.TryParse(_mainWindow.Y1CircleCoordinateTextBox.Text, out double y1) &&
                    double.TryParse(_mainWindow.RadiusTextBox.Text, out double radius) &&
                    double.TryParse(_mainWindow.CircleLineThicknessTextBox.Text, out double thickness))
            {
                _mainWindow._currentCircle = new Ellipse
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = thickness,
                    Fill = Brushes.Transparent
                };

                _mainWindow._currentCircle.Width = radius * 2;
                _mainWindow._currentCircle.Height = radius * 2;


                Canvas.SetLeft(_mainWindow._currentCircle, x1 - radius);
                Canvas.SetTop(_mainWindow._currentCircle, y1 - radius);

                _mainWindow._currentCircle.MouseLeftButtonDown += _mainWindow.ChangeCircleProperties;
                _mainWindow.DrawCanvas.Children.Add(_mainWindow._currentCircle);

            }

        }
        public void EditCircle()
        {
            if (double.TryParse(_mainWindow.X1CircleCoordinateTextBox.Text, out double x1) &&
                    double.TryParse(_mainWindow.Y1CircleCoordinateTextBox.Text, out double y1) &&
                    double.TryParse(_mainWindow.RadiusTextBox.Text, out double radius) &&
                    double.TryParse(_mainWindow.CircleLineThicknessTextBox.Text, out double thickness))
            {
                _mainWindow._currentCircle.StrokeThickness = thickness;

                _mainWindow._currentCircle.Width = radius * 2;
                _mainWindow._currentCircle.Height = radius * 2;
                Canvas.SetLeft(_mainWindow._currentCircle, x1 - radius);
                Canvas.SetTop(_mainWindow._currentCircle, y1 - radius);
            }
        }
    }
}
