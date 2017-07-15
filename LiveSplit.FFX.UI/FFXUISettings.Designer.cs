﻿namespace LiveSplit.FFX.UI
{
    partial class FFXUISettings
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblBackgroundColor = new System.Windows.Forms.Label();
            this.btnBackgroundColor1 = new System.Windows.Forms.Button();
            this.btnBackgroundColor2 = new System.Windows.Forms.Button();
            this.cmbGradientType = new System.Windows.Forms.ComboBox();
            this.gbText = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cbOverrideTextColor = new System.Windows.Forms.CheckBox();
            this.btnTextColor = new System.Windows.Forms.Button();
            this.lblTextColor = new System.Windows.Forms.Label();
            this.gbDisplayValue = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.cmbValue = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.lblValue = new System.Windows.Forms.Label();
            this.lblDisclaimer = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbText.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.gbDisplayValue.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 159F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblBackgroundColor, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnBackgroundColor1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnBackgroundColor2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbGradientType, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.gbText, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.gbDisplayValue, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 83F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(470, 372);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblBackgroundColor
            // 
            this.lblBackgroundColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBackgroundColor.AutoSize = true;
            this.lblBackgroundColor.Location = new System.Drawing.Point(3, 8);
            this.lblBackgroundColor.Name = "lblBackgroundColor";
            this.lblBackgroundColor.Size = new System.Drawing.Size(153, 13);
            this.lblBackgroundColor.TabIndex = 0;
            this.lblBackgroundColor.Text = "Background Color:";
            // 
            // btnBackgroundColor1
            // 
            this.btnBackgroundColor1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBackgroundColor1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBackgroundColor1.Location = new System.Drawing.Point(162, 3);
            this.btnBackgroundColor1.Name = "btnBackgroundColor1";
            this.btnBackgroundColor1.Size = new System.Drawing.Size(23, 23);
            this.btnBackgroundColor1.TabIndex = 1;
            this.btnBackgroundColor1.UseVisualStyleBackColor = true;
            this.btnBackgroundColor1.Click += new System.EventHandler(this.ColorButtonClick);
            // 
            // btnBackgroundColor2
            // 
            this.btnBackgroundColor2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBackgroundColor2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBackgroundColor2.Location = new System.Drawing.Point(191, 3);
            this.btnBackgroundColor2.Name = "btnBackgroundColor2";
            this.btnBackgroundColor2.Size = new System.Drawing.Size(23, 23);
            this.btnBackgroundColor2.TabIndex = 1;
            this.btnBackgroundColor2.UseVisualStyleBackColor = true;
            this.btnBackgroundColor2.Click += new System.EventHandler(this.ColorButtonClick);
            // 
            // cmbGradientType
            // 
            this.cmbGradientType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbGradientType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGradientType.FormattingEnabled = true;
            this.cmbGradientType.Items.AddRange(new object[] {
            "Plain",
            "Vertical",
            "Horizontal"});
            this.cmbGradientType.Location = new System.Drawing.Point(220, 4);
            this.cmbGradientType.Name = "cmbGradientType";
            this.cmbGradientType.Size = new System.Drawing.Size(247, 21);
            this.cmbGradientType.TabIndex = 2;
            this.cmbGradientType.SelectedIndexChanged += new System.EventHandler(this.cmbGradientType_SelectedIndexChanged);
            // 
            // gbText
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.gbText, 4);
            this.gbText.Controls.Add(this.tableLayoutPanel2);
            this.gbText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbText.Location = new System.Drawing.Point(3, 32);
            this.gbText.Name = "gbText";
            this.gbText.Size = new System.Drawing.Size(464, 77);
            this.gbText.TabIndex = 3;
            this.gbText.TabStop = false;
            this.gbText.Text = "Text";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 153F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 158F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 147F));
            this.tableLayoutPanel2.Controls.Add(this.cbOverrideTextColor, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnTextColor, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblTextColor, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(458, 58);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // cbOverrideTextColor
            // 
            this.cbOverrideTextColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbOverrideTextColor.AutoSize = true;
            this.cbOverrideTextColor.Location = new System.Drawing.Point(3, 6);
            this.cbOverrideTextColor.Name = "cbOverrideTextColor";
            this.cbOverrideTextColor.Size = new System.Drawing.Size(147, 17);
            this.cbOverrideTextColor.TabIndex = 0;
            this.cbOverrideTextColor.Text = "Override Layout Settings";
            this.cbOverrideTextColor.UseVisualStyleBackColor = true;
            this.cbOverrideTextColor.CheckedChanged += new System.EventHandler(this.cbOverrideTextColor_CheckedChanged);
            // 
            // btnTextColor
            // 
            this.btnTextColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTextColor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTextColor.Location = new System.Drawing.Point(156, 32);
            this.btnTextColor.Name = "btnTextColor";
            this.btnTextColor.Size = new System.Drawing.Size(23, 23);
            this.btnTextColor.TabIndex = 1;
            this.btnTextColor.UseVisualStyleBackColor = true;
            this.btnTextColor.Click += new System.EventHandler(this.ColorButtonClick);
            // 
            // lblTextColor
            // 
            this.lblTextColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTextColor.AutoSize = true;
            this.lblTextColor.Location = new System.Drawing.Point(3, 37);
            this.lblTextColor.Name = "lblTextColor";
            this.lblTextColor.Size = new System.Drawing.Size(147, 13);
            this.lblTextColor.TabIndex = 1;
            this.lblTextColor.Text = "Color:";
            // 
            // gbDisplayValue
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.gbDisplayValue, 4);
            this.gbDisplayValue.Controls.Add(this.tableLayoutPanel3);
            this.gbDisplayValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDisplayValue.Location = new System.Drawing.Point(3, 135);
            this.gbDisplayValue.Name = "gbDisplayValue";
            this.gbDisplayValue.Size = new System.Drawing.Size(464, 164);
            this.gbDisplayValue.TabIndex = 8;
            this.gbDisplayValue.TabStop = false;
            this.gbDisplayValue.Text = "Display Value";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.cmbType, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.cmbValue, 2, 3);
            this.tableLayoutPanel3.Controls.Add(this.lblType, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblValue, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.lblDisclaimer, 0, 5);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 6;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(458, 145);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // cmbType
            // 
            this.cmbType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(73, 23);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(382, 21);
            this.cmbType.TabIndex = 5;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // cmbValue
            // 
            this.cmbValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbValue.FormattingEnabled = true;
            this.cmbValue.Location = new System.Drawing.Point(73, 72);
            this.cmbValue.Name = "cmbValue";
            this.cmbValue.Size = new System.Drawing.Size(382, 21);
            this.cmbValue.TabIndex = 6;
            this.cmbValue.SelectedIndexChanged += new System.EventHandler(this.cmbValue_SelectedIndexChanged);
            // 
            // lblType
            // 
            this.lblType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(3, 28);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(44, 13);
            this.lblType.TabIndex = 4;
            this.lblType.Text = "Type:";
            // 
            // lblValue
            // 
            this.lblValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new System.Drawing.Point(3, 77);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(44, 13);
            this.lblValue.TabIndex = 4;
            this.lblValue.Text = "Value:";
            // 
            // lblDisclaimer
            // 
            this.lblDisclaimer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDisclaimer.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.lblDisclaimer, 4);
            this.lblDisclaimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisclaimer.ForeColor = System.Drawing.Color.Red;
            this.lblDisclaimer.Location = new System.Drawing.Point(3, 118);
            this.lblDisclaimer.Name = "lblDisclaimer";
            this.lblDisclaimer.Size = new System.Drawing.Size(452, 13);
            this.lblDisclaimer.TabIndex = 7;
            this.lblDisclaimer.Text = "Note: Some of these Values may not be used during runs.";
            this.lblDisclaimer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FFXUISettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FFXUISettings";
            this.Size = new System.Drawing.Size(476, 380);
            this.Load += new System.EventHandler(this.UISettings_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.gbText.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.gbDisplayValue.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblBackgroundColor;
        private System.Windows.Forms.Button btnBackgroundColor1;
        private System.Windows.Forms.Button btnBackgroundColor2;
        private System.Windows.Forms.GroupBox gbText;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.CheckBox cbOverrideTextColor;
        private System.Windows.Forms.Label lblTextColor;
        private System.Windows.Forms.Button btnTextColor;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.ComboBox cmbGradientType;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.ComboBox cmbValue;
        private System.Windows.Forms.GroupBox gbDisplayValue;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lblDisclaimer;
    }
}
