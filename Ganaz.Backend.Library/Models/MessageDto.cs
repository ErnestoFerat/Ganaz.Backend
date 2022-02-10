using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Ganaz.Backend.Library
{
	//{
	//	"first_name":"Joany",
	//	"last_name":"Robel",
	//	"timestamp":"2022-02-09T20:39:24.8896094Z",
	//	"sip":"https://127.0.0.1:33213/61e371e2-2647-48c9-9d65-6da436b8ddc3",
	//	"city":"South Purdy",
	//	"state":"OH",
	//	"phone_number":"5963966541",
	//	"priority":34
	//}

	/// <summary>
	/// Dto used to read data from Ganaz WebSocket
	/// </summary>
	public class MessageDto
	{
		[JsonProperty(PropertyName ="first_name")]
		public string FirstName{ get; set; }

		[JsonProperty(PropertyName = "last_name")]
		public string LastName { get; set; }

		[JsonProperty(PropertyName = "timestamp")]
		public string TimeStamp { get; set; }

		[JsonProperty(PropertyName = "sip")]
		public string Sip { get; set; }

		[JsonProperty(PropertyName = "city")]
		public string City { get; set; }

		[JsonProperty(PropertyName = "state")]
		public string State { get; set; }

		[JsonProperty(PropertyName = "phone_number")]
		public string PhoneNumber { get; set; }

		[JsonProperty(PropertyName = "priority")]
		public int Priority { get; set; }
	}

	
}
