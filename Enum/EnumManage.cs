namespace TicTacToe.Enum
{
	/// <summary>
	/// 棋盘的位置顺序
	/// </summary>
	public enum ARoWMode
	{
		Horizontall,//水平
		Vertically,//垂直
		DownRight,//斜角 \
		DownLeft,//斜角 /
	}
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
	public class Location
	{
		//x表示所在行,y表示所在列
		public int x;
		public int y;
		public Location(int x, int y)
		{
			this.x = x;
			this.y = y;
		}
		/// <summary>
		/// 检查坐标是否越界
		/// </summary>
		/// <param name="location"></param>
		/// <returns></returns>
		public bool IsCrossBlock()
		{
			return x > 5 || x < 0 || y > 5 || y < 0;
		}

		/// <summary>
		/// 判断2个位置是否相等
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType()) return base.Equals(obj);
			Location location = (Location)obj;
			return location.x == x && location.y == y;
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() + y.GetHashCode();
		}
	}
}
