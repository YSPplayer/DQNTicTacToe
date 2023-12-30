﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TicTacToe.GameUI;
using TicTacToe.CustomControl;

namespace TicTacToe
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			InitializeGameUI();
		}

		/// <summary>
		/// 初始化游戏场景
		/// </summary>
		void InitializeGameUI()
		{
			CreatePictureBoxGrid();
			//添加引用
			UIManageInit();
			//重置场景
			UImanage.ResetScene();
		}
		public void UIManageInit()
		{
			//存储组件对象
	 	   UImanage.labelOutMessage = labelOutMessage;
		   UImanage.labelTurnNumber = labelTurnNumber;
		}
		/// <summary>
		/// 初始化游戏盘
		/// </summary>
		public void CreatePictureBoxGrid()
		{
			int startX = 100; // 初始X坐标
			int startY = 130; // 初始Y坐标
			int size = 100; // PictureBox大小
			int rows = UImanage.MAX; // 行数
			int columns = UImanage.MAX; // 列数

			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					// 创建新的PictureBox，我们要获取当前组件的位置，所以要注入新的属性
					CPictureBox pictureBox = new CPictureBox(size, size, i,j, new Point(startX + j * size, startY + i * size),BorderStyle.None);
					pictureBox.BackColor = Color.Gray;
					pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
					//添加边框样式
					pictureBox.Paint += UIEvent.PictureBoxPaint;
					//添加点击事件
					pictureBox.Click += UIEvent.PictureBoxClick;
					//添加到数组中，方便管理
					UImanage.pictureBoxes[i, j] = pictureBox;
					// 添加PictureBox到窗体
					Controls.Add(pictureBox);

				}
			}
		}

		/// <summary>
		/// 开始游戏
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonStart_Click(object sender, EventArgs e)
		{
			UImanage.StartGame();
		}
	}
}
