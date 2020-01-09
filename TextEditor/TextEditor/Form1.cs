using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class Form1 : Form
    {
        //Caret Variables
        [DllImport("user32.dll")]
        static extern bool CreateCaret(IntPtr hWnd, IntPtr hBitmap, int nWidth, int nHeight);
        [DllImport("user32.dll")]
        static extern bool ShowCaret(IntPtr hWnd);

        //Is a file currently open?
        bool fileOpen = false;
        //Current open file's path
        string fileName = "";
        //Path of the folder
        string folderPath = "";
        //Is the settings currently open?
        bool settingsOpen = false;
        //Private Font Collection to allow form to display font without user needing to install
        PrivateFontCollection pfc = new PrivateFontCollection();
        public Form1()
        {
            InitializeComponent();
            
            
            //Read settings to set style
            var sr = new StreamReader(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"/resources/settings.txt");
            //Set a fontsize variable for later from the settings file
            var fontSize = float.Parse(sr.ReadLine());
            //Set a Color variable for text color
            Color textColor = Color.FromName(sr.ReadLine());
            //Assign previous color variable to textbox font
            textBox1.ForeColor = textColor;
            //Change the menu text color
            menuStrip1.ForeColor = Color.Black;
            //Assign the menu background color
            menuStrip1.BackColor = Color.Gray;
            //Assign ARGB color for the main background
            textBox1.BackColor = Color.FromArgb(01, 00, 128);
            this.BackColor = Color.FromArgb(01, 00, 128);
            //Custom renderer used for selection color
            menuStrip1.Renderer = new myRenderer();
            //Used for alignment
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
            //Dispose stream leader
            sr.Dispose();
            //Add embedded file to private font family
            pfc.AddFontFile(Path.GetDirectoryName(Application.ExecutablePath) + @"\Nouveau_IBM_Stretch.ttf");

            //Apply custom font to controls (probably sloppy dont know any alternatives)
            textBox1.Font = new Font(pfc.Families[0], fontSize, FontStyle.Regular);
            fileToolStripMenuItem.Font = new Font(pfc.Families[0], 12, FontStyle.Regular);
            settingsToolStripMenuItem.Font = new Font(pfc.Families[0], 12, FontStyle.Regular);
            newToolStripMenuItem.Font = new Font(pfc.Families[0], 12, FontStyle.Regular);
            saveAsToolStripMenuItem.Font = new Font(pfc.Families[0], 12, FontStyle.Regular);
            saveToolStripMenuItem.Font = new Font(pfc.Families[0], 12, FontStyle.Regular);
            openToolStripMenuItem.Font = new Font(pfc.Families[0], 12, FontStyle.Regular);
            changeSettingsToolStripMenuItem.Font = new Font(pfc.Families[0], 12, FontStyle.Regular);
            saveToolStripMenuItem1.Font = new Font(pfc.Families[0], 12, FontStyle.Regular);
            dEFAULTToolStripMenuItem.Font = new Font(pfc.Families[0], 12, FontStyle.Regular);
            label1.Font = new Font(pfc.Families[0], 12, FontStyle.Regular);
            label2.Font = new Font(pfc.Families[0], 12, FontStyle.Regular);

            

        }

        /// <summary>
        /// Triggered when "Save As" is clicked
        /// Intended to allow for user to select save location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Pretty simple, just using a folderBrowserDialog to browse for a location, then saving the file there
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

        /// <summary>
        /// Triggered when "Open" is clicked
        /// Intention is to allow users to browse for and open text files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Uses an openFileDialog control to navigate to a file
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //Search for file
                    var sr = new StreamReader(openFileDialog1.FileName);
                    //Assign text to the textbox
                    textBox1.Text = sr.ReadToEnd();
                    //Add the path to the title
                    this.Text = "BEITEN " + openFileDialog1.FileName;
                    //Let the program know a file is open currently
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

        /// <summary>
        /// Triggered when "Save" (from File option) is clicked
        /// Intention is to allow document to be saved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Simply writing to a previously opened txt file
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

        /// <summary>
        /// Triggered when "Change Settings" is clicked
        /// Intention is to allow user to customize the editor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Let the program know the settings are currently open
            settingsOpen = true;
            saveToolStripMenuItem_Click(sender, e);
            textBox1.Text = @"Font:" + Environment.NewLine + textBox1.Font.Size + Environment.NewLine + "Color:" + Environment.NewLine + textBox1.ForeColor.Name + Environment.NewLine + "Align:" + Environment.NewLine + textBox1.TextAlign;
        }
        /// <summary>
        /// Triggered when "Save"(In settings option) is clicked
        /// Intention is for the user to save their custom settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Check if settings is open
            if(settingsOpen)
            {
                //Assign variables based on the settings
                string size = textBox1.Lines[1];
                string color = textBox1.Lines[3];
                string text = textBox1.Lines[5];
                

                try
                {
                    //Edit alignment
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
                    //Edit font size
                    textBox1.Font = new Font(pfc.Families[0], float.Parse(size));
                    //Edit font color
                    textBox1.ForeColor = Color.FromName(color);
                    //Let the program know settings is no longer open
                    settingsOpen = false;
                    //Check if a file is currently active and then go back to where the user was
                    if(fileOpen)
                    {
                        var sr = new StreamReader(fileName);
                        textBox1.Text = sr.ReadToEnd();
                        this.Text = "BEITEN " + fileName;
                        fileOpen = true;
                        sr.Dispose();
                    }
                    //Write user settings to its file
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
        /// <summary>
        /// Triggered when "New" is clicked
        /// Intention is to give the user something to start off with when creating a document.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text =
            "//////////////////////////////////////////////////////////////////////////////////////////" + Environment.NewLine +
            "//////////////////////////////////////////////////////////////////////////////////////////" + Environment.NewLine +
            "///BEITEN v.1.3.2                                                                      ///" + Environment.NewLine +
            "///                                                                                    ///" + Environment.NewLine +
            "///                                  --UNTITLED DOC--                                  ///" + Environment.NewLine +
            "///                                                                                    ///" + Environment.NewLine +
            "///                                                                                    ///" + Environment.NewLine +
            "///BY JAKE BUTFILOSKI                                                                  ///" + Environment.NewLine +
            "//////////////////////////////////////////////////////////////////////////////////////////" + Environment.NewLine +
            "//////////////////////////////////////////////////////////////////////////////////////////" + Environment.NewLine;
        }
        /// <summary>
        /// Updates the word count and character count whenever a change is made in the textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            label1.Text = "CHARACTERS: " + textBox1.Text.Count();
            label2.Text = "WORD COUNT: " + CountWords(textBox1.Text);
        }

        /// <summary>
        /// Counts words for previous method
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Triggered when "Default" is clicked
        /// Intention is to restore user settings to how it was shipped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DEFAULTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sr = new StreamReader(System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"/resources/default.txt");
            textBox1.Font = new Font(pfc.Families[0], float.Parse(sr.ReadLine()));
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

        /// <summary>
        /// Used to create the caret
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Shown(object sender, EventArgs e)
        {
            CreateCaret(textBox1.Handle, IntPtr.Zero, 11, textBox1.Font.Height);
            ShowCaret(textBox1.Handle);
            
        }
    }
    /// <summary>
    /// Soley for changing the color of selection
    /// </summary>
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
