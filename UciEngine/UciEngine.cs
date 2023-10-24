using System.Diagnostics;

namespace Uci
{
    public class UciEngine : IUciEngine
    {
        #region Members
        /// <summary>
        /// Process
        /// </summary
        private Process? m_process;
        
        /// <summary>
        // Expected field and Property
        /// </summary>
        private string m_expected;

        /// <summary>
        /// Synchronization after command
        /// </summary>
        private bool m_syncAfterCommand;

        /// <summary>
        /// Command
        /// </summary>
        private string m_command;

        /// <summary>
        /// created
        /// </summary>
        private bool m_created = false;

        /// <summary>
        /// thrue when commands are finished
        /// </summary>
        private bool m_uciCommandExecuted;

        /// <summary>
        /// Validate time in msec
        /// </summary>
        private const int m_validateTime = 100;

        /// <summary>
        /// Event handler for process stdout.  This is where we parse responses
        /// </summary>
        public event DataReceivedEventHandler? OutputDataReceived;

        /// <summary>
        /// Delegate(s) listening to our event
        /// </summary>
        private event EventHandler<UciResponseReceivedEventArgs> m_onUciCommandExecuted;
        #endregion

        #region Ctor
        /// <summary>
        /// Create a new UCI engine object
        /// </summary>
        public UciEngine(string name)
        {

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("File name required!");
            }
            else if (!File.Exists(name))
            {
                throw new Exception($"File: {name} not exist!");
            }
            m_process = new Process();
            // Set process and startup variables
            m_process.EnableRaisingEvents = true;
            m_process.StartInfo.CreateNoWindow = true;
            m_process.StartInfo.RedirectStandardOutput = true;
            m_process.StartInfo.RedirectStandardInput = true;
            m_process.StartInfo.RedirectStandardError = true;
            m_process.StartInfo.UseShellExecute = false;
            m_process.StartInfo.FileName = name;
            EnginePath = name;
            // If this fails we'll get an exception writing in a sec
            Options = new Dictionary<string, string>();
            m_process.Start();

            // Start async reading of the output stream
            m_process.BeginOutputReadLine();
            // Add an EventHandler for the event raised when commands are finished
            m_onUciCommandExecuted += new EventHandler<UciResponseReceivedEventArgs>(UciResponseReceivedEventHandler!);
            m_command = UciCommand.Uci;
            m_expected = UciCommand.UciOk;

            // Hook into the stdout redirected stream -assumes the process
            // has in fact redirected this stream for us
            m_process.OutputDataReceived += new DataReceivedEventHandler(OnDataReceived);

            m_syncAfterCommand = false; // Reset from prior use

            Execute(m_process.StandardInput);
            m_created = true;
            Error = "";
        }
        #endregion

