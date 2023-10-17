using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Uci
{
    /// <summary>
    /// Simple interface for interacting with the UCI engine
    /// </summary>
    public interface IUciEngine : IDisposable
    {
        /// <summary>
        ///Event handler for process stdout.  This is where we parse responses
        /// </summary>
        event DataReceivedEventHandler? OutputDataReceived;

        /// <summary>
        /// Sends a command to the engine.
        /// </summary>
        /// <param name="commandString">UCI protocol string</param>
        void SendCommand(string commandString);

        /// <summary>
        /// SetOption
        /// </summary>
        /// <param name="option">Option name</param>
        /// <param name="value">value</param>
        void SetOption(string option, string value);

        /// <summary>
        /// Set Position
        /// </summary>
        /// <param name="fen">Option name</param>
        /// <param name="moves">value</param>
        void SetPosition(string? fen, string? moves = null);

        /// <summary>
        /// Validate
        /// </summary>
        void Validate();
    }
}
