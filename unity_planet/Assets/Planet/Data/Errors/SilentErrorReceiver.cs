using System;
using UnityEngine;

namespace Planet.Data.Errors
{
	public class SilentErrorReceiver : IErrorReceiver
	{
		public void Receive(Exception exception, string userHeader, string userMessage)
		{
			Debug.LogException(exception);
			Debug.Log($"{userHeader} {userMessage}");
		}
	}
}