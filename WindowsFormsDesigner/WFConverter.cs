using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.IO;

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

            if (text.Length >= 3)
            {
                return Color.FromArgb(numbers[0], numbers[1], numbers[2]);
            }
            return Color.FromName(text[0]);
        }

        public static string FromColor(Color color)
        {
            string text = "";
            if (color != null)
            {
                text = color.Name;
            }

            return text;
        }
        public static Type GetType(string input)
        {
            string type = input;
            string assemblyName = GetAssemblyOfTypeString(type).GetName().ToString();
            return Type.GetType($"{type}, {assemblyName}");
        }
        public static Type GetTypeFromInputString(string input)
        {
            Type type = null; 
            string[] text = input.Split(new char[] { '{', '}', ',', ' ', '=', '(', ')' });
            for (int i = 0; i < text.Length; i++)
            {
                int number;
                float numberF;
                double numberD;
                bool BoolValue;

                if (text[i].Contains("\""))
                {
                    return string.Empty.GetType();
                }
                else if (bool.TryParse(text[i], out BoolValue))
                {
                    return (BoolValue).GetType();
                }
                else if (int.TryParse(text[i], out number))
                {
                    return number.GetType();
                }
                else if (float.TryParse(text[i], out numberF))
                {
                    return numberF.GetType();
                }
                else if (double.TryParse(text[i], out numberD))
                {
                    return numberD.GetType();
                }
                else
                {
                    if (text[i] == "new")
                    {
                        return GetType(input);
                    }
                }
            }
            return type;
        }

        public static object CutFromString(string input)
        {
            List<object> output = new List<object>();

            bool isObject = false;

            string[] text = input.Split(new char[] { '{', '}', ',', ' ', '=', '(', ')' });

            string type = "";
            string assemblyName = "";

            int numberOfparameters = 0;

            for (int i = 0; i < text.Length; i++)
            {
                int number;
                float numberF;
                double numberD;
                bool BoolValue;
                if (isObject)
                {
                    if (type == "" )
                    {
                        type = text[i];
                        if (GetAssemblyOfTypeString(text[i]) != null)
                        {
                            assemblyName = GetAssemblyOfTypeString(text[i]).GetName().ToString();
                        }
                    }
                    else
                    {
                        if (text[i].Contains("\""))
                        {
                            output.Add(text[i].Replace("\"", ""));
                        }
                        else if (int.TryParse(text[i], out number))
                        {
                            output.Add(number);
                        }
                        else if (float.TryParse(text[i], out numberF))
                        {
                            output.Add(numberF);
                        }
                        else if (double.TryParse(text[i], out numberD))
                        {
                            output.Add(numberD);
                        }
                        else if (bool.TryParse(text[i], out BoolValue))
                        {
                            output.Add(BoolValue);
                        }
                    }
                }
                else
                {
                    if (text[i].Contains("\""))
                    {
                        output.Add(text[i].Replace("\"", ""));
                    }
                    else if (int.TryParse(text[i], out number))
                    {
                        output.Add(number);
                    }
                    else if (float.TryParse(text[i], out numberF))
                    {
                        output.Add(numberF);
                    }
                    else if (double.TryParse(text[i], out numberD))
                    {
                        output.Add(numberD);
                    }
                    else if (bool.TryParse(text[i], out BoolValue))
                    {
                        output.Add(BoolValue);
                    }
                    else
                    {
                        if (text[i] == "new")
                        {
                            isObject = true;
                        }
                    }
                }
            }


            if (isObject && type.Length > 0 && assemblyName.Length > 0)
            {
                List<object> contructorParams = new List<object>();
                for (int i = 0; i < output.Count; i++)
                {
                    contructorParams.Add(output[i]);
                }

                object[] returnOutput = new object[] { Activator.CreateInstance(Type.GetType($"{type}, {assemblyName}"), contructorParams.ToArray()) };
                return returnOutput;
            }
            else
            {
                object[] outp = output.ToArray();
                return outp[0];
            }
        }

        public static Assembly GetAssemblyOfTypeString(string input)
        {
            List<Assembly> assemblies = new List<Assembly>();
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            foreach (var path in Directory.GetFiles(assemblyFolder, "*.dll"))
            {
                assemblies.Add(Assembly.LoadFrom(path));
            }

            string[] text = input.Split(new char[] { '.' });

            if (text.Length > 0) 
            {
                foreach (Assembly assembly in assemblies)
                {
                    if (assembly.GetName().Name + "." + text[text.Length - 1] == input)
                    {
                        return assembly;
                    }
                    if (assembly.GetName().Name == input)
                    {
                        return assembly;
                    }
                } 
            }
            return null;
        }
    }
}
