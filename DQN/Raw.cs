using System;
using System.Collections.Generic;
using System.Linq;
using TicTacToe.Enum;

namespace TicTacToe.DQN
{
	/// <summary>
	/// 封装当前连续棋子的抽象类
	/// </summary>
	public abstract class Row
	{
		protected ARoWMode Mode { get; set; }
		public int Itype { get; set; }
		protected Location First { get; set; }
		protected Row(Location first, ARoWMode mode, int itype)
		{
			First = first;
			Mode = mode;
			Itype = itype;
		}
		/// <summary>
		/// 检查棋子的两端是否都被阻挡
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public bool IsBlocked(int[,] s)
		{
			return IsBlockedLeft(s) && IsBlockedRight(s);
		}

		/// <summary>
		/// 检查棋子的两端有一端被阻挡
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public bool IsBlockedOne(int[,] s)
		{
			return IsBlockedLeft(s) || IsBlockedRight(s);
		}

		/// <summary>
		/// 获取当前的首部和尾部的棋子类型,-2表示阻塞，0表示不存在
		/// </summary>
		protected int GetBlockedLeft(int[,] s, Location first)
		{
			//先生成一个默认的坐标
			Location location = new Location(-10, -10);
			//获取边缘棋子的坐标
			//水平方向
			if (Mode == ARoWMode.Horizontall) location = new Location(first.x, first.y - 1);
			else if (Mode == ARoWMode.Vertically) location = new Location(first.x - 1, first.y);
			else if (Mode == ARoWMode.DownLeft) location = new Location(first.x - 1, first.y - 1);
			else if (Mode == ARoWMode.DownRight) location = new Location(first.x - 1, first.y + 1);
			//数组越界，说明旁边有阻挡。或者当前的棋子不是我方棋子返回对应类型
			if (location.IsCrossBlock()) return -2;
			return s[location.x, location.y];
		}

		/// <summary>
		///检查棋子的左侧是否被阻挡,s是环境
		/// </summary>
		/// <returns></returns>
		protected bool IsBlockedLeft(int[,] s, Location first)
		{
			return GetBlockedLeft(s, first) != 0;
		}
		protected int GetBlockedRight(int[,] s, Location end)
		{
			//先生成一个默认的坐标
			Location location = new Location(-10, -10);
			//获取边缘棋子的坐标
			//水平方向
			if (Mode == ARoWMode.Horizontall) location = new Location(end.x, end.y + 1);
			else if (Mode == ARoWMode.Vertically) location = new Location(end.x + 1, end.y);
			else if (Mode == ARoWMode.DownLeft) location = new Location(end.x + 1, end.y + 1);
			else if (Mode == ARoWMode.DownRight) location = new Location(end.x + 1, end.y - 1);
			//数组越界，说明旁边有阻挡。或者当前的棋子不是我方棋子返回对应类型
			if (location.IsCrossBlock()) return -2;
			return s[location.x, location.y];
		}
		/// <summary>
		/// 检查棋子的右侧是否被阻挡
		/// </summary>
		/// <returns></returns>
		protected bool IsBlockedRight(int[,] s, Location end)
		{
			return GetBlockedRight(s, end) != 0;

		}
		public abstract bool IsBlockedLeft(int[,] s);
		public abstract bool IsBlockedRight(int[,] s);
	}

