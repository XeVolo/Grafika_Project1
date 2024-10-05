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
        private Point _previousPoint;
        private Point _startPoint;
        private DrawingMode _currentDravingMode;
        private Line _currentLine;
        private Line _selectedLine;
        public MainWindow()
        {
            InitializeComponent();
            _currentDravingMode = DrawingMode.Cursor;

        }
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _startPoint = e.GetPosition(DrawCanvas);
                _isDrawing = true;
                _previousPoint = e.GetPosition(DrawCanvas); // Pobierz pozycję kursora

                if (_currentDravingMode.Equals(DrawingMode.Line))
                {
                    // Tworzymy nową linię i dodajemy ją do Canvas
                    _currentLine = new Line
                    {
                        Stroke = Brushes.Black,
                        StrokeThickness = 2,
                        X1 = _startPoint.X,
                        Y1 = _startPoint.Y,
                        X2 = _startPoint.X, // Tymczasowo ustawiamy te same współrzędne
                        Y2 = _startPoint.Y
                    };
                    _currentLine.MouseLeftButtonDown += ChangeProperties;
                    DrawCanvas.Children.Add(_currentLine);
                    X1CoordinateTextBox.Text = _currentLine.X1.ToString("F0");
                    Y1CoordinateTextBox.Text = _currentLine.Y1.ToString("F0");
                    LineThicknessTextBox.Text = _currentLine.StrokeThickness.ToString("F0");
                }
            }
        }
       
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            switch (_currentDravingMode)
            {
                case DrawingMode.Cursor:
                    {

                        break;
                    }
                case DrawingMode.Pencil:
                    {
                        if (_isDrawing)
                        {
                            Point currentPoint = e.GetPosition(DrawCanvas); // Pobierz aktualną pozycję kursora

                            // Rysowanie linii pomiędzy poprzednim a aktualnym punktem
                            Line line = new Line
                            {
                                Stroke = Brushes.Black, // Kolor linii
                                StrokeThickness = 2,    // Grubość linii
                                X1 = _previousPoint.X,  // Punkt początkowy
                                Y1 = _previousPoint.Y,
                                X2 = currentPoint.X,    // Punkt końcowy
                                Y2 = currentPoint.Y
                            };

                            DrawCanvas.Children.Add(line); // Dodanie linii do Canvas
                            _previousPoint = currentPoint; // Ustawienie nowego poprzedniego punktu
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
            _selectedLine = null;
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

        private void ChangeProperties(object sender, MouseButtonEventArgs e)
        {
            Line clickedLine = sender as Line;
            if (clickedLine != null)
            {               
                _selectedLine = clickedLine;               
                X1CoordinateTextBox.Text = clickedLine.X1.ToString("F0");
                Y1CoordinateTextBox.Text = clickedLine.Y1.ToString("F0");
                X2CoordinateTextBox.Text = clickedLine.X2.ToString("F0");
                Y2CoordinateTextBox.Text = clickedLine.Y2.ToString("F0");
                LineThicknessTextBox.Text = clickedLine.StrokeThickness.ToString("F0");
            }
        }
        private void SetProperties_Click(object sender, RoutedEventArgs e)
        {
            if (_currentLine == null && _selectedLine == null)
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
                    _currentLine.MouseLeftButtonDown += ChangeProperties;
                    DrawCanvas.Children.Add(_currentLine);
                }
            }
            else if (_selectedLine != null)
            {
                //DrawCanvas.Children.Remove(_selectedLine);

                if (double.TryParse(LineThicknessTextBox.Text, out double thickness))
                {
                    _selectedLine.StrokeThickness = thickness;
                }

                
                if (double.TryParse(X1CoordinateTextBox.Text, out double x1) &&
                    double.TryParse(Y1CoordinateTextBox.Text, out double y1) &&
                    double.TryParse(X2CoordinateTextBox.Text, out double x2) &&
                    double.TryParse(Y2CoordinateTextBox.Text, out double y2))
                {
                    _selectedLine.X1 = x1; 
                    _selectedLine.Y1 = y1; 
                    _selectedLine.X2 = x2; 
                    _selectedLine.Y2 = y2;
                }               
            }
        }

        private void Pencil_Selected(object sender, RoutedEventArgs e)
        {
            _currentDravingMode = DrawingMode.Pencil;
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