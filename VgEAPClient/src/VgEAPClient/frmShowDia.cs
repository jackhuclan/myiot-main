// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Options;
using VgEAPClient.Common;

namespace VgEAPClient;

public partial class frmShowDia : Form
{
    private readonly EAPClientOptions _eAPClientOptions;

    public frmShowDia(IOptions<EAPClientOptions> options)
    {
        InitializeComponent();
        _eAPClientOptions = options.Value;
    }

    private class usrRecipe
    {
        public string tagCode { get; set; } = string.Empty;
        public string tagValue { get; set; } = string.Empty;
        public string tagPath { get; set; } = string.Empty;
    }

    private void BindGrid(List<usrRecipe> data)
    {
        dataGrid.Rows.Clear();
        dataGrid.Columns.Clear();
        dataGrid.DataSource = null;
        //关闭自动创建列
        dataGrid.AutoGenerateColumns = false;
        dataGrid.AutoSize = true;

        //取消最后一行空白列
        dataGrid.AllowUserToAddRows = false;
        // 列头隐藏
        //dataGrid.ColumnHeadersVisible = false;
        // 行头隐藏
        //dataGrid.RowHeadersVisible = false;
        // 禁止用户改变DataGridView1的所有列的列宽
        //dataGrid.AllowUserToResizeColumns = false;
        //禁止用户改变DataGridView1の所有行的行高
        //dataGrid.AllowUserToResizeRows = false;
        // Initialize and add a text box column.

        //DataGridViewColumn InnerID = new DataGridViewTextBoxColumn();
        //InnerID.DataPropertyName = "sInnerID";
        //InnerID.Name = "sInnerID";
        //InnerID.Width = 1;
        //InnerID.HeaderText = "主键";
        //InnerID.Visible = false;
        //dataGrid.Columns.Add(InnerID);

        DataGridViewColumn tagCode = new DataGridViewTextBoxColumn();
        tagCode.DataPropertyName = "tagCode";
        tagCode.Name = "tagCode";
        tagCode.Width = 90;
        tagCode.HeaderText = "ID";
        dataGrid.Columns.Add(tagCode);

        DataGridViewColumn tagValue = new DataGridViewTextBoxColumn();
        tagValue.DataPropertyName = "tagValue";
        tagValue.Name = "tagValue";
        tagValue.Width = 90;
        tagValue.HeaderText = "类型";
        dataGrid.Columns.Add(tagValue);

        DataGridViewButtonColumn Operate = new DataGridViewButtonColumn();
        Operate.Name = "Operate";
        Operate.Width = 60;
        Operate.HeaderText = "操作";
        Operate.UseColumnTextForButtonValue = true;
        Operate.Text = "加载";
        dataGrid.Columns.Add(Operate);

        DataGridViewColumn tagPath = new DataGridViewTextBoxColumn();
        tagPath.DataPropertyName = "tagPath";
        tagPath.Name = "tagPath";
        tagPath.Width = 550;
        tagPath.HeaderText = "详细路径";
        dataGrid.Columns.Add(tagPath);
        //禁止改变DataGridView的行高与列宽
        dataGrid.AllowUserToResizeRows = false;
        dataGrid.AllowUserToResizeColumns = true;

        dataGrid.DataSource = data;
        dataGrid.ReadOnly = true;
    }

    private void dataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        if (dataGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
        {
            DataGridViewRow row = dataGrid.Rows[e.RowIndex];
            string filePath = row.Cells["tagPath"].Value.ToString() ?? "";
            _eAPClientOptions.selectedDiaFile = filePath;
            this.DialogResult = DialogResult.OK;
        }
    }
}
