using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsDesigner
{
    public partial class FormEditor : Form
    {
        Type elementType = null;
        Control selectedElement = null;

        string FileDirectory;
        BuildForm buildForm = new BuildForm();

        string subscriptions = "";

        public FormEditor()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            controlsDictionary.Add(new Button().GetType());
            controlsDictionary.Add(new Label().GetType());
            buildForm.Size = panel1.Size;
            paramsItemBox.SelectedIndex = 0;

            buildForm.Show();
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {

            if (elementType != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Point position = e.Location;
                    Control control = Activator.CreateInstance(elementType) as Control;
                    Control clone = Activator.CreateInstance(elementType) as Control;

                    //Свойства создаваемого элемента 

                    string temporaryName = control.GetType().ToString();
                    string[] type = control.GetType().ToString().Split('.');
                    int indexer = 1;
                    if (type.Length > 0)
                    {
                        temporaryName = type[type.Length - 1];
                    }
                    List<string> names = new List<string>();
                    for (int i = 0; i < panel1.Controls.Count; i++)
                    {
                        names.Add(panel1.Controls[i].Name);
                    }
                    for (int i = 0; i < panel1.Controls.Count && names.Contains(temporaryName + indexer.ToString()); i++)
                    {
                        indexer++;
                    }
                    //indexer = panel1.Controls.Count;
                    temporaryName += indexer.ToString();

                    control.Name = temporaryName;
                    control.Text = temporaryName;
                    control.Location = position;
                    control.Size = new Size(75, 23);

                    SyncClone(control, clone);

                    buildForm.Controls.Add(clone);
                    panel1.Controls.Add(control);

                    items.SelectedIndex = -1;
                    elementType = null;
                    selectedElement = control;
                    UpdateTextBoxParams();
                    control.Click += new EventHandler(Element_MouseClick);

                    allElementsBox.Items.Add(new CollectionItem(control));
                }
                if (e.Button == MouseButtons.Right)
                {
                    selectedElement = null;
                    items.SelectedIndex = -1;
                    elementType = null;
                }
            }
        }
        private void Element_MouseClick(object sender, EventArgs e)
        {
            //label1.Text = sender.GetType().ToString();            
            if (sender is Control control)
            {
                selectedElement = control;
                UpdateTextBoxParams();
            }
        }


        private void SyncClone(Control main, Control clone)
        {
            if (main != null && clone != null)
            {
                clone.Name = main.Name;
                clone.Text = main.Text;
                clone.Location = main.Location;
                clone.Size = main.Size;
                clone.BackColor = main.BackColor;
                clone.ForeColor = main.ForeColor;
            }
        }

        private Control FindClone(Control main)
        {
            Control clone = null;
            foreach (Control control in buildForm.Controls)
            {
                if (control.Name == main.Name)
                {
                    clone = control;
                }
            }
            return clone;
        }

        private void WriteFile(string locationForm)
        {
            string ns_str = FindNamespace(locationForm);
            string cl_str = FindClass(locationForm);
            if (buildForm != null)
            {
                using (FileStream fsWriter = new FileStream(locationForm, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(fsWriter))
                    {

                        writer.WriteLine(string.Concat("namespace ", ns_str, "\n{"));
                        writer.WriteLine(string.Concat("\tpartial class ", cl_str, "\n\t{"));

                        writer.WriteLine("\t\tprivate System.ComponentModel.IContainer components = null;");
                        writer.WriteLine("\t\tprotected override void Dispose(bool disposing)\n" +
                            "\t\t{\n" +
                            "\t\t\tif (disposing && (components != null))\n" +
                            "\t\t\t{\n" +
                            "\t\t\t\tcomponents.Dispose();\n" +
                            "\t\t\t}\n" +
                            "\t\t\tbase.Dispose(disposing);\n" +
                            "\t\t}\n");


                        writer.WriteLine("\t\tprivate void InitializeComponent()\n" +
                            "\t\t{\n");

                        //Инициализация элементов
                        foreach (Control control in buildForm.Controls)
                        {
                            writer.WriteLine(string.Concat("\t\t\tthis.", control.Name, " = new ", control.GetType().ToString(), "();"));
                        }
                        writer.WriteLine("\t\t\tthis.SuspendLayout();");

                        //Инициализация полей
                        foreach (Control control in buildForm.Controls)
                        {
                            writer.WriteLine(string.Concat("\t\t\t// \n" +
                                "\t\t\t// ", control.Name, " \n" +
                                "\t\t\t//"));

                            writer.WriteLine(string.Concat("\t\t\tthis.", control.Name, ".Location = new System.Drawing.Point(",
                                    control.Location.X.ToString(), ", ", control.Location.Y.ToString(), ");"));
                            writer.WriteLine(string.Concat("\t\t\tthis.", control.Name, ".Name = \"", control.Name, "\";"));
                            writer.WriteLine(string.Concat("\t\t\tthis.", control.Name, ".Size = new System.Drawing.Size(",
                                control.Size.Width.ToString(), ", ", control.Size.Height.ToString(), ");"));
                            writer.WriteLine(string.Concat("\t\t\tthis.", control.Name, ".Text = \"", control.Text, "\";"));
                            if (!control.BackColor.IsSystemColor)
                            {
                                writer.WriteLine(string.Concat("\t\t\tthis.", control.Name, ".BackColor = System.Drawing.Color.", control.BackColor.Name.ToString(), ";"));
                            }
                            else
                            {
                                writer.WriteLine(string.Concat("\t\t\tthis.", control.Name, ".BackColor = System.Drawing.SystemColors.", control.BackColor.Name.ToString(), ";"));
                            }
                            if (!control.ForeColor.IsSystemColor)
                            {
                                writer.WriteLine(string.Concat("\t\t\tthis.", control.Name, ".ForeColor = System.Drawing.Color.", control.ForeColor.Name.ToString(), ";"));
                            }
                            else
                            {
                                writer.WriteLine(string.Concat("\t\t\tthis.", control.Name, ".ForeColor = System.Drawing.SystemColors.", control.ForeColor.Name.ToString(), ";"));
                            }
                        }
                        writer.WriteLine(string.Concat("\t\t\t// \n" +
                                "\t\t\t// ", cl_str, " \n" +
                                "\t\t\t//"));

                        writer.WriteLine(string.Concat("\t\t\tthis.AutoScaleDimensions = new System.Drawing.SizeF(",
                              AutoScaleDimensions.Width.ToString(), "F, ", AutoScaleDimensions.Height.ToString(), "F);"));
                        writer.WriteLine(string.Concat("\t\t\tthis.AutoScaleMode = System.Windows.Forms.AutoScaleMode.",
                              AutoScaleMode.ToString(), ";"));
                        writer.WriteLine(string.Concat("\t\t\tthis.ClientSize = new System.Drawing.Size(",
                               panel1.Size.Width.ToString(), ", ", panel1.Size.Height.ToString(), ");"));

                        writer.WriteLine(string.Concat("\t\t\tthis.Name = ", buildForm.Name, ";\n"));
                        writer.WriteLine(string.Concat("\t\t\tthis.Text = ", buildForm.Text, ";\n"));

                        writer.WriteLine("\n");
                        //Форма
                        writer.WriteLine("\t\t\tthis.ResumeLayout(false);\n" +
                            "\t\t\tthis.PerformLayout(); ");

                        //Добавление элементов
                        foreach (Control control in buildForm.Controls)
                        {
                            writer.WriteLine(string.Concat("\t\t\tthis.Controls.Add(", control.Name, ");"));
                        }

                        //Вызов метода подписок
                        writer.WriteLine("\t\t\tSubscriptions();");

                        writer.WriteLine("\t\t}");

                        //Метод с подписками
                        writer.WriteLine(string.Concat("\t\tprivate void Subscriptions()\n\t\t{\n", subscriptions, "\t\t}"));

                        //Объявление элементов
                        foreach (Control control in buildForm.Controls)
                        {
                            writer.WriteLine(string.Concat("\t\tprivate ", control.GetType().ToString(), " ", control.Name, ";"));
                        }
                        writer.WriteLine("\t}\n}");
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            buildForm.Controls.Clear();
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "desgigner files (*.Designer.cs)|*.Designer.cs|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    FileDirectory = openFileDialog.FileName;
                    //LoadCustomForm(FileDirectory);
                }
                directoryTextbox.Text = FileDirectory;
            }
            AddControlsFromFile(FileDirectory);
        }

        private void Сохранить_Click(object sender, EventArgs e)
        {
            WriteFile(FileDirectory);
        }

        private void items_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (items.SelectedIndex >= 0 && items.SelectedIndex < controlsDictionary.Count)
            {
                allElementsBox.SelectedIndex = -1;
                elementType = controlsDictionary[items.SelectedIndex];
                selectedElement = null;
            }
        }

        List<Type> controlsDictionary = new List<Type>();

        private void textBoxWidth_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int number;
                if (int.TryParse(textBoxWidth.Text, out number))
                {
                    buildForm.ClientSize = new Size(number, buildForm.ClientSize.Height);
                    panel1.Size = new Size(number, buildForm.ClientSize.Height);

                    widthNumericUpDown.Value = number;
                }
            }
            catch
            {

            }
        }

        private void textBoxHeight_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int number;
                if (int.TryParse(textBoxHeight.Text, out number))
                {
                    buildForm.ClientSize = new Size(buildForm.ClientSize.Width, number);
                    panel1.Size = new Size(buildForm.ClientSize.Width, number);

                    heightNumericUpDown.Value = number;
                }
            }
            catch
            {

            }
        }


        private void UpdateTextBoxParams()
        {
            if (selectedElement != null)
            {
                if (paramsItemBox.SelectedIndex == 0)
                {
                    textBoxElement.Text = selectedElement.Name;
                }
                if (paramsItemBox.SelectedIndex == 1)
                {
                    numericUpDown1.Show();
                    textBoxElement.Text = selectedElement.Size.Width.ToString();
                    numericUpDown1.Value = Math.Max(numericUpDown1.Minimum, Math.Min(numericUpDown1.Maximum, Convert.ToInt32(textBoxElement.Text)));
                }
                if (paramsItemBox.SelectedIndex == 2)
                {
                    numericUpDown1.Show();
                    textBoxElement.Text = selectedElement.Size.Height.ToString();
                    numericUpDown1.Value = Math.Max(numericUpDown1.Minimum, Math.Min(numericUpDown1.Maximum, Convert.ToInt32(textBoxElement.Text)));
                }
                if (paramsItemBox.SelectedIndex == 3)
                {
                    numericUpDown1.Show();
                    textBoxElement.Text = selectedElement.Location.X.ToString();
                    numericUpDown1.Value = Math.Max(numericUpDown1.Minimum, Math.Min(numericUpDown1.Maximum, Convert.ToInt32(textBoxElement.Text)));
                }
                if (paramsItemBox.SelectedIndex == 4)
                {
                    numericUpDown1.Show();
                    textBoxElement.Text = selectedElement.Location.Y.ToString();
                    numericUpDown1.Value = Math.Max(numericUpDown1.Minimum, Math.Min(numericUpDown1.Maximum, Convert.ToInt32(textBoxElement.Text)));
                }
                if (paramsItemBox.SelectedIndex == 5)
                {
                    textBoxElement.Text = selectedElement.Text;
                }
                if (paramsItemBox.SelectedIndex == 6)
                {
                    textBoxElement.Text = WFConverter.FromColor(selectedElement.BackColor);
                }
                if (paramsItemBox.SelectedIndex == 7)
                {
                    textBoxElement.Text = WFConverter.FromColor(selectedElement.ForeColor);
                }
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTextBoxParams();
        }
        private void textBoxElement_TextChanged(object sender, EventArgs e)
        {
            if (selectedElement != null)
            {
                if (paramsItemBox.SelectedIndex == 0)
                {
                    bool allowed = true;
                    foreach (Control control in panel1.Controls)
                    {
                        if (control.Name == textBoxElement.Text && control != selectedElement)
                        {
                            allowed = false;
                            MessageBox.Show("Это имя уже занято!");
                        }
                    }

                    if (allowed)
                    {
                        FindClone(selectedElement).Name = textBoxElement.Text;
                        selectedElement.Name = textBoxElement.Text;
                    }
                }
                if (paramsItemBox.SelectedIndex == 1)
                {
                    selectedElement.Size = WFConverter.ToWidth(textBoxElement.Text, selectedElement);
                }
                if (paramsItemBox.SelectedIndex == 2)
                {
                    selectedElement.Size = WFConverter.ToHeight(textBoxElement.Text, selectedElement);
                }
                if (paramsItemBox.SelectedIndex == 3)
                {
                    selectedElement.Location = WFConverter.ToPointX(textBoxElement.Text, selectedElement);
                }
                if (paramsItemBox.SelectedIndex == 4)
                {
                    selectedElement.Location = WFConverter.ToPointY(textBoxElement.Text, selectedElement);
                }
                if (paramsItemBox.SelectedIndex == 5)
                {
                    selectedElement.Text = textBoxElement.Text;
                }
                if (paramsItemBox.SelectedIndex == 6)
                {
                    selectedElement.BackColor = WFConverter.ToColor(textBoxElement.Text);
                }
                if (paramsItemBox.SelectedIndex == 7)
                {
                    selectedElement.ForeColor = WFConverter.ToColor(textBoxElement.Text);
                }
                SyncClone(selectedElement, FindClone(selectedElement));
            }
        }

        private void allElementsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (allElementsBox.SelectedIndex >= 0 && allElementsBox.SelectedIndex < allElementsBox.Items.Count)
            {
                if (allElementsBox.Items[allElementsBox.SelectedIndex] is CollectionItem item)
                {
                    items.SelectedIndex = -1;
                    elementType = null;
                    selectedElement = item.Item;
                    UpdateTextBoxParams();
                }
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (selectedElement != null && buildForm != null)
            {
                foreach (Control control in buildForm.Controls)
                {
                    if (control.Name == selectedElement.Name)
                    {
                        buildForm.Controls.Remove(control);
                        break;
                    }
                }
                panel1.Controls.Remove(selectedElement);
                foreach (CollectionItem control in allElementsBox.Items.OfType<CollectionItem>())
                {
                    if (control.Item == selectedElement)
                    {
                        allElementsBox.Items.Remove(control);
                        break;
                    }
                }
                paramsItemBox.SelectedIndex = -1;
                selectedElement = null;
                textBoxElement.Text = "";

            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int number;
            if (int.TryParse(textBoxElement.Text, out number))
            {
                textBoxElement.Text = numericUpDown1.Value.ToString();
            }
            UpdateTextBoxParams();
        }

        private void open_button_Click(object sender, EventArgs e)
        {
            if (directoryTextbox.Text != "")
            {
                Process txt = new Process();
                txt.StartInfo.FileName = "devenv";
                txt.StartInfo.Arguments = $@"{directoryTextbox.Text}";
                txt.Start();


            }
            else
            {
                MessageBox.Show("Укажите путь", Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string FindNamespace(string locationForm)
        {
            string result = "";
            using (FileStream stream = new FileStream(locationForm, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    Match match = Regex.Match(reader.ReadToEnd(), "namespace (.*?){", RegexOptions.Singleline);
                    result = match.Groups[1].ToString();
                }

            }
            return result;
        }
        private string FindClass(string locationForm)
        {
            string result = "";
            using (FileStream stream = new FileStream(locationForm, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    Match match = Regex.Match(reader.ReadToEnd(), "class (.*?){", RegexOptions.Singleline);
                    result = match.Groups[1].ToString();
                }

            }
            return result;
        }

        private void AddControlsFromFile(string locationForm)
        {
            using (FileStream stream = new FileStream(locationForm, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        Match match = Regex.Match(reader.ReadLine(), "private (.*?) (.*?);", RegexOptions.Singleline);

                        if (match.Groups[1].ToString().Length > 0)
                        {
                            Assembly asm = typeof(Control).Assembly;
                            string typeName = match.Groups[1].ToString();
                            string assemblyName = asm.GetName().ToString();
                            string shortName = asm.GetName().Name;
                            Type t = Type.GetType($"{typeName}, {assemblyName}");

                            if (t != null && t.IsSubclassOf(typeof(Control)))
                            {
                                Control control = Activator.CreateInstance(t) as Control;
                                Control clone = Activator.CreateInstance(t) as Control;

                                control.Name = match.Groups[2].ToString();
                                control.Text = control.Name;

                                SyncClone(control, clone);

                                buildForm.Controls.Add(clone);
                                panel1.Controls.Add(control);

                                control.Click += new EventHandler(Element_MouseClick);
                                allElementsBox.Items.Add(new CollectionItem(control));
                            }
                        }
                    }
                }
            }
            UpdateTextBoxParams();
            InitializeControls(locationForm);
        }

        private void InitializeControls(string locationForm)
        {
            foreach (Control control in panel1.Controls)
            {
                using (FileStream stream = new FileStream(locationForm, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            Match match = Regex.Match(reader.ReadLine(), @"this\.(.*?)\.(.*?) = (.*?);", RegexOptions.Singleline);
                            if (control.Name == match.Groups[1].ToString() && !match.Groups[3].ToString().Contains("()"))
                            {
                                PropertyInfo[] fields = control.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                                //PropertyInfo[] properties = control.GetType().GetProperties();

                                string[] text = match.Groups[2].Value.GetType().ToString().Split('.');
                                //  string name = text[text.Length - 1];
                                string orig = match.Groups[2].Value.ToString();

                                foreach (PropertyInfo field in fields)
                                {
                                    if (field.Name == orig)
                                    {
                                        string input = match.Groups[3].ToString();
                                        string typeofcolor = orig;
                                        SetProperties(input, typeofcolor, control);
                                    }
                                }
                                SyncClone(control, FindClone(control));
                            }
                        }
                    }
                }
            }
            using (FileStream stream = new FileStream(locationForm, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        Match match = Regex.Match(reader.ReadLine(), @"this\.(.*?) = (.*?);", RegexOptions.Singleline);
                        if (!match.Groups[3].ToString().Contains("()") && !match.Groups[1].ToString().Contains("."))
                        {
                            PropertyInfo[] fields = buildForm.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                            //PropertyInfo[] properties = control.GetType().GetProperties();

                            string[] text = match.Groups[1].Value.GetType().ToString().Split('.');
                            //  string name = text[text.Length - 1];
                            string orig = match.Groups[1].Value.ToString();

                            foreach (PropertyInfo field in fields)
                            {
                                if (field.Name == orig)
                                {
                                    string input = match.Groups[2].ToString();
                                    string typeofcolor = orig;
                                    SetFormProperties(input, typeofcolor, field, buildForm);
                                }
                            }
                        }
                    }
                }
            }
            InitializeSubscriptions(locationForm);
        }
        private void SetFormProperties(string input, string type_of_color, PropertyInfo field, Form form)
        {
            if (input.Contains(".Point"))
            {
                Match match1 = Regex.Match(input, @"System.Drawing.Point\((.*?),(.*?)\)");
                int x = Convert.ToInt32(match1.Groups[1].ToString());
                int y = Convert.ToInt32(match1.Groups[2].ToString());
                form.Location = new Point(x, y);
            }
            if (type_of_color.Contains("ForeColor") && input.Contains(".Color"))
            {
                string[] text = input.Split('.');
                form.ForeColor = Color.FromName(text[text.Length - 1]);
            }
            if (type_of_color.Contains("BackColor") && input.Contains(".Color"))
            {
                string[] text = input.Split('.');
                form.BackColor = Color.FromName(text[text.Length - 1]);
            }
            if (type_of_color.Contains("ForeColor") && input.Contains(".SystemColors"))
            {
                string[] text = input.Split('.');
                form.ForeColor = Color.FromName(text[text.Length - 1]);
            }
            if (type_of_color.Contains("BackColor") && input.Contains(".SystemColors"))
            {
                string[] text = input.Split('.');
                form.BackColor = Color.FromName(text[text.Length - 1]);
            }

            if (field.Name == "ClientSize")
            {
                Match match1 = Regex.Match(input, @"System\.Drawing\.Size\((.*?),(.*?)\)");
                int width = Convert.ToInt32(match1.Groups[1].ToString());
                int height = Convert.ToInt32(match1.Groups[2].ToString());
                form.ClientSize = new Size(width, height);
                panel1.Size = new Size(width, height);
            }
            else if (field.Name == "Name")
            {
                Match match1 = Regex.Match(input, "\"(.*?)\"");
                form.Name = string.Concat("\"", match1.Groups[1].ToString().Replace("\"", ""), "\"");
            }
            else if (field.Name == "Text")
            {
                Match match1 = Regex.Match(input, "\"(.*?)\"");
                form.Text = string.Concat("\"", match1.Groups[1].ToString().Replace("\"", ""), "\"");
            }
            else if (field.Name == "AutoScaleDimensions")
            {
                Match match1 = Regex.Match(input, @"System\.Drawing\.SizeF\((.*?)F,(.*?)F\)");
                float width = Convert.ToSingle(match1.Groups[1].ToString());
                float height = Convert.ToSingle(match1.Groups[2].ToString());
                form.AutoScaleDimensions = new SizeF(width, height);
            }
            else if (field.Name == "AutoScaleMode")
            {
                //Match match1 = Regex.Match(input, @"System\.Windows\.Forms\.AutoScaleMode\.(.?*)");
            }
            else if (input.Contains(".Size"))
            {
                try
                {
                    string f = field.Name;
                    Match match1 = Regex.Match(input, @"System.Drawing.Size\((.*?),(.*?)\)");
                    int width = Convert.ToInt32(match1.Groups[1].ToString());
                    int height = Convert.ToInt32(match1.Groups[2].ToString());
                    form.Size = new Size(width, height);
                    panel1.Size = new Size(width, height);
                }
                catch
                {
                    string f = field.Name;
                }
            }
        }

        private void SetProperties(string input, string type_of_color, Control control)
        {
            if (input.Contains("\""))
            {
                Match match1 = Regex.Match(input, "\"(.*?)\"");
                control.Text = match1.Groups[1].ToString();
            }
            if (input.Contains(".Size"))
            {
                Match match1 = Regex.Match(input, @"System.Drawing.Size\((.*?),(.*?)\)");
                int width = Convert.ToInt32(match1.Groups[1].ToString());
                int height = Convert.ToInt32(match1.Groups[2].ToString());
                control.Size = new Size(width, height);
            }
            if (input.Contains(".Point"))
            {
                Match match1 = Regex.Match(input, @"System.Drawing.Point\((.*?),(.*?)\)");
                int x = Convert.ToInt32(match1.Groups[1].ToString());
                int y = Convert.ToInt32(match1.Groups[2].ToString());
                control.Location = new Point(x, y);
            }
            if (type_of_color.Contains("ForeColor") && input.Contains(".Color"))
            {
                string[] text = input.Split('.', ';');
                control.ForeColor = Color.FromName(text[text.Length - 1]);
            }
            if (type_of_color.Contains("BackColor") && input.Contains(".Color"))
            {
                string[] text = input.Split('.', ';');
                control.BackColor = Color.FromName(text[text.Length - 1]);
            }
            if (type_of_color.Contains("ForeColor") && input.Contains(".SystemColors"))
            {
                string[] text = input.Split('.', ';');
                control.ForeColor = Color.FromName(text[text.Length - 1]);
            }
            if (type_of_color.Contains("BackColor") && input.Contains(".SystemColors"))
            {
                string[] text = input.Split('.', ';');
                control.BackColor = Color.FromName(text[text.Length - 1]);
            }
        }

        private void InitializeSubscriptions(string locationForm)
        {
            using (FileStream stream = new FileStream(locationForm, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        Match match = Regex.Match(reader.ReadLine(), @"(.*?) \+= (.*?);", RegexOptions.Singleline);
                        if (match.Groups[1].ToString().Length > 0 && match.Groups[2].ToString().Length > 0)
                        {
                            subscriptions += string.Concat(match.Groups[1].ToString(), " += ", match.Groups[2].ToString(), ";\n");
                        }
                    }
                }
            }
        }

        private void widthNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            int number;
            if (int.TryParse(widthNumericUpDown.Value.ToString(), out number))
            {
                textBoxWidth.Text = widthNumericUpDown.Value.ToString();
                panel1.Size = new Size(number, panel1.Size.Height);
                buildForm.ClientSize = new Size(number, panel1.Size.Height);
            }
        }

        private void heightNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            int number;
            if (int.TryParse(heightNumericUpDown.Value.ToString(), out number))
            {
                textBoxHeight.Text = heightNumericUpDown.Value.ToString();
                panel1.Size = new Size(panel1.Size.Width, number);
                buildForm.ClientSize = new Size(panel1.Size.Width, number);
            }
        }
    }
}