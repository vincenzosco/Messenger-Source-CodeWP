using System;

namespace Facebook.Base.Mqtt
{
	public class MqttException : Exception
	{
		public ConnectionFailureReason? ConnectionFailureReason { get; private set; }

		public MqttException(ConnectionFailureReason reason, Exception e)
			: base(reason.ToString(), e)
		{
			ConnectionFailureReason = reason;
		}

		public MqttException(ConnectionFailureReason reason)
			: base(reason.ToString())
		{
			ConnectionFailureReason = reason;
		}

		public MqttException(string message)
			: base(message)
		{
		}

		public MqttException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
