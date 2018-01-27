using System.Windows.Forms;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.groupBox_EqInfo = new Temonis.GroupBox();
            this.textBox_eqInfoIntensity = new System.Windows.Forms.TextBox();
            this.label_eqinfoMessage = new System.Windows.Forms.Label();
            this.label_eqinfoMagnitude = new System.Windows.Forms.Label();
            this.label_eqinfoMagnitudeHeader = new System.Windows.Forms.Label();
            this.label_eqinfoDepth = new System.Windows.Forms.Label();
            this.label_eqinfoDepthHeader = new System.Windows.Forms.Label();
            this.label_eqinfoEpicenter = new System.Windows.Forms.Label();
            this.label_eqinfoEpicenterHeader = new System.Windows.Forms.Label();
            this.label_eqinfoTime = new System.Windows.Forms.Label();
            this.label_eqinfoTimeHeader = new System.Windows.Forms.Label();
            this.groupBox_EEW = new Temonis.GroupBox();
            this.label_eewIntensity = new System.Windows.Forms.Label();
            this.label_eewIntensityHeader = new System.Windows.Forms.Label();
            this.label_eewMagnitude = new System.Windows.Forms.Label();
            this.label_eewMagnitudeHeader = new System.Windows.Forms.Label();
            this.label_eewDepth = new System.Windows.Forms.Label();
            this.label_eewDepthHeader = new System.Windows.Forms.Label();
            this.label_eewEpicenter = new System.Windows.Forms.Label();
            this.label_eewEpicenterHeader = new System.Windows.Forms.Label();
            this.label_eewTime = new System.Windows.Forms.Label();
            this.label_eewTimeHeader = new System.Windows.Forms.Label();
            this.label_eewMessage = new System.Windows.Forms.Label();
            this.groupBox_Kyoshin = new Temonis.GroupBox();
            this.label_kyoshinPrefecture = new System.Windows.Forms.Label();
            this.radioButton_Borehore = new System.Windows.Forms.RadioButton();
            this.radioButton_Surface = new System.Windows.Forms.RadioButton();
            this.label_kyoshinLatestTime = new System.Windows.Forms.Label();
            this.comboBox_MapType = new System.Windows.Forms.ComboBox();
            this.label_kyoshinMaxIntDetail = new System.Windows.Forms.Label();
            this.label_kyoshinMaxInt = new System.Windows.Forms.Label();
            this.label_kyoshinMaxIntHeader = new System.Windows.Forms.Label();
            this.pictureBox_kyoshinMap = new System.Windows.Forms.PictureBox();
            this.groupBox_EqInfo.SuspendLayout();
            this.groupBox_EEW.SuspendLayout();
            this.groupBox_Kyoshin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_kyoshinMap)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox_EqInfo
            // 
            this.groupBox_EqInfo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.groupBox_EqInfo.Controls.Add(this.textBox_eqInfoIntensity);
            this.groupBox_EqInfo.Controls.Add(this.label_eqinfoMessage);
            this.groupBox_EqInfo.Controls.Add(this.label_eqinfoMagnitude);
            this.groupBox_EqInfo.Controls.Add(this.label_eqinfoMagnitudeHeader);
            this.groupBox_EqInfo.Controls.Add(this.label_eqinfoDepth);
            this.groupBox_EqInfo.Controls.Add(this.label_eqinfoDepthHeader);
            this.groupBox_EqInfo.Controls.Add(this.label_eqinfoEpicenter);
            this.groupBox_EqInfo.Controls.Add(this.label_eqinfoEpicenterHeader);
            this.groupBox_EqInfo.Controls.Add(this.label_eqinfoTime);
            this.groupBox_EqInfo.Controls.Add(this.label_eqinfoTimeHeader);
            this.groupBox_EqInfo.Location = new System.Drawing.Point(420, 186);
            this.groupBox_EqInfo.Name = "groupBox_EqInfo";
            this.groupBox_EqInfo.Size = new System.Drawing.Size(512, 338);
            this.groupBox_EqInfo.TabIndex = 3;
            this.groupBox_EqInfo.TabStop = false;
            this.groupBox_EqInfo.Text = "地震情報";
            // 
            // textBox_eqInfoIntensity
            // 
            this.textBox_eqInfoIntensity.Location = new System.Drawing.Point(20, 147);
            this.textBox_eqInfoIntensity.Multiline = true;
            this.textBox_eqInfoIntensity.Name = "textBox_eqInfoIntensity";
            this.textBox_eqInfoIntensity.ReadOnly = true;
            this.textBox_eqInfoIntensity.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_eqInfoIntensity.Size = new System.Drawing.Size(472, 160);
            this.textBox_eqInfoIntensity.TabIndex = 0;
            // 
            // label_eqinfoMessage
            // 
            this.label_eqinfoMessage.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eqinfoMessage.Location = new System.Drawing.Point(20, 104);
            this.label_eqinfoMessage.Name = "label_eqinfoMessage";
            this.label_eqinfoMessage.Size = new System.Drawing.Size(472, 40);
            this.label_eqinfoMessage.TabIndex = 0;
            this.label_eqinfoMessage.Text = "情報";
            // 
            // label_eqinfoMagnitude
            // 
            this.label_eqinfoMagnitude.AutoSize = true;
            this.label_eqinfoMagnitude.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eqinfoMagnitude.Location = new System.Drawing.Point(132, 84);
            this.label_eqinfoMagnitude.Name = "label_eqinfoMagnitude";
            this.label_eqinfoMagnitude.Size = new System.Drawing.Size(51, 20);
            this.label_eqinfoMagnitude.TabIndex = 0;
            this.label_eqinfoMagnitude.Text = "NULL";
            // 
            // label_eqinfoMagnitudeHeader
            // 
            this.label_eqinfoMagnitudeHeader.AutoSize = true;
            this.label_eqinfoMagnitudeHeader.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eqinfoMagnitudeHeader.Location = new System.Drawing.Point(20, 84);
            this.label_eqinfoMagnitudeHeader.Name = "label_eqinfoMagnitudeHeader";
            this.label_eqinfoMagnitudeHeader.Size = new System.Drawing.Size(93, 20);
            this.label_eqinfoMagnitudeHeader.TabIndex = 0;
            this.label_eqinfoMagnitudeHeader.Text = "マグニチュード";
            // 
            // label_eqinfoDepth
            // 
            this.label_eqinfoDepth.AutoSize = true;
            this.label_eqinfoDepth.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eqinfoDepth.Location = new System.Drawing.Point(132, 64);
            this.label_eqinfoDepth.Name = "label_eqinfoDepth";
            this.label_eqinfoDepth.Size = new System.Drawing.Size(51, 20);
            this.label_eqinfoDepth.TabIndex = 0;
            this.label_eqinfoDepth.Text = "NULL";
            // 
            // label_eqinfoDepthHeader
            // 
            this.label_eqinfoDepthHeader.AutoSize = true;
            this.label_eqinfoDepthHeader.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eqinfoDepthHeader.Location = new System.Drawing.Point(20, 64);
            this.label_eqinfoDepthHeader.Name = "label_eqinfoDepthHeader";
            this.label_eqinfoDepthHeader.Size = new System.Drawing.Size(81, 20);
            this.label_eqinfoDepthHeader.TabIndex = 0;
            this.label_eqinfoDepthHeader.Text = "震源の深さ";
            // 
            // label_eqinfoEpicenter
            // 
            this.label_eqinfoEpicenter.AutoSize = true;
            this.label_eqinfoEpicenter.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eqinfoEpicenter.Location = new System.Drawing.Point(132, 44);
            this.label_eqinfoEpicenter.Name = "label_eqinfoEpicenter";
            this.label_eqinfoEpicenter.Size = new System.Drawing.Size(51, 20);
            this.label_eqinfoEpicenter.TabIndex = 0;
            this.label_eqinfoEpicenter.Text = "NULL";
            // 
            // label_eqinfoEpicenterHeader
            // 
            this.label_eqinfoEpicenterHeader.AutoSize = true;
            this.label_eqinfoEpicenterHeader.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eqinfoEpicenterHeader.Location = new System.Drawing.Point(20, 44);
            this.label_eqinfoEpicenterHeader.Name = "label_eqinfoEpicenterHeader";
            this.label_eqinfoEpicenterHeader.Size = new System.Drawing.Size(57, 20);
            this.label_eqinfoEpicenterHeader.TabIndex = 0;
            this.label_eqinfoEpicenterHeader.Text = "震源地";
            // 
            // label_eqinfoTime
            // 
            this.label_eqinfoTime.AutoSize = true;
            this.label_eqinfoTime.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eqinfoTime.Location = new System.Drawing.Point(132, 24);
            this.label_eqinfoTime.Name = "label_eqinfoTime";
            this.label_eqinfoTime.Size = new System.Drawing.Size(51, 20);
            this.label_eqinfoTime.TabIndex = 0;
            this.label_eqinfoTime.Text = "NULL";
            // 
            // label_eqinfoTimeHeader
            // 
            this.label_eqinfoTimeHeader.AutoSize = true;
            this.label_eqinfoTimeHeader.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eqinfoTimeHeader.Location = new System.Drawing.Point(20, 24);
            this.label_eqinfoTimeHeader.Name = "label_eqinfoTimeHeader";
            this.label_eqinfoTimeHeader.Size = new System.Drawing.Size(73, 20);
            this.label_eqinfoTimeHeader.TabIndex = 0;
            this.label_eqinfoTimeHeader.Text = "発生時刻";
            // 
            // groupBox_EEW
            // 
            this.groupBox_EEW.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.groupBox_EEW.Controls.Add(this.label_eewIntensity);
            this.groupBox_EEW.Controls.Add(this.label_eewIntensityHeader);
            this.groupBox_EEW.Controls.Add(this.label_eewMagnitude);
            this.groupBox_EEW.Controls.Add(this.label_eewMagnitudeHeader);
            this.groupBox_EEW.Controls.Add(this.label_eewDepth);
            this.groupBox_EEW.Controls.Add(this.label_eewDepthHeader);
            this.groupBox_EEW.Controls.Add(this.label_eewEpicenter);
            this.groupBox_EEW.Controls.Add(this.label_eewEpicenterHeader);
            this.groupBox_EEW.Controls.Add(this.label_eewTime);
            this.groupBox_EEW.Controls.Add(this.label_eewTimeHeader);
            this.groupBox_EEW.Controls.Add(this.label_eewMessage);
            this.groupBox_EEW.Location = new System.Drawing.Point(420, 8);
            this.groupBox_EEW.Name = "groupBox_EEW";
            this.groupBox_EEW.Size = new System.Drawing.Size(512, 158);
            this.groupBox_EEW.TabIndex = 2;
            this.groupBox_EEW.TabStop = false;
            this.groupBox_EEW.Text = "緊急地震速報";
            // 
            // label_eewIntensity
            // 
            this.label_eewIntensity.AutoSize = true;
            this.label_eewIntensity.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eewIntensity.Location = new System.Drawing.Point(132, 124);
            this.label_eewIntensity.Name = "label_eewIntensity";
            this.label_eewIntensity.Size = new System.Drawing.Size(51, 20);
            this.label_eewIntensity.TabIndex = 0;
            this.label_eewIntensity.Text = "NULL";
            // 
            // label_eewIntensityHeader
            // 
            this.label_eewIntensityHeader.AutoSize = true;
            this.label_eewIntensityHeader.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eewIntensityHeader.Location = new System.Drawing.Point(20, 124);
            this.label_eewIntensityHeader.Name = "label_eewIntensityHeader";
            this.label_eewIntensityHeader.Size = new System.Drawing.Size(105, 20);
            this.label_eewIntensityHeader.TabIndex = 0;
            this.label_eewIntensityHeader.Text = "最大予測震度";
            // 
            // label_eewMagnitude
            // 
            this.label_eewMagnitude.AutoSize = true;
            this.label_eewMagnitude.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eewMagnitude.Location = new System.Drawing.Point(132, 104);
            this.label_eewMagnitude.Name = "label_eewMagnitude";
            this.label_eewMagnitude.Size = new System.Drawing.Size(51, 20);
            this.label_eewMagnitude.TabIndex = 0;
            this.label_eewMagnitude.Text = "NULL";
            // 
            // label_eewMagnitudeHeader
            // 
            this.label_eewMagnitudeHeader.AutoSize = true;
            this.label_eewMagnitudeHeader.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eewMagnitudeHeader.Location = new System.Drawing.Point(20, 104);
            this.label_eewMagnitudeHeader.Name = "label_eewMagnitudeHeader";
            this.label_eewMagnitudeHeader.Size = new System.Drawing.Size(93, 20);
            this.label_eewMagnitudeHeader.TabIndex = 0;
            this.label_eewMagnitudeHeader.Text = "マグニチュード";
            // 
            // label_eewDepth
            // 
            this.label_eewDepth.AutoSize = true;
            this.label_eewDepth.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eewDepth.Location = new System.Drawing.Point(132, 84);
            this.label_eewDepth.Name = "label_eewDepth";
            this.label_eewDepth.Size = new System.Drawing.Size(51, 20);
            this.label_eewDepth.TabIndex = 0;
            this.label_eewDepth.Text = "NULL";
            // 
            // label_eewDepthHeader
            // 
            this.label_eewDepthHeader.AutoSize = true;
            this.label_eewDepthHeader.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eewDepthHeader.Location = new System.Drawing.Point(20, 84);
            this.label_eewDepthHeader.Name = "label_eewDepthHeader";
            this.label_eewDepthHeader.Size = new System.Drawing.Size(81, 20);
            this.label_eewDepthHeader.TabIndex = 0;
            this.label_eewDepthHeader.Text = "震源の深さ";
            // 
            // label_eewEpicenter
            // 
            this.label_eewEpicenter.AutoSize = true;
            this.label_eewEpicenter.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eewEpicenter.Location = new System.Drawing.Point(132, 64);
            this.label_eewEpicenter.Name = "label_eewEpicenter";
            this.label_eewEpicenter.Size = new System.Drawing.Size(51, 20);
            this.label_eewEpicenter.TabIndex = 0;
            this.label_eewEpicenter.Text = "NULL";
            // 
            // label_eewEpicenterHeader
            // 
            this.label_eewEpicenterHeader.AutoSize = true;
            this.label_eewEpicenterHeader.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eewEpicenterHeader.Location = new System.Drawing.Point(20, 64);
            this.label_eewEpicenterHeader.Name = "label_eewEpicenterHeader";
            this.label_eewEpicenterHeader.Size = new System.Drawing.Size(57, 20);
            this.label_eewEpicenterHeader.TabIndex = 0;
            this.label_eewEpicenterHeader.Text = "震源地";
            // 
            // label_eewTime
            // 
            this.label_eewTime.AutoSize = true;
            this.label_eewTime.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eewTime.Location = new System.Drawing.Point(132, 44);
            this.label_eewTime.Name = "label_eewTime";
            this.label_eewTime.Size = new System.Drawing.Size(51, 20);
            this.label_eewTime.TabIndex = 0;
            this.label_eewTime.Text = "NULL";
            // 
            // label_eewTimeHeader
            // 
            this.label_eewTimeHeader.AutoSize = true;
            this.label_eewTimeHeader.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eewTimeHeader.Location = new System.Drawing.Point(20, 44);
            this.label_eewTimeHeader.Name = "label_eewTimeHeader";
            this.label_eewTimeHeader.Size = new System.Drawing.Size(73, 20);
            this.label_eewTimeHeader.TabIndex = 0;
            this.label_eewTimeHeader.Text = "発生時刻";
            // 
            // label_eewMessage
            // 
            this.label_eewMessage.AutoSize = true;
            this.label_eewMessage.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_eewMessage.Location = new System.Drawing.Point(20, 24);
            this.label_eewMessage.Name = "label_eewMessage";
            this.label_eewMessage.Size = new System.Drawing.Size(252, 20);
            this.label_eewMessage.TabIndex = 0;
            this.label_eewMessage.Text = "緊急地震速報は発表されていません。";
            // 
            // groupBox_Kyoshin
            // 
            this.groupBox_Kyoshin.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.groupBox_Kyoshin.Controls.Add(this.label_kyoshinPrefecture);
            this.groupBox_Kyoshin.Controls.Add(this.radioButton_Borehore);
            this.groupBox_Kyoshin.Controls.Add(this.radioButton_Surface);
            this.groupBox_Kyoshin.Controls.Add(this.label_kyoshinLatestTime);
            this.groupBox_Kyoshin.Controls.Add(this.comboBox_MapType);
            this.groupBox_Kyoshin.Controls.Add(this.label_kyoshinMaxIntDetail);
            this.groupBox_Kyoshin.Controls.Add(this.label_kyoshinMaxInt);
            this.groupBox_Kyoshin.Controls.Add(this.label_kyoshinMaxIntHeader);
            this.groupBox_Kyoshin.Controls.Add(this.pictureBox_kyoshinMap);
            this.groupBox_Kyoshin.Location = new System.Drawing.Point(28, 8);
            this.groupBox_Kyoshin.Name = "groupBox_Kyoshin";
            this.groupBox_Kyoshin.Size = new System.Drawing.Size(376, 516);
            this.groupBox_Kyoshin.TabIndex = 1;
            this.groupBox_Kyoshin.TabStop = false;
            this.groupBox_Kyoshin.Text = "強震モニタ";
            // 
            // label_kyoshinPrefecture
            // 
            this.label_kyoshinPrefecture.AutoSize = true;
            this.label_kyoshinPrefecture.Font = new System.Drawing.Font("Meiryo UI", 8F);
            this.label_kyoshinPrefecture.Location = new System.Drawing.Point(356, 441);
            this.label_kyoshinPrefecture.Name = "label_kyoshinPrefecture";
            this.label_kyoshinPrefecture.Size = new System.Drawing.Size(0, 14);
            this.label_kyoshinPrefecture.TabIndex = 0;
            this.label_kyoshinPrefecture.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // radioButton_Borehore
            // 
            this.radioButton_Borehore.AutoSize = true;
            this.radioButton_Borehore.Location = new System.Drawing.Point(276, 481);
            this.radioButton_Borehore.Name = "radioButton_Borehore";
            this.radioButton_Borehore.Size = new System.Drawing.Size(67, 19);
            this.radioButton_Borehore.TabIndex = 1;
            this.radioButton_Borehore.Text = "地中(&B)";
            this.radioButton_Borehore.UseVisualStyleBackColor = true;
            // 
            // radioButton_Surface
            // 
            this.radioButton_Surface.AutoSize = true;
            this.radioButton_Surface.Checked = true;
            this.radioButton_Surface.Location = new System.Drawing.Point(196, 481);
            this.radioButton_Surface.Name = "radioButton_Surface";
            this.radioButton_Surface.Size = new System.Drawing.Size(67, 19);
            this.radioButton_Surface.TabIndex = 1;
            this.radioButton_Surface.TabStop = true;
            this.radioButton_Surface.Text = "地表(&S)";
            this.radioButton_Surface.UseVisualStyleBackColor = true;
            // 
            // label_kyoshinLatestTime
            // 
            this.label_kyoshinLatestTime.AutoSize = true;
            this.label_kyoshinLatestTime.Font = new System.Drawing.Font("Meiryo UI", 14F);
            this.label_kyoshinLatestTime.Location = new System.Drawing.Point(20, 24);
            this.label_kyoshinLatestTime.Name = "label_kyoshinLatestTime";
            this.label_kyoshinLatestTime.Size = new System.Drawing.Size(144, 24);
            this.label_kyoshinLatestTime.TabIndex = 0;
            this.label_kyoshinLatestTime.Text = "接続しています...";
            // 
            // comboBox_MapType
            // 
            this.comboBox_MapType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_MapType.FormattingEnabled = true;
            this.comboBox_MapType.Items.AddRange(new object[] {
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
            this.comboBox_MapType.Location = new System.Drawing.Point(24, 478);
            this.comboBox_MapType.Name = "comboBox_MapType";
            this.comboBox_MapType.Size = new System.Drawing.Size(152, 23);
            this.comboBox_MapType.TabIndex = 0;
            // 
            // label_kyoshinMaxIntDetail
            // 
            this.label_kyoshinMaxIntDetail.AutoSize = true;
            this.label_kyoshinMaxIntDetail.Font = new System.Drawing.Font("Meiryo UI", 8F);
            this.label_kyoshinMaxIntDetail.Location = new System.Drawing.Point(111, 93);
            this.label_kyoshinMaxIntDetail.Name = "label_kyoshinMaxIntDetail";
            this.label_kyoshinMaxIntDetail.Size = new System.Drawing.Size(55, 14);
            this.label_kyoshinMaxIntDetail.TabIndex = 0;
            this.label_kyoshinMaxIntDetail.Text = "他 0 地点";
            // 
            // label_kyoshinMaxInt
            // 
            this.label_kyoshinMaxInt.AutoSize = true;
            this.label_kyoshinMaxInt.Font = new System.Drawing.Font("Meiryo UI", 12F);
            this.label_kyoshinMaxInt.Location = new System.Drawing.Point(38, 88);
            this.label_kyoshinMaxInt.Name = "label_kyoshinMaxInt";
            this.label_kyoshinMaxInt.Size = new System.Drawing.Size(25, 20);
            this.label_kyoshinMaxInt.TabIndex = 0;
            this.label_kyoshinMaxInt.Text = "―";
            // 
            // label_kyoshinMaxIntHeader
            // 
            this.label_kyoshinMaxIntHeader.AutoSize = true;
            this.label_kyoshinMaxIntHeader.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label_kyoshinMaxIntHeader.Location = new System.Drawing.Point(16, 68);
            this.label_kyoshinMaxIntHeader.Name = "label_kyoshinMaxIntHeader";
            this.label_kyoshinMaxIntHeader.Size = new System.Drawing.Size(73, 20);
            this.label_kyoshinMaxIntHeader.TabIndex = 0;
            this.label_kyoshinMaxIntHeader.Text = "地表震度";
            // 
            // pictureBox_kyoshinMap
            // 
            this.pictureBox_kyoshinMap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox_kyoshinMap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_kyoshinMap.Location = new System.Drawing.Point(12, 64);
            this.pictureBox_kyoshinMap.Name = "pictureBox_kyoshinMap";
            this.pictureBox_kyoshinMap.Size = new System.Drawing.Size(352, 400);
            this.pictureBox_kyoshinMap.TabIndex = 0;
            this.pictureBox_kyoshinMap.TabStop = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 540);
            this.Controls.Add(this.groupBox_EqInfo);
            this.Controls.Add(this.groupBox_EEW);
            this.Controls.Add(this.groupBox_Kyoshin);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Temonis";
            this.Load += new System.EventHandler(this.Main_Load);
            this.groupBox_EqInfo.ResumeLayout(false);
            this.groupBox_EqInfo.PerformLayout();
            this.groupBox_EEW.ResumeLayout(false);
            this.groupBox_EEW.PerformLayout();
            this.groupBox_Kyoshin.ResumeLayout(false);
            this.groupBox_Kyoshin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_kyoshinMap)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TextBox textBox_eqInfoIntensity;
        public System.Windows.Forms.Label label_eqinfoMessage;
        public System.Windows.Forms.Label label_eqinfoMagnitude;
        private System.Windows.Forms.Label label_eqinfoMagnitudeHeader;
        public System.Windows.Forms.Label label_eqinfoDepth;
        private System.Windows.Forms.Label label_eqinfoDepthHeader;
        public System.Windows.Forms.Label label_eqinfoEpicenter;
        private System.Windows.Forms.Label label_eqinfoEpicenterHeader;
        private System.Windows.Forms.Label label_eqinfoTimeHeader;
        private GroupBox groupBox_EqInfo;
        public System.Windows.Forms.Label label_eqinfoTime;
        public System.Windows.Forms.Label label_eewIntensity;
        public System.Windows.Forms.Label label_eewIntensityHeader;
        public System.Windows.Forms.Label label_eewMagnitude;
        public System.Windows.Forms.Label label_eewMagnitudeHeader;
        public System.Windows.Forms.Label label_eewDepth;
        public System.Windows.Forms.Label label_eewDepthHeader;
        public System.Windows.Forms.Label label_eewEpicenter;
        public System.Windows.Forms.Label label_eewEpicenterHeader;
        public System.Windows.Forms.Label label_eewTime;
        public System.Windows.Forms.Label label_eewTimeHeader;
        public System.Windows.Forms.Label label_eewMessage;
        public System.Windows.Forms.Label label_kyoshinPrefecture;
        private System.Windows.Forms.RadioButton radioButton_Borehore;
        public System.Windows.Forms.RadioButton radioButton_Surface;
        public System.Windows.Forms.ComboBox comboBox_MapType;
        public System.Windows.Forms.Label label_kyoshinMaxIntDetail;
        public System.Windows.Forms.Label label_kyoshinMaxInt;
        private System.Windows.Forms.Label label_kyoshinMaxIntHeader;
        public System.Windows.Forms.PictureBox pictureBox_kyoshinMap;
        private GroupBox groupBox_Kyoshin;
        private System.Windows.Forms.Label label_kyoshinLatestTime;
        private GroupBox groupBox_EEW;
    }
}

