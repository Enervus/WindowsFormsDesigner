using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsDesigner
{
    public class WFConverter
    {
        public static string Name = "Конвертация";
        public static Size ToWidth(string input,Control selectedElement)
        {
            Size size = selectedElement.Size;
            try
            {
                size = new Size(Convert.ToInt32(input), selectedElement.Height);
            }
            catch
            {

            }
            return size;
        }
        public static Size ToHeight(string input, Control selectedElement)
        {
            Size size = selectedElement.Size;
            try
            {
                size = new Size(selectedElement.Width, Convert.ToInt32(input));
            }
            catch
            {

            }
            return size;
        }
        public static Point ToPointX(string input, Control selectedElement)
        {
            Point point = selectedElement.Location;
            try
            {
                point = new Point(Convert.ToInt32(input), selectedElement.Location.Y);
            }
            catch
            {

            }
            return point;
           
        }
        public static Point ToPointY(string input, Control selectedElement)
        {
            Point point = selectedElement.Location;
            try
            {
                point = new Point(selectedElement.Location.X, Convert.ToInt32(input));
            }
            catch
            {

            }
            return point;
        }
        public static Color ToColor(string input)
        {
            string[] text = input.Split(new char[] { '{', ',', ' ', '=', '}' });
            int[] numbers = new int[3];
            int foundNumbers = 0;

            for (int i = 0; i < text.Length && foundNumbers < 3; i++)
            {
                try
                {
                    int number = 0;
                    if (int.TryParse(text[i], out number))
                    {
                        if (number > 255)
                        {
                            number = 255;
                        }
                        if (number < 0)
                        {
                            number = 0;
                        }
                        numbers[foundNumbers] = number;
                        foundNumbers++;
                    }
                }
                catch
                {
                    MessageBox.Show("Введено неверное значение", Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


            return Color.FromArgb(numbers[0], numbers[1], numbers[2]);
        }

        public static string FromColor(Color color)
        {
            string text = "";
            if (color != null)
            {
                text = string.Concat(color.R.ToString(), ",", color.G.ToString(), ",", color.B.ToString());
            }

            return text;
        }
    }
}
