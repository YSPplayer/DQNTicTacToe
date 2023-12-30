using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
namespace TicTacToe.GameUI
{
	static class UIEvent {

		/// <summary>
		///绘制内边框 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public static void PictureBoxPaint(object sender, PaintEventArgs e)
		{
			PictureBox currentPictureBox = sender as PictureBox;
			if (currentPictureBox == null) return;
			// 获取PictureBox的Graphics对象
			Graphics g = e.Graphics;
			// 设置边框颜色和宽度
			Pen borderPen = new Pen(Color.Yellow, 2); // 自定义颜色和边框宽度
													  // 计算边框的位置和大小
			int borderSize = (int)(borderPen.Width); // 边框的宽度
			int adjustedWidth = currentPictureBox.Width - borderSize;
			int adjustedHeight = currentPictureBox.Height - borderSize;
			// 绘制边框
			g.DrawRectangle(borderPen, new Rectangle(borderSize / 2, borderSize / 2, adjustedWidth, adjustedHeight));
			// 释放资源
			borderPen.Dispose();
		}
	}
}
