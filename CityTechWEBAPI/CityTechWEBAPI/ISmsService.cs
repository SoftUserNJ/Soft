namespace CityTechWEBAPI
{
	public interface ISmsService
	{
		Task<bool> SendSmsAsync(string phoneNumber, string message);
	}
}
