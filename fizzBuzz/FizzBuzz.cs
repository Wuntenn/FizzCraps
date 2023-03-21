using System;
namespace fizzBuzz
{
    public static class FizzBuzz
    {
        public static void makeFizzBuzz()
        {
        }

        // Set up initial state 
        public static bool fizzBuzzSolution(string somestring)
        {

            // Clear the screen for a new game
            Console.Clear();

            int iterations = -1;

            bool inputIsValid = false;
            string input = String.Empty;

            int startRange = -1;
            int endRange = -1;

            // Make sure that a value positive (real) number is entered as the start range
            while ((!inputIsValid) || (startRange < 0))
            {
                Console.WriteLine("Please enter a start number for the fizzbuzz range: ");
                input = Console.ReadLine() ?? "not set";
                inputIsValid = int.TryParse(input, out startRange);
            }

            // reset flag to test next input
            inputIsValid = false;

            // Make sure that a valid or greater that startRange number is used
            while ((!inputIsValid) || (endRange <= startRange))
            {
                Console.WriteLine("Please enter and end number for the fizzbuzz range: ");
                input = Console.ReadLine() ?? "not set";
                inputIsValid = int.TryParse(input, out endRange);
            }

            iterations = (endRange - startRange) + 1;

            Console.WriteLine("The entered numbers were: " + startRange + " and: " + endRange);

            foreach (int idx in Enumerable.Range(startRange, iterations))
            {
                Console.WriteLine($"{fizzBuzzify(idx)}");
            }

            return true;
        }

        private static string fizzBuzzify(int num)
        {
            // set output string to empty 
            string fizzyStream = String.Empty;

            // set flags for divisible by 3 and 5 
            bool isDivisibleBy3 = (num % 3 == 0);
            bool isDivisibleBy5 = (num % 5 == 0);

            // resolve output string 
            if (isDivisibleBy3) fizzyStream += "Fizz";
            if (isDivisibleBy5) fizzyStream += "Buzz";
            if (String.IsNullOrEmpty(fizzyStream)) fizzyStream = num.ToString();

            return fizzyStream;
        }


    }
}

