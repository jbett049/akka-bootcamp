using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTail
{
    class Messages
    {
        #region Neutral / System Messages
        /// <summary>
        /// Marker class to continue processing
        /// </summary>
        public class ContinueProcessing { }
        #endregion

        #region Success Messages
        /// <summary>
        /// Base class for signalling that user input was valid
        /// </summary>
        public class InputSuccess
        {
            public InputSuccess(string reason)
            {
                Reason = reason;
            }

            public string Reason { get; private set; } 
        }
        #endregion

        #region Error Messages
        /// <summary>
        /// Base class for signalling that the user input was not valid
        /// </summary>
        public class InputError
        {
            public InputError (string reason)
            {
                Reason = reason;
            }
            public string Reason { get; private set; }
        }

        /// <summary>
        /// User input was blank
        /// </summary>
        public class NullInputError : InputError
        {
            public NullInputError(string reason) : base(reason) { }
        }

        /// <summary>
        /// User input was invalid
        /// </summary>
        public class ValidationError : InputError
        {
            public ValidationError(string reason) : base(reason) { }
        } 
        #endregion
    }
}
