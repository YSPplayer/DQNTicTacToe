using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace TicTacToe.GameCore
{
	/// <summary>
	/// 游戏模式
	/// </summary>
	public enum Mode
	{ 
		PVP,//双人模式
		PVE,//人机模式
		EVE,//人机观战模式
	}
	public enum Player 
	{ 
		None,//未开始游戏
		White,//黑棋
		Black,//白旗
		All,//和局
	}
	struct Location
	{
		public int x;
		public int y;
		public Location(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
	}
	static class Game
	{
		public const int MAX = 6;
		//回合数
		private static int turn;
		//当前的玩家
		private static Player currentPlayer;
		//对UI界面的内核逻辑映射
		//0表示空区域，-1表示黑棋，1表示白旗
		private static int[,] gameSquares;
		private static bool isAi;//检查当前玩家是否为ai
		//游戏模式
		private static Mode Mode;
		//随机数种子
		private static Random random;
		//方便取随机数的x,y坐标数组
		private static List<Location> locations;
		static Game() 
		{
			locations = new List<Location>();
			gameSquares = new int[MAX, MAX];
			random = new Random();
			ClearGame();
		}

		/// <summary>
		/// 设置玩家的先后游戏顺序
		/// </summary>
		private static void SetFirst()
		{
			if (Mode == Mode.PVE)
			{
				int number = random.Next(0, 2); // 生成0或1的随机数
				isAi = number == 0 ? true : false;
			}
			else
			{
				//EVE模式，总是true
				isAi = true;
			}
		}
		public static bool IsAi()
		{
			return isAi;
		}

		public static void RemoveLocation(Location location)
		{
			for (int i = 0; i < locations.Count; i++)
			{
				Location loc = locations[i];
				if (location.x == loc.x && location.y == loc.y)
				{
					//移除掉当前元素
					locations.RemoveAt(i);
					break;
				}
			}
		}

		/// <summary>
		/// 获取下一个玩家
		/// </summary>
		private static void SetNextPlayer()
		{
			if (currentPlayer == Player.None) return;
			currentPlayer = currentPlayer == Player.White ? Player.Black : Player.White;
			//ai模式下设置人机和玩家的攻击顺序
			if (Mode == Mode.PVE) isAi = isAi ? false : true;
			return;
		}

		/// <summary>
		/// 返回当前的游戏玩家
		/// </summary>
		public static Player GetTurnPlayer() 
		{
			return currentPlayer;
		}

		/// <summary>
		/// 设置游戏模式
		/// </summary>
		public static void SetGameMode(Mode mode)
		{
			Mode = mode;
		}
		/// <summary>
		/// 获取游戏模式
		/// </summary>
		public static Mode GetGameMode()
		{
			return Mode;
		}


		/// <summary>
		/// 清除游戏
		/// </summary>
		public static void ClearGame()
		{
			turn = 0;
			currentPlayer = Player.None;
			isAi = false;
			ClearGameSquares();
		}

		/// <summary>
		/// 重置我们的游戏界面
		/// </summary>
		private static void ClearGameSquares() 
		{
			//重置loctions坐标
			locations.Clear();
			for (int i = 0; i < gameSquares.GetLength(0); i++)
			{
				for (int j = 0; j < gameSquares.GetLength(1); j++)
				{
					//重置游戏逻辑
					gameSquares[i, j] = 0;
					//存储位置数组，ai会调用
					locations.Add(new Location(i,j));
				}
			}
		}

		/// <summary>
		/// 如果有棋子返回true，否则返回false
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static bool HasPiece(Location location)
		{
			return gameSquares[location.x, location.y] != 0;
		}


		/// <summary>
		/// 把棋子下到指定的地方
		/// </summary>
		public static void DropPiece(Player player, Location location)
		{
			gameSquares[location.x, location.y]  = player == Player.White ? 1 : -1;
			//落子后这个位置坐标就不能用了，同时移除掉对应的坐标
			RemoveLocation(location);
		}

		/// <summary>
		/// ai下棋
		/// </summary>
		public static Location DropAiPiece() 
		{
			int index = random.Next(0, locations.Count);
			if (index >= locations.Count || index < 0) index = 0;
			//获取到随机索引值
			Location loc = locations[index];
			DropPiece(currentPlayer, loc);
			//返回我们的位置，因为我们要渲染对应索引的组件
			return loc;
		}

		/// <summary>
		/// 更新游戏环境
		/// </summary>
		public static Player UpDateTurn()
		{
			//检查游戏是否结束
			bool win = CheckGameProcess();
			//游戏没有结束，继续
			if (!win)
			{
				++turn;
				//如果没有空的位置，和局
				if (turn >= MAX * MAX) return Player.All;
				SetNextPlayer();
				return Player.None;
			}
			else
			{
				//游戏结束

				return currentPlayer;
			}
		}

		/// <summary>
		/// 检查是否有连续相同的三个元素，有游戏就胜利
		/// </summary>
		/// <returns></returns>
		public static bool CheckGameProcess()
		{
			int rows = gameSquares.GetLength(0);
			int cols = gameSquares.GetLength(1);
			// 检查水平方向
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols - 3; j++) // 减3是因为我们现在检查4个连续的棋子
				{
					if (gameSquares[i, j] != 0 &&
						gameSquares[i, j] == gameSquares[i, j + 1] &&
						gameSquares[i, j] == gameSquares[i, j + 2] &&
						gameSquares[i, j] == gameSquares[i, j + 3])
						return true;
				}
			}

			// 检查垂直方向
			for (int i = 0; i < rows - 3; i++) // 同样减3
			{
				for (int j = 0; j < cols; j++)
				{
					if (gameSquares[i, j] != 0 &&
						gameSquares[i, j] == gameSquares[i + 1, j] &&
						gameSquares[i, j] == gameSquares[i + 2, j] &&
						gameSquares[i, j] == gameSquares[i + 3, j])
						return true;
				}
			}

			// 检查主对角线（从左上到右下）
			for (int i = 0; i < rows - 3; i++)
			{
				for (int j = 0; j < cols - 3; j++)
				{
					if (gameSquares[i, j] != 0 &&
						gameSquares[i, j] == gameSquares[i + 1, j + 1] &&
						gameSquares[i, j] == gameSquares[i + 2, j + 2] &&
						gameSquares[i, j] == gameSquares[i + 3, j + 3])
						return true;
				}
			}

			// 检查反对角线（从左下到右上）
			for (int i = 3; i < rows; i++)
			{
				for (int j = 0; j < cols - 3; j++)
				{
					if (gameSquares[i, j] != 0 &&
						gameSquares[i, j] == gameSquares[i - 1, j + 1] &&
						gameSquares[i, j] == gameSquares[i - 2, j + 2] &&
						gameSquares[i, j] == gameSquares[i - 3, j + 3])
						return true;
				}
			}
			return false;
		}
		/// <summary>
		/// 获取当前的游戏回合
		/// </summary>
		public static int GetCurrentTurn()
		{
			return turn;
		}

		/// <summary>
		/// 开始游戏
		/// </summary>
		public static void StartGame()
		{
			ClearGame();
			turn = 0;
			//白棋先攻
			currentPlayer = Player.White;
			//有ai，就是人机模式，加入人机
			if (Mode != Mode.PVP) SetFirst();
		}

		

	}
}
