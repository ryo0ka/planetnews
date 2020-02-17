using System;

namespace Planet.Data.Errors
{
	public interface IErrorReceiver
	{
		void Receive(Exception exception, string userHeader, string userMessage);
	}
}