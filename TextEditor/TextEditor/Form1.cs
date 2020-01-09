using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class Form1 : Form
    {
        bool fileOpen = false;
        string fileName = "";
        string filePath = "";

        string folderPath = "";

        bool settingsOpen = false;
        public Form1()
        {
            InitializeComponent();
            
            

            var sr = new StreamReader(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"/resources/settings.txt");
            textBox1.Font = new Font(textBox1.Font.FontFamily, float.Parse(sr.ReadLine()));
            Color textColor = Color.FromName(sr.ReadLine());
            textBox1.ForeColor = textColor;
            menuStrip1.ForeColor = Color.Black;
            menuStrip1.BackColor = Color.Gray;
            textBox1.BackColor = Color.FromArgb(01, 00, 128);
            this.BackColor = Color.FromArgb(01, 00, 128);
            menuStrip1.Renderer = new myRenderer();
            var text = sr.ReadLine();
            switch(text)
            {
                case "LEFT":
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    break;
                case "RIGHT":
                    textBox1.TextAlign = HorizontalAlignment.Right;
                    break;
                case "CENTER":
                    textBox1.TextAlign = HorizontalAlignment.Center;
                    break;
            }
            sr.Dispose();
            
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    folderPath = folderBrowserDialog1.SelectedPath;
                    System.IO.StreamWriter writer = new System.IO.StreamWriter(folderPath + @"\" + toolStripTextBox1.Text + ".txt");
                    writer.Write(textBox1.Text);
                    writer.Close();
                    writer.Dispose();

                    var sr = new StreamReader(folderPath + @"\" + toolStripTextBox1.Text + ".txt");
                    textBox1.Text = sr.ReadToEnd();
                    this.Text = "BEITEN " + folderPath + @"\" + toolStripTextBox1.Text + ".txt";
                    fileOpen = true;
                    fileName = folderPath + @"\" + toolStripTextBox1.Text + ".txt";
                    sr.Dispose();

                } catch(Exception ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void SetText(string v)
        {
            throw new NotImplementedException();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var sr = new StreamReader(openFileDialog1.FileName);
                    textBox1.Text = sr.ReadToEnd();
                    this.Text = "BEITEN " + openFileDialog1.FileName;
                    fileOpen = true;
                    fileName = openFileDialog1.FileName;
                    sr.Dispose();
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(fileOpen)
            {
                try
                {
                    System.IO.StreamWriter writer = new System.IO.StreamWriter(fileName); //open the file for writing.
                    writer.Write(textBox1.Text);
                    writer.Close();
                    writer.Dispose();
                } catch(Exception ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void changeSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            settingsOpen = true;
            saveToolStripMenuItem_Click(sender, e);
            textBox1.Text = @"Font:" + Environment.NewLine + textBox1.Font.Size + Environment.NewLine + "Color:" + Environment.NewLine + textBox1.ForeColor.Name + Environment.NewLine + "Align:" + Environment.NewLine + textBox1.TextAlign;
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(settingsOpen)
            {
                string size = textBox1.Lines[1];
                string color = textBox1.Lines[3];
                string text = textBox1.Lines[5];
                

                try
                {
                    switch (text)
                    {
                        case "LEFT":
                            textBox1.TextAlign = HorizontalAlignment.Left;
                            break;
                        case "RIGHT":
                            textBox1.TextAlign = HorizontalAlignment.Right;
                            break;
                        case "CENTER":
                            textBox1.TextAlign = HorizontalAlignment.Center;
                            break;
                    }
                    textBox1.Font = new Font(textBox1.Font.FontFamily, float.Parse(size));
                    textBox1.ForeColor = Color.FromName(color);
                    settingsOpen = false;
                    if(fileOpen)
                    {
                        var sr = new StreamReader(fileName);
                        textBox1.Text = sr.ReadToEnd();
                        this.Text = "BEITEN " + fileName;
                        fileOpen = true;
                        sr.Dispose();
                    }

                    System.IO.StreamWriter writer = new StreamWriter(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"/resources/settings.txt");
                    writer.Write(textBox1.Lines[1] + Environment.NewLine + textBox1.Lines[3] + Environment.NewLine + textBox1.Lines[5] + Environment.NewLine);
                    writer.Close();
                    writer.Dispose();

                } catch (Exception ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text =
            "//////////////////////////////////////////////////////////////////////////////////////////" + Environment.NewLine +
            "//////////////////////////////////////////////////////////////////////////////////////////" + Environment.NewLine +
            "///BEITEN v.1.1.0                                                                      ///" + Environment.NewLine +
            "///                                                                                    ///" + Environment.NewLine +
            "///                                  --UNTITLED DOCUMENT--                             ///" + Environment.NewLine +
            "///                                                                                    ///" + Environment.NewLine +
            "///                                                                                    ///" + Environment.NewLine +
            "///BY JAKE BUTFILOSKI                                                                  ///" + Environment.NewLine +
            "//////////////////////////////////////////////////////////////////////////////////////////" + Environment.NewLine +
            "//////////////////////////////////////////////////////////////////////////////////////////" + Environment.NewLine;
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            label1.Text = "CHARACTERS: " + textBox1.Text.Count();
            label2.Text = "WORD COUNT: " + CountWords(textBox1.Text);
        }

        public static int CountWords(string a)
        {
            int count = 0;
            bool inWord = false;

            foreach (char t in a)
            {
                if (char.IsWhiteSpace(t))
                {
                    inWord = false;
                }
                else
                {
                    if (!inWord) count++;
                    inWord = true;
                }
            }
            return count;
        }

        private void DEFAULTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sr = new StreamReader(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"/resources/default.txt");
            textBox1.Font = new Font(textBox1.Font.FontFamily, float.Parse(sr.ReadLine()));
            Color textColor = Color.FromName(sr.ReadLine());
            textBox1.ForeColor = textColor;
            menuStrip1.ForeColor = Color.Black;
            menuStrip1.BackColor = Color.Gray;
            textBox1.BackColor = Color.FromArgb(01, 00, 128);
            this.BackColor = Color.FromArgb(01, 00, 128);
            menuStrip1.Renderer = new myRenderer();
            var text = sr.ReadLine();
            switch (text)
            {
                case "LEFT":
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    break;
                case "RIGHT":
                    textBox1.TextAlign = HorizontalAlignment.Right;
                    break;
                case "CENTER":
                    textBox1.TextAlign = HorizontalAlignment.Center;
                    break;
            }
        }
    }
    class myRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs myMenu)
        {
            if (!myMenu.Item.Selected)
                base.OnRenderMenuItemBackground(myMenu);

            else
            {
                Rectangle menuRectangle = new Rectangle(Point.Empty, myMenu.Item.Size);

                //Fill Color
                myMenu.Graphics.FillRectangle(Brushes.Gray, menuRectangle);

                // Border Color
                myMenu.Graphics.DrawRectangle(Pens.Gray, 1, 0, menuRectangle.Width - 2, menuRectangle.Height - 1);
            }
        }
    }
}
