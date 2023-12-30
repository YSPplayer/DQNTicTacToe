using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TicTacToe.GameUI
{
	class UImanage 
	{

		public static PictureBox[,] pictureBoxes;
		static UImanage()
		{
			//创建4*4的数组，初始索引处的值为null
			pictureBoxes = new PictureBox[4, 4];
		}

	}
}