        #region Properties
        /// <summary>
        /// Best Move
        /// </summary>
        public string BestMove { get; set; } = "";
        /// <summary>
        /// Engine internal name
        /// </summary>
        public string? EngineName { get; private set; }
        /// <summary>
        /// Engine file name
        /// </summary>
        public string EnginePath { get; private set; }
        /// <summary>
        /// Engine options
        /// </summary>
        public Dictionary<string, string> Options { get; private set; }
        /// <summary>
        /// Best Move
        /// </summary>
        public string Error { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Send a string to the engine
        /// </summary>
        /// <param name="commandString">command to sent</param>
        public void SendCommand(string commandString)
        {

            if (m_process == null)
                throw new Exception("Process not working");
            BestMove = "";
            m_command = commandString;
            m_expected = "";
            Error = "";
            m_uciCommandExecuted = false;
            if (commandString.StartsWith(UciCommand.IsReady))
                m_expected = UciCommand.ReadyOk;
            else if (commandString.StartsWith(UciCommand.Uci))
                m_expected = UciCommand.UciOk;
            else if (commandString.StartsWith(UciCommand.Debug))
                m_expected = UciCommand.DebugResponse;
            else if (commandString.StartsWith("go"))
            {
                m_expected = UciCommand.BestMoveResponse;
            }
            Debug.WriteLine($"Enqueueing command pair (\"{commandString}\", \"{m_expected}\")");
            // Hook into the stdout redirected stream -assumes the process
            // has in fact redirected this stream for us
            m_process.OutputDataReceived += new DataReceivedEventHandler(OnDataReceived);

            m_syncAfterCommand = false; // Reset from prior use
            if (m_expected == "")
            {
                // No response is given, so sync to "isready/readyok"
                m_syncAfterCommand = true;
            }

            Execute(m_process.StandardInput);
            if (Error != "")
            {
                Dispose();
                throw new Exception(Error);
            }
        }

        /// <summary>
        /// SetOption
        /// </summary>
        /// <param name="option">Option name</param>
        /// <param name="value">value</param>
        public void SetOption(string option, string value)
        {
            if (Options.ContainsKey(option))
            {
                SendCommand($"setoption name {option} value {value}");
                Options[option] = value;
            }
            else
                Error = $"Option {option} not exist";
        }

        /// <summary>
        /// Set Position
        /// </summary>
        /// <param name="fen">Option name</param>
        /// <param name="moves">value</param>
        public void SetPosition(string? fen, string? moves = null)
        {
            string command = fen == null ? UciCommand.PositionStartpos : (UciCommand.PositionFen + fen);
            if (moves != null)
                command += " moves " + moves;
            SendCommand(command);
        }

        /// <summary>
        /// Go GameTime
        /// </summary>
        /// <param name="wtime">white has x msec left on the clock</param>
        /// <param name="btime">black has x msec left on the clock</param>
        /// <param name="winc">white increment per move in mseconds if x > 0</param>
        /// <param name="binc">black increment per move in mseconds if x > 0</param>
        /// <param name="movestogo">there are x moves to the next time control,
        ///                         this will only be sent if x > 0,
        ///                         if you don't get this and get the wtime and btime it's sudden death</param>
        public void GoGameTime(int wtime, TimeSpan btime, int winc, int binc, int movestogo)
        {
            SendCommand(string.Format(UciCommand.GoGameTime, wtime, btime, winc, binc, movestogo));
        }

        /// <summary>
        /// Go MoveTime
        /// </summary>
        /// <param name="movetime">search exactly x mseconds</param>
        public void GoMoveTime(int movetime)
        {
            SendCommand(string.Format(UciCommand.GoMoveTime, movetime));
        }

        /// <summary>
        /// Go MoveTime
        /// </summary>
        /// <param name="depth">search x plies only.</param>
        public void GoDepth(int depth)
        {
            SendCommand(string.Format(UciCommand.GoDepth, depth));
        }

        /// <summary>
        /// Go MoveTime
        /// </summary>
        /// <param name="nodes">search x nodes only,</param>
        public void GoNodes(int nodes)
        {
            SendCommand(string.Format(UciCommand.GoNodes, nodes));
        }

        /// <summary>
        /// Go MoveTime
        /// </summary>
        /// <param name="mate">search for a mate in x moves</param>
        public void GoMate(int mate)
        {
            SendCommand(string.Format(UciCommand.GoMate, mate));
        }

        /// <summary>
        /// Go Infinite
        /// </summary>
        public void GoInfinite()
        {
            SendCommand(UciCommand.GoInfinite);
        }

        /// <summary>
        /// Validate
        /// </summary>
        public void Validate()
        {
            GoMoveTime(m_validateTime);
        }

        /// <summary>
        /// Cleanup
        /// </summary>
        public void Dispose()
        {
            // removes our handler (really we could leave this up the entire app life)
            if (m_process != null && !m_process.HasExited)
            {
                // someone should be waiting on the process quitting if they care

                m_process.StandardInput.WriteLine("quit");
                m_process.WaitForExit();
                // removes our handler (really we could leave this up the entire app life)
                m_process.OutputDataReceived -= OnDataReceived;
                m_process.Dispose();
            }
            m_onUciCommandExecuted -= UciResponseReceivedEventHandler!;
            m_process = null;
        }

        /// <summary>
        /// Cleanup
        /// </summary>
        ~UciEngine()
        {
            Dispose();
        }

        /// <summary>
        /// Spins up the thread to execute the command and waits on the response
        /// event *not* the thread to close
        /// </summary>
        /// <param name="sw">redirected stream for process stdin</param>
        /// <returns>wait result</returns>
        private bool Execute(StreamWriter sw)
        {
            // Spin up a thread to do the work
            Thread thread = new Thread(() => ExecuteThreadProc(sw));
            thread.Start();

            // better to wait at the caller level in the real app and let this 
            // thread return since it's probably the UI thread, or a worker that
            // could do other work while waiting on the engine
            while (!m_uciCommandExecuted)
            {
                Thread.Sleep(1);
            }
            return true;
        }

        /// <summary>
        /// Thread method for the actual write of the command
        /// </summary>
        /// <param name="sw">stdin StreamWriter for the process</param>
        private void ExecuteThreadProc(StreamWriter sw)
        {
            sw.WriteLine(m_command);

            // if SyncAfterCommand == true, then also send IsReady and set 
            // expected to ReadyOk
            if (m_syncAfterCommand)
            {
                m_syncAfterCommand = false;
                m_command = UciCommand.IsReady;
                m_expected = UciCommand.ReadyOk;
                sw.WriteLine(m_command);
            }

            // Done, exit thread - wait is elsewhere
        }

        /// <summary>
        /// Handler for the IUCIChessEngine event fired after commands are processed
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">holds the response string</param>
        private void UciResponseReceivedEventHandler(object sender, UciResponseReceivedEventArgs e)
        {
            // If we're asking for a move - then save the response we care about
            // the SAN for the move - it comes right after "bestmove"
            // If no move (e.g. mate) will return 'bestmove (none)'
            Trace.WriteLine($"Engine {EngineName} UciEngine: {e.Response}");
            if (e.Response.StartsWith("bestmove"))
            {
                string[] parts = e.Response.Split(' ');
                BestMove = parts[1];
            }
        }

        /// <summary>
        /// Event handler for process stdout.  This is where we parse responses
        /// </summary>
        /// <param name="sender">ignored</param>
        /// <param name="e">The string sent to stdout</param>
        private void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            string find;
            int startpos;
            int endpos;
            string key;
            string value;
            if (e.Data == null)
            {
                if (Error != "")
                {
                    // raise interface event here if there is a handler
                    if (m_onUciCommandExecuted != null)
                    {
                        m_process!.OutputDataReceived -= OnDataReceived;
                    }
                    m_uciCommandExecuted = true;
                }
                return;
            }
            OutputDataReceived?.Invoke(this, e);
            find = "info string ERROR: ";
            if (e.Data.StartsWith(find))
            {
                if (Error != "")
                    Error += "\r\n";
                Error += e.Data.Substring(find.Length);
            }
            find = "id name ";
            if (e.Data.StartsWith(find))
                EngineName = e.Data.Substring(find.Length);
            if (!m_created)
            {
                find = "option name ";
                if (e.Data.StartsWith(find))
                {
                    startpos = find.Length;
                    endpos = e.Data.IndexOf(" type");
                    key = e.Data.Substring(startpos, endpos - startpos);
                    find = "default ";
                    startpos = e.Data.IndexOf(find) + find.Length;
                    if (startpos > find.Length)
                    {
                        endpos = e.Data.IndexOf(" ", startpos);
                        if (endpos > -1)
                            value = e.Data.Substring(startpos, endpos - startpos);
                        else
                            value = e.Data.Substring(startpos);
                        value = value.Replace("<empty>", "");
                        Options[key] = value;
                    }
                }
            }
            // compare e.Data to the expected string
            if (e.Data.StartsWith(m_expected))
            {
                // raise interface event here if there is a handler
                if (m_onUciCommandExecuted != null)
                {
                    m_onUciCommandExecuted(this, new UciResponseReceivedEventArgs(e.Data));
                    // Stop listening until next command / started in SendUCICommand
                    // again this could be left up
                    m_process!.OutputDataReceived -= OnDataReceived;
                }
                m_uciCommandExecuted = true;
            }
        }
        #endregion
    }
}
    
