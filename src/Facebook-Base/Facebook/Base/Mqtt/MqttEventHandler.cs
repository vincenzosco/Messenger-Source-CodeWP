namespace Facebook.Base.Mqtt
{
	public delegate void MqttEventHandler(AbstractMqttClient sender);
	public delegate void MqttEventHandler<in TEventArgs>(AbstractMqttClient sender, TEventArgs e);
}
