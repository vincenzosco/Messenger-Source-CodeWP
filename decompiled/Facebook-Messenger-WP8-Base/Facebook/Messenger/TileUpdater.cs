using System;
using System.Linq;
using Facebook.Base.Logging;
using Microsoft.Phone.Shell;

namespace Facebook.Messenger
{
	public static class TileUpdater
	{
		private static readonly Uri IconicTileSmall = new Uri("/Assets/Tiles/IconicTileSmall.png", UriKind.Relative);

		private static readonly Uri IconicTileMedium = new Uri("/Assets/Tiles/IconicTileMedium.png", UriKind.Relative);

		public static void ResetCount()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected Obj, but got Unknown
			try
			{
				ShellTile val = ShellTile.ActiveTiles.FirstOrDefault();
				if (val != null)
				{
					IconicTileData val2 = new IconicTileData();
					val2.Count = 0;
					((ShellTileData)val2).Title = string.Empty;
					val2.WideContent1 = string.Empty;
					val2.WideContent2 = string.Empty;
					val2.WideContent3 = string.Empty;
					val2.SmallIconImage = IconicTileSmall;
					val2.IconImage = IconicTileMedium;
					IconicTileData val3 = val2;
					val.Update((ShellTileData)(object)val3);
				}
			}
			catch (Exception ex)
			{
				Logger.Error("Couldn't update tile count", "ResetCount", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\TileUpdater.cs", 37);
				Logger.Error(ex.ToString(), "ResetCount", "c:\\Open\\messenger\\Facebook\\Facebook-Messenger-WP8-Base\\TileUpdater.cs", 38);
			}
		}
	}
}
