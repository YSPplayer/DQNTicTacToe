using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicTacToe.Enum;

namespace TicTacToe.DQN
{
	/// <summary>
	/// 模型训练相关
	/// </summary>
	class TrainManage
	{
		private const int MAX = 6;
		private static int[,] s;
		private static int[,] s1;
		private static Location dropLocation;
		private static Player player;
		public static int turn;
		public static int actionCount;//游戏当前还有多少种可能的动作数量
		static TrainManage()
		{
			Clear();
		}

		/// <summary>
		/// 拷贝当前的环境,s1的环境拷贝给s
		/// </summary>
		private static void CopyS(int[,] s,int[,] s1)
		{
			int rows = s1.GetLength(0);
			int cols = s1.GetLength(1);
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
				{
					s[i, j] = s1[i, j];
				}
			}
		}

		/// <summary>
		/// 设置当前获取奖励函数需要的环境参数 
		/// </summary>
		public static void SetCurrentParameters(Location location,Player currentPlayer,int[,] gameSquares,int aCount)
		{
			dropLocation = new Location(location.x, location.y);
			player = currentPlayer;
			actionCount = aCount;
			CopyS(s, gameSquares);
		}
		public static void SetEndParameters(int[,] gameSquares)
		{
			CopyS(s1, gameSquares);
		}

		public static void Clear()
		{
			player = Player.None;
			dropLocation = new Location(-10, -10);
			s = new int[MAX, MAX];
			s1 = new int[MAX, MAX];
			turn = 0;
			actionCount = 0;
		}

		/// <summary>
		/// 设置DQN算法训练模型需要的参数 环境，行动，立即奖励r
		/// </summary>
		/// <param name="s1"></param>
		/// <param name="dropLocation"></param>
		/// <param name=""></param>
		public static void SetSampleParameters(Player winPlayer,int[,] outs,out Location outDropLocation,out int outValue,out int aCount)
		{
			CopyS(outs, s1);
			outDropLocation = new Location(dropLocation.x,dropLocation.y);
			outValue = GetReward(winPlayer);
			aCount = actionCount;
		}

		/*
		   自己的回报一定要比对方对我们威胁小
		   之前的堵塞，比如说棋子的2端被堵塞，不需要重复计算，因为无法改变这个事实
		   奖励函数定义【回报对于传入的当前玩家而言】
		   s:未行动之前的游戏环境
           s1:行动后的游戏环境
		   对于s1，如果是传入的玩家游戏胜利，奖励+20，如果是传入的玩家失败，奖励 -20，如果平局，奖励+10
		   对于
 		*/
		/// <summary>
		/// 奖励函数，s表示未下之前的游戏状态，s1表示下过之后的游戏状态,player表示当前的旗手,winPlayer表示获胜的玩家
		/// </summary> 
		/// <returns></returns>
		private static int GetReward(Player winPlayer)
		{
			if (winPlayer != Player.None)
			{
				//游戏结束 平局
				if (winPlayer == Player.All) return 10;
				//游戏失败或者获胜
				return player == winPlayer ? 20 : -20;
			}
			else
			{
				if (player == Player.None) return 0;
				//游戏继续
				int reward = 0;
				int iplayer = player == Player.White ? 1 : -1;
				//三行奖励
				reward += GetRawsReward(3,iplayer, dropLocation,s, s1);
				//二行奖励
				reward += GetRawsReward(2, iplayer, dropLocation, s, s1);
				reward += GetRandomReward(reward,iplayer);
				return reward;
			}
		}

		private static int GetRandomReward(int reward,int iplayer)
		{
			//如果奖励为0就是说明我们是单独落子，没有走上面的函数，默认扣1回报
			if (reward != 0) return 0;
			if ((iplayer == 1 && turn > 2) || (iplayer == -1 && turn > 3)) return -1;
			return 0;
		}

