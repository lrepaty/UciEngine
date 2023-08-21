namespace Uci
{
    public class UciCommand
    {
        // Commands to Engine
        // ==================
        /// <summary>UCI "uci" command</summary>
        public static string Uci = "uci";

        /// <summary>UCI "isready" response</summary>
        public static string IsReady = "isready";

        /// <summary>UCI "new game" command</summary>
        public static string UciNewGame = "ucinewgame";

        /// <summary>UCI "position fen" command</summary>
        public static string PositionFen = "position fen ";

        /// <summary>UCI "position fen" command</summary>
        public static string PositionStartpos = "position startpos ";

        /// <summary>UCI "position" command</summary>
        public static string SetOption = "setoption name {0} value {1}";

        /// <summary>UCI "go wtime" command</summary>
        public static string GoGameTime = "go wtime {0} btime {1} winc {2} binc {3} movestogo {4}";

        /// <summary>UCI "go movetime" command</summary>
        public static string GoMoveTime = "go movetime {0}";

        /// <summary>UCI "go depth" command</summary>
        public static string GoDepth = "go depth {0}";

        /// <summary>UCI "go nodes" command</summary>
        public static string GoNodes = "go nodes {0}";

        /// <summary>UCI "go mate" command</summary>
        public static string GoMate = "go mate {0}";

        /// <summary>UCI "go infinite" command</summary>
        public static string GoInfinite = "go infinite";

        /// <summary>UCI "quit" command</summary>
        public static string Debug = "d";

        /// <summary>UCI "quit" command</summary>
        public static string Stop = "stop";

        /// <summary>UCI "quit" command</summary>
        public static string UciQuit = "quit";


        // Commands from Engine
        // ====================
        /// <summary>UCI "uciok" response</summary>
        public static string UciOk = "uciok";

        /// <summary>UCI "readyok" command</summary>
        public static string ReadyOk = "readyok";

        /// <summary>UCI "bestmove" command</summary>
        public static string BestMoveResponse = "bestmove";

        /// <summary>UCI "Debug" command</summary>
        public static string DebugResponse = "Checkers: ";
    }
}
