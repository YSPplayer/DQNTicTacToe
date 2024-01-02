
namespace TicTacToe
{
	partial class Form1
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
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonStart = new System.Windows.Forms.Button();
			this.radioPlayer = new System.Windows.Forms.RadioButton();
			this.radioAi = new System.Windows.Forms.RadioButton();
			this.labelTitle = new System.Windows.Forms.Label();
			this.labelOutMessage = new System.Windows.Forms.Label();
			this.labelTurn = new System.Windows.Forms.Label();
			this.labelTurnNumber = new System.Windows.Forms.Label();
			this.radioDoubleAi = new System.Windows.Forms.RadioButton();
			this.buttonReset = new System.Windows.Forms.Button();
			this.checkBoxRecordSamples = new System.Windows.Forms.CheckBox();
			this.checkBoxAlgorithmAI = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// buttonStart
			// 
			this.buttonStart.Location = new System.Drawing.Point(30, 30);
			this.buttonStart.Name = "buttonStart";
			this.buttonStart.Size = new System.Drawing.Size(89, 32);
			this.buttonStart.TabIndex = 0;
			this.buttonStart.Text = "开始";
			this.buttonStart.UseVisualStyleBackColor = true;
			this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
			// 
			// radioPlayer
			// 
			this.radioPlayer.AutoSize = true;
			this.radioPlayer.Location = new System.Drawing.Point(345, 45);
			this.radioPlayer.Name = "radioPlayer";
			this.radioPlayer.Size = new System.Drawing.Size(47, 16);
			this.radioPlayer.TabIndex = 2;
			this.radioPlayer.TabStop = true;
			this.radioPlayer.Text = "双人";
			this.radioPlayer.UseVisualStyleBackColor = true;
			this.radioPlayer.CheckedChanged += new System.EventHandler(this.radioPlayer_CheckedChanged);
			// 
			// radioAi
			// 
			this.radioAi.AutoSize = true;
			this.radioAi.Location = new System.Drawing.Point(423, 45);
			this.radioAi.Name = "radioAi";
			this.radioAi.Size = new System.Drawing.Size(35, 16);
			this.radioAi.TabIndex = 3;
			this.radioAi.TabStop = true;
			this.radioAi.Text = "ai";
			this.radioAi.UseVisualStyleBackColor = true;
			this.radioAi.CheckedChanged += new System.EventHandler(this.radioAi_CheckedChanged);
			// 
			// labelTitle
			// 
			this.labelTitle.AutoSize = true;
			this.labelTitle.Location = new System.Drawing.Point(28, 87);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(41, 12);
			this.labelTitle.TabIndex = 4;
			this.labelTitle.Text = "信息：";
			// 
			// labelOutMessage
			// 
			this.labelOutMessage.AutoSize = true;
			this.labelOutMessage.Location = new System.Drawing.Point(66, 87);
			this.labelOutMessage.Name = "labelOutMessage";
			this.labelOutMessage.Size = new System.Drawing.Size(53, 12);
			this.labelOutMessage.TabIndex = 5;
			this.labelOutMessage.Text = "黑棋胜利";
			// 
			// labelTurn
			// 
			this.labelTurn.AutoSize = true;
			this.labelTurn.Location = new System.Drawing.Point(28, 118);
			this.labelTurn.Name = "labelTurn";
			this.labelTurn.Size = new System.Drawing.Size(53, 12);
			this.labelTurn.TabIndex = 6;
			this.labelTurn.Text = "回合数：";
			// 
			// labelTurnNumber
			// 
			this.labelTurnNumber.AutoSize = true;
			this.labelTurnNumber.Location = new System.Drawing.Point(77, 118);
			this.labelTurnNumber.Name = "labelTurnNumber";
			this.labelTurnNumber.Size = new System.Drawing.Size(11, 12);
			this.labelTurnNumber.TabIndex = 7;
			this.labelTurnNumber.Text = "1";
			// 
			// radioDoubleAi
			// 
			this.radioDoubleAi.AutoSize = true;
			this.radioDoubleAi.Location = new System.Drawing.Point(495, 45);
			this.radioDoubleAi.Name = "radioDoubleAi";
			this.radioDoubleAi.Size = new System.Drawing.Size(47, 16);
			this.radioDoubleAi.TabIndex = 8;
			this.radioDoubleAi.TabStop = true;
			this.radioDoubleAi.Text = "双ai";
			this.radioDoubleAi.UseVisualStyleBackColor = true;
			this.radioDoubleAi.CheckedChanged += new System.EventHandler(this.radioDoubleAi_CheckedChanged);
			// 
			// buttonReset
			// 
			this.buttonReset.Location = new System.Drawing.Point(155, 30);
			this.buttonReset.Name = "buttonReset";
			this.buttonReset.Size = new System.Drawing.Size(89, 32);
			this.buttonReset.TabIndex = 9;
			this.buttonReset.Text = "重置";
			this.buttonReset.UseVisualStyleBackColor = true;
			this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
			// 
			// checkBoxRecordSamples
			// 
			this.checkBoxRecordSamples.AutoSize = true;
			this.checkBoxRecordSamples.Location = new System.Drawing.Point(578, 45);
			this.checkBoxRecordSamples.Name = "checkBoxRecordSamples";
			this.checkBoxRecordSamples.Size = new System.Drawing.Size(72, 16);
			this.checkBoxRecordSamples.TabIndex = 10;
			this.checkBoxRecordSamples.Text = "记录样本";
			this.checkBoxRecordSamples.UseVisualStyleBackColor = true;
			// 
			// checkBoxAlgorithmAI
			// 
			this.checkBoxAlgorithmAI.AutoSize = true;
			this.checkBoxAlgorithmAI.Location = new System.Drawing.Point(656, 45);
			this.checkBoxAlgorithmAI.Name = "checkBoxAlgorithmAI";
			this.checkBoxAlgorithmAI.Size = new System.Drawing.Size(84, 16);
			this.checkBoxAlgorithmAI.TabIndex = 11;
			this.checkBoxAlgorithmAI.Text = "开启算法ai";
			this.checkBoxAlgorithmAI.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.ClientSize = new System.Drawing.Size(784, 761);
			this.Controls.Add(this.checkBoxAlgorithmAI);
			this.Controls.Add(this.checkBoxRecordSamples);
			this.Controls.Add(this.buttonReset);
			this.Controls.Add(this.radioDoubleAi);
			this.Controls.Add(this.labelTurnNumber);
			this.Controls.Add(this.labelTurn);
			this.Controls.Add(this.labelOutMessage);
			this.Controls.Add(this.labelTitle);
			this.Controls.Add(this.radioAi);
			this.Controls.Add(this.radioPlayer);
			this.Controls.Add(this.buttonStart);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonStart;
		private System.Windows.Forms.RadioButton radioPlayer;
		private System.Windows.Forms.RadioButton radioAi;
		private System.Windows.Forms.Label labelTitle;
		private System.Windows.Forms.Label labelOutMessage;
		private System.Windows.Forms.Label labelTurn;
		private System.Windows.Forms.Label labelTurnNumber;
		private System.Windows.Forms.RadioButton radioDoubleAi;
		private System.Windows.Forms.Button buttonReset;
		private System.Windows.Forms.CheckBox checkBoxRecordSamples;
		private System.Windows.Forms.CheckBox checkBoxAlgorithmAI;
	}
}

