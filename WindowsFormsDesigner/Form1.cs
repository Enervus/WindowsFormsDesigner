﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;
using System.Diagnostics;

namespace WindowsFormsDesigner
{
    public partial class FormEditor : Form
    {
        static CodeDomProvider provider;
        Type elementType = null;
        Control selectedElement = null;

        string FileDirectory;
        BuildForm buildForm = new BuildForm();

        string formNamespace = "";
        string DesginerName = "";

        public FormEditor()
        {
            InitializeComponent();
            provider = new CSharpCodeProvider();
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
                if(e.Button == MouseButtons.Right)
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
        private void LoadCustomForm(string formLocation)
        {
            string outputFile = "";
            StreamReader reader = new StreamReader(formLocation);
            while (!reader.EndOfStream)
            {
                string s = reader.ReadLine();
                if (s.Contains("+="))
                {
                    outputFile += "//";
                }
                outputFile += s;
            }
        }

        private void SyncClone(Control main, Control clone)
        {
            clone.Name = main.Name;
            clone.Text = main.Text;
            clone.Location = main.Location;
            clone.Size = main.Size;
            clone.BackColor = main.BackColor;
            clone.ForeColor = main.ForeColor;
        }

        private Control FindClone(Control main)
        {
            Control clone = null;
            foreach(Control control in buildForm.Controls)
            {
                if(control.Name == main.Name)
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
                            writer.WriteLine(string.Concat("\t\t\tthis.", control.Name, ".BackColor = System.Drawing.Color.FromArgb(", control.BackColor.ToArgb(),");"));
                            writer.WriteLine(string.Concat("\t\t\tthis.", control.Name, ".ForeColor = System.Drawing.Color.FromArgb(", control.ForeColor.ToArgb(), ");"));
                        }
                        writer.WriteLine(string.Concat("\t\t\t// \n" +
                                "\t\t\t// ", cl_str, " \n" +
                                "\t\t\t//")) ;
                        writer.WriteLine(string.Concat("\t\t\t this.AutoScaleDimensions = new System.Drawing.SizeF(",
                              this.AutoScaleDimensions.Width.ToString(), ", ", this.AutoScaleDimensions.Height.ToString(), ");"));
                        writer.WriteLine(string.Concat("\t\t\t this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.",
                              this.AutoScaleMode.ToString(), ";"));
                        writer.WriteLine(string.Concat("\t\t\tthis.ClientSize = new System.Drawing.Size(",
                               panel1.Size.Width.ToString(), ", ", panel1.Size.Height.ToString(), ");"));
                      
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
                        writer.WriteLine("\t\tprivate void Subscriptions()\n\t\t{ }");

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
        private CodeExpression ConcatString(CodeExpression left, CodeExpression middle, CodeExpression right)
        {
            return new CodeSnippetExpression(CodeToString(left) + " + " + CodeToString(middle) + " + " + CodeToString(right));
        }
        private string CodeToString(CodeExpression expr)
        {
            using (TextWriter tx = new StringWriter())
            {
                provider.GenerateCodeFromExpression(expr, tx, new CodeGeneratorOptions());
                return tx.ToString();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
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
                FindNamespace(FileDirectory);
            }
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
                buildForm.ClientSize = new Size(Convert.ToInt32(textBoxWidth.Text), buildForm.ClientSize.Height);
                panel1.Size = new Size(Convert.ToInt32(textBoxWidth.Text), buildForm.ClientSize.Height);
            }
            catch
            {

            }
        }

        private void textBoxHeight_TextChanged(object sender, EventArgs e)
        {
            try
            {
                buildForm.ClientSize = new Size(buildForm.ClientSize.Width, Convert.ToInt32(textBoxHeight.Text));
                panel1.Size = new Size(buildForm.ClientSize.Width, Convert.ToInt32(textBoxHeight.Text));
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
                    numericUpDown1.Value = Convert.ToInt32(textBoxElement.Text);
                }
                if (paramsItemBox.SelectedIndex == 2)
                {
                    numericUpDown1.Show();
                    textBoxElement.Text = selectedElement.Size.Height.ToString();
                    numericUpDown1.Value = Convert.ToInt32(textBoxElement.Text);
                }
                if (paramsItemBox.SelectedIndex == 3)
                {
                    numericUpDown1.Show();
                    textBoxElement.Text = selectedElement.Location.X.ToString();
                    numericUpDown1.Value = Convert.ToInt32(textBoxElement.Text);
                }
                if (paramsItemBox.SelectedIndex == 4)
                {
                    numericUpDown1.Show();
                    textBoxElement.Text = selectedElement.Location.Y.ToString();
                    numericUpDown1.Value = Convert.ToInt32(textBoxElement.Text);
                }
                if (paramsItemBox.SelectedIndex == 5)
                {
                    textBoxElement.Text = selectedElement.Text;
                }
                if(paramsItemBox.SelectedIndex == 6)
                {
                    textBoxElement.Text = WFConverter.FromColor(selectedElement.BackColor);
                }
                if(paramsItemBox.SelectedIndex == 7)
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
                    foreach(Control control in panel1.Controls)
                    {
                        if(control.Name == textBoxElement.Text && control != selectedElement)
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
            if(selectedElement != null && buildForm != null)
            {
                foreach(Control control in buildForm.Controls)
                {
                    if(control.Name == selectedElement.Name)
                    {
                        buildForm.Controls.Remove(control);
                        break;
                    }
                }
                panel1.Controls.Remove(selectedElement);
                foreach (CollectionItem control in allElementsBox.Items.OfType<CollectionItem>())
                {
                    if(control.Item == selectedElement)
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

                textBoxElement.Text = numericUpDown1.Value.ToString();
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
                MessageBox.Show("Укажите путь",Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string FindNamespace(string locationForm)
        {
            string result = "";
            using (FileStream stream = new FileStream(locationForm,FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    Match match = Regex.Match(reader.ReadToEnd(),"namespace (.*?){",RegexOptions.Singleline);
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
    }
}