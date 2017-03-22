using System;
using Akka.Actor;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for reading FROM the console. 
    /// Also responsible for calling <see cref="ActorSystem.Terminate"/>.
    /// </summary>
    class ConsoleReaderActor : UntypedActor
    {
        public const string StartCommand = "start";
        public const string ExitCommand = "exit";
        private IActorRef _consoleWriterActor;

        public ConsoleReaderActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            if (message.Equals(StartCommand))
            {
                DoPrintInstructions();
            }
            else if (message is Messages.InputError)
            {
                _consoleWriterActor.Tell(message as Messages.InputError);
            }

            GetAndValidateInput();
        }

        /// <summary>
        /// Prints the initial instructions to the console for the user.
        /// </summary>
        private void DoPrintInstructions()
        {
            Console.WriteLine("Write whatever you want into the console!");
            Console.WriteLine("Some entries will pass validation, and some won't...\n\n");
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
        }

        /// <summary>
        /// Reads input from the console, validates it, and then signals the appropriate response
        /// (Continue processing, error, success, etc.).
        /// </summary>
        private void GetAndValidateInput()
        {
            string message = Console.ReadLine();
            // Empty message
            if (string.IsNullOrEmpty(message.Trim()))
            {
                // Inform the user that the input was blank, and that they need to supply an input
                Self.Tell(new Messages.NullInputError("No input was received from the user!"));
            }
            // Exit command
            else if (String.Equals(message, ExitCommand, StringComparison.OrdinalIgnoreCase))
            {
                // Shut down the Actor system, so that we may exit.
                Context.System.Terminate();
            }
            // Validate the message
            else
            {
                bool isValid = IsValid(message);
                if (isValid)
                {
                    // Inform the Writer
                    _consoleWriterActor.Tell(new Messages.InputSuccess("Thank you! Message received was valid."));

                    // Continue
                    Self.Tell(new Messages.ContinueProcessing());
                }
                else
                {
                    // Validation Error
                    Self.Tell(new Messages.ValidationError("Invalid: The input had an odd number of characters."));
                }
            }
        }

        /// <summary>
        /// Validates <see cref="message"/>
        /// Currently verrifies whether the message contains an even number of characters.
        /// </summary>
        /// <param name="message">string to be validated</param>
        /// <returns>True when the string has an even length, False otherwise</returns>
        private static bool IsValid(string message)
        {
            bool isValid = message.Trim().Length % 2 == 0;
            return isValid;
        }
    }
}