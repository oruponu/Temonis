using Temonis.Controls;

namespace Temonis
{
    partial class MainWindow
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.GroupBox_EqInfo = new Temonis.Controls.GroupBox();
            this.DataGridView_EqInfoIntensity = new Temonis.Controls.DataGridView();
            this.intensity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pref = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.city = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Label_EqInfoMessage = new Temonis.Controls.Label();
            this.Label_EqInfoMagnitude = new Temonis.Controls.Label();
            this.label_EqInfoMagnitudeHeader = new Temonis.Controls.Label();
            this.Label_EqInfoDepth = new Temonis.Controls.Label();
            this.label_EqInfoDepthHeader = new Temonis.Controls.Label();
            this.Label_EqInfoEpicenter = new Temonis.Controls.Label();
            this.label_EqInfoEpicenterHeader = new Temonis.Controls.Label();
            this.Label_EqInfoDateTime = new Temonis.Controls.Label();
            this.label_EqInfoDateTimeHeader = new Temonis.Controls.Label();
            this.GroupBox_EEW = new Temonis.Controls.GroupBox();
            this.Label_EEWIntensity = new Temonis.Controls.Label();
            this.Label_EEWIntensityHeader = new Temonis.Controls.Label();
            this.Label_EEWMagnitude = new Temonis.Controls.Label();
            this.Label_EEWMagnitudeHeader = new Temonis.Controls.Label();
            this.Label_EEWDepth = new Temonis.Controls.Label();
            this.Label_EEWDepthHeader = new Temonis.Controls.Label();
            this.Label_EEWEpicenter = new Temonis.Controls.Label();
            this.Label_EEWEpicenterHeader = new Temonis.Controls.Label();
            this.Label_EEWDateTime = new Temonis.Controls.Label();
            this.Label_EEWDateTimeHeader = new Temonis.Controls.Label();
            this.Label_EEWMessage = new Temonis.Controls.Label();
            this.GroupBox_Kyoshin = new Temonis.Controls.GroupBox();
            this.Label_KyoshinPrefecture = new Temonis.Controls.Label();
            this.radioButton_Borehore = new System.Windows.Forms.RadioButton();
            this.RadioButton_Surface = new System.Windows.Forms.RadioButton();
            this.label_KyoshinLatestTime = new Temonis.Controls.Label();
            this.ComboBox_MapType = new System.Windows.Forms.ComboBox();
            this.Label_KyoshinMaxIntDetail = new Temonis.Controls.Label();
            this.Label_KyoshinMaxInt = new Temonis.Controls.Label();
            this.label_KyoshinMaxIntHeader = new Temonis.Controls.Label();
            this.PictureBox_KyoshinMap = new Temonis.Controls.PictureBox();
            this.GroupBox_EqInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_EqInfoIntensity)).BeginInit();
            this.GroupBox_EEW.SuspendLayout();
            this.GroupBox_Kyoshin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_KyoshinMap)).BeginInit();
            this.SuspendLayout();
            // 
            // GroupBox_EqInfo
            // 
            this.GroupBox_EqInfo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.GroupBox_EqInfo.Controls.Add(this.DataGridView_EqInfoIntensity);
            this.GroupBox_EqInfo.Controls.Add(this.Label_EqInfoMessage);
            this.GroupBox_EqInfo.Controls.Add(this.Label_EqInfoMagnitude);
            this.GroupBox_EqInfo.Controls.Add(this.label_EqInfoMagnitudeHeader);
            this.GroupBox_EqInfo.Controls.Add(this.Label_EqInfoDepth);
            this.GroupBox_EqInfo.Controls.Add(this.label_EqInfoDepthHeader);
            this.GroupBox_EqInfo.Controls.Add(this.Label_EqInfoEpicenter);
            this.GroupBox_EqInfo.Controls.Add(this.label_EqInfoEpicenterHeader);
            this.GroupBox_EqInfo.Controls.Add(this.Label_EqInfoDateTime);
            this.GroupBox_EqInfo.Controls.Add(this.label_EqInfoDateTimeHeader);
            this.GroupBox_EqInfo.Location = new System.Drawing.Point(420, 186);
            this.GroupBox_EqInfo.Name = "GroupBox_EqInfo";
            this.GroupBox_EqInfo.Size = new System.Drawing.Size(512, 338);
            this.GroupBox_EqInfo.TabIndex = 3;
            this.GroupBox_EqInfo.TabStop = false;
            this.GroupBox_EqInfo.Text = "地震情報";
            // 
            // DataGridView_EqInfoIntensity
            // 
            this.DataGridView_EqInfoIntensity.AllowUserToAddRows = false;
            this.DataGridView_EqInfoIntensity.AllowUserToResizeColumns = false;
            this.DataGridView_EqInfoIntensity.AllowUserToResizeRows = false;
            this.DataGridView_EqInfoIntensity.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DataGridView_EqInfoIntensity.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(116)))), ((int)(((byte)(116)))));
            this.DataGridView_EqInfoIntensity.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.DataGridView_EqInfoIntensity.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Meiryo UI", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DataGridView_EqInfoIntensity.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.DataGridView_EqInfoIntensity.ColumnHeadersVisible = false;
            this.DataGridView_EqInfoIntensity.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.intensity,
            this.pref,
            this.city});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Meiryo UI", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridView_EqInfoIntensity.DefaultCellStyle = dataGridViewCellStyle3;
            this.DataGridView_EqInfoIntensity.Location = new System.Drawing.Point(20, 147);
            this.DataGridView_EqInfoIntensity.Name = "DataGridView_EqInfoIntensity";
            this.DataGridView_EqInfoIntensity.ReadOnly = true;
            this.DataGridView_EqInfoIntensity.RowHeadersVisible = false;
            this.DataGridView_EqInfoIntensity.RowTemplate.Height = 21;
            this.DataGridView_EqInfoIntensity.Size = new System.Drawing.Size(472, 173);
            this.DataGridView_EqInfoIntensity.TabIndex = 0;
            this.DataGridView_EqInfoIntensity.TabStop = false;
            this.DataGridView_EqInfoIntensity.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.DataGridView_EqInfoIntensity_RowPostPaint);
            // 
            // intensity
            // 
            this.intensity.HeaderText = "";
            this.intensity.Name = "intensity";
            this.intensity.ReadOnly = true;
            this.intensity.Width = 52;
            // 
            // pref
            // 
            this.pref.HeaderText = "";
            this.pref.Name = "pref";
            this.pref.ReadOnly = true;
            this.pref.Width = 58;
            // 
            // city
            // 
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.city.DefaultCellStyle = dataGridViewCellStyle2;
            this.city.HeaderText = "";
            this.city.Name = "city";
            this.city.ReadOnly = true;
            // 
            // Label_EqInfoMessage
            // 
            this.Label_EqInfoMessage.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.Label_EqInfoMessage.Location = new System.Drawing.Point(20, 104);
            this.Label_EqInfoMessage.Name = "Label_EqInfoMessage";
            this.Label_EqInfoMessage.Size = new System.Drawing.Size(472, 40);
            this.Label_EqInfoMessage.TabIndex = 0;
            this.Label_EqInfoMessage.Text = "情報";
            // 
            // Label_EqInfoMagnitude
            // 
            this.Label_EqInfoMagnitude.AutoSize = true;
            this.Label_EqInfoMagnitude.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.Label_EqInfoMagnitude.Location = new System.Drawing.Point(130, 84);
            this.Label_EqInfoMagnitude.Name = "Label_EqInfoMagnitude";
            this.Label_EqInfoMagnitude.Size = new System.Drawing.Size(51, 20);
            this.Label_EqInfoMagnitude.TabIndex = 0;
            this.Label_EqInfoMagnitude.Text = "NULL";
            // 
            // label_EqInfoMagnitudeHeader
            // 
            this.label_EqInfoMagnitudeHeader.AutoSize = true;
            this.label_EqInfoMagnitudeHeader.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_EqInfoMagnitudeHeader.Location = new System.Drawing.Point(20, 84);
            this.label_EqInfoMagnitudeHeader.Name = "label_EqInfoMagnitudeHeader";
            this.label_EqInfoMagnitudeHeader.Size = new System.Drawing.Size(93, 20);
            this.label_EqInfoMagnitudeHeader.TabIndex = 0;
            this.label_EqInfoMagnitudeHeader.Text = "マグニチュード";
            // 
            // Label_EqInfoDepth
            // 
            this.Label_EqInfoDepth.AutoSize = true;
            this.Label_EqInfoDepth.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.Label_EqInfoDepth.Location = new System.Drawing.Point(130, 64);
            this.Label_EqInfoDepth.Name = "Label_EqInfoDepth";
            this.Label_EqInfoDepth.Size = new System.Drawing.Size(51, 20);
            this.Label_EqInfoDepth.TabIndex = 0;
            this.Label_EqInfoDepth.Text = "NULL";
            // 
            // label_EqInfoDepthHeader
            // 
            this.label_EqInfoDepthHeader.AutoSize = true;
            this.label_EqInfoDepthHeader.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_EqInfoDepthHeader.Location = new System.Drawing.Point(20, 64);
            this.label_EqInfoDepthHeader.Name = "label_EqInfoDepthHeader";
            this.label_EqInfoDepthHeader.Size = new System.Drawing.Size(81, 20);
            this.label_EqInfoDepthHeader.TabIndex = 0;
            this.label_EqInfoDepthHeader.Text = "震源の深さ";
            // 
            // Label_EqInfoEpicenter
            // 
            this.Label_EqInfoEpicenter.AutoSize = true;
            this.Label_EqInfoEpicenter.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.Label_EqInfoEpicenter.Location = new System.Drawing.Point(130, 44);
            this.Label_EqInfoEpicenter.Name = "Label_EqInfoEpicenter";
            this.Label_EqInfoEpicenter.Size = new System.Drawing.Size(51, 20);
            this.Label_EqInfoEpicenter.TabIndex = 0;
            this.Label_EqInfoEpicenter.Text = "NULL";
            // 
            // label_EqInfoEpicenterHeader
            // 
            this.label_EqInfoEpicenterHeader.AutoSize = true;
            this.label_EqInfoEpicenterHeader.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_EqInfoEpicenterHeader.Location = new System.Drawing.Point(20, 44);
            this.label_EqInfoEpicenterHeader.Name = "label_EqInfoEpicenterHeader";
            this.label_EqInfoEpicenterHeader.Size = new System.Drawing.Size(57, 20);
            this.label_EqInfoEpicenterHeader.TabIndex = 0;
            this.label_EqInfoEpicenterHeader.Text = "震源地";
            // 
            // Label_EqInfoDateTime
            // 
            this.Label_EqInfoDateTime.AutoSize = true;
            this.Label_EqInfoDateTime.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.Label_EqInfoDateTime.Location = new System.Drawing.Point(130, 24);
            this.Label_EqInfoDateTime.Name = "Label_EqInfoDateTime";
            this.Label_EqInfoDateTime.Size = new System.Drawing.Size(51, 20);
            this.Label_EqInfoDateTime.TabIndex = 0;
            this.Label_EqInfoDateTime.Text = "NULL";
            // 
            // label_EqInfoDateTimeHeader
            // 
            this.label_EqInfoDateTimeHeader.AutoSize = true;
            this.label_EqInfoDateTimeHeader.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_EqInfoDateTimeHeader.Location = new System.Drawing.Point(20, 24);
            this.label_EqInfoDateTimeHeader.Name = "label_EqInfoDateTimeHeader";
            this.label_EqInfoDateTimeHeader.Size = new System.Drawing.Size(73, 20);
            this.label_EqInfoDateTimeHeader.TabIndex = 0;
            this.label_EqInfoDateTimeHeader.Text = "発生時刻";
            // 
            // GroupBox_EEW
            // 
            this.GroupBox_EEW.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.GroupBox_EEW.Controls.Add(this.Label_EEWIntensity);
            this.GroupBox_EEW.Controls.Add(this.Label_EEWIntensityHeader);
            this.GroupBox_EEW.Controls.Add(this.Label_EEWMagnitude);
            this.GroupBox_EEW.Controls.Add(this.Label_EEWMagnitudeHeader);
            this.GroupBox_EEW.Controls.Add(this.Label_EEWDepth);
            this.GroupBox_EEW.Controls.Add(this.Label_EEWDepthHeader);
            this.GroupBox_EEW.Controls.Add(this.Label_EEWEpicenter);
            this.GroupBox_EEW.Controls.Add(this.Label_EEWEpicenterHeader);
            this.GroupBox_EEW.Controls.Add(this.Label_EEWDateTime);
            this.GroupBox_EEW.Controls.Add(this.Label_EEWDateTimeHeader);
            this.GroupBox_EEW.Controls.Add(this.Label_EEWMessage);
            this.GroupBox_EEW.Location = new System.Drawing.Point(420, 8);
            this.GroupBox_EEW.Name = "GroupBox_EEW";
            this.GroupBox_EEW.Size = new System.Drawing.Size(512, 158);
            this.GroupBox_EEW.TabIndex = 2;
            this.GroupBox_EEW.TabStop = false;
            this.GroupBox_EEW.Text = "緊急地震速報";
            // 
            // Label_EEWIntensity
            // 
            this.Label_EEWIntensity.AutoSize = true;
            this.Label_EEWIntensity.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.Label_EEWIntensity.Location = new System.Drawing.Point(130, 124);
            this.Label_EEWIntensity.Name = "Label_EEWIntensity";
            this.Label_EEWIntensity.Size = new System.Drawing.Size(51, 20);
            this.Label_EEWIntensity.TabIndex = 0;
            this.Label_EEWIntensity.Text = "NULL";
            // 
            // Label_EEWIntensityHeader
            // 
            this.Label_EEWIntensityHeader.AutoSize = true;
            this.Label_EEWIntensityHeader.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.Label_EEWIntensityHeader.Location = new System.Drawing.Point(20, 124);
            this.Label_EEWIntensityHeader.Name = "Label_EEWIntensityHeader";
            this.Label_EEWIntensityHeader.Size = new System.Drawing.Size(105, 20);
            this.Label_EEWIntensityHeader.TabIndex = 0;
            this.Label_EEWIntensityHeader.Text = "最大予測震度";
            // 
            // Label_EEWMagnitude
            // 
            this.Label_EEWMagnitude.AutoSize = true;
            this.Label_EEWMagnitude.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.Label_EEWMagnitude.Location = new System.Drawing.Point(130, 104);
            this.Label_EEWMagnitude.Name = "Label_EEWMagnitude";
            this.Label_EEWMagnitude.Size = new System.Drawing.Size(51, 20);
            this.Label_EEWMagnitude.TabIndex = 0;
            this.Label_EEWMagnitude.Text = "NULL";
            // 
            // Label_EEWMagnitudeHeader
            // 
            this.Label_EEWMagnitudeHeader.AutoSize = true;
            this.Label_EEWMagnitudeHeader.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.Label_EEWMagnitudeHeader.Location = new System.Drawing.Point(20, 104);
            this.Label_EEWMagnitudeHeader.Name = "Label_EEWMagnitudeHeader";
            this.Label_EEWMagnitudeHeader.Size = new System.Drawing.Size(93, 20);
            this.Label_EEWMagnitudeHeader.TabIndex = 0;
            this.Label_EEWMagnitudeHeader.Text = "マグニチュード";
            // 
            // Label_EEWDepth
            // 
            this.Label_EEWDepth.AutoSize = true;
            this.Label_EEWDepth.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.Label_EEWDepth.Location = new System.Drawing.Point(130, 84);
            this.Label_EEWDepth.Name = "Label_EEWDepth";
            this.Label_EEWDepth.Size = new System.Drawing.Size(51, 20);
            this.Label_EEWDepth.TabIndex = 0;
            this.Label_EEWDepth.Text = "NULL";
            // 
            // Label_EEWDepthHeader
            // 
            this.Label_EEWDepthHeader.AutoSize = true;
            this.Label_EEWDepthHeader.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.Label_EEWDepthHeader.Location = new System.Drawing.Point(20, 84);
            this.Label_EEWDepthHeader.Name = "Label_EEWDepthHeader";
            this.Label_EEWDepthHeader.Size = new System.Drawing.Size(81, 20);
            this.Label_EEWDepthHeader.TabIndex = 0;
            this.Label_EEWDepthHeader.Text = "震源の深さ";
            // 
            // Label_EEWEpicenter
            // 
            this.Label_EEWEpicenter.AutoSize = true;
            this.Label_EEWEpicenter.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.Label_EEWEpicenter.Location = new System.Drawing.Point(130, 64);
            this.Label_EEWEpicenter.Name = "Label_EEWEpicenter";
            this.Label_EEWEpicenter.Size = new System.Drawing.Size(51, 20);
            this.Label_EEWEpicenter.TabIndex = 0;
            this.Label_EEWEpicenter.Text = "NULL";
            // 
            // Label_EEWEpicenterHeader
            // 
            this.Label_EEWEpicenterHeader.AutoSize = true;
            this.Label_EEWEpicenterHeader.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.Label_EEWEpicenterHeader.Location = new System.Drawing.Point(20, 64);
            this.Label_EEWEpicenterHeader.Name = "Label_EEWEpicenterHeader";
            this.Label_EEWEpicenterHeader.Size = new System.Drawing.Size(57, 20);
            this.Label_EEWEpicenterHeader.TabIndex = 0;
            this.Label_EEWEpicenterHeader.Text = "震源地";
            // 
            // Label_EEWDateTime
            // 
            this.Label_EEWDateTime.AutoSize = true;
            this.Label_EEWDateTime.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.Label_EEWDateTime.Location = new System.Drawing.Point(130, 44);
            this.Label_EEWDateTime.Name = "Label_EEWDateTime";
            this.Label_EEWDateTime.Size = new System.Drawing.Size(51, 20);
            this.Label_EEWDateTime.TabIndex = 0;
            this.Label_EEWDateTime.Text = "NULL";
            // 
            // Label_EEWDateTimeHeader
            // 
            this.Label_EEWDateTimeHeader.AutoSize = true;
            this.Label_EEWDateTimeHeader.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.Label_EEWDateTimeHeader.Location = new System.Drawing.Point(20, 44);
            this.Label_EEWDateTimeHeader.Name = "Label_EEWDateTimeHeader";
            this.Label_EEWDateTimeHeader.Size = new System.Drawing.Size(73, 20);
            this.Label_EEWDateTimeHeader.TabIndex = 0;
            this.Label_EEWDateTimeHeader.Text = "発生時刻";
            // 
            // Label_EEWMessage
            // 
            this.Label_EEWMessage.AutoSize = true;
            this.Label_EEWMessage.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.Label_EEWMessage.Location = new System.Drawing.Point(20, 24);
            this.Label_EEWMessage.Name = "Label_EEWMessage";
            this.Label_EEWMessage.Size = new System.Drawing.Size(252, 20);
            this.Label_EEWMessage.TabIndex = 0;
            this.Label_EEWMessage.Text = "緊急地震速報は発表されていません。";
            // 
            // GroupBox_Kyoshin
            // 
            this.GroupBox_Kyoshin.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.GroupBox_Kyoshin.Controls.Add(this.Label_KyoshinPrefecture);
            this.GroupBox_Kyoshin.Controls.Add(this.radioButton_Borehore);
            this.GroupBox_Kyoshin.Controls.Add(this.RadioButton_Surface);
            this.GroupBox_Kyoshin.Controls.Add(this.label_KyoshinLatestTime);
            this.GroupBox_Kyoshin.Controls.Add(this.ComboBox_MapType);
            this.GroupBox_Kyoshin.Controls.Add(this.Label_KyoshinMaxIntDetail);
            this.GroupBox_Kyoshin.Controls.Add(this.Label_KyoshinMaxInt);
            this.GroupBox_Kyoshin.Controls.Add(this.label_KyoshinMaxIntHeader);
            this.GroupBox_Kyoshin.Controls.Add(this.PictureBox_KyoshinMap);
            this.GroupBox_Kyoshin.Location = new System.Drawing.Point(28, 8);
            this.GroupBox_Kyoshin.Name = "GroupBox_Kyoshin";
            this.GroupBox_Kyoshin.Size = new System.Drawing.Size(376, 516);
            this.GroupBox_Kyoshin.TabIndex = 1;
            this.GroupBox_Kyoshin.TabStop = false;
            this.GroupBox_Kyoshin.Text = "強震モニタ";
            // 
            // Label_KyoshinPrefecture
            // 
            this.Label_KyoshinPrefecture.AutoSize = true;
            this.Label_KyoshinPrefecture.Font = new System.Drawing.Font("Meiryo UI", 8F);
            this.Label_KyoshinPrefecture.Location = new System.Drawing.Point(356, 441);
            this.Label_KyoshinPrefecture.Name = "Label_KyoshinPrefecture";
            this.Label_KyoshinPrefecture.Size = new System.Drawing.Size(0, 14);
            this.Label_KyoshinPrefecture.TabIndex = 0;
            this.Label_KyoshinPrefecture.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // radioButton_Borehore
            // 
            this.radioButton_Borehore.AutoSize = true;
            this.radioButton_Borehore.Location = new System.Drawing.Point(286, 481);
            this.radioButton_Borehore.Name = "radioButton_Borehore";
            this.radioButton_Borehore.Size = new System.Drawing.Size(49, 19);
            this.radioButton_Borehore.TabIndex = 1;
            this.radioButton_Borehore.Text = "地中";
            this.radioButton_Borehore.UseVisualStyleBackColor = true;
            // 
            // RadioButton_Surface
            // 
            this.RadioButton_Surface.AutoSize = true;
            this.RadioButton_Surface.Checked = true;
            this.RadioButton_Surface.Location = new System.Drawing.Point(224, 481);
            this.RadioButton_Surface.Name = "RadioButton_Surface";
            this.RadioButton_Surface.Size = new System.Drawing.Size(49, 19);
            this.RadioButton_Surface.TabIndex = 1;
            this.RadioButton_Surface.TabStop = true;
            this.RadioButton_Surface.Text = "地表";
            this.RadioButton_Surface.UseVisualStyleBackColor = true;
            // 
            // label_KyoshinLatestTime
            // 
            this.label_KyoshinLatestTime.AutoSize = true;
            this.label_KyoshinLatestTime.Font = new System.Drawing.Font("Meiryo UI", 14F);
            this.label_KyoshinLatestTime.Location = new System.Drawing.Point(20, 24);
            this.label_KyoshinLatestTime.Name = "label_KyoshinLatestTime";
            this.label_KyoshinLatestTime.Size = new System.Drawing.Size(144, 24);
            this.label_KyoshinLatestTime.TabIndex = 0;
            this.label_KyoshinLatestTime.Text = "接続しています...";
            // 
            // ComboBox_MapType
            // 
            this.ComboBox_MapType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_MapType.FormattingEnabled = true;
            this.ComboBox_MapType.Items.AddRange(new object[] {
            "リアルタイム震度",
            "最大加速度",
            "最大速度",
            "最大変位",
            "0.125Hz速度応答",
            "0.25Hz速度応答",
            "0.5Hz速度応答",
            "1.0Hz速度応答",
            "2.0Hz速度応答",
            "4.0Hz速度応答"});
            this.ComboBox_MapType.Location = new System.Drawing.Point(24, 478);
            this.ComboBox_MapType.Name = "ComboBox_MapType";
            this.ComboBox_MapType.Size = new System.Drawing.Size(152, 23);
            this.ComboBox_MapType.TabIndex = 0;
            // 
            // Label_KyoshinMaxIntDetail
            // 
            this.Label_KyoshinMaxIntDetail.AutoSize = true;
            this.Label_KyoshinMaxIntDetail.Font = new System.Drawing.Font("Meiryo UI", 8F);
            this.Label_KyoshinMaxIntDetail.Location = new System.Drawing.Point(111, 93);
            this.Label_KyoshinMaxIntDetail.Name = "Label_KyoshinMaxIntDetail";
            this.Label_KyoshinMaxIntDetail.Size = new System.Drawing.Size(55, 14);
            this.Label_KyoshinMaxIntDetail.TabIndex = 0;
            this.Label_KyoshinMaxIntDetail.Text = "他 0 地点";
            // 
            // Label_KyoshinMaxInt
            // 
            this.Label_KyoshinMaxInt.AutoSize = true;
            this.Label_KyoshinMaxInt.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.Label_KyoshinMaxInt.Location = new System.Drawing.Point(38, 88);
            this.Label_KyoshinMaxInt.Name = "Label_KyoshinMaxInt";
            this.Label_KyoshinMaxInt.Size = new System.Drawing.Size(25, 20);
            this.Label_KyoshinMaxInt.TabIndex = 0;
            this.Label_KyoshinMaxInt.Text = "―";
            // 
            // label_KyoshinMaxIntHeader
            // 
            this.label_KyoshinMaxIntHeader.AutoSize = true;
            this.label_KyoshinMaxIntHeader.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_KyoshinMaxIntHeader.Location = new System.Drawing.Point(16, 68);
            this.label_KyoshinMaxIntHeader.Name = "label_KyoshinMaxIntHeader";
            this.label_KyoshinMaxIntHeader.Size = new System.Drawing.Size(73, 20);
            this.label_KyoshinMaxIntHeader.TabIndex = 0;
            this.label_KyoshinMaxIntHeader.Text = "地表震度";
            // 
            // PictureBox_KyoshinMap
            // 
            this.PictureBox_KyoshinMap.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(116)))), ((int)(((byte)(116)))));
            this.PictureBox_KyoshinMap.Location = new System.Drawing.Point(12, 64);
            this.PictureBox_KyoshinMap.Name = "PictureBox_KyoshinMap";
            this.PictureBox_KyoshinMap.Size = new System.Drawing.Size(352, 400);
            this.PictureBox_KyoshinMap.TabIndex = 0;
            this.PictureBox_KyoshinMap.TabStop = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 540);
            this.Controls.Add(this.GroupBox_EqInfo);
            this.Controls.Add(this.GroupBox_EEW);
            this.Controls.Add(this.GroupBox_Kyoshin);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Temonis";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.GroupBox_EqInfo.ResumeLayout(false);
            this.GroupBox_EqInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView_EqInfoIntensity)).EndInit();
            this.GroupBox_EEW.ResumeLayout(false);
            this.GroupBox_EEW.PerformLayout();
            this.GroupBox_Kyoshin.ResumeLayout(false);
            this.GroupBox_Kyoshin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox_KyoshinMap)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        internal Temonis.Controls.Label Label_EqInfoMessage;
        internal Temonis.Controls.Label Label_EqInfoMagnitude;
        private Temonis.Controls.Label label_EqInfoMagnitudeHeader;
        internal Temonis.Controls.Label Label_EqInfoDepth;
        private Temonis.Controls.Label label_EqInfoDepthHeader;
        internal Temonis.Controls.Label Label_EqInfoEpicenter;
        private Temonis.Controls.Label label_EqInfoEpicenterHeader;
        private Temonis.Controls.Label label_EqInfoDateTimeHeader;
        internal Temonis.Controls.Label Label_EqInfoDateTime;
        internal Temonis.Controls.Label Label_EEWIntensity;
        internal Temonis.Controls.Label Label_EEWIntensityHeader;
        internal Temonis.Controls.Label Label_EEWMagnitude;
        internal Temonis.Controls.Label Label_EEWMagnitudeHeader;
        internal Temonis.Controls.Label Label_EEWDepth;
        internal Temonis.Controls.Label Label_EEWDepthHeader;
        internal Temonis.Controls.Label Label_EEWEpicenter;
        internal Temonis.Controls.Label Label_EEWEpicenterHeader;
        internal Temonis.Controls.Label Label_EEWDateTime;
        internal Temonis.Controls.Label Label_EEWDateTimeHeader;
        internal Temonis.Controls.Label Label_EEWMessage;
        internal Temonis.Controls.Label Label_KyoshinPrefecture;
        private System.Windows.Forms.RadioButton radioButton_Borehore;
        internal System.Windows.Forms.RadioButton RadioButton_Surface;
        internal System.Windows.Forms.ComboBox ComboBox_MapType;
        internal Temonis.Controls.Label Label_KyoshinMaxIntDetail;
        internal Temonis.Controls.Label Label_KyoshinMaxInt;
        private Temonis.Controls.Label label_KyoshinMaxIntHeader;
        internal Temonis.Controls.PictureBox PictureBox_KyoshinMap;
        private Temonis.Controls.Label label_KyoshinLatestTime;
        internal Temonis.Controls.GroupBox GroupBox_EqInfo;
        internal Temonis.Controls.GroupBox GroupBox_EEW;
        internal Temonis.Controls.GroupBox GroupBox_Kyoshin;
        internal Temonis.Controls.DataGridView DataGridView_EqInfoIntensity;
        private System.Windows.Forms.DataGridViewTextBoxColumn intensity;
        private System.Windows.Forms.DataGridViewTextBoxColumn pref;
        private System.Windows.Forms.DataGridViewTextBoxColumn city;
    }
}
