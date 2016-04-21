using System;

namespace GoComicsToGo
{

    internal static class Messenger
    {

        internal static void ArgumentsValid(string name, DateTime min, DateTime max)
        {
            Console.WriteLine();
            Console.WriteLine("Valid Arguments:");
            Console.WriteLine(string.Concat("%1 - ", name));
            Console.WriteLine(string.Concat("%2 - ", min.ToString(Constants.DATE_MASK)));
            Console.WriteLine(string.Concat("%3 - ", max.ToString(Constants.DATE_MASK)));
            Console.WriteLine();
        }

        internal static void ArgumentsInvalid()
        {
            Console.WriteLine();
            Console.WriteLine("Invalid Arguments:");
            Console.WriteLine("%1 - [Comic Name]");
            Console.WriteLine("%2 - [Date Minimum]");
            Console.WriteLine("%3 - [Date Maximum]");
            Console.WriteLine();
        }

        internal static void Exit()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to exit ...");
            Console.ReadKey();
        }

        internal static void ProcessedItem(DateTime date, bool success)
        {
            Console.WriteLine(string.Format("Processing {0} - {1}", date.ToString(Constants.DATE_MASK), success ? "Success" : "Failure"));
        }

    }

}