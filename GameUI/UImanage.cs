using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TicTacToe.GameCore;
using TicTacToe.CustomControl;
using System.Drawing;
using System.Drawing.Imaging;

namespace TicTacToe.GameUI
{
	class UImanage
	{
		public static int MAX;
		public static CPictureBox[,] pictureBoxes;
		public static Label labelOutMessage;
		public static Label labelTurnNumber;
		//游戏是否开始
		private static bool isGame;
		static UImanage()
		{
			MAX = Game.MAX;
			//创建4*4的数组，初始索引处的值为null
			pictureBoxes = new CPictureBox[MAX, MAX];
			isGame = false;
		}

		/// <summary>
		/// 获取当前落子的格子位置，传入core检查
		/// </summary>
		/// <returns></returns>
		private static Location GetPieceLocation(CPictureBox pictureBox)
		{
			return new Location(pictureBox.GameX, pictureBox.GameY);
		}

		/// <summary>
		/// 重置场景
		/// </summary>
		public static void ResetScene() {
			for (int i = 0; i < pictureBoxes.GetLength(0); i++)
			{
				for (int j = 0; j < pictureBoxes.GetLength(1); j++)
				{
					PictureBox pictureBox = pictureBoxes[i, j];
					//重置图片
					pictureBox.Image = null;
				}
			}
			//重置文本
			labelOutMessage.Text = Tag.noStart;
			labelTurnNumber.Text = Tag.defaultTurn;
			//重置游戏数据
			Game.ClearGame();
		}

		/// <summary>
		/// 游戏开始放置棋子
		/// </summary>
		public static void PlaceChessPiece(CPictureBox pictureBox)
		{
			//不在游戏中或者当前位置已经有棋子不触发
			if (!isGame || CheckPieceError(pictureBox)) return;
			//落子
			DropPiece(Game.GetTurnPlayer(), pictureBox);
			//更新环境
			UpDateTurn();
		}
		private static void UpDateTurn()
		{
			//更新当前的环境
			Player player = Game.UpDateTurn();
			//根据玩家来判断当前游戏是否结束
			if (player == Player.None)
			{
				//进入下一轮游戏
				int turn = Game.GetCurrentTurn();
				labelTurnNumber.Text = turn.ToString();
			}
			else if (player == Player.All)
			{
				//和局
				labelOutMessage.Text = Tag.drawn;
				isGame = false;
			}
			else
			{
				//游戏胜利
				labelOutMessage.Text = player == Player.White ? Tag.winWhite : Tag.winBlack;
				//重置游戏
				isGame = false;
			}
			
		}

		/// <summary>
		///下棋子 
		/// </summary>
		private static void DropPiece(Player player, CPictureBox pictureBox) 
		{
			//根据玩家类型来落子
			if (player == Player.None) return;
			Game.DropPiece(player, new Location(pictureBox.GameX, pictureBox.GameY));
			SetPieceImage(player,pictureBox);
		}

		private static void SetPieceImage(Player player,CPictureBox pictureBox)
		{
			Bitmap bitmap = new Bitmap(pictureBox.Width, pictureBox.Height, PixelFormat.Format32bppArgb);
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				// 设置抗锯齿
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				// 设置圆形的颜色和边框颜色
				Color circleColor = player == Player.White ? Color.White : Color.Black;
				// 创建用于填充圆形的Brush
				Brush brush = new SolidBrush(circleColor);
			    // 设置画笔
				Pen pen = new Pen(circleColor, 3); // 黑色画笔，宽度为3
				// 计算圆形的位置和大小
				int diameter = Math.Min(pictureBox.Width, pictureBox.Height) - 30; // 圆的直径
				int x = (pictureBox.Width - diameter) / 2;
				int y = (pictureBox.Height - diameter) / 2;
				// 绘制填充的圆形
				g.FillEllipse(brush, x, y, diameter, diameter);
				// 绘制圆形
				g.DrawEllipse(pen, x, y, diameter, diameter);
			}
			pictureBox.Image = bitmap;
		}

		/// <summary>
		/// 开始游戏
		/// </summary>
		public static void StartGame()
		{
			ResetScene();
			isGame = true;
			labelOutMessage.Text = Tag.start;
			//初始化游戏逻辑数据
			Game.StartGame();
		}

		/// <summary>
		/// 检查当前位置是否有棋子，有棋子或者索引位置有问题则返回true,否则返回false
		/// </summary>
		/// <param name="pictureBox"></param>
		/// <returns></returns>
		private static bool CheckPieceError(CPictureBox pictureBox)
		{
			//落子位置出错
		    return pictureBox.Image != null || pictureBox.GameY < 0 || pictureBox.GameX < 0 || Game.HasPiece(new Location(pictureBox.GameX,pictureBox.GameY));
		}
	}
}
