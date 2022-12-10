namespace TapChat
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.entryBox = new System.Windows.Forms.GroupBox();
            this.enterBtn = new System.Windows.Forms.Button();
            this.chatBox = new System.Windows.Forms.GroupBox();
            this.chatUsers = new System.Windows.Forms.ListBox();
            this.field = new System.Windows.Forms.GroupBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.entryBox.SuspendLayout();
            this.chatBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(25, 48);
            this.textBox1.Name = "textBox1";
            this.textBox1.PlaceholderText = "Username";
            this.textBox1.Size = new System.Drawing.Size(610, 31);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // entryBox
            // 
            this.entryBox.Controls.Add(this.textBox1);
            this.entryBox.Controls.Add(this.enterBtn);
            this.entryBox.Location = new System.Drawing.Point(12, 12);
            this.entryBox.Name = "entryBox";
            this.entryBox.Size = new System.Drawing.Size(709, 301);
            this.entryBox.TabIndex = 1;
            this.entryBox.TabStop = false;
            // 
            // enterBtn
            // 
            this.enterBtn.Location = new System.Drawing.Point(25, 109);
            this.enterBtn.Name = "enterBtn";
            this.enterBtn.Size = new System.Drawing.Size(285, 34);
            this.enterBtn.TabIndex = 1;
            this.enterBtn.Text = "Enter chat";
            this.enterBtn.UseVisualStyleBackColor = true;
            this.enterBtn.Click += new System.EventHandler(this.enterBtn_Click);
            // 
            // chatBox
            // 
            this.chatBox.Controls.Add(this.chatUsers);
            this.chatBox.Controls.Add(this.field);
            this.chatBox.Enabled = false;
            this.chatBox.Location = new System.Drawing.Point(14, 18);
            this.chatBox.Name = "chatBox";
            this.chatBox.Size = new System.Drawing.Size(755, 403);
            this.chatBox.TabIndex = 2;
            this.chatBox.TabStop = false;
            this.chatBox.Visible = false;
            // 
            // chatUsers
            // 
            this.chatUsers.FormattingEnabled = true;
            this.chatUsers.ItemHeight = 25;
            this.chatUsers.Location = new System.Drawing.Point(4, 8);
            this.chatUsers.Name = "chatUsers";
            this.chatUsers.Size = new System.Drawing.Size(128, 379);
            this.chatUsers.TabIndex = 0;
            // 
            // field
            // 
            this.field.Location = new System.Drawing.Point(147, 8);
            this.field.Name = "field";
            this.field.Size = new System.Drawing.Size(602, 379);
            this.field.TabIndex = 1;
            this.field.TabStop = false;
            this.field.Click += new System.EventHandler(this.OnFieldClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chatBox);
            this.Controls.Add(this.entryBox);
            this.Name = "Form1";
            this.Text = "TapChat";
            this.entryBox.ResumeLayout(false);
            this.entryBox.PerformLayout();
            this.chatBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TextBox textBox1;
        private GroupBox entryBox;
        private Button enterBtn;
        private GroupBox chatBox;
        private ListBox chatUsers;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private GroupBox field;
    }
}