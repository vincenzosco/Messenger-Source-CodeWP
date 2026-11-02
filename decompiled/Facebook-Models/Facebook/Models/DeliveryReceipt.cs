namespace Facebook.Models;

public class DeliveryReceipt
{
	public string Id { get; set; }

	public long Timestamp { get; set; }

	public static DeliveryReceipt CreateCopy(DeliveryReceipt receipt)
	{
		DeliveryReceipt deliveryReceipt = new DeliveryReceipt();
		deliveryReceipt.Id = receipt.Id;
		deliveryReceipt.Timestamp = receipt.Timestamp;
		return deliveryReceipt;
	}
}
