// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using VgEAPClient.Common;

namespace VgEAPClient;
public partial class FileLoadFrom : Form
{
    private int _filetype = 0;

    public string SelectFilePath { get; set; } = string.Empty;

    private FrmMain _frmMain;
    public FileLoadFrom(List<string> FileList, FrmMain frmMain, int FileType = 0)
    {
        InitializeComponent();

        _frmMain = frmMain;
        _filetype = FileType;
        string tppename = "钻带文件";
        switch (_filetype)
        {
            case 0:
                tppename = "钻带文件";
                break;
            case 1:
                tppename = "直径文件";
                break;
            case 2:
                tppename = "刀具文件";
                break;
        }
        Text = $@"{tppename}选择";
        for (int i = 0; i < FileList.Count; i++)
        {
            int index = dataGridView1.Rows.Add();
            dataGridView1.Rows[index].Cells["FID"].Value = i + 1;
            dataGridView1.Rows[index].Cells["FTYPE"].Value = tppename;
            dataGridView1.Rows[index].Cells["FOP"].Value = "加载";
            //dataGridView1.Rows[index].Cells["FOP"].ReadOnly = !File.Exists(FileList[i]);
            SetCellButton((DataGridViewButtonCell)dataGridView1.Rows[index].Cells["FOP"], !File.Exists(FileList[i]));
            dataGridView1.Rows[index].Cells["FPATH"].Value = FileList[i];
        }

        _frmMain = frmMain;
    }

    private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        if (dataGridView1.Columns[e.ColumnIndex].Name == "FOP")
        {
            var ReadOnly = dataGridView1.Rows[e.RowIndex].Cells["FOP"].ReadOnly;
            string path = dataGridView1.Rows[e.RowIndex].Cells["FPATH"].Value.ToStringEx();
            if (File.Exists(path))
            {
                SelectFilePath = path;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                _frmMain.ShowWarnBox($@"{path} 文件不存在", _frmMain._eAPClientOptions.IsLoadFileMsgBoxShow);
            }

        }
    }

    private void SetCellButton(DataGridViewButtonCell buttonCell, bool ReadOnly)
    {
        buttonCell.ReadOnly = ReadOnly;
        if (ReadOnly)
        {

            buttonCell.FlatStyle = FlatStyle.Popup;
            buttonCell.Style.BackColor = Color.LightGray;
            buttonCell.Style.ForeColor = Color.DarkGray;
            buttonCell.Style.SelectionBackColor = Color.LightGray;
            buttonCell.Style.SelectionForeColor = Color.DarkGray;
        }
        else
        {
            buttonCell.FlatStyle = FlatStyle.Standard;
        }
    }
}