		/// <summary>
		/// 获取连续排列的奖励
		/// </summary>
		/// <returns></returns>
		private static int GetRawsReward(int type,int iplayer, Location dropLocation, int[,] s, int[,] s1)
		{
			int reward = 0;
			if (type == 3) 
			{
				//三行
				//获取当前状态s下所有3个棋子连续分布的集合
				List<ThreeInARow> beforeThreeInARows = ThreeInARow.CheckForThreeInARow(s);
				//获取行动之后状态s1下所有3个棋子连续分布的集合
				List<ThreeInARow> currentThreeInARows = ThreeInARow.CheckForThreeInARow(s1);
				//落子之后我们游戏环境中新增的row
				List<ThreeInARow> changeThreeInARows = ThreeInARow.UnionRows(currentThreeInARows, beforeThreeInARows);
				if (changeThreeInARows.Count > 0)
				{
					//说明我们的运动之后场上新增了连续分布的棋子
					//如果让我方这边得以新增,不可能自己落子让对面的棋子新增，所以不考虑这个逻辑
					List<ThreeInARow> ownChangeThreeInARows = changeThreeInARows.Where(row => row != null && row.Itype == iplayer).ToList();
					//都不堵塞
					int block0 = 0;
					//1端堵塞
					int block1 = 0;
					//2端堵塞
					int block2 = 0;
					ThreeInARow.SetBlockedCount(s1, ownChangeThreeInARows, out block0, out block1, out block2);
					//如果下过棋子之后造成2端都堵塞，回报-2，造成1端堵塞，回报-1，没造成堵塞，回报 + 3
					int blockvalue = block0 * 3 + block1 * (-1) + block2 * (-2);
					reward += blockvalue;
				}
				//没有新增，只计算s1状态下对方对我们的威胁
				//如果下过棋子之后，对方有3个以上棋子，该棋子2端都堵塞
				/*
				  有3种可能性，对方场上有3个棋子，2端都为空你没有阻挡，1端为空你没有阻挡
				 */
				//先获取到对方的rows,2边的rows对象都是相同的，因为不存在我方动修改对方的棋子，但是棋子的阻塞有变化，我们来看这个变化
				List<ThreeInARow> enemyBeforeThreeInARows = beforeThreeInARows.Where(row => row != null && row.Itype == -iplayer).ToList();
				List<ThreeInARow> enemyCurrentThreeInARows = currentThreeInARows.Where(row => row != null && row.Itype == -iplayer).ToList();
				int before_block0 = 0;//未阻塞
				int before_block1 = 0;//阻塞1个
				int before_block2 = 0;//阻塞2个
				int current_block0 = 0;
				int current_block1 = 0;
				int current_block2 = 0;
				//获取之前堵塞的连线棋子
				ThreeInARow.SetBlockedCount(s, enemyBeforeThreeInARows, out before_block0, out before_block1, out before_block2);
				//获取当前堵塞的连续棋子
				ThreeInARow.SetBlockedCount(s1, enemyCurrentThreeInARows, out current_block0, out current_block1, out current_block2);
				//获取落子后对方棋子的阻塞状态
				//如果落子后对方没有阻塞，回报 -5，有1处格挡，回报-4，有2处格挡，回报+6
				//格挡需要算最新的差值，
				//对方棋子未堵塞，说明我们没有加以限制
				if (current_block0 > 0) reward += current_block0 * (-5);
				//对方棋子只阻塞一个
				if (current_block1 > 0) reward += current_block1 * (-4);
				if(current_block2 - before_block2 > 0) reward += (current_block2 - before_block2) * (6);
			}
			else if (type == 2)
			{
				//两行
				//不过滤三行
				//获取当前状态s下所有2个棋子连续分布的集合
				//.Where(row => row != null && !row.IsOverTwoInARow(s)).ToList()
				List<TwoInARow> beforeTwoInARows = TwoInARow.CheckForTwoInARow(s);
				//获取行动之后状态s1下所有2个棋子连续分布的集合
				List<TwoInARow> currentTwoInARows = TwoInARow.CheckForTwoInARow(s1);
				//落子之后我们游戏环境中新增的row
				List<TwoInARow> changeTwoInARows = TwoInARow.UnionRows(currentTwoInARows, beforeTwoInARows);
				if (changeTwoInARows.Count > 0)
				{
					//说明我们的运动之后场上新增了连续分布的棋子
					//如果让我方这边得以新增,不可能自己落子让对面的棋子新增，所以不考虑这个逻辑
					List<TwoInARow> ownChangeTwoInARows = changeTwoInARows.Where(row => row != null && row.Itype == iplayer).ToList();
					//都不堵塞
					int block0 = 0;
					//1端堵塞
					int block1 = 0;
					//2端堵塞
					int block2 = 0;
					TwoInARow.SetBlockedCount(s1, ownChangeTwoInARows, out block0, out block1, out block2);
					//如果下过棋子之后造成2端都堵塞，回报-2，造成1端堵塞，回报-1，没造成堵塞，回报 + 2
					int blockvalue = block0 * 2 + block1 * (-1) + block2 * (-2);
					reward += blockvalue;
				}
				//没有新增，只计算s1状态下对方对我们的威胁
				//如果下过棋子之后，对方有3个以上棋子，该棋子2端都堵塞
				/*
				  有3种可能性，对方场上有3个棋子，2端都为空你没有阻挡，1端为空你没有阻挡
				 */
				//先获取到对方的rows,2边的rows对象都是相同的，因为不存在我方动修改对方的棋子，但是棋子的阻塞有变化，我们来看这个变化
				List<TwoInARow> enemyBeforeTwoInARows = beforeTwoInARows.Where(row => row != null && row.Itype == -iplayer).ToList();
				List<TwoInARow> enemyCurrentTwoInARows = currentTwoInARows.Where(row => row != null && row.Itype == -iplayer).ToList();
				int before_block0 = 0;
				int before_block1 = 0;
				int before_block2 = 0;
				int current_block0 = 0;
				int current_block1 = 0;
				int current_block2 = 0;
				//获取之前堵塞的连线棋子
				TwoInARow.SetBlockedCount(s, enemyBeforeTwoInARows, out before_block0, out before_block1, out before_block2);
				//获取当前堵塞的连续棋子
				TwoInARow.SetBlockedCount(s1, enemyCurrentTwoInARows, out current_block0, out current_block1, out current_block2);
				//获取落子后对方棋子的阻塞状态
				//如果落子后对方没有阻塞，回报 -3，有1处格挡，回报-1，有2处格挡，回报+3
				if (current_block0 > 0) reward += current_block0 * (-3);
				//这个用来判断，如果是历史遗留问题才增加负向回报，否则我们刚刚下过了就默认回报0
				if (current_block1 > 0 && current_block1 == before_block1) reward += current_block1 * (-1);
				if (current_block2 - before_block2 > 0) reward += (current_block2 - before_block2) * (3);
				//检查一个棋子的时候，即检查当前落子位置是否包含在2个棋子中，每包含一次加1分。【包括3个子连续】
				int chainRowCount = currentTwoInARows.Count(row => row != null && row.IsContainLocation(dropLocation));
				if (chainRowCount < 0) chainRowCount = 0;
				reward += chainRowCount;
			}
			return reward;
		}

	}
}
