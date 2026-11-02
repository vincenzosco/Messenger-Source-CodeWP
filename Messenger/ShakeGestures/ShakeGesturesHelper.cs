using System;
using System.Collections.Generic;
using Microsoft.Phone.Applications.Common;

namespace ShakeGestures
{
	public class ShakeGesturesHelper
	{
		private const double ShakeMagnitudeWithoutGravitationThreshold_Default = 0.2;

		private const int StillCounterThreshold_Default = 20;

		private const double StillMagnitudeWithoutGravitationThreshold_Default = 0.02;

		private const int MaximumStillVectorsNeededForAverage_Default = 20;

		private const int MinimumStillVectorsNeededForAverage_Default = 5;

		private const int MinimumShakeVectorsNeededForShake_Default = 10;

		private const double WeakMagnitudeWithoutGravitationThreshold_Default = 0.2;

		private const int MinimumRequiredMovesForShake_Default = 3;

		private Simple3DVector _lastStillVector = new Simple3DVector(0.0, -1.0, 0.0);

		private bool _isInShakeState;

		private int _stillCounter;

		private List<Simple3DVector> _shakeSignal = new List<Simple3DVector>();

		private int[] _shakeHistogram = new int[3];

		private LinkedList<Simple3DVector> _stillSignal = new LinkedList<Simple3DVector>();

		public static ShakeGesturesHelper Instance = new ShakeGesturesHelper();

		public double ShakeMagnitudeWithoutGravitationThreshold { get; set; }

		public int StillCounterThreshold { get; set; }

		public double StillMagnitudeWithoutGravitationThreshold { get; set; }

		public int MaximumStillVectorsNeededForAverage { get; set; }

		public int MinimumStillVectorsNeededForAverage { get; set; }

		public int MinimumShakeVectorsNeededForShake { get; set; }

		public double WeakMagnitudeWithoutGravitationThreshold { get; set; }

		public int MinimumRequiredMovesForShake { get; set; }

		public bool Active
		{
			get
			{
				return AccelerometerHelper.Instance.Active;
			}
			set
			{
				AccelerometerHelper.Instance.Active = value;
			}
		}

		public event EventHandler<ShakeGestureEventArgs> ShakeGesture;

		private ShakeGesturesHelper()
		{
			ShakeMagnitudeWithoutGravitationThreshold = 0.2;
			StillCounterThreshold = 20;
			StillMagnitudeWithoutGravitationThreshold = 0.02;
			MinimumStillVectorsNeededForAverage = 5;
			MaximumStillVectorsNeededForAverage = 20;
			MinimumShakeVectorsNeededForShake = 10;
			WeakMagnitudeWithoutGravitationThreshold = 0.2;
			MinimumRequiredMovesForShake = 3;
			AccelerometerHelper.Instance.ReadingChanged += OnAccelerometerHelperReadingChanged;
		}

		private void OnAccelerometerHelperReadingChanged(object sender, AccelerometerHelperReadingEventArgs e)
		{
			Simple3DVector optimalyFilteredAcceleration = e.OptimalyFilteredAcceleration;
			bool flag = Math.Abs(_lastStillVector.Magnitude - optimalyFilteredAcceleration.Magnitude) > ShakeMagnitudeWithoutGravitationThreshold;
			if (!_isInShakeState && flag)
			{
				_isInShakeState = true;
				ClearShakeSignal();
				ProcessStillSignal();
				AddVectorToShakeSignal(optimalyFilteredAcceleration);
			}
			else if (!_isInShakeState && !flag)
			{
				AddVectorToStillSignal(optimalyFilteredAcceleration);
			}
			else if (_isInShakeState && flag)
			{
				AddVectorToShakeSignal(optimalyFilteredAcceleration);
				_stillCounter = 0;
				if (ProcessShakeSignal())
				{
					ClearShakeSignal();
				}
			}
			else
			{
				if (!_isInShakeState || flag)
				{
					return;
				}
				AddVectorToShakeSignal(optimalyFilteredAcceleration);
				_stillCounter++;
				if (_stillCounter > StillCounterThreshold)
				{
					_stillSignal.Clear();
					for (int i = 0; i < StillCounterThreshold; i++)
					{
						int count = _shakeSignal.Count;
						int stillCounterThreshold = StillCounterThreshold;
						AddVectorToStillSignal(optimalyFilteredAcceleration);
					}
					_shakeSignal.RemoveRange(_shakeSignal.Count - StillCounterThreshold, StillCounterThreshold);
					_isInShakeState = false;
					_stillCounter = 0;
					if (ProcessShakeSignal())
					{
						ClearShakeSignal();
					}
				}
			}
		}

