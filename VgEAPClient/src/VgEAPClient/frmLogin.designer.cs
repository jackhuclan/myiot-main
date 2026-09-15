namespace VgEAPClient;
partial class frmLogin
{
    /// <summary>
    /// 必需的设计器变量。
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// 清理所有正在使用的资源。
    /// </summary>
    /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows 窗体设计器生成的代码

    /// <summary>
    /// 设计器支持所需的方法 - 不要
    /// 使用代码编辑器修改此方法的内容。
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
        panel1 = new Panel();
        pictureBox3 = new PictureBox();
        pictureBox5 = new PictureBox();
        pictureBox2 = new PictureBox();
        btnCancel = new Button();
        btnLogin = new Button();
        txtUserName = new TextBox();
        txtUserID = new TextBox();
        label1 = new Label();
        label2 = new Label();
        panel2 = new Panel();
        pictureBox4 = new PictureBox();
        panel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
        panel2.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
        SuspendLayout();
        // 
        // panel1
        // 
        resources.ApplyResources(panel1, "panel1");
        panel1.Controls.Add(pictureBox3);
        panel1.Controls.Add(pictureBox5);
        panel1.Controls.Add(pictureBox2);
        panel1.Controls.Add(btnCancel);
        panel1.Controls.Add(btnLogin);
        panel1.Controls.Add(txtUserName);
        panel1.Controls.Add(txtUserID);
        panel1.Controls.Add(label1);
        panel1.Controls.Add(label2);
        panel1.Name = "panel1";
        // 
        // pictureBox3
        // 
        resources.ApplyResources(pictureBox3, "pictureBox3");
        pictureBox3.Name = "pictureBox3";
        pictureBox3.TabStop = false;
        // 
        // pictureBox5
        // 
        resources.ApplyResources(pictureBox5, "pictureBox5");
        pictureBox5.Name = "pictureBox5";
        pictureBox5.TabStop = false;
        // 
        // pictureBox2
        // 
        resources.ApplyResources(pictureBox2, "pictureBox2");
        pictureBox2.Name = "pictureBox2";
        pictureBox2.TabStop = false;
        // 
        // btnCancel
        // 
        resources.ApplyResources(btnCancel, "btnCancel");
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Name = "btnCancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;
        // 
        // btnLogin
        // 
        resources.ApplyResources(btnLogin, "btnLogin");
        btnLogin.Name = "btnLogin";
        btnLogin.UseVisualStyleBackColor = true;
        btnLogin.Click += btnLogin_Click;
        // 
        // txtUserName
        // 
        resources.ApplyResources(txtUserName, "txtUserName");
        txtUserName.BorderStyle = BorderStyle.FixedSingle;
        txtUserName.Name = "txtUserName";
        // 
        // txtUserID
        // 
        resources.ApplyResources(txtUserID, "txtUserID");
        txtUserID.BorderStyle = BorderStyle.FixedSingle;
        txtUserID.Name = "txtUserID";
        // 
        // label1
        // 
        resources.ApplyResources(label1, "label1");
        label1.Name = "label1";
        // 
        // label2
        // 
        resources.ApplyResources(label2, "label2");
        label2.Name = "label2";
        // 
        // panel2
        // 
        resources.ApplyResources(panel2, "panel2");
        panel2.BackColor = Color.White;
        panel2.Controls.Add(pictureBox4);
        panel2.Controls.Add(panel1);
        panel2.Name = "panel2";
        // 
        // pictureBox4
        // 
        resources.ApplyResources(pictureBox4, "pictureBox4");
        pictureBox4.BackColor = SystemColors.Control;
        pictureBox4.Name = "pictureBox4";
        pictureBox4.TabStop = false;
        // 
        // frmLogin
        // 
        resources.ApplyResources(this, "$this");
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(panel2);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmLogin";
        Load += frmLogin_Load;
        panel1.ResumeLayout(false);
        panel1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
        panel2.ResumeLayout(false);
        panel2.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private Panel panel1;
    private TextBox txtUserID;
    private Label label2;
    private Label label1;
    private Button btnCancel;
    private Button btnLogin;
    private TextBox txtUserName;
    private PictureBox pictureBox2;
    private Panel panel2;
    private PictureBox pictureBox5;
    private PictureBox pictureBox3;
    private PictureBox pictureBox4;
}
