using System;
using Akka.Actor;

namespace WinTail
{
    public class TailCoordinatorActor : UntypedActor
    {
        #region Message types

        /// <summary>
        /// Start tailing the file at user-specified path.
        /// </summary>
        public class StartTail
        {
            public StartTail(string filePath, IActorRef reporterActor)
            {
                FilePath = filePath;
                ReporterActor = reporterActor;
            }

            public string FilePath { get; private set; }

            public IActorRef ReporterActor { get; private set; }
        }

        /// <summary>
        /// Stop tailing the file at user-specified path.
        /// </summary>
        public class StopTail
        {
            public StopTail(string filePath)
            {
                FilePath = filePath;
            }

            public string FilePath { get; private set; }
        }

        #endregion

        protected override void OnReceive(object message)
        {
            if (message is StartTail)
            {
                var msg = message as StartTail;
                Context.ActorOf(Props.Create<TailActor>(msg.ReporterActor, msg.FilePath));
            }
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            // On a per-child basis
            return new OneForOneStrategy(
                10, // Max retries
                TimeSpan.FromSeconds(30), // Time range for retries
                x => // Custom directives for the child
                {
                    // Resume if arithmetic
                    if (x is ArithmeticException) return Directive.Resume;
                    // Stop if we do not support the input
                    else if (x is NotSupportedException) return Directive.Stop;
                    // For all other scenarios, restart the child.
                    else return Directive.Restart;
                });
        }
    }
}