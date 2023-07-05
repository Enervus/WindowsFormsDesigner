
namespace WindowsFormsDesigner
{
    partial class FormEditor
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.items = new System.Windows.Forms.ListBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.directoryTextbox = new System.Windows.Forms.TextBox();
            this.Открыть = new System.Windows.Forms.Button();
            this.Сохранить = new System.Windows.Forms.Button();
            this.textBoxElement = new System.Windows.Forms.TextBox();
            this.paramsItemBox = new System.Windows.Forms.ListBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.textBoxWidth = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBoxHeight = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.allElementsBox = new System.Windows.Forms.ListBox();
            this.deleteButton = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.open_button = new System.Windows.Forms.Button();
            this.widthNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.heightNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panel1.Location = new System.Drawing.Point(180, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(796, 612);
            this.panel1.TabIndex = 0;
            this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
            // 
            // items
            // 
            this.items.FormattingEnabled = true;
            this.items.Items.AddRange(new object[] {
            "Button",
            "Label"});
            this.items.Location = new System.Drawing.Point(6, 116);
            this.items.Name = "items";
            this.items.Size = new System.Drawing.Size(156, 30);
            this.items.TabIndex = 1;
            this.items.SelectedIndexChanged += new System.EventHandler(this.items_SelectedIndexChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // directoryTextbox
            // 
            this.directoryTextbox.Location = new System.Drawing.Point(5, 10);
            this.directoryTextbox.Name = "directoryTextbox";
            this.directoryTextbox.Size = new System.Drawing.Size(157, 20);
            this.directoryTextbox.TabIndex = 2;
            // 
            // Открыть
            // 
            this.Открыть.Location = new System.Drawing.Point(5, 37);
            this.Открыть.Name = "Открыть";
            this.Открыть.Size = new System.Drawing.Size(75, 36);
            this.Открыть.TabIndex = 3;
            this.Открыть.Text = "Указать файл";
            this.Открыть.UseVisualStyleBackColor = true;
            this.Открыть.Click += new System.EventHandler(this.button1_Click);
            // 
            // Сохранить
            // 
            this.Сохранить.Location = new System.Drawing.Point(87, 36);
            this.Сохранить.Name = "Сохранить";
            this.Сохранить.Size = new System.Drawing.Size(75, 37);
            this.Сохранить.TabIndex = 3;
            this.Сохранить.Text = "Сохранить";
            this.Сохранить.UseVisualStyleBackColor = true;
            this.Сохранить.Click += new System.EventHandler(this.Сохранить_Click);
            // 
            // textBoxElement
            // 
            this.textBoxElement.Location = new System.Drawing.Point(5, 152);
            this.textBoxElement.Name = "textBoxElement";
            this.textBoxElement.Size = new System.Drawing.Size(133, 20);
            this.textBoxElement.TabIndex = 4;
            this.textBoxElement.TextChanged += new System.EventHandler(this.textBoxElement_TextChanged);
            // 
            // paramsItemBox
            // 
            this.paramsItemBox.FormattingEnabled = true;
            this.paramsItemBox.Items.AddRange(new object[] {
            "Name",
            "Width",
            "Height",
            "Location.X",
            "Location.Y",
            "Text",
            "BackColor",
            "ForeColor"});
            this.paramsItemBox.Location = new System.Drawing.Point(6, 178);
            this.paramsItemBox.Name = "paramsItemBox";
            this.paramsItemBox.Size = new System.Drawing.Size(156, 82);
            this.paramsItemBox.TabIndex = 5;
            this.paramsItemBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(174, 621);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // textBoxWidth
            // 
            this.textBoxWidth.Location = new System.Drawing.Point(59, 266);
            this.textBoxWidth.Name = "textBoxWidth";
            this.textBoxWidth.Size = new System.Drawing.Size(79, 20);
            this.textBoxWidth.TabIndex = 7;
            this.textBoxWidth.Text = "800";
            this.textBoxWidth.TextChanged += new System.EventHandler(this.textBoxWidth_TextChanged);
            // 
            // textBox4
            // 
            this.textBox4.Enabled = false;
            this.textBox4.Location = new System.Drawing.Point(5, 266);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(48, 20);
            this.textBox4.TabIndex = 8;
            this.textBox4.Text = "Width";
            // 
            // textBox5
            // 
            this.textBox5.Enabled = false;
            this.textBox5.Location = new System.Drawing.Point(6, 292);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(47, 20);
            this.textBox5.TabIndex = 8;
            this.textBox5.Text = "Height";
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Location = new System.Drawing.Point(59, 292);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(79, 20);
            this.textBoxHeight.TabIndex = 7;
            this.textBoxHeight.Text = "600";
            this.textBoxHeight.TextChanged += new System.EventHandler(this.textBoxHeight_TextChanged);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // allElementsBox
            // 
            this.allElementsBox.FormattingEnabled = true;
            this.allElementsBox.Location = new System.Drawing.Point(6, 320);
            this.allElementsBox.Name = "allElementsBox";
            this.allElementsBox.Size = new System.Drawing.Size(156, 264);
            this.allElementsBox.TabIndex = 10;
            this.allElementsBox.SelectedIndexChanged += new System.EventHandler(this.allElementsBox_SelectedIndexChanged);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(6, 590);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(156, 23);
            this.deleteButton.TabIndex = 11;
            this.deleteButton.Text = "Удалить элемент";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(144, 152);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(18, 20);
            this.numericUpDown1.TabIndex = 12;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Visible = false;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // open_button
            // 
            this.open_button.Location = new System.Drawing.Point(6, 79);
            this.open_button.Name = "open_button";
            this.open_button.Size = new System.Drawing.Size(156, 23);
            this.open_button.TabIndex = 13;
            this.open_button.Text = "Открыть";
            this.open_button.UseVisualStyleBackColor = true;
            this.open_button.Click += new System.EventHandler(this.open_button_Click);
            // 
            // widthNumericUpDown
            // 
            this.widthNumericUpDown.Increment = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.widthNumericUpDown.Location = new System.Drawing.Point(144, 266);
            this.widthNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.widthNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.widthNumericUpDown.Name = "widthNumericUpDown";
            this.widthNumericUpDown.Size = new System.Drawing.Size(18, 20);
            this.widthNumericUpDown.TabIndex = 12;
            this.widthNumericUpDown.Value = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.widthNumericUpDown.Visible = false;
            this.widthNumericUpDown.ValueChanged += new System.EventHandler(this.widthNumericUpDown_ValueChanged);
            // 
            // heightNumericUpDown
            // 
            this.heightNumericUpDown.Increment = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.heightNumericUpDown.Location = new System.Drawing.Point(144, 292);
            this.heightNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.heightNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.heightNumericUpDown.Name = "heightNumericUpDown";
            this.heightNumericUpDown.Size = new System.Drawing.Size(18, 20);
            this.heightNumericUpDown.TabIndex = 14;
            this.heightNumericUpDown.Value = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.heightNumericUpDown.Visible = false;
            this.heightNumericUpDown.ValueChanged += new System.EventHandler(this.heightNumericUpDown_ValueChanged);
            // 
            // FormEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 621);
            this.Controls.Add(this.heightNumericUpDown);
            this.Controls.Add(this.open_button);
            this.Controls.Add(this.widthNumericUpDown);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.allElementsBox);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBoxHeight);
            this.Controls.Add(this.textBoxWidth);
            this.Controls.Add(this.paramsItemBox);
            this.Controls.Add(this.textBoxElement);
            this.Controls.Add(this.Сохранить);
            this.Controls.Add(this.Открыть);
            this.Controls.Add(this.directoryTextbox);
            this.Controls.Add(this.items);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter1);
            this.Name = "FormEditor";
            this.Text = "FormEditor";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.widthNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heightNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox items;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox directoryTextbox;
        private System.Windows.Forms.Button Открыть;
        private System.Windows.Forms.Button Сохранить;
        private System.Windows.Forms.TextBox textBoxElement;
        private System.Windows.Forms.ListBox paramsItemBox;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TextBox textBoxWidth;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBoxHeight;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListBox allElementsBox;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button open_button;
        private System.Windows.Forms.NumericUpDown widthNumericUpDown;
        private System.Windows.Forms.NumericUpDown heightNumericUpDown;
    }
}

