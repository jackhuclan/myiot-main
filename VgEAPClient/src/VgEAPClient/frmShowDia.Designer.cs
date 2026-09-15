
namespace VgEAPClient;

partial class frmShowDia
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
        dataGrid = new DataGridView();
        ((System.ComponentModel.ISupportInitialize)dataGrid).BeginInit();
        SuspendLayout();
        // 
        // dataGrid
        // 
        dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGrid.Dock = DockStyle.Fill;
        dataGrid.Location = new Point(0, 0);
        dataGrid.Margin = new Padding(4, 4, 4, 4);
        dataGrid.Name = "dataGrid";
        dataGrid.RowHeadersWidth = 51;
        dataGrid.Size = new Size(978, 691);
        dataGrid.TabIndex = 0;
        dataGrid.CellContentClick += dataGrid_CellContentClick;
        // 
        // frmShowDia
        // 
        AutoScaleDimensions = new SizeF(120F, 120F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(978, 691);
        Controls.Add(dataGrid);
        Margin = new Padding(4, 4, 4, 4);
        MaximumSize = new Size(996, 738);
        MinimumSize = new Size(996, 738);
        Name = "frmShowDia";
        StartPosition = FormStartPosition.CenterParent;
        Text = "请选择参数文件";
        ((System.ComponentModel.ISupportInitialize)dataGrid).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private DataGridView dataGrid;
}
