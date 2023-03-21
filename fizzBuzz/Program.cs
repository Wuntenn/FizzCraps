// Import demos
using static fizzBuzz.Craps;
using static fizzBuzz.FizzBuzz;
    
// Start Demo
menu();

// Front-end code
void menu()
{
    // Set up menu
    var modes = new Dictionary<string, Func<string, bool>>();
    modes.Add("Fizz Buzz Demo", new Func<string, bool>(str => fizzBuzzSolution(str) ));
    modes.Add("Craps Demo", new Func<string, bool>(str => crapsSolution(str) ));

    int selectedMenuItem = -1;
    bool inputValid = false;

    bool shouldExit = false;

    while (!shouldExit)
    {
        while ((selectedMenuItem < 0) || (selectedMenuItem > modes.Count - 1) || !inputValid)
        {
            Console.Clear();
            Console.WriteLine("Please enter the number for the demo mode: ");

            for (int dIdx = 0; dIdx < modes.Count; dIdx++)
            {
                // Make sure that menu item is selected 
                KeyValuePair<string, Func<string, bool>> mode = modes.ElementAt(dIdx);

                Console.WriteLine($"{dIdx} - {mode.Key}");
            }

            string input = Console.ReadLine() ?? "not set";
            inputValid = int.TryParse(input, out selectedMenuItem);

        }

        // Invoke demo
        KeyValuePair<string, Func<string, bool>> selectedMode = modes.ElementAt(selectedMenuItem);
        selectedMode.Value("foo");

        // reset the selected menu item (to allow returning to menu)
        selectedMenuItem = -1;


        // Allow exit after demo
        Console.WriteLine("Exit?: (Y)es, (N)o");
        string shouldExitInput = Console.ReadLine() ?? "not set";
        shouldExit = ((shouldExitInput.ToLower() == "y") || (shouldExitInput.ToLower() == "y"));
    }
}

