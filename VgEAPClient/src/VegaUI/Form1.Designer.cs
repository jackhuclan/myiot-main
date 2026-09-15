// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VegaUI;

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
        uiSwitch1 = new UISwitch();
        SuspendLayout();
        // 
        // uiSwitch1
        // 
        uiSwitch1.Location = new Point(96, 47);
        uiSwitch1.Name = "uiSwitch1";
        uiSwitch1.RectSides = ToolStripStatusLabelBorderSides.None;
        uiSwitch1.Size = new Size(94, 29);
        uiSwitch1.TabIndex = 0;
        uiSwitch1.Text = "uiSwitch1";
        uiSwitch1.ActiveChanged += uiSwitch1_ActiveChanged;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(9F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = SystemColors.ActiveCaption;
        ClientSize = new Size(800, 450);
        Controls.Add(uiSwitch1);
        Name = "Form1";
        Text = "Form1";
        ResumeLayout(false);
    }

    #endregion

    private UISwitch uiSwitch1;
}
