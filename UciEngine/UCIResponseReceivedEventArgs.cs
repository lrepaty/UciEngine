namespace Uci
{
    /// <summary>
    /// Response will always be a string.  This is pretty much the same as 
    /// DataReceivedEventArgs, but mine as I can't set the data on the other one
    /// </summary>
    public class UciResponseReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Store the response string in a class
        /// </summary>
        /// <param name="data">the response from the UCI engine</param>
        public UciResponseReceivedEventArgs(string data)
        {
            response = data;
        }

        /// <summary>
        /// stored response string
        /// </summary>
        private readonly string response;

        /// <summary>
        /// Property accessor for response string
        /// </summary>
        public string Response
        {
            get { return response; }
        }
    }
}
