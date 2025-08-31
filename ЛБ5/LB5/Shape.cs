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

        public int Width { get; set; } = 10;
        public int Height { get; set; } = 10;

        public void draw(Canvas canvas, System.Windows.Point point)
        {
            Polygon polygon = new Polygon();
            polygon.Points.Add(point);
            polygon.Points.Add(new System.Windows.Point(point.X + Width, point.Y));
            polygon.Points.Add(new System.Windows.Point(point.X, point.Y + Height));
            polygon.Fill = new SolidColorBrush((Color)Background);
            polygon.Stroke = new SolidColorBrush((Color)Foreground);
            polygon.StrokeThickness = Thickness;
            canvas.Children.Add(polygon);

            //Ellipse ellipse = new Ellipse();
            //ellipse.Width = Width;
            //ellipse.Height = Height;
            //Margin = new Thickness(position.X - Width / 2, position.Y - Height / 2, 0, 0)
            //ellipse.Fill = new SolidColorBrush((Color)Background);
            //ellipse.Stroke = new SolidColorBrush((Color)Foreground);
            //ellipse.StrokeThickness = Thickness;
            //canvas.Children.Add(ellipse);
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
            return $"Thickness = {Thickness}  Background = {Background}  Foreground = {Foreground}\n +" +
                $"Width = {Width}  Height = {Height}";
        }
    }
}
