namespace SAI_undo
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.openFileButton = new System.Windows.Forms.Button();
            this.versionTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.currentUndoTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.newUndoTextBox = new System.Windows.Forms.TextBox();
            this.updateButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // openFileButton
            // 
            this.openFileButton.Location = new System.Drawing.Point(58, 278);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(171, 78);
            this.openFileButton.TabIndex = 0;
            this.openFileButton.Text = "Open";
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // versionTextBox
            // 
            this.versionTextBox.Location = new System.Drawing.Point(58, 94);
            this.versionTextBox.Name = "versionTextBox";
            this.versionTextBox.ReadOnly = true;
            this.versionTextBox.Size = new System.Drawing.Size(144, 31);
            this.versionTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "SAI Version:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(204, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Current Max Undos:";
            // 
            // currentUndoTextBox
            // 
            this.currentUndoTextBox.Location = new System.Drawing.Point(58, 205);
            this.currentUndoTextBox.Name = "currentUndoTextBox";
            this.currentUndoTextBox.ReadOnly = true;
            this.currentUndoTextBox.Size = new System.Drawing.Size(100, 31);
            this.currentUndoTextBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(369, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(235, 25);
            this.label3.TabIndex = 5;
            this.label3.Text = "New Amount Of Undos:";
            // 
            // newUndoTextBox
            // 
            this.newUndoTextBox.Location = new System.Drawing.Point(374, 94);
            this.newUndoTextBox.Name = "newUndoTextBox";
            this.newUndoTextBox.Size = new System.Drawing.Size(100, 31);
            this.newUndoTextBox.TabIndex = 6;
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(374, 278);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(163, 78);
            this.updateButton.TabIndex = 7;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(75, 387);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(448, 25);
            this.label4.TabIndex = 8;
            this.label4.Text = "Program written by @Best_Dude55 on Twitter";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 421);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.newUndoTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.currentUndoTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.versionTextBox);
            this.Controls.Add(this.openFileButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "SAI Undo Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.TextBox versionTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox currentUndoTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox newUndoTextBox;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.Label label4;
    }
}

