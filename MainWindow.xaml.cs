using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Project1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isDrawing;
        private bool _isEditing;
        private Point _startPoint;
        private DrawingMode _currentDrawingMode;
        public Line? _currentLine;
        public Rectangle? _currentRectangle;
        public Ellipse? _currentCircle;
        private Point _offset;
        private TextDrawing _textdrawing;
        public MainWindow()
        {
            _currentDrawingMode = DrawingMode.Cursor;
            InitializeComponent();
            _textdrawing = new TextDrawing(this);
        }
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _startPoint = e.GetPosition(DrawCanvas);
                _isDrawing = true;

                switch (_currentDrawingMode)
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

                            _currentRectangle.MouseLeftButtonDown += ChangeRectangleProperties;
                            DrawCanvas.Children.Add(_currentRectangle);

                            X1CoordinateTextBox.Text = _startPoint.X.ToString("F0");
                            Y1CoordinateTextBox.Text = _startPoint.Y.ToString("F0");
                            LineThicknessTextBox.Text = _currentRectangle.StrokeThickness.ToString("F0");
                            break;
                        }
                    case DrawingMode.Circle:
                        {
                            _currentCircle = new Ellipse
                            {
                                Stroke = Brushes.Black,
                                StrokeThickness = 2,
                                Fill = Brushes.Transparent
                            };

                            
                            Canvas.SetLeft(_currentCircle, _startPoint.X);
                            Canvas.SetTop(_currentCircle, _startPoint.Y);
                            _currentCircle.MouseLeftButtonDown += ChangeCircleProperties;
                            DrawCanvas.Children.Add(_currentCircle);

                            X1CircleCoordinateTextBox.Text = _startPoint.X.ToString("F0");
                            Y1CircleCoordinateTextBox.Text = _startPoint.Y.ToString("F0");
                            CircleLineThicknessTextBox.Text = _currentCircle.StrokeThickness.ToString("F0");
                            break;
                        }
                }
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                _isEditing = true;
            }
        }
       
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            switch (_currentDrawingMode)
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
                             
                            
                            _offset = currentPosition;

                            X1CoordinateTextBox.Text = _currentLine.X1.ToString("F0");
                            Y1CoordinateTextBox.Text = _currentLine.Y1.ToString("F0");
                            X2CoordinateTextBox.Text = _currentLine.X2.ToString("F0");
                            Y2CoordinateTextBox.Text = _currentLine.Y2.ToString("F0");
                        } else if (_isDrawing && _currentRectangle != null)
                        {
                            Point currentPosition = e.GetPosition(DrawCanvas);
                            double offsetX = currentPosition.X - _offset.X;
                            double offsetY = currentPosition.Y - _offset.Y;

                            Canvas.SetLeft(_currentRectangle, Canvas.GetLeft(_currentRectangle) + offsetX);
                            Canvas.SetTop(_currentRectangle, Canvas.GetTop(_currentRectangle) + offsetY);

                            _offset = currentPosition;

                            double rectLeft = Canvas.GetLeft(_currentRectangle);
                            double rectTop = Canvas.GetTop(_currentRectangle);
                            double rectRight = rectLeft + _currentRectangle.Width;
                            double rectBottom = rectTop + _currentRectangle.Height;

                            X1CoordinateTextBox.Text = rectLeft.ToString("F0");
                            Y1CoordinateTextBox.Text = rectTop.ToString("F0");
                            X2CoordinateTextBox.Text = rectRight.ToString("F0");
                            Y2CoordinateTextBox.Text = rectBottom.ToString("F0");
                            LineThicknessTextBox.Text = _currentRectangle.StrokeThickness.ToString("F0");
                        } else if (_isDrawing && _currentCircle != null)
                        {
                            Point currentPosition = e.GetPosition(DrawCanvas);
                            double offsetX = currentPosition.X - _offset.X;
                            double offsetY = currentPosition.Y - _offset.Y;

                            Canvas.SetLeft(_currentCircle, Canvas.GetLeft(_currentCircle) + offsetX);
                            Canvas.SetTop(_currentCircle, Canvas.GetTop(_currentCircle) + offsetY);

                            _offset = currentPosition;
                            
                            double radius = _currentCircle.Width / 2;
                            double centerX = Canvas.GetLeft(_currentCircle) + radius;
                            double centerY = Canvas.GetTop(_currentCircle) + radius;

                            X1CircleCoordinateTextBox.Text = centerX.ToString("F0");
                            Y1CircleCoordinateTextBox.Text = centerY.ToString("F0");
                            RadiusTextBox.Text = radius.ToString("F0");
                            CircleLineThicknessTextBox.Text = _currentCircle.StrokeThickness.ToString("F0");
                        }
                        if (_isEditing && _currentLine != null)
                        {

                            Point currentPoint = e.GetPosition(DrawCanvas);


                            _currentLine.X2 = currentPoint.X;
                            _currentLine.Y2 = currentPoint.Y;
                            X2CoordinateTextBox.Text = _currentLine.X2.ToString("F0");
                            Y2CoordinateTextBox.Text = _currentLine.Y2.ToString("F0");
                        }
                        else if (_isEditing && _currentRectangle != null)
                        {

                            Point currentPoint = e.GetPosition(DrawCanvas);
                            double width = currentPoint.X - _startPoint.X;
                            double height = currentPoint.Y - _startPoint.Y;


                            _currentRectangle.Width = Math.Abs(width);
                            _currentRectangle.Height = Math.Abs(height);


                            if (width < 0)
                                Canvas.SetLeft(_currentRectangle, currentPoint.X);
                            if (height < 0)
                                Canvas.SetTop(_currentRectangle, currentPoint.Y);

                            X2CoordinateTextBox.Text = currentPoint.X.ToString("F0");
                            Y2CoordinateTextBox.Text = currentPoint.Y.ToString("F0");
                        }
                        else if (_isEditing && _currentCircle != null)
                        {
                            Point currentPoint = e.GetPosition(DrawCanvas);
                            double radius = Math.Sqrt(Math.Pow(currentPoint.X - _startPoint.X, 2) + Math.Pow(currentPoint.Y - _startPoint.Y, 2));

                            _currentCircle.Width = radius * 2;
                            _currentCircle.Height = radius * 2;


                            Canvas.SetLeft(_currentCircle, _startPoint.X - radius);
                            Canvas.SetTop(_currentCircle, _startPoint.Y - radius);



                            RadiusTextBox.Text = radius.ToString("F0");
                        }
                        break;
                    }               
                case DrawingMode.Line:
                    {
                        if (_isDrawing  && _currentLine != null)
                        {

                            Point currentPoint = e.GetPosition(DrawCanvas);


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

                            Point currentPoint = e.GetPosition(DrawCanvas);
                            double width = currentPoint.X - _startPoint.X;
                            double height = currentPoint.Y - _startPoint.Y;


                            _currentRectangle.Width = Math.Abs(width);
                            _currentRectangle.Height = Math.Abs(height);


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
                        if (_isDrawing && _currentCircle != null)
                        {
                            Point currentPoint = e.GetPosition(DrawCanvas);
                            double radius = Math.Sqrt(Math.Pow(currentPoint.X - _startPoint.X, 2) + Math.Pow(currentPoint.Y - _startPoint.Y, 2));

                            _currentCircle.Width = radius * 2;
                            _currentCircle.Height = radius * 2;

                            
                                Canvas.SetLeft(_currentCircle, _startPoint.X - radius);                            
                                Canvas.SetTop(_currentCircle, _startPoint.Y - radius);


                            
                            RadiusTextBox.Text = radius.ToString("F0");
                            
                        }
                        break;
                    }
            }            
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = false;
            _isEditing = false;                 
        }
        private void Reset_Button(object sender, RoutedEventArgs e)
        {
            Reset();
        }
        private void Reset()
        {
            _currentLine = null;
            _currentRectangle = null;
            _currentCircle = null;
            X1CoordinateTextBox.Text = "";
            Y1CoordinateTextBox.Text = "";
            X2CoordinateTextBox.Text = "";
            Y2CoordinateTextBox.Text = "";
            LineThicknessTextBox.Text = "";
            X1CircleCoordinateTextBox.Text = "";
            Y1CircleCoordinateTextBox.Text = "";
            RadiusTextBox.Text = "";
            CircleLineThicknessTextBox.Text = "";
        }
        private void Save_Button(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "XAML Files (*.xaml)|*.xaml",
                Title = "Zapisz Canvas do pliku"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                {

                    string xaml = XamlWriter.Save(DrawCanvas);
                    sw.Write(xaml);
                }
            }
        }
        private void Load_Button(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "XAML Files (*.xaml)|*.xaml",
                Title = "Wczytaj Canvas z pliku"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                {
                    string xaml = sr.ReadToEnd();

                    Canvas loadedCanvas = (Canvas)XamlReader.Parse(xaml);


                    DrawCanvas.Children.Clear();
                    foreach (UIElement element in loadedCanvas.Children)
                    {
                        if (element is Line line)
                        {
                            Line newLine = new Line
                            {
                                Stroke = Brushes.Black,
                                StrokeThickness = line.StrokeThickness,
                                X1 = line.X1,
                                Y1 = line.Y1,
                                X2 = line.X2,
                                Y2 = line.Y2
                            };
                            newLine.MouseLeftButtonDown += ChangeLineProperties;
                            DrawCanvas.Children.Add(newLine);
                        }
                        else if (element is Rectangle rectangle)
                        {
                            Rectangle newRectangle = new Rectangle
                            {
                                Stroke = Brushes.Black,
                                StrokeThickness = rectangle.StrokeThickness,
                                Fill = Brushes.Transparent
                            };

                            Canvas.SetLeft(newRectangle, Canvas.GetLeft(rectangle));
                            Canvas.SetTop(newRectangle, Canvas.GetTop(rectangle));

                            newRectangle.Width = rectangle.Width;
                            newRectangle.Height = rectangle.Height;

                            newRectangle.MouseLeftButtonDown += ChangeRectangleProperties;
                            DrawCanvas.Children.Add(newRectangle);
                        }
                        else if (element is Ellipse circle)
                        {
                            Ellipse newCircle = new Ellipse
                            {
                                Stroke = Brushes.Black,
                                StrokeThickness = circle.StrokeThickness,
                                Fill = Brushes.Transparent
                            };

                            newCircle.Width = circle.Width;
                            newCircle.Height = circle.Height;


                            Canvas.SetLeft(newCircle, Canvas.GetLeft(circle));
                            Canvas.SetTop(newCircle, Canvas.GetTop(circle));

                            newCircle.MouseLeftButtonDown += ChangeCircleProperties;
                            DrawCanvas.Children.Add(newCircle);
                        }
                    }
                }
            }
        }

        public void ChangeLineProperties(object sender, MouseButtonEventArgs e)
        {
            
            if (_currentDrawingMode.Equals(DrawingMode.Cursor))
            {
                BasicStackPanel.Visibility = Visibility.Visible;
                CircleStackPanel.Visibility = Visibility.Collapsed;
                _currentRectangle = null;
                _currentCircle = null;
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
        public void ChangeRectangleProperties(object sender, MouseButtonEventArgs e)
        {

            if (_currentDrawingMode.Equals(DrawingMode.Cursor))
            {
                BasicStackPanel.Visibility = Visibility.Visible;
                CircleStackPanel.Visibility = Visibility.Collapsed;
                _currentLine = null;
                _currentCircle = null;
                _offset = e.GetPosition(DrawCanvas);
                Rectangle clickedRectangle = sender as Rectangle;
                if (clickedRectangle != null)
                {

                    _currentRectangle = clickedRectangle;

                    double rectLeft = Canvas.GetLeft(_currentRectangle);
                    double rectTop = Canvas.GetTop(_currentRectangle);
                    double rectRight = rectLeft + _currentRectangle.Width;
                    double rectBottom = rectTop + _currentRectangle.Height;

                       X1CoordinateTextBox.Text = rectLeft.ToString("F0");
                       Y1CoordinateTextBox.Text = rectTop.ToString("F0");
                       X2CoordinateTextBox.Text = rectRight.ToString("F0");
                       Y2CoordinateTextBox.Text = rectBottom.ToString("F0");
                       LineThicknessTextBox.Text = clickedRectangle.StrokeThickness.ToString("F0");
                }
            }
        }
        public void ChangeCircleProperties(object sender, MouseButtonEventArgs e)
        {

            if (_currentDrawingMode.Equals(DrawingMode.Cursor))
            {
                BasicStackPanel.Visibility = Visibility.Collapsed;
                CircleStackPanel.Visibility = Visibility.Visible;
                _currentRectangle = null;
                _currentLine = null;
                _offset = e.GetPosition(DrawCanvas);
                Ellipse clickedCircle = sender as Ellipse;
                if (clickedCircle != null)
                {

                    _currentCircle = clickedCircle;
                    double radius = _currentCircle.Width / 2;
                    double centerX = Canvas.GetLeft(_currentCircle)+radius;
                    double centerY = Canvas.GetTop(_currentCircle)+ radius;

                    X1CircleCoordinateTextBox.Text = centerX.ToString("F0");
                    Y1CircleCoordinateTextBox.Text = centerY.ToString("F0");
                    RadiusTextBox.Text = radius.ToString("F0");
                    CircleLineThicknessTextBox.Text = _currentCircle.StrokeThickness.ToString("F0");
                }
            }
        }
        private void SetProperties_Button(object sender, RoutedEventArgs e)
        {
            switch (_currentDrawingMode)
            {
                case DrawingMode.Cursor:
                    {
                        if (_currentLine != null)
                        {
                            _textdrawing.EditLine();
                        }
                        else if (_currentRectangle != null)
                        {
                            _textdrawing.EditRectangle();
                        }else if (_currentCircle != null)
                        {
                            _textdrawing.EditCircle();
                        }
                        
                        break;
                    }
                case DrawingMode.Line:
                    {
                        if (_currentLine == null)
                        {
                            _textdrawing.NewLine();
                        }
                        else if (_currentLine != null)
                        {
                            _textdrawing.EditLine();
                        }
                        break;
                    }
                case DrawingMode.Rectangle:
                    {
                        if(_currentRectangle == null)
                        {
                            _textdrawing.NewRectangle();
                        }
                        else if (_currentRectangle != null)
                        {
                            _textdrawing.EditRectangle();
                        }
                        break;
                    }
                case DrawingMode.Circle:
                    {
                        if (_currentCircle == null)
                        {
                            _textdrawing.NewCircle();
                        }
                        else if (_currentCircle != null)
                        {
                            _textdrawing.EditCircle();
                        }
                        break;
                    }
            }
        }              

        private void Cursor_Selected(object sender, RoutedEventArgs e)
        {
            BasicStackPanel.Visibility = Visibility.Visible;
            CircleStackPanel.Visibility = Visibility.Collapsed;
            Reset();
            _currentDrawingMode = DrawingMode.Cursor;
        }
        private void Line_Selected(object sender, RoutedEventArgs e)
        {
            BasicStackPanel.Visibility = Visibility.Visible;
            CircleStackPanel.Visibility = Visibility.Collapsed;
            Reset();
            _currentDrawingMode = DrawingMode.Line;
        }
        private void Rectangle_Selected(object sender, RoutedEventArgs e)
        {
            BasicStackPanel.Visibility = Visibility.Visible;
            CircleStackPanel.Visibility = Visibility.Collapsed;
            Reset();
            _currentDrawingMode = DrawingMode.Rectangle;
        }
        private void Circle_Selected(object sender, RoutedEventArgs e)
        {
            BasicStackPanel.Visibility = Visibility.Collapsed;
            CircleStackPanel.Visibility = Visibility.Visible;
            Reset();
            _currentDrawingMode = DrawingMode.Circle;
        }
               
    }
}