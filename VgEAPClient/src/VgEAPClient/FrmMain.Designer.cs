namespace VgEAPClient
{
    partial class FrmMain
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            splitContainer1 = new SplitContainer();
            txtOpcuaServer = new TextBox();
            label8 = new Label();
            opcuaSwitch = new VegaUI.UISwitch();
            label7 = new Label();
            cnc95Switch = new VegaUI.UISwitch();
            label6 = new Label();
            cnc84Swith = new VegaUI.UISwitch();
            groupBox1 = new GroupBox();
            dataGrid = new DataGridView();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            label2 = new Label();
            cimSwitch = new VegaUI.UISwitch();
            pgLotInfo = new PropertyGrid();
            groupBox3 = new GroupBox();
            numCompleted = new NumericUpDown();
            label9 = new Label();
            btnLotCompleted = new Button();
            groupBox2 = new GroupBox();
            dataGridCimMsg = new DataGridView();
            btnDateTimeSync = new Button();
            label4 = new Label();
            autoSwitch = new VegaUI.UISwitch();
            label1 = new Label();
            txtWorkOrder = new TextBox();
            label5 = new Label();
            uiSwPM = new VegaUI.UISwitch();
            tabPage2 = new TabPage();
            btnUpdate = new Button();
            btnSave = new Button();
            pgSettings = new PropertyGrid();
            tabPage5 = new TabPage();
            tabPage4 = new TabPage();
            dgvlog = new DataGridView();
            FSEQ = new DataGridViewTextBoxColumn();
            FTIME = new DataGridViewTextBoxColumn();
            FTYPE = new DataGridViewTextBoxColumn();
            FMSG = new DataGridViewTextBoxColumn();
            tabPage3 = new TabPage();
            pictureBox2 = new PictureBox();
            label3 = new Label();
            tabPage6 = new TabPage();
            btnCncStop = new Button();
            btnCncStart = new Button();
            btnLoadFile = new Button();
            checkIsDirectLoad = new CheckBox();
            groupBox6 = new GroupBox();
            datagvLocalAtpFilePath = new DataGridView();
            groupBox5 = new GroupBox();
            datagvLocalParaFilePath = new DataGridView();
            groupBox4 = new GroupBox();
            datagvLocalPgmFilePath = new DataGridView();
            label10 = new Label();
            txtLocalWorkOrder = new TextBox();
            notifyIcon = new NotifyIcon(components);
            contextMenuStrip = new ContextMenuStrip(components);
            ShowToolStripMenuItem = new ToolStripMenuItem();
            ExitToolStripMenuItem = new ToolStripMenuItem();
            saveFileDialog1 = new SaveFileDialog();
            toolTip = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGrid).BeginInit();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numCompleted).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridCimMsg).BeginInit();
            tabPage2.SuspendLayout();
            tabPage5.SuspendLayout();
            tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvlog).BeginInit();
            tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            tabPage6.SuspendLayout();
            groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)datagvLocalAtpFilePath).BeginInit();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)datagvLocalParaFilePath).BeginInit();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)datagvLocalPgmFilePath).BeginInit();
            contextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(splitContainer1, "splitContainer1");
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(txtOpcuaServer);
            splitContainer1.Panel1.Controls.Add(label8);
            splitContainer1.Panel1.Controls.Add(opcuaSwitch);
            splitContainer1.Panel1.Controls.Add(label7);
            splitContainer1.Panel1.Controls.Add(cnc95Switch);
            splitContainer1.Panel1.Controls.Add(label6);
            splitContainer1.Panel1.Controls.Add(cnc84Swith);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(groupBox1);
            // 
            // txtOpcuaServer
            // 
            resources.ApplyResources(txtOpcuaServer, "txtOpcuaServer");
            txtOpcuaServer.Name = "txtOpcuaServer";
            // 
            // label8
            // 
            resources.ApplyResources(label8, "label8");
            label8.Name = "label8";
            // 
            // opcuaSwitch
            // 
            opcuaSwitch.ActiveColor = Color.Green;
            opcuaSwitch.ActiveText = "已开启";
            resources.ApplyResources(opcuaSwitch, "opcuaSwitch");
            opcuaSwitch.InActiveText = "断开连接";
            opcuaSwitch.Name = "opcuaSwitch";
            opcuaSwitch.RectSides = ToolStripStatusLabelBorderSides.None;
            opcuaSwitch.ValueChanged += opcuaSwitch_ValueChanged;
            // 
            // label7
            // 
            resources.ApplyResources(label7, "label7");
            label7.Name = "label7";
            // 
            // cnc95Switch
            // 
            cnc95Switch.ActiveColor = Color.Green;
            cnc95Switch.ActiveText = "已连接";
            resources.ApplyResources(cnc95Switch, "cnc95Switch");
            cnc95Switch.InActiveText = "断开连接";
            cnc95Switch.Name = "cnc95Switch";
            cnc95Switch.RectSides = ToolStripStatusLabelBorderSides.None;
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.Name = "label6";
            // 
            // cnc84Swith
            // 
            cnc84Swith.ActiveColor = Color.Green;
            cnc84Swith.ActiveText = "已连接";
            resources.ApplyResources(cnc84Swith, "cnc84Swith");
            cnc84Swith.InActiveText = "断开连接";
            cnc84Swith.Name = "cnc84Swith";
            cnc84Swith.RectSides = ToolStripStatusLabelBorderSides.None;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dataGrid);
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
            // 
            // dataGrid
            // 
            dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(dataGrid, "dataGrid");
            dataGrid.Name = "dataGrid";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage6);
            resources.ApplyResources(tabControl1, "tabControl1");
            tabControl1.Multiline = true;
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.PaleTurquoise;
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(cimSwitch);
            tabPage1.Controls.Add(pgLotInfo);
            tabPage1.Controls.Add(groupBox3);
            tabPage1.Controls.Add(groupBox2);
            tabPage1.Controls.Add(btnDateTimeSync);
            tabPage1.Controls.Add(label4);
            tabPage1.Controls.Add(autoSwitch);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(txtWorkOrder);
            tabPage1.Controls.Add(label5);
            tabPage1.Controls.Add(uiSwPM);
            resources.ApplyResources(tabPage1, "tabPage1");
            tabPage1.Name = "tabPage1";
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.Name = "label2";
            // 
            // cimSwitch
            // 
            cimSwitch.ActiveColor = Color.Green;
            cimSwitch.ActiveText = "已连接";
            resources.ApplyResources(cimSwitch, "cimSwitch");
            cimSwitch.InActiveText = "断开连接";
            cimSwitch.Name = "cimSwitch";
            cimSwitch.RectSides = ToolStripStatusLabelBorderSides.None;
            cimSwitch.ValueChanged += cimSwitch_ValueChanged;
            // 
            // pgLotInfo
            // 
            resources.ApplyResources(pgLotInfo, "pgLotInfo");
            pgLotInfo.Name = "pgLotInfo";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(numCompleted);
            groupBox3.Controls.Add(label9);
            groupBox3.Controls.Add(btnLotCompleted);
            resources.ApplyResources(groupBox3, "groupBox3");
            groupBox3.Name = "groupBox3";
            groupBox3.TabStop = false;
            // 
            // numCompleted
            // 
            resources.ApplyResources(numCompleted, "numCompleted");
            numCompleted.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            numCompleted.Name = "numCompleted";
            // 
            // label9
            // 
            resources.ApplyResources(label9, "label9");
            label9.Name = "label9";
            // 
            // btnLotCompleted
            // 
            resources.ApplyResources(btnLotCompleted, "btnLotCompleted");
            btnLotCompleted.Name = "btnLotCompleted";
            btnLotCompleted.UseVisualStyleBackColor = true;
            btnLotCompleted.Click += btnLotCompleted_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dataGridCimMsg);
            resources.ApplyResources(groupBox2, "groupBox2");
            groupBox2.Name = "groupBox2";
            groupBox2.TabStop = false;
            // 
            // dataGridCimMsg
            // 
            dataGridCimMsg.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(dataGridCimMsg, "dataGridCimMsg");
            dataGridCimMsg.Name = "dataGridCimMsg";
            // 
            // btnDateTimeSync
            // 
            resources.ApplyResources(btnDateTimeSync, "btnDateTimeSync");
            btnDateTimeSync.Name = "btnDateTimeSync";
            btnDateTimeSync.UseVisualStyleBackColor = true;
            btnDateTimeSync.Click += btnDateTimeSync_Click;
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.Name = "label4";
            // 
            // autoSwitch
            // 
            autoSwitch.ActiveColor = Color.Green;
            autoSwitch.ActiveText = "自动模式";
            resources.ApplyResources(autoSwitch, "autoSwitch");
            autoSwitch.InActiveText = "手动模式";
            autoSwitch.Name = "autoSwitch";
            autoSwitch.RectSides = ToolStripStatusLabelBorderSides.None;
            autoSwitch.ValueChanged += autoSwitch_ValueChanged;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.Name = "label1";
            // 
            // txtWorkOrder
            // 
            resources.ApplyResources(txtWorkOrder, "txtWorkOrder");
            txtWorkOrder.Name = "txtWorkOrder";
            txtWorkOrder.KeyDown += txtWorkOrder_KeyDown;
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.Name = "label5";
            // 
            // uiSwPM
            // 
            uiSwPM.ActiveColor = Color.Red;
            uiSwPM.ActiveText = "开始保养";
            resources.ApplyResources(uiSwPM, "uiSwPM");
            uiSwPM.InActiveColor = Color.Green;
            uiSwPM.InActiveText = "保养结束";
            uiSwPM.Name = "uiSwPM";
            uiSwPM.RectSides = ToolStripStatusLabelBorderSides.None;
            uiSwPM.ValueChanged += uiSwPM_ValueChanged;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(btnUpdate);
            tabPage2.Controls.Add(btnSave);
            tabPage2.Controls.Add(pgSettings);
            resources.ApplyResources(tabPage2, "tabPage2");
            tabPage2.Name = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            resources.ApplyResources(btnUpdate, "btnUpdate");
            btnUpdate.Name = "btnUpdate";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnSave
            // 
            resources.ApplyResources(btnSave, "btnSave");
            btnSave.Name = "btnSave";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // pgSettings
            // 
            resources.ApplyResources(pgSettings, "pgSettings");
            pgSettings.Name = "pgSettings";
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(splitContainer1);
            resources.ApplyResources(tabPage5, "tabPage5");
            tabPage5.Name = "tabPage5";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(dgvlog);
            resources.ApplyResources(tabPage4, "tabPage4");
            tabPage4.Name = "tabPage4";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // dgvlog
            // 
            dgvlog.AllowUserToAddRows = false;
            dgvlog.AllowUserToDeleteRows = false;
            dgvlog.AllowUserToResizeRows = false;
            resources.ApplyResources(dgvlog, "dgvlog");
            dgvlog.BackgroundColor = Color.White;
            dgvlog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvlog.Columns.AddRange(new DataGridViewColumn[] { FSEQ, FTIME, FTYPE, FMSG });
            dgvlog.Name = "dgvlog";
            dgvlog.ReadOnly = true;
            dgvlog.RowHeadersVisible = false;
            // 
            // FSEQ
            // 
            FSEQ.DataPropertyName = "FSEQ";
            resources.ApplyResources(FSEQ, "FSEQ");
            FSEQ.Name = "FSEQ";
            FSEQ.ReadOnly = true;
            FSEQ.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // FTIME
            // 
            FTIME.DataPropertyName = "FTIME";
            resources.ApplyResources(FTIME, "FTIME");
            FTIME.Name = "FTIME";
            FTIME.ReadOnly = true;
            FTIME.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // FTYPE
            // 
            FTYPE.DataPropertyName = "FTYPE";
            resources.ApplyResources(FTYPE, "FTYPE");
            FTYPE.Name = "FTYPE";
            FTYPE.ReadOnly = true;
            FTYPE.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // FMSG
            // 
            FMSG.DataPropertyName = "FMSG";
            resources.ApplyResources(FMSG, "FMSG");
            FMSG.Name = "FMSG";
            FMSG.ReadOnly = true;
            FMSG.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // tabPage3
            // 
            tabPage3.BackColor = SystemColors.ActiveCaption;
            tabPage3.Controls.Add(pictureBox2);
            tabPage3.Controls.Add(label3);
            resources.ApplyResources(tabPage3, "tabPage3");
            tabPage3.Name = "tabPage3";
            // 
            // pictureBox2
            // 
            resources.ApplyResources(pictureBox2, "pictureBox2");
            pictureBox2.Name = "pictureBox2";
            pictureBox2.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.ForeColor = Color.Red;
            label3.Name = "label3";
            // 
            // tabPage6
            // 
            tabPage6.BackColor = Color.PaleTurquoise;
            tabPage6.Controls.Add(btnCncStop);
            tabPage6.Controls.Add(btnCncStart);
            tabPage6.Controls.Add(btnLoadFile);
            tabPage6.Controls.Add(checkIsDirectLoad);
            tabPage6.Controls.Add(groupBox6);
            tabPage6.Controls.Add(groupBox5);
            tabPage6.Controls.Add(groupBox4);
            tabPage6.Controls.Add(label10);
            tabPage6.Controls.Add(txtLocalWorkOrder);
            resources.ApplyResources(tabPage6, "tabPage6");
            tabPage6.Name = "tabPage6";
            // 
            // btnCncStop
            // 
            resources.ApplyResources(btnCncStop, "btnCncStop");
            btnCncStop.Name = "btnCncStop";
            btnCncStop.UseVisualStyleBackColor = true;
            btnCncStop.Click += btnCncStop_Click;
            // 
            // btnCncStart
            // 
            resources.ApplyResources(btnCncStart, "btnCncStart");
            btnCncStart.Name = "btnCncStart";
            btnCncStart.UseVisualStyleBackColor = true;
            btnCncStart.Click += btnCncStart_Click;
            // 
            // btnLoadFile
            // 
            resources.ApplyResources(btnLoadFile, "btnLoadFile");
            btnLoadFile.Name = "btnLoadFile";
            btnLoadFile.UseVisualStyleBackColor = true;
            btnLoadFile.Click += btnLoadFile_Click;
            // 
            // checkIsDirectLoad
            // 
            resources.ApplyResources(checkIsDirectLoad, "checkIsDirectLoad");
            checkIsDirectLoad.Name = "checkIsDirectLoad";
            checkIsDirectLoad.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(datagvLocalAtpFilePath);
            resources.ApplyResources(groupBox6, "groupBox6");
            groupBox6.Name = "groupBox6";
            groupBox6.TabStop = false;
            // 
            // datagvLocalAtpFilePath
            // 
            datagvLocalAtpFilePath.AllowUserToAddRows = false;
            datagvLocalAtpFilePath.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(datagvLocalAtpFilePath, "datagvLocalAtpFilePath");
            datagvLocalAtpFilePath.Name = "datagvLocalAtpFilePath";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(datagvLocalParaFilePath);
            resources.ApplyResources(groupBox5, "groupBox5");
            groupBox5.Name = "groupBox5";
            groupBox5.TabStop = false;
            // 
            // datagvLocalParaFilePath
            // 
            datagvLocalParaFilePath.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(datagvLocalParaFilePath, "datagvLocalParaFilePath");
            datagvLocalParaFilePath.Name = "datagvLocalParaFilePath";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(datagvLocalPgmFilePath);
            resources.ApplyResources(groupBox4, "groupBox4");
            groupBox4.Name = "groupBox4";
            groupBox4.TabStop = false;
            // 
            // datagvLocalPgmFilePath
            // 
            datagvLocalPgmFilePath.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(datagvLocalPgmFilePath, "datagvLocalPgmFilePath");
            datagvLocalPgmFilePath.Name = "datagvLocalPgmFilePath";
            // 
            // label10
            // 
            resources.ApplyResources(label10, "label10");
            label10.Name = "label10";
            // 
            // txtLocalWorkOrder
            // 
            resources.ApplyResources(txtLocalWorkOrder, "txtLocalWorkOrder");
            txtLocalWorkOrder.Name = "txtLocalWorkOrder";
            txtLocalWorkOrder.KeyDown += txtLocalWorkOrder_KeyDown;
            // 
            // notifyIcon
            // 
            notifyIcon.ContextMenuStrip = contextMenuStrip;
            resources.ApplyResources(notifyIcon, "notifyIcon");
            notifyIcon.DoubleClick += notifyIcon_DoubleClick;
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.ImageScalingSize = new Size(20, 20);
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { ShowToolStripMenuItem, ExitToolStripMenuItem });
            contextMenuStrip.Name = "contextMenuStrip";
            resources.ApplyResources(contextMenuStrip, "contextMenuStrip");
            // 
            // ShowToolStripMenuItem
            // 
            ShowToolStripMenuItem.Name = "ShowToolStripMenuItem";
            resources.ApplyResources(ShowToolStripMenuItem, "ShowToolStripMenuItem");
            ShowToolStripMenuItem.Click += ShowToolStripMenuItem_Click;
            // 
            // ExitToolStripMenuItem
            // 
            ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            resources.ApplyResources(ExitToolStripMenuItem, "ExitToolStripMenuItem");
            ExitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            // 
            // toolTip
            // 
            toolTip.ToolTipIcon = ToolTipIcon.Info;
            // 
            // FrmMain
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = SystemColors.Info;
            Controls.Add(tabControl1);
            Name = "FrmMain";
            FormClosing += frmScanner_FormClosing;
            FormClosed += OnFormClosed;
            Load += frmScanner_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGrid).EndInit();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numCompleted).EndInit();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridCimMsg).EndInit();
            tabPage2.ResumeLayout(false);
            tabPage5.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvlog).EndInit();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            tabPage6.ResumeLayout(false);
            tabPage6.PerformLayout();
            groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)datagvLocalAtpFilePath).EndInit();
            groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)datagvLocalParaFilePath).EndInit();
            groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)datagvLocalPgmFilePath).EndInit();
            contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private TabControl tabControl1;
        private TabPage tabPage1;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem ShowToolStripMenuItem;
        private ToolStripMenuItem ExitToolStripMenuItem;
        private TabPage tabPage3;
        private Label label3;
        private PictureBox pictureBox2;
        private SaveFileDialog saveFileDialog1;
        private VegaUI.UISwitch uiSwPM;
        private Label label5;
        private ToolTip toolTip;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private Label label1;
        private TextBox txtWorkOrder;
        private SplitContainer splitContainer1;
        private Label label4;
        private VegaUI.UISwitch autoSwitch;
        private Button btnDateTimeSync;
        private Label label6;
        private VegaUI.UISwitch cnc84Swith;
        private Label label7;
        private VegaUI.UISwitch cnc95Switch;
        private Label label8;
        private VegaUI.UISwitch opcuaSwitch;
        private TextBox txtOpcuaServer;
        private Button btnLotCompleted;
        private GroupBox groupBox2;
        private DataGridView dataGridCimMsg;
        private GroupBox groupBox3;
        private Label label9;
        private NumericUpDown numCompleted;
        private PropertyGrid pgLotInfo;
        private Label label2;
        private VegaUI.UISwitch cimSwitch;
        private GroupBox groupBox1;
        private DataGridView dataGrid;
        private TabPage tabPage2;
        private PropertyGrid pgSettings;
        private Button btnUpdate;
        private Button btnSave;
        private TabPage tabPage6;
        private GroupBox groupBox4;
        private DataGridView datagvLocalPgmFilePath;
        private Label label10;
        private TextBox txtLocalWorkOrder;
        private GroupBox groupBox5;
        private DataGridView datagvLocalParaFilePath;
        private GroupBox groupBox6;
        private DataGridView datagvLocalAtpFilePath;
        private Button btnLoadFile;
        private CheckBox checkIsDirectLoad;
        private Button btnCncStop;
        private Button btnCncStart;
        private DataGridView dgvlog;
        private DataGridViewTextBoxColumn FSEQ;
        private DataGridViewTextBoxColumn FTIME;
        private DataGridViewTextBoxColumn FTYPE;
        private DataGridViewTextBoxColumn FMSG;
    }
}
