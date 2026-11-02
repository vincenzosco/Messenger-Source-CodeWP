using System;
using Microsoft.Devices.Sensors;

namespace Microsoft.Phone.Applications.Common
{
	public sealed class AccelerometerHelper : IDisposable
	{
		private const double MaximumCalibrationTiltAngle = Math.PI / 9.0;

		private const double MaximumStabilityTiltDeltaAngle = Math.PI / 360.0;

		private const int SamplesCount = 25;

		private const double LowPassFilterCoef = 0.1;

		private const double NoiseMaxAmplitude = 0.05;

		private const string AccelerometerCalibrationKeyName = "AccelerometerCalibration";

		private Accelerometer _sensor;

		private static double _maximumCalibrationOffset = Math.Sin(Math.PI / 9.0);

		private static double _maximumStabilityDeltaOffset = Math.Sin(Math.PI / 360.0);

		private int _deviceStableCount;

		private bool _initialized;

		private Simple3DVector[] _sampleBuffer = new Simple3DVector[25];

		private Simple3DVector _previousLowPassOutput;

		private Simple3DVector _previousOptimalFilterOutput;

		private Simple3DVector _sampleSum = new Simple3DVector(0.0, 0.0, -25.0);

		private int _sampleIndex;

		private Simple3DVector _averageAcceleration;

		private bool _active;

		public static AccelerometerHelper Instance = new AccelerometerHelper();

		public bool DeviceStable
		{
			get
			{
				return _deviceStableCount >= 25;
			}
		}

		private static Simple3DVector AccelerometerCalibrationPersisted
		{
			get
			{
				double x = ApplicationSettingHelper.TryGetValueWithDefault("AccelerometerCalibrationX", 0.0);
				double y = ApplicationSettingHelper.TryGetValueWithDefault("AccelerometerCalibrationY", 0.0);
				return new Simple3DVector(x, y, 0.0);
			}
			set
			{
				bool flag = ApplicationSettingHelper.AddOrUpdateValue("AccelerometerCalibrationX", value.X);
				if (flag | ApplicationSettingHelper.AddOrUpdateValue("AccelerometerCalibrationY", value.Y))
				{
					ApplicationSettingHelper.Save();
				}
			}
		}

		public Simple3DVector ZeroAccelerationCalibrationOffset { get; private set; }

		public bool? NoAccelerometer { get; private set; }

		public bool Active
		{
			get
			{
				return _active;
			}
			set
			{
				if (!((!NoAccelerometer) ?? true))
				{
					return;
				}
				if (value)
				{
					if (!_active)
					{
						StartAccelerometer();
					}
				}
				else if (_active)
				{
					StopAccelerometer();
				}
			}
		}

		public event EventHandler<AccelerometerHelperReadingEventArgs> ReadingChanged;

		private AccelerometerHelper()
		{
			_sampleIndex = 0;
			ZeroAccelerationCalibrationOffset = AccelerometerCalibrationPersisted;
		}

		public void Dispose()
		{
			if (_sensor != null)
			{
				_sensor.Dispose();
			}
		}

		public bool CanCalibrate(bool xAxis, bool yAxis)
		{
			bool result = false;
			lock (_sampleBuffer)
			{
				if (DeviceStable)
				{
					double num = 0.0;
					if (xAxis)
					{
						num += _averageAcceleration.X * _averageAcceleration.X;
					}
					if (yAxis)
					{
						num += _averageAcceleration.Y * _averageAcceleration.Y;
					}
					num = Math.Sqrt(num);
					if (num <= _maximumCalibrationOffset)
					{
						result = true;
					}
				}
			}
			return result;
		}

		public bool Calibrate(bool xAxis, bool yAxis)
		{
			bool result = false;
			lock (_sampleBuffer)
			{
				if (CanCalibrate(xAxis, yAxis))
				{
					ZeroAccelerationCalibrationOffset = new Simple3DVector(xAxis ? (0.0 - _averageAcceleration.X) : ZeroAccelerationCalibrationOffset.X, yAxis ? (0.0 - _averageAcceleration.Y) : ZeroAccelerationCalibrationOffset.Y, 0.0);
					AccelerometerCalibrationPersisted = ZeroAccelerationCalibrationOffset;
					result = true;
				}
			}
			return result;
		}