	/// <summary>
	/// 封装当前棋盘上三个连续棋子的类
	/// </summary>
	public class ThreeInARow: Row
	{
		protected Location Second { get; set; }
		protected Location Third { get; set; }
		public ThreeInARow(Location first, Location second, Location third, ARoWMode mode, int itype):base(first,mode,itype)
		{
			Second = second;
			Third = third;
		}
		/// <summary>
		/// 检查当前的list集合中的棋子阻塞状态数量
		/// </summary>
		/// <returns></returns>
		public static void SetBlockedCount(int[,] s, List<ThreeInARow> rows, out int block0, out int block1, out int block2)
		{
			//都不堵塞
			int tblock0 = 0;
			//1端堵塞
			int tblock1 = 0;
			//2端堵塞
			int tblock2 = 0;
			//计算两端边界被堵塞的情况
			foreach (var row in rows)
			{
				if (row.IsBlocked(s))
				{
					++tblock2;
					continue;
				}
				else if (row.IsBlockedOne(s))
				{
					++tblock1;
					continue;
				}
				else ++tblock0;
			}
			block0 = tblock0;
			block1 = tblock1;
			block2 = tblock2;
		}
		/// <summary>
		/// 判断2个raw是否相等
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType()) return base.Equals(obj);
			ThreeInARow raw =  (ThreeInARow)obj;
			return Mode == raw.Mode && Itype == raw.Itype && First.Equals(First) && Second.Equals(Second) && Third.Equals(Third);
		}

		/// <summary>
		/// 对于相同的对象返回相同的hash值，不然存储在树形结构的容器里会出错，不同类型的类hash值可以一样
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return Mode.GetHashCode() + Itype.GetHashCode() + First.GetHashCode() + Second.GetHashCode() + Third.GetHashCode();
		}

		/// <summary>
		///检查棋子的左侧是否被阻挡,s是环境
		/// </summary>
		/// <returns></returns>
		public override bool IsBlockedLeft(int[,] s)
		{
			return IsBlockedLeft(s, First);
		}

		/// <summary>
		/// 检查棋子的右侧是否被阻挡
		/// </summary>
		/// <returns></returns>
		public override bool IsBlockedRight(int[,] s)
		{
			return IsBlockedRight(s, Third);
		}

		/// <summary>
		/// 返回集合A和集合B中不同的元素
		/// </summary>
		/// <returns></returns>
		public static List<ThreeInARow> UnionRows(List<ThreeInARow> current, List<ThreeInARow> before)
		{
			//如果小于0说明我们这个是新的，直接返回就行
			if (before.Count <= 0) return new List<ThreeInARow>(current);
			//说明旧的环境s状态下不存在这个元素，我们把它加入到我们的返回值里
			List<ThreeInARow> result = current.Except(before).ToList(); ;
			return result;
		}

		/// <summary>
		/// 检查环境s中的三个连续的棋子
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static List<ThreeInARow> CheckForThreeInARow(int[,] s)
		{
			var result = new List<ThreeInARow>();
			int rows = s.GetLength(0);
			int cols = s.GetLength(1);

			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
				{
					// Check horizontally
					if (j <= cols - 3 && s[i, j] != 0 && s[i, j] == s[i, j + 1] && s[i, j] == s[i, j + 2])
					{
						result.Add(new ThreeInARow(new Location(i, j), new Location(i, j + 1), new Location(i, j + 2), ARoWMode.Horizontall, s[i, j]));
					}
					// Check vertically
					if (i <= rows - 3 && s[i, j] != 0 && s[i, j] == s[i + 1, j] && s[i, j] == s[i + 2, j])
					{
						result.Add(new ThreeInARow(new Location(i, j), new Location(i + 1, j), new Location(i + 2, j), ARoWMode.Vertically, s[i, j]));
					}

					// Check diagonal (down-right)
					if (i <= rows - 3 && j <= cols - 3 && s[i, j] != 0  && s[i, j] == s[i + 1, j + 1] && s[i, j] == s[i + 2, j + 2])
					{
						result.Add(new ThreeInARow(new Location(i, j), new Location(i + 1, j + 1), new Location(i + 2, j + 2), ARoWMode.DownRight, s[i, j]));
					}

					// Check diagonal (down-left)
					if (i <= rows - 3 && j >= 2 && s[i, j] != 0 && s[i, j] == s[i + 1, j - 1] && s[i, j] == s[i + 2, j - 2])
					{
						result.Add(new ThreeInARow(new Location(i, j), new Location(i + 1, j - 1), new Location(i + 2, j - 2), ARoWMode.DownLeft, s[i, j]));
					}
				}
			}
			return result;
		}
	}

	/// <summary>
	/// 封装当前棋盘上二个连续棋子的类
	/// </summary>
	public class TwoInARow : Row
	{
		protected Location Second { get; set; }
		public TwoInARow(Location first, Location second, ARoWMode mode, int itype) : base(first, mode, itype)
		{
			Second = second;
		}

		/// <summary>
		/// 棋列中是否包含指定位置的棋子
		/// </summary>
		/// <returns></returns>
		public bool IsContainLocation(Location location)
		{
			return location.Equals(First) || location.Equals(Second);
		}
		/// <summary>
		/// 检查当前的list集合中的棋子阻塞状态数量
		/// </summary>
		/// <returns></returns>
		public static void SetBlockedCount(int[,] s, List<TwoInARow> rows, out int block0, out int block1, out int block2)
		{
			//都不堵塞
			int tblock0 = 0;
			//1端堵塞
			int tblock1 = 0;
			//2端堵塞
			int tblock2 = 0;
			//计算两端边界被堵塞的情况
			foreach (var row in rows)
			{
				if (row.IsBlocked(s))
				{
					++tblock2;
					continue;
				}
				else if (row.IsBlockedOne(s))
				{
					++tblock1;
					continue;
				}
				else ++tblock0;
			}
			block0 = tblock0;
			block1 = tblock1;
			block2 = tblock2;
		}
		/// <summary>
		/// 判断2个raw是否相等
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType()) return base.Equals(obj);
			TwoInARow raw = (TwoInARow)obj;
			return Mode == raw.Mode && Itype == raw.Itype && First.Equals(First) && Second.Equals(Second);
		}

		/// <summary>
		/// 对于相同的对象返回相同的hash值，不然存储在树形结构的容器里会出错，不同类型的类hash值可以一样
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return Mode.GetHashCode() + Itype.GetHashCode() + First.GetHashCode() + Second.GetHashCode();
		}

		/// <summary>
		///检查棋子的左侧是否被阻挡,s是环境
		/// </summary>
		/// <returns></returns>
		public override bool IsBlockedLeft(int[,] s)
		{
			return IsBlockedLeft(s, First);
		}

		/// <summary>
		/// 检查棋子的右侧是否被阻挡
		/// </summary>
		/// <returns></returns>
		public override bool IsBlockedRight(int[,] s)
		{
			return IsBlockedRight(s, Second);
		}

		/// <summary>
		/// 检查当前的连续是否多于2，比如水平2个的白子，其实左右有新的白子，这个时候我们就要过滤掉这个选项
		/// </summary>
		/// <returns></returns>
		public bool IsOverTwoInARow(int[,]s)
		{
			return GetBlockedLeft(s, First) == Itype || GetBlockedRight(s, Second) == Itype;
		}

		/// <summary>
		/// 返回集合A和集合B中不同的元素
		/// </summary>
		/// <returns></returns>
		public static List<TwoInARow> UnionRows(List<TwoInARow> current, List<TwoInARow> before)
		{
			//如果小于0说明我们这个是新的，直接返回就行
			if (before.Count <= 0) return new List<TwoInARow>(current);
			List<TwoInARow> result = new List<TwoInARow>();
			//这里因为before被过滤，所以我们的筛选逻辑就是before中没有的全部加进来
			foreach (var row in current)
			{
				//说明before中存在这个元素，跳过
				if (before.Count(brow => brow != null && brow.Equals(row)) > 0) continue;
				result.Add(row);
			}
			return result;
		}
		public static List<TwoInARow> CheckForTwoInARow(int[,] s)
		{
			var result = new List<TwoInARow>();
			int rows = s.GetLength(0);
			int cols = s.GetLength(1);
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
				{
					// Check horizontally
					if (j <= cols - 2 && s[i, j] != 0 && s[i, j] == s[i, j + 1])
					{
						result.Add(new TwoInARow(new Location(i, j), new Location(i, j + 1), ARoWMode.Horizontall, s[i, j]));
					}
					// Check vertically
					if (i <= rows - 2 && s[i, j] != 0  && s[i, j] == s[i + 1, j])
					{
						result.Add(new TwoInARow(new Location(i, j), new Location(i + 1, j), ARoWMode.Vertically, s[i, j]));
					}

					// Check diagonal (down-right)
					if (i <= rows - 2 && j <= cols - 2 && s[i, j] != 0 && s[i, j] == s[i + 1, j + 1])
					{
						result.Add(new TwoInARow(new Location(i, j), new Location(i + 1, j + 1), ARoWMode.DownRight, s[i, j]));
					}

					// Check diagonal (down-left)
					if (i <= rows - 2 && j >= 1 && s[i, j] != 0  && s[i, j] == s[i + 1, j - 1])
					{
						result.Add(new TwoInARow(new Location(i, j), new Location(i + 1, j - 1), ARoWMode.DownLeft, s[i, j]));
					}
				}
			}
			return result;
		}
	}
}
