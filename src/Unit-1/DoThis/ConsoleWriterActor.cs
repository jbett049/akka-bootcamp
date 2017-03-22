using System;
using Akka.Actor;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for serializing message writes to the console.
    /// (write one message at a time, champ :)
    /// </summary>
    class ConsoleWriterActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            // We received an input error (either null, or failed validation)
            if (message is Messages.InputError)
            {
                var msg = message as Messages.InputError;
                WriteMessage(msg.Reason, ConsoleColor.DarkRed);
            }
            // We received a successful input, pass it on!
            else if (message is Messages.InputSuccess)
            {
                var msg = message as Messages.InputSuccess;
                WriteMessage(msg.Reason, ConsoleColor.Green);
            }
            else
            {
                WriteMessage(message, ConsoleColor.White);
            }

            Console.ResetColor();
        }

        private void WriteMessage(object reason, ConsoleColor colour)
        {
            Console.ForegroundColor = colour;
            Console.WriteLine(reason);
        }
    }
}
