using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TicTacToe.GameUI;

namespace TicTacToe
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			CreatePictureBoxGrid();
		}

		/// <summary>
		/// 初始化游戏盘
		/// </summary>
		public void CreatePictureBoxGrid()
		{
			int startX = 200; // 初始X坐标
			int startY = 200; // 初始Y坐标
			int size = 100; // PictureBox大小
			int rows = 4; // 行数
			int columns = 4; // 列数

			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					// 创建新的PictureBox
					PictureBox pictureBox = new PictureBox
					{
						Width = size,
						Height = size,
						Location = new Point(startX + j * size, startY + i * size),
						BorderStyle = BorderStyle.None // 可选：添加边框样式
					};
					pictureBox.BackColor = Color.Gray;
					pictureBox.Image = Properties.Resources.black;
					pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
					//添加边框样式
					pictureBox.Paint += UIEvent.PictureBoxPaint;
					//添加到数组中，方便管理
					UImanage.pictureBoxes[i, j] = pictureBox;
					// 添加PictureBox到窗体
					Controls.Add(pictureBox);

				}
			}
		}
		private void Form1_Load(object sender, EventArgs e)
		{

		}

		
	}
}
