using System;

namespace Planet.Data
{
	public interface IErrorReceiver
	{
		void Receive(Exception exception, string userHeader, string userMessage);
	}
}