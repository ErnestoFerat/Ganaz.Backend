namespace Ganaz.Backend
{
	public class NumbersFunction
	{	
		/// <summary>
		/// Gets missing number
		/// </summary>
		/// <param name="numbers">Array of Numbers</param>
		/// <returns></returns>
		public static int GetMissingNumber(int[] numbers)
		{
			var intialNumber = numbers[0];
			for (int i = 0; i < numbers.Length; i++)
			{
				var currentNumber = numbers[i];

				if (intialNumber == currentNumber)
				{
					intialNumber++;
				}
				else
				{
					break;
				}
			}

			return intialNumber++;
		}
	}
}