		private void StartAccelerometer()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected Obj, but got Unknown
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				_sensor = new Accelerometer();
				if (_sensor != null)
				{
					if ((int)_sensor.State == 0)
					{
						NoAccelerometer = false;
						_sensor = null;
						return;
					}
					_sensor.ReadingChanged += sensor_ReadingChanged;
					_sensor.Start();
					_active = true;
					NoAccelerometer = false;
				}
				else
				{
					_active = false;
					NoAccelerometer = true;
				}
			}
			catch (Exception)
			{
				_active = false;
				NoAccelerometer = true;
			}
		}

		private void StopAccelerometer()
		{
			try
			{
				if (_sensor != null)
				{
					_sensor.ReadingChanged -= sensor_ReadingChanged;
					_sensor.Stop();
					_sensor = null;
					_active = false;
					_initialized = false;
				}
			}
			catch (Exception)
			{
				_active = false;
				NoAccelerometer = true;
			}
		}

		private static double LowPassFilter(double newInputValue, double priorOutputValue)
		{
			return priorOutputValue + 0.1 * (newInputValue - priorOutputValue);
		}

		private static double FastLowAmplitudeNoiseFilter(double newInputValue, double priorOutputValue)
		{
			double result = newInputValue;
			if (Math.Abs(newInputValue - priorOutputValue) <= 0.05)
			{
				result = priorOutputValue + 0.1 * (newInputValue - priorOutputValue);
			}
			return result;
		}

		private void sensor_ReadingChanged(object sender, AccelerometerReadingEventArgs e)
		{
			Simple3DVector simple3DVector = new Simple3DVector(e.X, e.Y, e.Z);
			Simple3DVector lowPassFilteredAcceleration;
			Simple3DVector simple3DVector2;
			Simple3DVector simple3DVector4;
			lock (_sampleBuffer)
			{
				if (!_initialized)
				{
					_sampleSum = simple3DVector * 25.0;
					_averageAcceleration = simple3DVector;
					for (int i = 0; i < 25; i++)
					{
						_sampleBuffer[i] = _averageAcceleration;
					}
					_previousLowPassOutput = _averageAcceleration;
					_previousOptimalFilterOutput = _averageAcceleration;
					_initialized = true;
				}
				lowPassFilteredAcceleration = (_previousLowPassOutput = new Simple3DVector(LowPassFilter(simple3DVector.X, _previousLowPassOutput.X), LowPassFilter(simple3DVector.Y, _previousLowPassOutput.Y), LowPassFilter(simple3DVector.Z, _previousLowPassOutput.Z)));
				simple3DVector2 = (_previousOptimalFilterOutput = new Simple3DVector(FastLowAmplitudeNoiseFilter(simple3DVector.X, _previousOptimalFilterOutput.X), FastLowAmplitudeNoiseFilter(simple3DVector.Y, _previousOptimalFilterOutput.Y), FastLowAmplitudeNoiseFilter(simple3DVector.Z, _previousOptimalFilterOutput.Z)));
				_sampleIndex++;
				if (_sampleIndex >= 25)
				{
					_sampleIndex = 0;
				}
				Simple3DVector simple3DVector3 = simple3DVector2;
				_sampleSum += simple3DVector3;
				_sampleSum -= _sampleBuffer[_sampleIndex];
				_sampleBuffer[_sampleIndex] = simple3DVector3;
				simple3DVector4 = (_averageAcceleration = _sampleSum / 25.0);
				Simple3DVector simple3DVector5 = simple3DVector4 - simple3DVector2;
				if (Math.Abs(simple3DVector5.X) > _maximumStabilityDeltaOffset || Math.Abs(simple3DVector5.Y) > _maximumStabilityDeltaOffset || Math.Abs(simple3DVector5.Z) > _maximumStabilityDeltaOffset)
				{
					_deviceStableCount = 0;
				}
				else if (_deviceStableCount < 25)
				{
					_deviceStableCount++;
				}
				simple3DVector += ZeroAccelerationCalibrationOffset;
				lowPassFilteredAcceleration += ZeroAccelerationCalibrationOffset;
				simple3DVector2 += ZeroAccelerationCalibrationOffset;
				simple3DVector4 += ZeroAccelerationCalibrationOffset;
			}
			if (ReadingChanged != null)
			{
				AccelerometerHelperReadingEventArgs e2 = new AccelerometerHelperReadingEventArgs();
				e2.RawAcceleration = simple3DVector;
				e2.LowPassFilteredAcceleration = lowPassFilteredAcceleration;
				e2.OptimalyFilteredAcceleration = simple3DVector2;
				e2.AverageAcceleration = simple3DVector4;
				ReadingChanged(this, e2);
			}
		}
	}
}
