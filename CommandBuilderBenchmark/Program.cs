using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using CommandBuilderBenchmark.Benchmarks;

namespace CommandBuilderBenchmark {
    class Program {
        static void Main() {
            var switcher = new BenchmarkSwitcher(new[] {
                typeof(CmdBuilder),
                typeof(CmdConverter),
                typeof(FormatBenchmark),
            });
            var args = new string[] { "0" };
            switcher.Run(args); 
        }
    }

    public class BenchmarkConfig : ManualConfig {
        public BenchmarkConfig() {
            AddExporter(MarkdownExporter.GitHub); 
            AddDiagnoser(MemoryDiagnoser.Default);
            AddJob(Job.ShortRun);
        }
    }

}