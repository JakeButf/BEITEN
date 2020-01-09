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
            
            menuStrip1.ForeColor = Color.White;

            var sr = new StreamReader(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"/settings.txt");
            textBox1.Font = new Font(textBox1.Font.FontFamily, float.Parse(sr.ReadLine()));
            textBox1.ForeColor = Color.FromName(sr.ReadLine());
            var text = sr.ReadLine();
            switch(text)
            {
                case "Left":
                    textBox1.TextAlign = HorizontalAlignment.Left;
                    break;
                case "Right":
                    textBox1.TextAlign = HorizontalAlignment.Right;
                    break;
                case "Center":
                    textBox1.TextAlign = HorizontalAlignment.Center;
                    break;
            }
            
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
                        case "Left":
                            textBox1.TextAlign = HorizontalAlignment.Left;
                            break;
                        case "Right":
                            textBox1.TextAlign = HorizontalAlignment.Right;
                            break;
                        case "Center":
                            textBox1.TextAlign = HorizontalAlignment.Center;
                            break;
                    }
                    textBox1.Font = new Font(textBox1.Font.FontFamily, float.Parse(size));
                    textBox1.ForeColor = Color.FromName(color);
                    settingsOpen = false;
                    var sr = new StreamReader(fileName);
                    textBox1.Text = sr.ReadToEnd();
                    this.Text = "BEITEN " + fileName;
                    fileOpen = true;
                    sr.Dispose();
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
            "///BEITEN v.1.0.0                                                                      ///" + Environment.NewLine +
            "///                                                                                    ///" + Environment.NewLine +
            "///                                  --UNTITLED DOCUMENT--                             ///" + Environment.NewLine +
            "///                                                                                    ///" + Environment.NewLine +
            "///                                                                                    ///" + Environment.NewLine +
            "///BY JAKE BUTFILOSKI                                                                  ///" + Environment.NewLine +
            "//////////////////////////////////////////////////////////////////////////////////////////" + Environment.NewLine +
            "//////////////////////////////////////////////////////////////////////////////////////////" + Environment.NewLine;
        }
    }
}
