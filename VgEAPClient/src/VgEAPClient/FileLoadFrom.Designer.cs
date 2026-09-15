// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient;

partial class FileLoadFrom
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileLoadFrom));
        dataGridView1 = new DataGridView();
        FID = new DataGridViewTextBoxColumn();
        FTYPE = new DataGridViewTextBoxColumn();
        FOP = new DataGridViewButtonColumn();
        FPATH = new DataGridViewTextBoxColumn();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        SuspendLayout();
        // 
        // dataGridView1
        // 
        resources.ApplyResources(dataGridView1, "dataGridView1");
        dataGridView1.AllowUserToAddRows = false;
        dataGridView1.AllowUserToDeleteRows = false;
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Columns.AddRange(new DataGridViewColumn[] { FID, FTYPE, FOP, FPATH });
        dataGridView1.Name = "dataGridView1";
        dataGridView1.ReadOnly = true;
        dataGridView1.CellContentClick += dataGridView1_CellContentClick;
        // 
        // FID
        // 
        resources.ApplyResources(FID, "FID");
        FID.Name = "FID";
        FID.ReadOnly = true;
        // 
        // FTYPE
        // 
        resources.ApplyResources(FTYPE, "FTYPE");
        FTYPE.Name = "FTYPE";
        FTYPE.ReadOnly = true;
        // 
        // FOP
        // 
        resources.ApplyResources(FOP, "FOP");
        FOP.Name = "FOP";
        FOP.ReadOnly = true;
        // 
        // FPATH
        // 
        resources.ApplyResources(FPATH, "FPATH");
        FPATH.Name = "FPATH";
        FPATH.ReadOnly = true;
        FPATH.Resizable = DataGridViewTriState.True;
        FPATH.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // FileLoadFrom
        // 
        resources.ApplyResources(this, "$this");
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(dataGridView1);
        Name = "FileLoadFrom";
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private DataGridView dataGridView1;
    private DataGridViewTextBoxColumn FID;
    private DataGridViewTextBoxColumn FTYPE;
    private DataGridViewButtonColumn FOP;
    private DataGridViewTextBoxColumn FPATH;
}