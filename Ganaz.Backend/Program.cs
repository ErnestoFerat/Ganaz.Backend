namespace Ganaz.Backend
{
	class Program
	{		
		static void Main(string[] args)
		{
			int[] numbers = {11, 12, 13, 14, 16, 17 };
			
			var missingNumnber = NumbersFunction.GetMissingNumber(numbers);
		}
	}
}
