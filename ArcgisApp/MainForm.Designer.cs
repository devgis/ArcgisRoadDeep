namespace ArcgisApp
{
    partial class MainForm
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
            //Ensures that any ESRI libraries that have been used are unloaded in the correct order. 
            //Failure to do this may result in random crashes on exit due to the operating system unloading 
            //the libraries in the incorrect order. 
            ESRI.ArcGIS.ADF.COMSupport.AOUninitialize.Shutdown();

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusBarXY = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbStart = new System.Windows.Forms.ComboBox();
            this.cbEnd = new System.Windows.Forms.ComboBox();
            this.btRoute = new System.Windows.Forms.Button();
            this.btReset = new System.Windows.Forms.Button();
            this.btPause = new System.Windows.Forms.Button();
            this.gbRoute = new System.Windows.Forms.GroupBox();
            this.tbResult = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbTime = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbMinLength = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btGenData = new System.Windows.Forms.Button();
            this.timerRun = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.tlpMain.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.gbRoute.SuspendLayout();
            this.SuspendLayout();
            // 
            // axMapControl1
            // 
            this.axMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMapControl1.Location = new System.Drawing.Point(3, 3);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.tlpMain.SetRowSpan(this.axMapControl1, 2);
            this.axMapControl1.Size = new System.Drawing.Size(650, 485);
            this.axMapControl1.TabIndex = 2;
            this.axMapControl1.OnMouseMove += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseMoveEventHandler(this.axMapControl1_OnMouseMove);
            this.axMapControl1.OnMapReplaced += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMapReplacedEventHandler(this.axMapControl1_OnMapReplaced);
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 0);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(859, 28);
            this.axToolbarControl1.TabIndex = 3;
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axTOCControl1.Location = new System.Drawing.Point(659, 453);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(194, 35);
            this.axTOCControl1.TabIndex = 4;
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(466, 278);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 5;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 28);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 513);
            this.splitter1.TabIndex = 6;
            this.splitter1.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarXY});
            this.statusStrip1.Location = new System.Drawing.Point(3, 519);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(856, 22);
            this.statusStrip1.Stretch = false;
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusBar1";
            // 
            // statusBarXY
            // 
            this.statusBarXY.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.statusBarXY.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusBarXY.Name = "statusBarXY";
            this.statusBarXY.Size = new System.Drawing.Size(50, 17);
            this.statusBarXY.Text = "Test 123";
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tlpMain.Controls.Add(this.axTOCControl1, 1, 1);
            this.tlpMain.Controls.Add(this.axMapControl1, 0, 0);
            this.tlpMain.Controls.Add(this.tableLayoutPanel1, 1, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(3, 28);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 450F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Size = new System.Drawing.Size(856, 491);
            this.tlpMain.TabIndex = 8;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.cbStart, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.cbEnd, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btRoute, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btReset, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btPause, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.gbRoute, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btGenData, 2, 0);
            this.tableLayoutPanel1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(656, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 450);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "起点：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "终点：";
            // 
            // cbStart
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.cbStart, 2);
            this.cbStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStart.FormattingEnabled = true;
            this.cbStart.Location = new System.Drawing.Point(69, 33);
            this.cbStart.Name = "cbStart";
            this.cbStart.Size = new System.Drawing.Size(128, 25);
            this.cbStart.TabIndex = 2;
            // 
            // cbEnd
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.cbEnd, 2);
            this.cbEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEnd.FormattingEnabled = true;
            this.cbEnd.Location = new System.Drawing.Point(69, 63);
            this.cbEnd.Name = "cbEnd";
            this.cbEnd.Size = new System.Drawing.Size(128, 25);
            this.cbEnd.TabIndex = 3;
            // 
            // btRoute
            // 
            this.btRoute.Location = new System.Drawing.Point(3, 93);
            this.btRoute.Name = "btRoute";
            this.btRoute.Size = new System.Drawing.Size(60, 23);
            this.btRoute.TabIndex = 4;
            this.btRoute.Text = "路径";
            this.btRoute.UseVisualStyleBackColor = true;
            this.btRoute.Click += new System.EventHandler(this.btRoute_Click);
            // 
            // btReset
            // 
            this.btReset.Location = new System.Drawing.Point(69, 93);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(60, 23);
            this.btReset.TabIndex = 5;
            this.btReset.Text = "重置";
            this.btReset.UseVisualStyleBackColor = true;
            this.btReset.Click += new System.EventHandler(this.btReset_Click);
            // 
            // btPause
            // 
            this.btPause.Enabled = false;
            this.btPause.Location = new System.Drawing.Point(135, 93);
            this.btPause.Name = "btPause";
            this.btPause.Size = new System.Drawing.Size(62, 23);
            this.btPause.TabIndex = 6;
            this.btPause.Text = "暂停";
            this.btPause.UseVisualStyleBackColor = true;
            this.btPause.Click += new System.EventHandler(this.btPause_Click);
            // 
            // gbRoute
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.gbRoute, 3);
            this.gbRoute.Controls.Add(this.tbResult);
            this.gbRoute.Controls.Add(this.label5);
            this.gbRoute.Controls.Add(this.tbTime);
            this.gbRoute.Controls.Add(this.label4);
            this.gbRoute.Controls.Add(this.tbMinLength);
            this.gbRoute.Controls.Add(this.label3);
            this.gbRoute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRoute.Location = new System.Drawing.Point(3, 123);
            this.gbRoute.Name = "gbRoute";
            this.tableLayoutPanel1.SetRowSpan(this.gbRoute, 2);
            this.gbRoute.Size = new System.Drawing.Size(194, 324);
            this.gbRoute.TabIndex = 7;
            this.gbRoute.TabStop = false;
            this.gbRoute.Text = "最优路径";
            // 
            // tbResult
            // 
            this.tbResult.Location = new System.Drawing.Point(8, 92);
            this.tbResult.Multiline = true;
            this.tbResult.Name = "tbResult";
            this.tbResult.ReadOnly = true;
            this.tbResult.Size = new System.Drawing.Size(177, 216);
            this.tbResult.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 17);
            this.label5.TabIndex = 4;
            this.label5.Text = "路径ID:";
            // 
            // tbTime
            // 
            this.tbTime.Location = new System.Drawing.Point(73, 48);
            this.tbTime.Name = "tbTime";
            this.tbTime.ReadOnly = true;
            this.tbTime.Size = new System.Drawing.Size(112, 23);
            this.tbTime.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "疏散时间:";
            // 
            // tbMinLength
            // 
            this.tbMinLength.Location = new System.Drawing.Point(73, 22);
            this.tbMinLength.Name = "tbMinLength";
            this.tbMinLength.ReadOnly = true;
            this.tbMinLength.Size = new System.Drawing.Size(112, 23);
            this.tbMinLength.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "路径长度:";
            // 
            // btGenData
            // 
            this.btGenData.Location = new System.Drawing.Point(135, 3);
            this.btGenData.Name = "btGenData";
            this.btGenData.Size = new System.Drawing.Size(62, 23);
            this.btGenData.TabIndex = 8;
            this.btGenData.Text = "模拟数据";
            this.btGenData.UseVisualStyleBackColor = true;
            this.btGenData.Click += new System.EventHandler(this.btGenData_Click);
            // 
            // timerRun
            // 
            this.timerRun.Interval = 10000;
            this.timerRun.Tick += new System.EventHandler(this.timerRun_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 541);
            this.Controls.Add(this.tlpMain);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.axToolbarControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "最优路径";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tlpMain.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.gbRoute.ResumeLayout(false);
            this.gbRoute.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusBarXY;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbStart;
        private System.Windows.Forms.ComboBox cbEnd;
        private System.Windows.Forms.Button btRoute;
        private System.Windows.Forms.Button btReset;
        private System.Windows.Forms.Button btPause;
        private System.Windows.Forms.GroupBox gbRoute;
        private System.Windows.Forms.TextBox tbResult;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbMinLength;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timerRun;
        private System.Windows.Forms.Button btGenData;
    }
}

