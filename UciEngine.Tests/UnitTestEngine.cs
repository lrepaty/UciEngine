using System.Diagnostics;
using Uci;

namespace UciEngine.Tests
{
    [TestClass]
    public class UnitTestEngine
    {
        private string? response;
        private string uciPath = "Uci/uci.bin";
        private string cfishPath = "Uci/Cfish NUMA 060821 x64 BMI2.exe";
        private string sugarPath = "Uci/SugaR AI 2.40 64.exe";
        [TestMethod]
        public void TestUciStartEngine()
        {
            string expected;
            string actual;
            expected = uciPath;
            Uci.UciEngine uciEngine = new Uci.UciEngine(expected);
            actual = uciEngine.EnginePath;
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestUciSetOption()
        {
            string expected;
            string actual;
            expected = "4";
            Uci.UciEngine uciEngine = new Uci.UciEngine(uciPath);
            uciEngine.SetOption("Threads", expected);
            actual = uciEngine.Options["Threads"];
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestUciUci()
        {
            string expected;
            string? actual;
            expected = UciCommand.UciOk;
            Uci.UciEngine uciEngine = new Uci.UciEngine(uciPath);
            uciEngine.OutputDataReceived += OutputDataReceived;
            uciEngine.SendCommand(UciCommand.Uci);
            actual = response;
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestUciIsReady()
        {
            string expected;
            string? actual;
            expected = UciCommand.ReadyOk;
            Uci.UciEngine uciEngine = new Uci.UciEngine(uciPath);
            uciEngine.OutputDataReceived += OutputDataReceived;
            uciEngine.SendCommand(UciCommand.IsReady);
            actual = response;
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestUciIsDebug()
        {
            string expected;
            string? actual;
            expected = UciCommand.DebugResponse;
            Uci.UciEngine uciEngine = new Uci.UciEngine(uciPath);
            uciEngine.OutputDataReceived += OutputDataReceived;
            uciEngine.SendCommand(UciCommand.Debug);
            actual = response;
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestUciGo()
        {
            string expected;
            string? actual;
            expected = UciCommand.BestMoveResponse;
            Uci.UciEngine uciEngine = new Uci.UciEngine(uciPath);
            uciEngine.OutputDataReceived += OutputDataReceived;
            uciEngine.GoMoveTime(100);
            actual = response;
            actual.Should().StartWith(expected);
        }

        [TestMethod]
        public void TestUciValidate()
        {
            string expected;
            string? actual;
            expected = UciCommand.BestMoveResponse;
            Uci.UciEngine uciEngine = new Uci.UciEngine(uciPath);
            uciEngine.OutputDataReceived += OutputDataReceived;
            uciEngine.Validate();
            actual = response;
            actual.Should().StartWith(expected);
        }


        [TestMethod]
        public void TestUciEvalFile()
        {
            string expected;
            string? actual;
            expected = UciCommand.BestMoveResponse;
            Uci.UciEngine uciEngine = new Uci.UciEngine(uciPath);
            uciEngine.OutputDataReceived += OutputDataReceived;
            uciEngine.SetOption("EvalFile", "Test.nnue");
            uciEngine.Validate();
            actual = response;
            actual.Should().StartWith(expected);
        }

        [TestMethod]
        public void TestCfishStartEngine()
        {
            string expected;
            string actual;
            expected = cfishPath;
            Uci.UciEngine uciEngine = new Uci.UciEngine(expected);
            actual = uciEngine.EnginePath;
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestCfishSetOption()
        {
            Uci.UciEngine uciEngine = new Uci.UciEngine(cfishPath);
            Action action = new Action(() => uciEngine.SetOption("Threads", "4"));
            action.Should().Throw<Exception>();
        }

        [TestMethod]
        public void TestCfishUci()
        {
            string expected;
            string? actual;
            expected = UciCommand.UciOk;
            Uci.UciEngine uciEngine = new Uci.UciEngine(cfishPath);
            uciEngine.OutputDataReceived += OutputDataReceived;
            uciEngine.SendCommand(UciCommand.Uci);
            actual = response;
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestCfishIsReady()
        {
            Uci.UciEngine uciEngine = new Uci.UciEngine(cfishPath);
            uciEngine.OutputDataReceived += OutputDataReceived;
            Action action = new Action(() => uciEngine.SendCommand(UciCommand.IsReady));
            action.Should().Throw<Exception>();
        }

        [TestMethod]
        public void TestCfishIsDebug()
        {
            string expected;
            string? actual;
            expected = UciCommand.DebugResponse;
            Uci.UciEngine uciEngine = new Uci.UciEngine(cfishPath);
            uciEngine.OutputDataReceived += OutputDataReceived;
            uciEngine.SendCommand(UciCommand.Debug);
            actual = response;
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestCfishGo()
        {
            string expected;
            expected = UciCommand.ReadyOk;
            Uci.UciEngine uciEngine = new Uci.UciEngine(cfishPath);
            uciEngine.OutputDataReceived += OutputDataReceived;
            Action action = new Action(() => uciEngine.GoMoveTime(100));
            action.Should().Throw<Exception>();
        }

        [TestMethod]
        public void TestCfishValidate()
        {
            string expected;
            expected = UciCommand.ReadyOk;
            Uci.UciEngine uciEngine = new Uci.UciEngine(cfishPath);
            uciEngine.OutputDataReceived += OutputDataReceived;
            Action action = new Action(() => uciEngine.Validate());
            action.Should().Throw<Exception>();
        }

        [TestMethod]
        public void TestCfishEvalFile()
        {
            string expected;
            expected = UciCommand.ReadyOk;
            Uci.UciEngine uciEngine = new Uci.UciEngine(cfishPath);
            uciEngine.OutputDataReceived += OutputDataReceived;
            Action action = new Action(() =>
            {
                uciEngine.SetOption("EvalFile", "Test.nnue");
                uciEngine.Validate();
            });
            action.Should().Throw<Exception>();
        }

        [TestMethod]
        public void TestSugaRStartEngine()
        {
            string expected;
            string actual;
            expected = sugarPath;
            Uci.UciEngine uciEngine = new Uci.UciEngine(expected);
            actual = uciEngine.EnginePath;
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestSugaRSetOption()
        {
            string expected;
            string actual;
            expected = "4";
            Uci.UciEngine uciEngine = new Uci.UciEngine(sugarPath);
            uciEngine.SetOption("Threads", expected);
            actual = uciEngine.Options["Threads"];
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestSugaRUci()
        {
            string expected;
            string? actual;
            expected = UciCommand.UciOk;
            Uci.UciEngine uciEngine = new Uci.UciEngine(sugarPath);
            uciEngine.OutputDataReceived += OutputDataReceived;
            uciEngine.SendCommand(UciCommand.Uci);
            actual = response;
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestSugaRIsReady()
        {
            string expected;
            string? actual;
            expected = UciCommand.ReadyOk;
            Uci.UciEngine uciEngine = new Uci.UciEngine(sugarPath);
            uciEngine.OutputDataReceived += OutputDataReceived;
            uciEngine.SendCommand(UciCommand.IsReady);
            actual = response;
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestSugaRIsDebug()
        {
            string expected;
            string? actual;
            expected = UciCommand.DebugResponse;
            Uci.UciEngine uciEngine = new Uci.UciEngine(sugarPath);
            uciEngine.OutputDataReceived += OutputDataReceived;
            uciEngine.SendCommand(UciCommand.Debug);
            actual = response;
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestSugaRGo()
        {
            string expected;
            expected = UciCommand.ReadyOk;
            Uci.UciEngine uciEngine = new Uci.UciEngine(sugarPath);
            uciEngine.OutputDataReceived += OutputDataReceived;
            Action action = new Action(() => uciEngine.GoMoveTime(100));
            action.Should().Throw<Exception>();
        }

        [TestMethod]
        public void TestSugaRValidate()
        {
            string expected;
            expected = UciCommand.ReadyOk;
            Uci.UciEngine uciEngine = new Uci.UciEngine(sugarPath);
            uciEngine.OutputDataReceived += OutputDataReceived;
            Action action = new Action(() => uciEngine.Validate());
            action.Should().Throw<Exception>();
        }

        [TestMethod]
        public void TestSugaREvalFile()
        {
            string expected;
            expected = UciCommand.ReadyOk;
            Uci.UciEngine uciEngine = new Uci.UciEngine(sugarPath);
            uciEngine.OutputDataReceived += OutputDataReceived;
            Action action = new Action(() =>
            {
                uciEngine.SetOption("EvalFile", "Test.nnue");
                uciEngine.Validate();
            });
            action.Should().Throw<Exception>();
        }

        public void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            response = e.Data;
        }
    }
}
