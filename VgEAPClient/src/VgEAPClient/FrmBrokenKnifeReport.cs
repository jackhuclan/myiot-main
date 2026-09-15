// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.Options;

using VgAutoDrill.Fundation.Utils;

using VgEAPClient.Common;
using VgEAPClient.Common.CNC.Status;
using VgEAPClient.Common.Communication.Outbound;

using WindowsFormsLifetime;

namespace VgEAPClient;

public partial class FrmBrokenKnifeReport : Form
{
    private readonly IEQPDataReporter _httpDataReporter;
    private readonly IGuiContext _guiContext;
    private readonly IFormProvider _formProvider;
    private readonly EAPClientOptions _eAPClientOptions;
    private string _strLotId = string.Empty;
    private string _strItemNum = string.Empty;
    private DrillStatusData _drillStatusData = new DrillStatusData();
    private FrmMain _frmMain;

    public FrmBrokenKnifeReport(
        IEQPDataReporter httpDataReporter,
        IGuiContext guiContext,
        IFormProvider formProvider,
        IOptions<EAPClientOptions> options,
        FrmMain frmMain)
    {
        InitializeComponent();
        _httpDataReporter = httpDataReporter;
        _guiContext = guiContext;
        _formProvider = formProvider;
        _eAPClientOptions = options.Value;
        _frmMain = frmMain;

    }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="strLotId">工单号</param>
    /// <param name="strItemNum">批次号（3位）</param>
    /// <param name="drillStatusData"></param>
    public void InitFrmData(string strLotId, string strItemNum, DrillStatusData drillStatusData)
    {
        _strLotId = strLotId;
        _strItemNum = strItemNum;
        _drillStatusData.CurProgramData.DiaFilePath = drillStatusData?.CurProgramData?.DiaFilePath ?? "";
        _drillStatusData.CurProgramData.PgmFilePath = drillStatusData?.CurProgramData?.PgmFilePath ?? "";
        _drillStatusData.CncToolData.ToolN = drillStatusData?.CncToolData?.ToolN ?? "";
        _drillStatusData.CncToolData.ToolB = drillStatusData?.CncToolData?.ToolB ?? "";
        _drillStatusData.CurDrillOrRout = drillStatusData?.CurDrillOrRout ?? "";
        _drillStatusData.BrokenToolData.BrkToolId = drillStatusData?.BrokenToolData?.BrkToolId ?? "";
        _drillStatusData.BrokenToolData.BrkToolSpindle = drillStatusData?.BrokenToolData?.BrkToolSpindle ?? "";
        _drillStatusData.BrokenToolData.BrkToolDia = drillStatusData?.BrokenToolData?.BrkToolDia ?? "";
        _drillStatusData.CncPgmNum = drillStatusData?.CncPgmNum ?? 0;
        ShowInfo();
    }

