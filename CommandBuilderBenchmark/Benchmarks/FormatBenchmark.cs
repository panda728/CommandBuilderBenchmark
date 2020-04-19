using System.Text;
using BenchmarkDotNet.Attributes;
using Cysharp.Text;

namespace CommandBuilderBenchmark.Benchmarks {
    [Config(typeof(BenchmarkConfig))]
    public class FormatBenchmark {
        readonly int x;
        readonly int y;
        readonly string format;

        public FormatBenchmark() {
            x = int.Parse("100");
            y = int.Parse("200");
            format = "x:{0}, y:{1}";
        }

        [Benchmark]
        public string StringFormat() {
            return string.Format(format, x, y);
        }

        [Benchmark]
        public string ZStringFormat() {
            return ZString.Format(format, x, y);
        }
    }
}