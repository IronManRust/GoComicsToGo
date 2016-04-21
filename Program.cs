using System;

namespace GoComicsToGo
{

    class Program
    {

        static void Main(string[] args)
        {
            DateTime min;
            DateTime max;
            if (args.Length == 3 && DateTime.TryParse(args[1], out min) && DateTime.TryParse(args[2], out max))
            {
                Messenger.ArgumentsValid(args[0].Trim().ToLower(), min, max);
                Processor.Process(args[0].Trim().ToLower(), min, max);
            }
            else
            {
                Messenger.ArgumentsInvalid();
            }
            Messenger.Exit();
        }

    }

}