    public void ShowInfo()
    {
        txtItemId.Text = "";
        labPgmName.Text = "----";
        labSpindleNum.Text = "--";
        labBrkToolId.Text = "--";
        labBrkToolDia.Text = "--";
        comboBrokenNumber.Text = "";
        comboReplaceNum.Text = "";
        comboBrkToolType.Text = "";
        comboBladeLength.Text = "";
        comboBrkReason.Text = "";
        comboPressureFoot.Text = "";
        rbtnInner.Checked = false;
        rbtnOuter.Checked = false;
        //Action action = () =>
        //{
        //    //txtLotId.Text = _strLotId;
        //    // 2025/6/25 崇达需求将此显示从工单号调整为批次号（3位） 修改人：Evan
        //    txtItemId.Text = _strItemNum;
        //    // 2025/6/25 崇达需求新增轴号显示 修改人：Evan
        //    labSpindleNum.Text = _drillStatusData.BrokenToolData.BrkToolSpindle;
        //    // 2025/6/25 崇达需求新增断刀直径显示 修改人：Evan
        //    labBrkToolDia.Text = _drillStatusData.BrokenToolData.BrkToolDia;
        //    // 2025/7/1  崇达需求新增钻带名显示
        //    labPgmName.Text = Path.GetFileName(_drillStatusData.CurProgramData.PgmFilePath);

        //    // 2025/7/1  崇达需求 断刀刃长 断刀原因默认选择为空
        //    comboBladeLength.SelectedIndex = -1;
        //    comboBrkReason.SelectedIndex = -1;

        //    // 2025/7/7 崇达需求 增加刀序显示
        //    labBrkToolId.Text = _drillStatusData.BrokenToolData.BrkToolId;

        //    // 2025/7/7 崇达需求 刃长 断刀原因可修改待选项
        //    comboBladeLength.Items.Clear();
        //    comboBladeLength.Items.AddRange(_eAPClientOptions.MsgBoxBrokenToolInfoSet.BrokenLengthItems.ToArray());

        //    comboBrkReason.Items.Clear();
        //    comboBrkReason.Items.AddRange(_eAPClientOptions.MsgBoxBrokenToolInfoSet.BrokenReasonItems.ToArray());

        //    // 2025/7/7 崇达需求 断针支数默认1、更换磨次默认M1、单/双刃默认单刃、压力脚是否已换默认是
        //    comboBrokenNumber.Text = "1";
        //    comboReplaceNum.Text = "M1";
        //    comboBrkToolType.Text = "单刃";
        //    comboPressureFoot.Text = "是";
        //};
        //Invoke(action);

        //txtLotId.Text = _strLotId;
        // 2025/6/25 崇达需求将此显示从工单号调整为批次号（3位） 修改人：Evan
        txtItemId.Text = _strItemNum;
        // 2025/6/25 崇达需求新增轴号显示 修改人：Evan
        labSpindleNum.Text = _drillStatusData.BrokenToolData.BrkToolSpindle;
        // 2025/6/25 崇达需求新增断刀直径显示 修改人：Evan
        labBrkToolDia.Text = _drillStatusData.BrokenToolData.BrkToolDia;
        // 2025/7/1  崇达需求新增钻带名显示
        labPgmName.Text = Path.GetFileName(_drillStatusData.CurProgramData.PgmFilePath);

        // 2025/7/1  崇达需求 断刀刃长 断刀原因默认选择为空
        comboBladeLength.SelectedIndex = -1;
        comboBrkReason.SelectedIndex = -1;

        // 2025/7/7 崇达需求 增加刀序显示
        labBrkToolId.Text = _drillStatusData.BrokenToolData.BrkToolId;

        // 2025/7/7 崇达需求 刃长 断刀原因可修改待选项
        comboBladeLength.Items.Clear();
        comboBladeLength.Items.AddRange(_eAPClientOptions.MsgBoxBrokenToolInfoSet.BrokenLengthItems.ToArray());

        comboBrkReason.Items.Clear();
        comboBrkReason.Items.AddRange(_eAPClientOptions.MsgBoxBrokenToolInfoSet.BrokenReasonItems.ToArray());

        // 2025/7/7 崇达需求 断针支数默认1、更换磨次默认M1、单/双刃默认单刃、压力脚是否已换默认是
        comboBrokenNumber.Text = "1";
        comboReplaceNum.Text = "M1";
        comboBrkToolType.Text = "单刃";
        comboPressureFoot.Text = "是";
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
        if (!rbtnInner.Checked && !rbtnOuter.Checked)
        {
            //DialogResult dr = MessageBox.Show("断针位置未选择，请选择", "Tips", MessageBoxButtons.OK,
            //    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
            _frmMain.ShowWarnBox("断针位置未选择，请选择".VgTs(), true);
            return;
        }
        if (!string.IsNullOrEmpty(txtItemId.Text.Trim()))
        {
            if (_strItemNum.Trim() != txtItemId.Text.Trim())
            {
                if (_eAPClientOptions.MsgBoxBrokenToolInfoSet.ItemIdMinLength > 0
                    && txtItemId.Text.Trim().Length < _eAPClientOptions.MsgBoxBrokenToolInfoSet.ItemIdMinLength)
                {
                    _frmMain.ShowWarnBox(string.Format("批次号长度不能小于{0}".VgTs(), _eAPClientOptions.MsgBoxBrokenToolInfoSet.ItemIdMinLength), true);
                    return;
                }
            }

            var request = new BrokenKnifeAlarmReportBody()
            {
                EquipmentID = _eAPClientOptions.EquipmentID,
                LotID = _strLotId,
                ItemNum = txtItemId.Text.Trim(),
                FilePath = _drillStatusData.CurProgramData.DiaFilePath,
                DrillPath = _drillStatusData.CurProgramData.PgmFilePath,
                KnifeSeq = _drillStatusData.BrokenToolData.BrkToolId,
                AxisNum = _drillStatusData.BrokenToolData.BrkToolSpindle,
                KnifeDia = _drillStatusData.BrokenToolData.BrkToolDia,
                SetLife = _drillStatusData.CncToolData.ToolN,
                UsedLife = _drillStatusData.CncToolData.ToolB,
                BrokenTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                HolesNum = _drillStatusData.CurDrillOrRout,
                PanelSite = (rbtnInner.Checked ? 1 : 2).ToString(),
                /// 2025/6/25 崇达需求新增 断针数量属性上报 修改人：Evan
                BrokenNum = comboBrokenNumber.Text.ToInt(),
                /// 2025/6/25 崇达需求新增 断刀原因属性上报 修改人：Evan
                BrokenReason = comboBrkReason.Text.Trim(),
                /// 2025/6/25 崇达需求新增 断刀类型属性上报 修改人：Evan
                Knife = comboBrkToolType.Text.Trim().Equals("单刃") ? 1 : 2,
                /// 2025/6/25 崇达需求新增 断刃长度属性上报 修改人：Evan
                KnifeLength = comboBladeLength.Text.ToFloat(),
                /// 2025/6/25 崇达需求新增 压力脚属性上报 修改人：Evan
                PressureFoot = comboPressureFoot.Text.Trim().Equals("是") ? 1 : 2,
                /// 2025/6/25 崇达需求新增 更换磨次属性上报 修改人：Evan
                ReplaceNum = comboReplaceNum.Text.ToString() ?? "",
                /// 2025-7-1  崇达需求新增 趟数上报 修改人：Panda
                NumRank = _drillStatusData.CncPgmNum.ToStringEx(),
            };
            _httpDataReporter.SendBrokenKnifeAlarmReport(request);
            _frmMain.ShowSendBox($@"FrmBrokenKnifeReport - SendBrokenKnifeAlarmReport - {request.ToJson()}");
            Close();
        }
        else
        {
            //DialogResult dr = MessageBox.Show("LotID 为空，请输入工单号.", "Tips", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, MessageBoxOptions.DefaultDesktopOnly);
            _frmMain.ShowWarnBox("批次号为空，请输入批次号.".VgTs(), true);
        }
    }

    private void FrmBrokenKnifeReport_FormClosing(object sender, FormClosingEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }
}
