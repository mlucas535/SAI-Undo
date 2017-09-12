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

        private string findVersionNumber() //Fix loop to not run infinitely in case incorrect file is loaded.
        {
            byte[] versionSeries = { 0x61, 0x69, 0x20, 0x56, 0x65, 0x72, 0x73, 0x69, 0x6F, 0x6E, 0x20, 0x20, 0x3D, 0x20 }; //represents the text "ai Version  = "
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

                        if (beginLetter == 0x53) //Checks to see if the read byte is the character 'S'
                        {
                            byte[] versionCompare = { };
                            try
                            {
                                versionCompare = reader.ReadBytes(14); //reads the next 14 bytes to see if they match the version number string
                            }
                            catch (EndOfStreamException e)
                            {
                                MessageBox.Show("Error loading SAI!\nDid you make sure to load the .exe file and not the shortcut?", "Error");
                                return "Error";
                            }

                            if (compareVersionText(versionSeries, versionCompare))
                            {
                                byte[] versionNumberArray = reader.ReadBytes(5); //read the next 5 bytes, which are the 5 characters representing the version
                                verNumber = System.Text.Encoding.UTF8.GetString(versionNumberArray); //translate those bytes to a string
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
            catch (IOException e)
            {
                MessageBox.Show("Error accessing file.\nMake sure SAI is not currently running on your computer and try again.", "Error");
                return "Error";
            }
        }

        private int findUndoOffset()
        {
            byte[] undoInstruction = new byte[4];

            /*The following if/else statement stores the proper assembly instructions that come
             * before the location where the undo limit is stored. Basically, if we can find these
             * instructions in the exe, the following 4 bytes in memory represent the undo limit.
             * I found the proper instruction by using Cheat Engine with SAI, and found the location
             * that keeps track of the current number of undos. I then looked at the assembly instructions
             * to find the hexadecimal representation, then searched that in the .exe using HxD to find the
             * following instructions
             * */
            if (versionNumber.Equals("1.1.0")) { //The comparison instruction is different for this version, so 1.1.0 must be handled differently
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

                        if (beginByte == undoInstruction[0]) //check for the first byte in the target instruction
                        {
                            byte[] instructionCompare = { };
                            try
                            {
                                if (versionNumber.Equals("1.1.0")) //read the next x bytes, depending on version loaded
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

                            if (versionNumber.Equals("1.1.0")) //adjust offset accordingly depending on version
                            {
                                offset += 2;
                            }
                            else
                            {
                                offset += 3;
                            }

                            if (compareInstructions(undoInstruction, instructionCompare)) //check to see if we've found the correct location for the instructions
                            {
                                offset++;
                                string sampleString = reader.ReadInt32().ToString(); //read the next integer (4 bytes), which represents the undo limit
                                currentUndoTextBox.Text = sampleString;
                                return offset; //return the offset so we know the location of the undo limit in the .exe
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
            catch(IOException e)
            {
                currentUndoTextBox.Text = "Error";
                return -1;
            }
        }

        private bool compareInstructions(byte[] goalInstruction, byte[] readInstruction) //see if the two parameters are the same instruction
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
                    newUndoNumber = Int32.Parse(newUndoTextBox.Text); //get user input
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
                    writer.Seek(offset, 0); //place the BinaryWriter cursor at the proper offset that was found earlier to change the undo limit
                    writer.Write(updatedUndos);//update the undo limit bytes
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
