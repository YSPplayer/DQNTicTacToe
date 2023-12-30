
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
			this.buttonReset = new System.Windows.Forms.Button();
			this.radioPlayer = new System.Windows.Forms.RadioButton();
			this.radioAi = new System.Windows.Forms.RadioButton();
			this.labelTitle = new System.Windows.Forms.Label();
			this.labelOutMessage = new System.Windows.Forms.Label();
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
			// 
			// buttonReset
			// 
			this.buttonReset.Location = new System.Drawing.Point(161, 30);
			this.buttonReset.Name = "buttonReset";
			this.buttonReset.Size = new System.Drawing.Size(89, 32);
			this.buttonReset.TabIndex = 1;
			this.buttonReset.Text = "重置";
			this.buttonReset.UseVisualStyleBackColor = true;
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
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ActiveCaption;
			this.ClientSize = new System.Drawing.Size(784, 761);
			this.Controls.Add(this.labelOutMessage);
			this.Controls.Add(this.labelTitle);
			this.Controls.Add(this.radioAi);
			this.Controls.Add(this.radioPlayer);
			this.Controls.Add(this.buttonReset);
			this.Controls.Add(this.buttonStart);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonStart;
		private System.Windows.Forms.Button buttonReset;
		private System.Windows.Forms.RadioButton radioPlayer;
		private System.Windows.Forms.RadioButton radioAi;
		private System.Windows.Forms.Label labelTitle;
		private System.Windows.Forms.Label labelOutMessage;
	}
}

