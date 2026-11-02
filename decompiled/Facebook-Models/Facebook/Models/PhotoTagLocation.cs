using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Facebook.Models;

public class PhotoTagLocation
{
	private float? _xCoordFraction;

	private float? _yCoordFraction;

	[JsonProperty("x")]
	[DataMember]
	public float? XCoordFraction
	{
		get
		{
			return _xCoordFraction;
		}
		set
		{
			_xCoordFraction = value;
			if (!XCoordPercent.HasValue)
			{
				XCoordPercent = _xCoordFraction * 100f;
			}
		}
	}

	[DataMember]
	[JsonProperty("y")]
	public float? YCoordFraction
	{
		get
		{
			return _yCoordFraction;
		}
		set
		{
			_yCoordFraction = value;
			if (!YCoordPercent.HasValue)
			{
				YCoordPercent = _yCoordFraction * 100f;
			}
		}
	}

	public float? XCoordAbs { get; set; }

	public float? YCoordAbs { get; set; }

	public float? XCoordPercent { get; set; }

	public float? YCoordPercent { get; set; }
}