		private void AddVectorToStillSignal(Simple3DVector currentVector)
		{
			_stillSignal.AddFirst(currentVector);
			if (_stillSignal.Count > 2 * MaximumStillVectorsNeededForAverage)
			{
				_stillSignal.RemoveLast();
			}
		}

		private void AddVectorToShakeSignal(Simple3DVector currentVector)
		{
			Simple3DVector simple3DVector = currentVector - _lastStillVector;
			_shakeSignal.Add(simple3DVector);
			if (!(simple3DVector.Magnitude < WeakMagnitudeWithoutGravitationThreshold))
			{
				ShakeType shakeType = ClassifyVectorShakeType(simple3DVector);
				_shakeHistogram[(int)shakeType]++;
			}
		}

		private void ClearShakeSignal()
		{
			_shakeSignal.Clear();
			Array.Clear(_shakeHistogram, 0, _shakeHistogram.Length);
		}

		private void ProcessStillSignal()
		{
			Simple3DVector simple3DVector = new Simple3DVector(0.0, 0.0, 0.0);
			int num = 0;
			foreach (Simple3DVector item in _stillSignal)
			{
				if (Math.Abs(_lastStillVector.Magnitude - item.Magnitude) < StillMagnitudeWithoutGravitationThreshold)
				{
					simple3DVector += item;
					num++;
					if (num >= MaximumStillVectorsNeededForAverage)
					{
						break;
					}
				}
			}
			if (num >= MinimumStillVectorsNeededForAverage)
			{
				_lastStillVector = simple3DVector / num;
			}
		}

		private ShakeType ClassifyVectorShakeType(Simple3DVector v)
		{
			double num = Math.Abs(v.X);
			double num2 = Math.Abs(v.Y);
			double num3 = Math.Abs(v.Z);
			if ((num >= num2) & (num >= num3))
			{
				return ShakeType.X;
			}
			if ((num2 >= num) & (num2 >= num3))
			{
				return ShakeType.Y;
			}
			return ShakeType.Z;
		}

		private bool ProcessShakeSignal()
		{
			int num = _shakeHistogram[0];
			int num2 = _shakeHistogram[1];
			int num3 = _shakeHistogram[2];
			ShakeType? shakeType = null;
			if ((num >= num2) & (num >= num3) & (num >= MinimumShakeVectorsNeededForShake))
			{
				shakeType = ShakeType.X;
			}
			else if ((num2 >= num) & (num2 >= num3) & (num2 >= MinimumShakeVectorsNeededForShake))
			{
				shakeType = ShakeType.Y;
			}
			else if ((num3 >= num) & (num3 >= num2) & (num3 >= MinimumShakeVectorsNeededForShake))
			{
				shakeType = ShakeType.Z;
			}
			if (shakeType.HasValue)
			{
				int num4 = CountSignChanges(shakeType.Value);
				if (num4 < MinimumRequiredMovesForShake)
				{
					shakeType = null;
				}
			}
			if (shakeType.HasValue)
			{
				EventHandler<ShakeGestureEventArgs> shakeGesture = ShakeGesture;
				if (shakeGesture != null)
				{
					shakeGesture(this, new ShakeGestureEventArgs(shakeType.Value));
				}
				return true;
			}
			return false;
		}

		private int CountSignChanges(ShakeType shakeType)
		{
			int num = 0;
			int num2 = 0;
			int? num3 = null;
			for (int i = 0; i < _shakeSignal.Count; i++)
			{
				switch (shakeType)
				{
				case ShakeType.X:
					num2 = Math.Sign(_shakeSignal[i].X);
					break;
				case ShakeType.Y:
					num2 = Math.Sign(_shakeSignal[i].Y);
					break;
				case ShakeType.Z:
					num2 = Math.Sign(_shakeSignal[i].Z);
					break;
				}
				if (num2 != 0)
				{
					if (!num3.HasValue)
					{
						num3 = num2;
					}
					if (num2 != num3)
					{
						num++;
					}
					num3 = num2;
				}
			}
			return num;
		}
	}
}
