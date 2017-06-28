using System;

namespace LiveSplit.FFX
{
    partial class FFXSettings
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbMain = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelCustomSettings = new System.Windows.Forms.Label();
            this.listView = new System.Windows.Forms.ListView();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.btnUncheckAll = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelOptions = new System.Windows.Forms.Label();
            this.checkboxStart = new System.Windows.Forms.CheckBox();
            this.checkboxSplit = new System.Windows.Forms.CheckBox();
            this.checkboxReset = new System.Windows.Forms.CheckBox();
            this.checkboxRemoveLoads = new System.Windows.Forms.CheckBox();
            this.gbMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMain
            // 
            this.gbMain.Controls.Add(this.panel1);
            this.gbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbMain.Location = new System.Drawing.Point(0, 0);
            this.gbMain.Name = "gbMain";
            this.gbMain.Size = new System.Drawing.Size(476, 304);
            this.gbMain.TabIndex = 1;
            this.gbMain.TabStop = false;
            this.gbMain.Text = "Auto-Split Settings";
            this.gbMain.Validated += new System.EventHandler(this.ConfirmSplits);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Controls.Add(this.flowLayoutPanel2);
            this.panel1.Location = new System.Drawing.Point(6, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(468, 283);
            this.panel1.TabIndex = 23;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelCustomSettings);
            this.panel2.Controls.Add(this.listView);
            this.panel2.Location = new System.Drawing.Point(3, 29);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(467, 214);
            this.panel2.TabIndex = 23;
            // 
            // labelCustomSettings
            // 
            this.labelCustomSettings.AutoSize = true;
            this.labelCustomSettings.Location = new System.Drawing.Point(3, 5);
            this.labelCustomSettings.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.labelCustomSettings.Name = "labelCustomSettings";
            this.labelCustomSettings.Size = new System.Drawing.Size(35, 13);
            this.labelCustomSettings.TabIndex = 18;
            this.labelCustomSettings.Text = "Splits:";
            // 
            // listView
            // 
            this.listView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listView.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.listView.AutoArrange = false;
            this.listView.CheckBoxes = true;
            this.listView.LabelWrap = false;
            this.listView.Location = new System.Drawing.Point(38, 1);
            this.listView.Margin = new System.Windows.Forms.Padding(0);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(427, 213);
            this.listView.TabIndex = 21;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.List;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.btnCheckAll);
            this.flowLayoutPanel1.Controls.Add(this.btnUncheckAll);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(316, 249);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(149, 30);
            this.flowLayoutPanel1.TabIndex = 20;
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Location = new System.Drawing.Point(3, 3);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(62, 23);
            this.btnCheckAll.TabIndex = 5;
            this.btnCheckAll.Text = "Check All";
            this.btnCheckAll.UseVisualStyleBackColor = true;
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // btnUncheckAll
            // 
            this.btnUncheckAll.AutoSize = true;
            this.btnUncheckAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnUncheckAll.Location = new System.Drawing.Point(71, 3);
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Size = new System.Drawing.Size(75, 23);
            this.btnUncheckAll.TabIndex = 6;
            this.btnUncheckAll.Text = "Uncheck All";
            this.btnUncheckAll.UseVisualStyleBackColor = true;
            this.btnUncheckAll.Click += new System.EventHandler(this.btnUncheckAll_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.labelOptions);
            this.flowLayoutPanel2.Controls.Add(this.checkboxStart);
            this.flowLayoutPanel2.Controls.Add(this.checkboxSplit);
            this.flowLayoutPanel2.Controls.Add(this.checkboxReset);
            this.flowLayoutPanel2.Controls.Add(this.checkboxRemoveLoads);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(465, 23);
            this.flowLayoutPanel2.TabIndex = 22;
            // 
            // labelOptions
            // 
            this.labelOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelOptions.AutoSize = true;
            this.labelOptions.Location = new System.Drawing.Point(3, 5);
            this.labelOptions.Name = "labelOptions";
            this.labelOptions.Size = new System.Drawing.Size(46, 13);
            this.labelOptions.TabIndex = 13;
            this.labelOptions.Text = "Options:";
            // 
            // checkboxStart
            // 
            this.checkboxStart.Checked = true;
            this.checkboxStart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkboxStart.Location = new System.Drawing.Point(55, 3);
            this.checkboxStart.Name = "checkboxStart";
            this.checkboxStart.Size = new System.Drawing.Size(48, 17);
            this.checkboxStart.TabIndex = 12;
            this.checkboxStart.Text = "Start";
            this.checkboxStart.UseVisualStyleBackColor = true;
            // 
            // checkboxSplit
            // 
            this.checkboxSplit.Checked = true;
            this.checkboxSplit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkboxSplit.Location = new System.Drawing.Point(109, 3);
            this.checkboxSplit.Name = "checkboxSplit";
            this.checkboxSplit.Size = new System.Drawing.Size(46, 17);
            this.checkboxSplit.TabIndex = 14;
            this.checkboxSplit.Text = "Split";
            this.checkboxSplit.UseVisualStyleBackColor = true;
            // 
            // checkboxReset
            // 
            this.checkboxReset.Checked = true;
            this.checkboxReset.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkboxReset.Location = new System.Drawing.Point(161, 3);
            this.checkboxReset.Name = "checkboxReset";
            this.checkboxReset.Size = new System.Drawing.Size(54, 17);
            this.checkboxReset.TabIndex = 15;
            this.checkboxReset.Text = "Reset";
            this.checkboxReset.UseVisualStyleBackColor = true;
            // 
            // checkboxRemoveLoads
            // 
            this.checkboxRemoveLoads.AutoSize = true;
            this.checkboxRemoveLoads.Checked = true;
            this.checkboxRemoveLoads.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkboxRemoveLoads.Location = new System.Drawing.Point(221, 3);
            this.checkboxRemoveLoads.Name = "checkboxRemoveLoads";
            this.checkboxRemoveLoads.Size = new System.Drawing.Size(98, 17);
            this.checkboxRemoveLoads.TabIndex = 16;
            this.checkboxRemoveLoads.Text = "Remove Loads";
            this.checkboxRemoveLoads.UseVisualStyleBackColor = true;
            // 
            // FFXSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbMain);
            this.Name = "FFXSettings";
            this.Size = new System.Drawing.Size(476, 304);
            this.gbMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMain;
        private System.Windows.Forms.CheckBox checkboxRemoveLoads;
        private System.Windows.Forms.CheckBox checkboxReset;
        private System.Windows.Forms.CheckBox checkboxSplit;
        private System.Windows.Forms.Label labelOptions;
        private System.Windows.Forms.CheckBox checkboxStart;
        private System.Windows.Forms.Label labelCustomSettings;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnCheckAll;
        private System.Windows.Forms.Button btnUncheckAll;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.ListView listView;
    }
}
