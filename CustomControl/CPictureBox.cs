using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace TicTacToe.CustomControl
{
	class CPictureBox: PictureBox
	{
		//当前组件所在的坐标
		public int GameX { get; private set; }
		public int GameY { get; private set; }
		public CPictureBox(int width,int height, int gameX, int gameY, Point point, BorderStyle style)
		{
			Width = width;
			Height = height;
			GameX = gameX;
			GameY = gameY;
			Location = point;
			BorderStyle = style;
		}

	}
}
