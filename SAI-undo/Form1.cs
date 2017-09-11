using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace SAI_undo
{
    public partial class Form1 : Form
    {
        private string filePath;
        private string versionNumber;
        private int offset;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog selectSAIDialog = new OpenFileDialog();
            

            selectSAIDialog.ShowDialog();
            filePath = selectSAIDialog.FileName;
            //textBox1.Text = filePath;

            versionNumber = findVersionNumber();
            versionTextBox.Text = versionNumber;
            offset = findUndoOffset();
        }

        private bool compareVersionText(byte[] goalArray, byte[] readData)
        {
            for(int i = 0; i < 14; i++)
            {
                if(goalArray[i] != readData[i])
                {
                    return false;
                }
            }
            return true;
        }

        private string findVersionNumber() //Fix loop to not run infinitely in case incorrect file is loaded
        {
            byte[] versionSeries = { 0x61, 0x69, 0x20, 0x56, 0x65, 0x72, 0x73, 0x69, 0x6F, 0x6E, 0x20, 0x20, 0x3D, 0x20 };
            string verNumber = "";

            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {
                    byte beginLetter;
                    int offset = 0;
                    do
                    {
                        try { beginLetter = reader.ReadByte(); }
                        catch (EndOfStreamException e)
                        {
                            MessageBox.Show("Error loading SAI!\nDid you make sure to load the .exe file and not the shortcut?", "Error");
                            return "Error";
                        }

                        if (beginLetter == 0x53)
                        {
                            byte[] versionCompare = { };
                            try
                            {
                                versionCompare = reader.ReadBytes(14);
                            }
                            catch (EndOfStreamException e)
                            {
                                MessageBox.Show("Error loading SAI!\nDid you make sure to load the .exe file and not the shortcut?", "Error");
                                return "Error";
                            }

                            if (compareVersionText(versionSeries, versionCompare))
                            {
                                byte[] versionNumberArray = reader.ReadBytes(5);
                                verNumber = System.Text.Encoding.UTF8.GetString(versionNumberArray);
                                versionTextBox.Text = verNumber;
                                return verNumber;
                            }
                        }
                        offset++;
                    } while (true);
                }
            }
            catch(UnauthorizedAccessException e)
            {
                MessageBox.Show("Error accessing file.\nYou may have to re-run this program as an administrator.", "Error");
                return "Error";
            }
        }

        private int findUndoOffset()
        {
            byte[] undoInstruction = new byte[4];

            if (versionNumber.Equals("1.1.0")) {
                undoInstruction[0] = 0x74;
                undoInstruction[1] = 0x3C;
                undoInstruction[2] = 0xBB;
            }else
            {
                undoInstruction[0] = 0xFF;
                undoInstruction[1] = 0x40;
                undoInstruction[2] = 0x08;
                undoInstruction[3] = 0xBF;
            }

            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {
                    byte beginByte;
                    int offset = 0;
                    do
                    {
                        try
                        {
                            beginByte = reader.ReadByte();
                        }
                        catch (EndOfStreamException e)
                        {
                            currentUndoTextBox.Text = "Error";
                            return -1;
                        }

                        if (beginByte == undoInstruction[0])
                        {
                            byte[] instructionCompare = { };
                            try
                            {
                                if (versionNumber.Equals("1.1.0"))
                                {
                                    instructionCompare = reader.ReadBytes(2);
                                }
                                else
                                {
                                    instructionCompare = reader.ReadBytes(3);
                                }
                                
                            }
                            catch (EndOfStreamException e)
                            {
                                currentUndoTextBox.Text = "Error";
                                return -1;
                            }

                            if (versionNumber.Equals("1.1.0"))
                            {
                                offset += 2;
                            }
                            else
                            {
                                offset += 3;
                            }

                            if (compareInstructions(undoInstruction, instructionCompare))
                            {
                                offset++;
                                string sampleString = reader.ReadInt32().ToString();
                                currentUndoTextBox.Text = sampleString;
                                return offset;
                            }
                        }
                        offset++;
                    } while (true);
                }
            }
            catch(UnauthorizedAccessException e)
            {
                currentUndoTextBox.Text = "Error";
                return -1;
            }
        }

        private bool compareInstructions(byte[] goalInstruction, byte[] readInstruction)
        {
            int length;
            if (versionNumber.Equals("1.1.0"))
            {
                length = 2;
            }
            else
            {
                length = 3;
            }

            for (int i = 0; i < length; i++)
            {
                if (goalInstruction[i + 1] != readInstruction[i])
                {
                    return false;
                }
            }
            return true;
        }

        private void displayCurrentUndoSetting()
        {

        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (versionTextBox.Text.Equals("Error"))
            {
                MessageBox.Show("Please load a valid copy of SAI before continuing.", "Error");
            }else if(versionTextBox.Text.Equals(""))
            {
                MessageBox.Show("Please load SAI using the 'Open' button before continuing.", "Error");
            }
            else
            {
                int newUndoNumber = 0;
                try
                {
                    newUndoNumber = Int32.Parse(newUndoTextBox.Text);
                }catch(FormatException ex)
                {
                    MessageBox.Show("Please enter a valid number", "Error");
                    return;
                }

                if(newUndoNumber >= 1 && newUndoNumber <= 5000)
                    updateUndos(offset);
                else
                    MessageBox.Show("Please input a number between 1 and 5000", "Error");
            }
        }

        private void updateUndos(int offset)
        {
            try
            {
                using (BinaryWriter writer = new BinaryWriter(new FileStream(filePath, FileMode.Open, FileAccess.Write)))
                {
                    int updatedUndos = Int32.Parse(newUndoTextBox.Text);
                    writer.Seek(offset, 0);
                    writer.Write(updatedUndos);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Error writing to file.", "Error");
            }

            findUndoOffset();
        }
    }
}
