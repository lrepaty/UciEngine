using System.Diagnostics;

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
    }
}
