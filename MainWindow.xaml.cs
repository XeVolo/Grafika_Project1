using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isDrawing;
        private Point _startPoint;
        private DrawingMode _currentDravingMode;
        private Line _currentLine;
        private Rectangle _currentRectangle;
        private Point _offset;
        public MainWindow()
        {
            _currentDravingMode = DrawingMode.Cursor;
            InitializeComponent();
        }
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _startPoint = e.GetPosition(DrawCanvas);
                _isDrawing = true;

                switch (_currentDravingMode)
                {
                    case DrawingMode.Line:
                        {
                            _currentLine = new Line
                            {
                                Stroke = Brushes.Black,
                                StrokeThickness = 2,
                                X1 = _startPoint.X,
                                Y1 = _startPoint.Y,
                                X2 = _startPoint.X,
                                Y2 = _startPoint.Y
                            };
                            _currentLine.MouseLeftButtonDown += ChangeLineProperties;
                            DrawCanvas.Children.Add(_currentLine);
                            X1CoordinateTextBox.Text = _currentLine.X1.ToString("F0");
                            Y1CoordinateTextBox.Text = _currentLine.Y1.ToString("F0");
                            LineThicknessTextBox.Text = _currentLine.StrokeThickness.ToString("F0");
                            break;
                        }
                    case DrawingMode.Rectangle:
                        {
                            _currentRectangle = new Rectangle
                            {
                                Stroke = Brushes.Black,
                                StrokeThickness = 2,
                                Fill = Brushes.Transparent
                            };

                            Canvas.SetLeft(_currentRectangle, _startPoint.X);
                            Canvas.SetTop(_currentRectangle, _startPoint.Y);

                            DrawCanvas.Children.Add(_currentRectangle);

                            X1CoordinateTextBox.Text = _startPoint.X.ToString("F0");
                            Y1CoordinateTextBox.Text = _startPoint.Y.ToString("F0");
                            LineThicknessTextBox.Text = _currentRectangle.StrokeThickness.ToString("F0");
                            break;
                        }
                    case DrawingMode.Circle:
                        {

                            break;
                        }
                }
            }
        }
       
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            switch (_currentDravingMode)
            {
                case DrawingMode.Cursor:
                    {
                        if (_isDrawing && _currentLine != null)
                        {
                            Point currentPosition = e.GetPosition(DrawCanvas);
                            _currentLine.X1 += currentPosition.X - _offset.X;
                            _currentLine.Y1 += currentPosition.Y - _offset.Y;
                            _currentLine.X2 += currentPosition.X - _offset.X;
                            _currentLine.Y2 += currentPosition.Y - _offset.Y;

                            // Aktualizacja offsetu
                            _offset = currentPosition;
                            X1CoordinateTextBox.Text = _currentLine.X1.ToString("F0");
                            Y1CoordinateTextBox.Text = _currentLine.Y1.ToString("F0");
                            X2CoordinateTextBox.Text = _currentLine.X2.ToString("F0");
                            Y2CoordinateTextBox.Text = _currentLine.Y2.ToString("F0");
                        }
                        break;
                    }               
                case DrawingMode.Line:
                    {
                        if (_isDrawing  && _currentLine != null)
                        {
                            // Pobieramy aktualną pozycję kursora
                            Point currentPoint = e.GetPosition(DrawCanvas);

                            // Aktualizujemy współrzędne końcowego punktu linii
                            _currentLine.X2 = currentPoint.X;
                            _currentLine.Y2 = currentPoint.Y;
                            X2CoordinateTextBox.Text = _currentLine.X2.ToString("F0");
                            Y2CoordinateTextBox.Text = _currentLine.Y2.ToString("F0");
                        }
                        break;
                    }
                case DrawingMode.Rectangle:
                    {
                        if (_isDrawing && _currentRectangle != null)
                        {
                            // Obliczanie aktualnego rozmiaru prostokąta
                            Point currentPoint = e.GetPosition(DrawCanvas);
                            double width = currentPoint.X - _startPoint.X;
                            double height = currentPoint.Y - _startPoint.Y;

                            // Ustawiamy szerokość i wysokość prostokąta
                            _currentRectangle.Width = Math.Abs(width);
                            _currentRectangle.Height = Math.Abs(height);

                            // Przemieszczamy prostokąt, jeśli rysujemy od prawego do lewego
                            if (width < 0)
                                Canvas.SetLeft(_currentRectangle, currentPoint.X);
                            if (height < 0)
                                Canvas.SetTop(_currentRectangle, currentPoint.Y);

                            X2CoordinateTextBox.Text = currentPoint.X.ToString("F0");
                            Y2CoordinateTextBox.Text = currentPoint.Y.ToString("F0");
                        }
                        break;
                    }
                case DrawingMode.Circle:
                    {

                        break;
                    }
            }            
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = false; 
            if (_currentLine != null)
            {
                X2CoordinateTextBox.Text = _currentLine.X2.ToString("F0");
                Y2CoordinateTextBox.Text = _currentLine.Y2.ToString("F0");
            }
            
            
        }
        private void Reset_Button(object sender, RoutedEventArgs e)
        {
            _currentLine = null;
            _currentRectangle = null;
            X1CoordinateTextBox.Text = "";
            Y1CoordinateTextBox.Text = "";
            X2CoordinateTextBox.Text = "";
            Y2CoordinateTextBox.Text = "";
            LineThicknessTextBox.Text = "";
        }
        private void Save_Button(object sender, RoutedEventArgs e)
        {
            
        }
        private void Load_Button(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeLineProperties(object sender, MouseButtonEventArgs e)
        {
            
            if (_currentDravingMode.Equals(DrawingMode.Cursor))
            {
                _currentRectangle = null;
                _offset = e.GetPosition(DrawCanvas);
                Line clickedLine = sender as Line;
                if (clickedLine != null)
                {

                    _currentLine = clickedLine;
                    X1CoordinateTextBox.Text = clickedLine.X1.ToString("F0");
                    Y1CoordinateTextBox.Text = clickedLine.Y1.ToString("F0");
                    X2CoordinateTextBox.Text = clickedLine.X2.ToString("F0");
                    Y2CoordinateTextBox.Text = clickedLine.Y2.ToString("F0");
                    LineThicknessTextBox.Text = clickedLine.StrokeThickness.ToString("F0");
                }
            }
        }
        private void SetProperties_Click(object sender, RoutedEventArgs e)
        {
            switch (_currentDravingMode)
            {
                case DrawingMode.Cursor:
                    {
                        if (_currentLine != null)
                        {
                            EditLine();
                        }
                        else if (_currentRectangle != null)
                        {

                        }
                        
                        break;
                    }
                case DrawingMode.Line:
                    {
                        if (_currentLine == null)
                        {
                            NewLine();
                        }
                        else if (_currentLine != null)
                        {
                            EditLine();
                        }
                        break;
                    }
                case DrawingMode.Rectangle:
                    {
                        if(_currentRectangle == null)
                        {
                            NewRectangle();
                        }
                        break;
                    }
                case DrawingMode.Circle:
                    {

                        break;
                    }
            }
        }

        private void NewLine()
        {
            if (double.TryParse(X1CoordinateTextBox.Text, out double x1) &&
                    double.TryParse(Y1CoordinateTextBox.Text, out double y1) &&
                    double.TryParse(X2CoordinateTextBox.Text, out double x2) &&
                    double.TryParse(Y2CoordinateTextBox.Text, out double y2) &&
                    double.TryParse(LineThicknessTextBox.Text, out double thickness))
            {
                _currentLine = new Line
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = thickness,
                    X1 = x1,
                    Y1 = y1,
                    X2 = x2,
                    Y2 = y2
                };
                _currentLine.MouseLeftButtonDown += ChangeLineProperties;
                DrawCanvas.Children.Add(_currentLine);
            }
        }
        private void EditLine()
        {
            
            if (double.TryParse(X1CoordinateTextBox.Text, out double x1) &&
                double.TryParse(Y1CoordinateTextBox.Text, out double y1) &&
                double.TryParse(X2CoordinateTextBox.Text, out double x2) &&
                double.TryParse(Y2CoordinateTextBox.Text, out double y2) &&
                double.TryParse(LineThicknessTextBox.Text, out double thickness))
            {
                _currentLine.X1 = x1;
                _currentLine.Y1 = y1;
                _currentLine.X2 = x2;
                _currentLine.Y2 = y2;
                _currentLine.StrokeThickness = thickness;
            }
        }
        private void NewRectangle()
        {
            if (double.TryParse(X1CoordinateTextBox.Text, out double x1) &&
                    double.TryParse(Y1CoordinateTextBox.Text, out double y1) &&
                    double.TryParse(X2CoordinateTextBox.Text, out double x2) &&
                    double.TryParse(Y2CoordinateTextBox.Text, out double y2) &&
                    double.TryParse(LineThicknessTextBox.Text, out double thickness))
            {
                _currentRectangle = new Rectangle
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = thickness,
                    Fill = Brushes.Transparent
                };

                Canvas.SetLeft(_currentRectangle, x1);
                Canvas.SetTop(_currentRectangle, y1);


                double width = x2 - x1;
                double height = y2 - y1;
                
                _currentRectangle.Width = Math.Abs(width);
                _currentRectangle.Height = Math.Abs(height);
                
                if (width < 0)
                    Canvas.SetLeft(_currentRectangle, x2);
                if (height < 0)
                    Canvas.SetTop(_currentRectangle, y2);

                DrawCanvas.Children.Add(_currentRectangle);
                
            }
        }
        private void EditRectangle()
        {
            if (double.TryParse(X1CoordinateTextBox.Text, out double x1) &&
                    double.TryParse(Y1CoordinateTextBox.Text, out double y1) &&
                    double.TryParse(X2CoordinateTextBox.Text, out double x2) &&
                    double.TryParse(Y2CoordinateTextBox.Text, out double y2) &&
                    double.TryParse(LineThicknessTextBox.Text, out double thickness))
            {
                _currentLine = new Line
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = thickness,
                    X1 = x1,
                    Y1 = y1,
                    X2 = x2,
                    Y2 = y2
                };
                _currentLine.MouseLeftButtonDown += ChangeLineProperties;
                DrawCanvas.Children.Add(_currentLine);
            }
        }

        private void Cursor_Selected(object sender, RoutedEventArgs e)
        {
            _currentDravingMode = DrawingMode.Cursor;
        }
        private void Line_Selected(object sender, RoutedEventArgs e)
        {
            _currentDravingMode = DrawingMode.Line;
        }
        private void Rectangle_Selected(object sender, RoutedEventArgs e)
        {
            _currentDravingMode = DrawingMode.Rectangle;
        }
        private void Circle_Selected(object sender, RoutedEventArgs e)
        {
            _currentDravingMode = DrawingMode.Circle;
        }
               
    }
}