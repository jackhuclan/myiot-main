// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Options;
using VgEAPClient.Common;
using VgEAPClient.Common.Communication.Outbound;
using WindowsFormsLifetime;

namespace VgEAPClient;

public partial class frmLogin : Form
{
    private readonly EAPClientOptions _eAPClientOptions;
    private readonly IEQPDataReporter _httpDataReporter;
    private readonly IFormProvider _formProvider;

    public frmLogin(IOptions<EAPClientOptions> options,
        IGuiContext guiContext,
        IEQPDataReporter httpDataReporter,
        IFormProvider formProvider)
    {
        InitializeComponent();
        _eAPClientOptions = options.Value;
        _httpDataReporter = httpDataReporter;
        _formProvider = formProvider;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void btnLogin_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(txtUserID.Text.Trim()))
        {
            var frmMain = _formProvider.GetForm<FrmMain>();
            this.Hide();
            frmMain.StartPosition = FormStartPosition.CenterScreen;
            frmMain.Show();

            _httpDataReporter.SendUserCheckCardReport(
                new UserCheckCardReportBody
                {
                    EquipmentID = _eAPClientOptions.EquipmentID,
                    UserId = txtUserID.Text.Trim(),
                    UserName = txtUserName.Text.Trim(),
                });
            DialogResult = DialogResult.OK;
        }
        else
        {
            MessageBox.Show("请输入正确的用户名和密码".VgTs(), "错误".VgTs(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            DialogResult = DialogResult.No;
        }
    }

    private void frmLogin_Load(object sender, EventArgs e)
    {
        txtUserID.Text = _eAPClientOptions.UserID;
    }
}
