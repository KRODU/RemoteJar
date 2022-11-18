namespace RemoteJar
{
    partial class FrmRemoteJarMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ListJarId = new System.Windows.Forms.ListBox();
            this.LblJarId = new System.Windows.Forms.Label();
            this.GridDir = new System.Windows.Forms.DataGridView();
            this.ClmCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ClmHostName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClmHost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClmPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtPathList = new System.Windows.Forms.Label();
            this.contextMenuStripLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemLogClear = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.GridTask = new System.Windows.Forms.DataGridView();
            this.ClmTaskHost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClmTaskPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClmTask = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTaskList = new System.Windows.Forms.Label();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainerLow = new System.Windows.Forms.SplitContainer();
            this.ckForceKill = new System.Windows.Forms.CheckBox();
            this.btnPopupPath = new System.Windows.Forms.Button();
            this.btnTaskStart = new System.Windows.Forms.Button();
            this.txtLocalPath = new System.Windows.Forms.TextBox();
            this.ckRun = new System.Windows.Forms.CheckBox();
            this.lblLocalPath = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.timerSec = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.GridDir)).BeginInit();
            this.contextMenuStripLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridTask)).BeginInit();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLow)).BeginInit();
            this.splitContainerLow.Panel1.SuspendLayout();
            this.splitContainerLow.Panel2.SuspendLayout();
            this.splitContainerLow.SuspendLayout();
            this.SuspendLayout();
            // 
            // ListJarId
            // 
            this.ListJarId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListJarId.HorizontalScrollbar = true;
            this.ListJarId.ItemHeight = 12;
            this.ListJarId.Location = new System.Drawing.Point(3, 23);
            this.ListJarId.Name = "ListJarId";
            this.ListJarId.Size = new System.Drawing.Size(198, 422);
            this.ListJarId.TabIndex = 0;
            this.ListJarId.SelectedIndexChanged += new System.EventHandler(this.ListJarId_SelectedIndexChanged);
            // 
            // LblJarId
            // 
            this.LblJarId.AutoSize = true;
            this.LblJarId.Location = new System.Drawing.Point(3, 4);
            this.LblJarId.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.LblJarId.Name = "LblJarId";
            this.LblJarId.Size = new System.Drawing.Size(113, 12);
            this.LblJarId.TabIndex = 0;
            this.LblJarId.Text = "JarID 목록에서 선택";
            // 
            // GridDir
            // 
            this.GridDir.AllowUserToAddRows = false;
            this.GridDir.AllowUserToDeleteRows = false;
            this.GridDir.AllowUserToResizeRows = false;
            this.GridDir.BackgroundColor = System.Drawing.Color.White;
            this.GridDir.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridDir.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ClmCheck,
            this.ClmHostName,
            this.ClmHost,
            this.ClmPath});
            this.GridDir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridDir.Location = new System.Drawing.Point(207, 23);
            this.GridDir.Name = "GridDir";
            this.GridDir.RowHeadersVisible = false;
            this.GridDir.RowTemplate.Height = 23;
            this.GridDir.ShowCellErrors = false;
            this.GridDir.ShowCellToolTips = false;
            this.GridDir.ShowEditingIcon = false;
            this.GridDir.ShowRowErrors = false;
            this.GridDir.Size = new System.Drawing.Size(607, 422);
            this.GridDir.TabIndex = 1;
            this.GridDir.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridDir_ColumnHeaderMouseClick);
            // 
            // ClmCheck
            // 
            this.ClmCheck.HeaderText = "";
            this.ClmCheck.Name = "ClmCheck";
            this.ClmCheck.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ClmCheck.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ClmCheck.Width = 30;
            // 
            // ClmHostName
            // 
            this.ClmHostName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ClmHostName.HeaderText = "HostName";
            this.ClmHostName.Name = "ClmHostName";
            this.ClmHostName.ReadOnly = true;
            this.ClmHostName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ClmHostName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ClmHostName.Width = 70;
            // 
            // ClmHost
            // 
            this.ClmHost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ClmHost.HeaderText = "Host";
            this.ClmHost.Name = "ClmHost";
            this.ClmHost.ReadOnly = true;
            this.ClmHost.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ClmHost.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ClmHost.Width = 36;
            // 
            // ClmPath
            // 
            this.ClmPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ClmPath.HeaderText = "Path";
            this.ClmPath.Name = "ClmPath";
            this.ClmPath.ReadOnly = true;
            this.ClmPath.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ClmPath.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ClmPath.Width = 36;
            // 
            // txtPathList
            // 
            this.txtPathList.AutoSize = true;
            this.txtPathList.Location = new System.Drawing.Point(207, 4);
            this.txtPathList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.txtPathList.Name = "txtPathList";
            this.txtPathList.Size = new System.Drawing.Size(165, 12);
            this.txtPathList.TabIndex = 2;
            this.txtPathList.Text = "업로드할 서버 및 경로를 선택";
            // 
            // contextMenuStripLog
            // 
            this.contextMenuStripLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemLogClear,
            this.ToolStripMenuItemCopy});
            this.contextMenuStripLog.Name = "contextMenuStripLog";
            this.contextMenuStripLog.Size = new System.Drawing.Size(182, 48);
            // 
            // ToolStripMenuItemLogClear
            // 
            this.ToolStripMenuItemLogClear.Name = "ToolStripMenuItemLogClear";
            this.ToolStripMenuItemLogClear.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.ToolStripMenuItemLogClear.Size = new System.Drawing.Size(181, 22);
            this.ToolStripMenuItemLogClear.Text = "로그 지우기";
            this.ToolStripMenuItemLogClear.Click += new System.EventHandler(this.ToolStripMenuItemLogClear_Click);
            // 
            // ToolStripMenuItemCopy
            // 
            this.ToolStripMenuItemCopy.Name = "ToolStripMenuItemCopy";
            this.ToolStripMenuItemCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.ToolStripMenuItemCopy.Size = new System.Drawing.Size(181, 22);
            this.ToolStripMenuItemCopy.Text = "복사";
            this.ToolStripMenuItemCopy.Click += new System.EventHandler(this.ToolStripMenuItemCopy_Click);
            // 
            // GridTask
            // 
            this.GridTask.AllowUserToAddRows = false;
            this.GridTask.AllowUserToDeleteRows = false;
            this.GridTask.AllowUserToResizeRows = false;
            this.GridTask.BackgroundColor = System.Drawing.Color.White;
            this.GridTask.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridTask.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ClmTaskHost,
            this.ClmTaskPath,
            this.ClmTask});
            this.GridTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridTask.Location = new System.Drawing.Point(820, 23);
            this.GridTask.Name = "GridTask";
            this.GridTask.ReadOnly = true;
            this.GridTask.RowHeadersVisible = false;
            this.GridTask.RowTemplate.Height = 23;
            this.GridTask.Size = new System.Drawing.Size(541, 422);
            this.GridTask.TabIndex = 2;
            // 
            // ClmTaskHost
            // 
            this.ClmTaskHost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ClmTaskHost.HeaderText = "Host";
            this.ClmTaskHost.Name = "ClmTaskHost";
            this.ClmTaskHost.ReadOnly = true;
            this.ClmTaskHost.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ClmTaskHost.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ClmTaskHost.Width = 36;
            // 
            // ClmTaskPath
            // 
            this.ClmTaskPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ClmTaskPath.HeaderText = "Path";
            this.ClmTaskPath.Name = "ClmTaskPath";
            this.ClmTaskPath.ReadOnly = true;
            this.ClmTaskPath.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ClmTaskPath.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ClmTaskPath.Width = 36;
            // 
            // ClmTask
            // 
            this.ClmTask.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ClmTask.HeaderText = "Task";
            this.ClmTask.Name = "ClmTask";
            this.ClmTask.ReadOnly = true;
            this.ClmTask.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ClmTask.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ClmTask.Width = 39;
            // 
            // lblTaskList
            // 
            this.lblTaskList.AutoSize = true;
            this.lblTaskList.Location = new System.Drawing.Point(820, 4);
            this.lblTaskList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.lblTaskList.Name = "lblTaskList";
            this.lblTaskList.Size = new System.Drawing.Size(109, 12);
            this.lblTaskList.TabIndex = 5;
            this.lblTaskList.Text = "현재 작업중인 목록";
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 3;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelMain.Controls.Add(this.LblJarId, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.txtPathList, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.ListJarId, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.GridDir, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.GridTask, 2, 1);
            this.tableLayoutPanelMain.Controls.Add(this.lblTaskList, 2, 0);
            this.tableLayoutPanelMain.Controls.Add(this.splitContainerLow, 0, 2);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(1364, 591);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // splitContainerLow
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.splitContainerLow, 3);
            this.splitContainerLow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLow.IsSplitterFixed = true;
            this.splitContainerLow.Location = new System.Drawing.Point(3, 451);
            this.splitContainerLow.Name = "splitContainerLow";
            // 
            // splitContainerLow.Panel1
            // 
            this.splitContainerLow.Panel1.AutoScroll = true;
            this.splitContainerLow.Panel1.Controls.Add(this.ckForceKill);
            this.splitContainerLow.Panel1.Controls.Add(this.btnPopupPath);
            this.splitContainerLow.Panel1.Controls.Add(this.btnTaskStart);
            this.splitContainerLow.Panel1.Controls.Add(this.txtLocalPath);
            this.splitContainerLow.Panel1.Controls.Add(this.ckRun);
            this.splitContainerLow.Panel1.Controls.Add(this.lblLocalPath);
            // 
            // splitContainerLow.Panel2
            // 
            this.splitContainerLow.Panel2.Controls.Add(this.txtLog);
            this.splitContainerLow.Size = new System.Drawing.Size(1358, 137);
            this.splitContainerLow.SplitterDistance = 450;
            this.splitContainerLow.TabIndex = 3;
            // 
            // ckForceKill
            // 
            this.ckForceKill.AutoSize = true;
            this.ckForceKill.Location = new System.Drawing.Point(9, 12);
            this.ckForceKill.Name = "ckForceKill";
            this.ckForceKill.Size = new System.Drawing.Size(124, 16);
            this.ckForceKill.TabIndex = 0;
            this.ckForceKill.Text = "대기없이 강제종료";
            this.ckForceKill.UseVisualStyleBackColor = true;
            // 
            // btnPopupPath
            // 
            this.btnPopupPath.Location = new System.Drawing.Point(419, 61);
            this.btnPopupPath.Name = "btnPopupPath";
            this.btnPopupPath.Size = new System.Drawing.Size(29, 23);
            this.btnPopupPath.TabIndex = 3;
            this.btnPopupPath.Text = "...";
            this.btnPopupPath.UseVisualStyleBackColor = true;
            this.btnPopupPath.Click += new System.EventHandler(this.BtnPopupPath_Click);
            // 
            // btnTaskStart
            // 
            this.btnTaskStart.Location = new System.Drawing.Point(9, 90);
            this.btnTaskStart.Name = "btnTaskStart";
            this.btnTaskStart.Size = new System.Drawing.Size(134, 23);
            this.btnTaskStart.TabIndex = 4;
            this.btnTaskStart.Text = "작업 시작";
            this.btnTaskStart.UseVisualStyleBackColor = true;
            this.btnTaskStart.Click += new System.EventHandler(this.BtnTaskStart_Click);
            // 
            // txtLocalPath
            // 
            this.txtLocalPath.Location = new System.Drawing.Point(9, 61);
            this.txtLocalPath.Name = "txtLocalPath";
            this.txtLocalPath.Size = new System.Drawing.Size(404, 21);
            this.txtLocalPath.TabIndex = 2;
            // 
            // ckRun
            // 
            this.ckRun.AutoSize = true;
            this.ckRun.Location = new System.Drawing.Point(139, 12);
            this.ckRun.Name = "ckRun";
            this.ckRun.Size = new System.Drawing.Size(148, 16);
            this.ckRun.TabIndex = 1;
            this.ckRun.Text = "작업 완료 후 바로 실행";
            this.ckRun.UseVisualStyleBackColor = true;
            // 
            // lblLocalPath
            // 
            this.lblLocalPath.AutoSize = true;
            this.lblLocalPath.Location = new System.Drawing.Point(7, 46);
            this.lblLocalPath.Name = "lblLocalPath";
            this.lblLocalPath.Size = new System.Drawing.Size(97, 12);
            this.lblLocalPath.TabIndex = 2;
            this.lblLocalPath.Text = "업로드 파일 경로";
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.White;
            this.txtLog.ContextMenuStrip = this.contextMenuStripLog;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Location = new System.Drawing.Point(0, 0);
            this.txtLog.MaxLength = 1000000;
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(904, 137);
            this.txtLog.TabIndex = 0;
            // 
            // timerSec
            // 
            this.timerSec.Enabled = true;
            this.timerSec.Interval = 1000;
            this.timerSec.Tick += new System.EventHandler(this.TimerSec_Tick);
            // 
            // FrmRemoteJarMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1364, 591);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.MinimumSize = new System.Drawing.Size(1380, 630);
            this.Name = "FrmRemoteJarMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RemoteJar";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmRemoteJarMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmRemoteJarMain_FormClosed);
            this.Load += new System.EventHandler(this.FrmRemoteJarMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GridDir)).EndInit();
            this.contextMenuStripLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridTask)).EndInit();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.splitContainerLow.Panel1.ResumeLayout(false);
            this.splitContainerLow.Panel1.PerformLayout();
            this.splitContainerLow.Panel2.ResumeLayout(false);
            this.splitContainerLow.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLow)).EndInit();
            this.splitContainerLow.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ListJarId;
        private System.Windows.Forms.Label LblJarId;
        private System.Windows.Forms.DataGridView GridDir;
        private System.Windows.Forms.Label txtPathList;
        private System.Windows.Forms.DataGridView GridTask;
        private System.Windows.Forms.Label lblTaskList;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ClmCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClmHostName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClmHost;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClmPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClmTaskHost;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClmTaskPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClmTask;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripLog;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemLogClear;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCopy;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.SplitContainer splitContainerLow;
        private System.Windows.Forms.CheckBox ckForceKill;
        private System.Windows.Forms.Button btnPopupPath;
        private System.Windows.Forms.Button btnTaskStart;
        private System.Windows.Forms.TextBox txtLocalPath;
        private System.Windows.Forms.CheckBox ckRun;
        private System.Windows.Forms.Label lblLocalPath;
        private System.Windows.Forms.Timer timerSec;
    }
}

