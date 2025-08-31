using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using Color = System.Windows.Media.Color;

namespace LB5
{
    public class Shape
    {
        public Shape()
        {
        }

        public Shape(int thickness, Color? background, Color? foreground, int width, int height)
        {
            Thickness = thickness;
            Background = background;
            Foreground = foreground;
            Width = width;
            Height = height;
        }

        public int Thickness { get; set; } = 0;
        public Color? Background { get; set; }
        public Color? Foreground { get; set; }

        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;

        public void draw(Canvas canvas, System.Windows.Point point)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = Width;
            ellipse.Height = Height;
            ellipse.Margin = new Thickness(point.X - Width / 2, point.Y - Height / 2, 0, 0);

            // Создаем радиальный градиент
            RadialGradientBrush gradient = new RadialGradientBrush();
            gradient.GradientStops.Add(new GradientStop((Color)Background, 0));
            gradient.GradientStops.Add(new GradientStop((Color)Foreground, 1));
            gradient.RadiusX = 0.7;
            gradient.RadiusY = 0.7;
            gradient.Center = new System.Windows.Point(0.5, 0.5);

            ellipse.Fill = gradient;
            ellipse.Stroke = new SolidColorBrush((Color)Foreground);
            ellipse.StrokeThickness = Thickness;

            // Добавляем анимацию смещения стоп-точки
            DoubleAnimation animation = new DoubleAnimation();
            animation.To = 0.3;
            animation.Duration = TimeSpan.FromSeconds(2);
            animation.AutoReverse = true;
            animation.RepeatBehavior = RepeatBehavior.Forever;

            gradient.GradientStops[1].BeginAnimation(GradientStop.OffsetProperty, animation);

            canvas.Children.Add(ellipse);

            // Добавляем перечеркивающие диагональные линии (в пределах эллипса)
            double centerX = point.X;
            double centerY = point.Y;
            double halfWidth = Width / 2;
            double halfHeight = Height / 2;

            Line diagonalLine1 = new Line();
            diagonalLine1.X1 = centerX - halfWidth * 0.7;
            diagonalLine1.Y1 = centerY - halfHeight * 0.7;
            diagonalLine1.X2 = centerX + halfWidth * 0.7;
            diagonalLine1.Y2 = centerY + halfHeight * 0.7;
            diagonalLine1.Stroke = new SolidColorBrush((Color)Foreground);
            diagonalLine1.StrokeThickness = Thickness;

            Line diagonalLine2 = new Line();
            diagonalLine2.X1 = centerX + halfWidth * 0.7;
            diagonalLine2.Y1 = centerY - halfHeight * 0.7;
            diagonalLine2.X2 = centerX - halfWidth * 0.7;
            diagonalLine2.Y2 = centerY + halfHeight * 0.7;
            diagonalLine2.Stroke = new SolidColorBrush((Color)Foreground);
            diagonalLine2.StrokeThickness = Thickness;
            canvas.Children.Add(diagonalLine1);
            canvas.Children.Add(diagonalLine2);
        }
        public void save()
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Файлы xml|*.xml|Все файлы|*.*";
            if (fileDialog.ShowDialog() == false) return;
            XmlSerializer serializer = new XmlSerializer(typeof(Shape));
            using (FileStream file = new FileStream(fileDialog.FileName, FileMode.Create))
            {
                serializer.Serialize(file, this);
            }
        }

        public static Shape load()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Файлы xml|*.xml|Все файлы|*.*";
            if (fileDialog.ShowDialog() == false) return null;
            XmlSerializer serializer = new XmlSerializer(typeof(Shape));
            Shape shape;
            using (FileStream file = new FileStream(fileDialog.FileName, FileMode.Open))
            {
                shape = (Shape)serializer.Deserialize(file);
            }
            return shape;
        }

        public override string? ToString()
        {
            return $"Thickness = {Thickness}  Background = {Background}  Foreground = {Foreground}\n" +
                $"Width = {Width}  Height = {Height}";
        }
    }
}