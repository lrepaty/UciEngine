using System.Diagnostics;
using Uci;

namespace UciEngine.Tests
{
    [TestClass]
    public class UnitTestEngine
    {
        private string? response;
        [TestMethod]
        public void TestStartEngine()
        {
            string expected;
            string actual;
            expected = "Uci/uci.bin";
            Uci.UciEngine uciEngine = new Uci.UciEngine(expected);
            actual = uciEngine.EnginePath;
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestSetOption()
        {
            string expected;
            string actual;
            expected = "4";
            Uci.UciEngine uciEngine = new Uci.UciEngine("Uci/uci.bin");
            uciEngine.SetOption("Threads", expected);
            actual = uciEngine.Options["Threads"];
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestUci()
        {
            string expected;
            string? actual;
            expected = UciCommand.UciOk;
            Uci.UciEngine uciEngine = new Uci.UciEngine("Uci/uci.bin");
            uciEngine.OutputDataReceived += OutputDataReceived;
            uciEngine.SendCommand(UciCommand.Uci);
            actual = response;
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestIsReady()
        {
            string expected;
            string? actual;
            expected = UciCommand.ReadyOk;
            Uci.UciEngine uciEngine = new Uci.UciEngine("Uci/uci.bin");
            uciEngine.OutputDataReceived += OutputDataReceived;
            uciEngine.SendCommand(UciCommand.IsReady);
            actual = response;
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestIsDebug()
        {
            string expected;
            string? actual;
            expected = UciCommand.DebugResponse;
            Uci.UciEngine uciEngine = new Uci.UciEngine("Uci/uci.bin");
            uciEngine.OutputDataReceived += OutputDataReceived;
            uciEngine.SendCommand(UciCommand.Debug);
            actual = response;
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestCfishIsReady()
        {
            string expected;
            expected = UciCommand.ReadyOk;
            Uci.UciEngine uciEngine = new Uci.UciEngine("Uci/Cfish NUMA 060821 x64 BMI2.exe");
            uciEngine.OutputDataReceived += OutputDataReceived;
            Action action = new Action(() => uciEngine.Validate());
            action.Should().Throw<Exception>();
        }

        [TestMethod]
        public void TestCfishGo()
        {
            string expected;
            expected = UciCommand.ReadyOk;
            Uci.UciEngine uciEngine = new Uci.UciEngine("Uci/Cfish NUMA 060821 x64 BMI2.exe");
            uciEngine.OutputDataReceived += OutputDataReceived;
            Action action = new Action(() => uciEngine.Validate());
            action.Should().Throw<Exception>();
        }

        [TestMethod]
        public void TestCfishEvalFile()
        {
            string expected;
            expected = UciCommand.ReadyOk;
            Uci.UciEngine uciEngine = new Uci.UciEngine("Uci/Cfish NUMA 060821 x64 BMI2.exe");
            uciEngine.OutputDataReceived += OutputDataReceived;
            uciEngine.SetOption("EvalFile", "Test.nnue");
            Action action = new Action(() => uciEngine.Validate());
            action.Should().Throw<Exception>();
        }

        [TestMethod]
        public void TestSugaRIsReady()
        {
            string expected;
            string? actual;
            expected = UciCommand.ReadyOk;
            Uci.UciEngine uciEngine = new Uci.UciEngine("Uci/SugaR AI 2.40 64.exe");
            uciEngine.OutputDataReceived += OutputDataReceived;
            uciEngine.SendCommand(UciCommand.IsReady);
            actual = response;
            actual.Should().Be(expected);
        }

        [TestMethod]
        public void TestSugaR2GO()
        {
            string expected;
            expected = UciCommand.ReadyOk;
            Uci.UciEngine uciEngine = new Uci.UciEngine("Uci/SugaR AI 2.40 64.exe");
            uciEngine.OutputDataReceived += OutputDataReceived;
            Action action = new Action(() => uciEngine.Validate());
            action.Should().Throw<Exception>();
        }

        [TestMethod]
        public void TestSugaR2EvalFile()
        {
            string expected;
            expected = UciCommand.ReadyOk;
            Uci.UciEngine uciEngine = new Uci.UciEngine("Uci/SugaR AI 2.40 64.exe");
            uciEngine.OutputDataReceived += OutputDataReceived;
            uciEngine.SetOption("EvalFile", "Test.nnue");
            Action action = new Action(() => uciEngine.Validate());
            action.Should().Throw<Exception>();
        }

        public void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            response = e.Data;
        }
    }
}
