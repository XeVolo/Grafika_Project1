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
                    DrawCanvas.Children.Add(_currentLine);
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
            _isDrawing = false; // Zatrzymanie rysowania
            _currentLine = null;
        }

        private void Save_Button(object sender, RoutedEventArgs e)
        {
            
        }
        private void Load_Button(object sender, RoutedEventArgs e)
        {

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