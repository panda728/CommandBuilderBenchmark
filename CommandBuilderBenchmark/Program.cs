using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using CommandBuilderBenchmark.Benchmarks;

namespace CommandBuilderBenchmark {
    class Program {
        static void Main() {
            // Switcherは複数ベンチマークを作りたい場合ベンリ。
            var switcher = new BenchmarkSwitcher(new[] {
                typeof(CmdBuilder),
                typeof(CmdConverter),
                typeof(FormatBenchmark),
            });

            // 今回は一個だけなのでSwitcherは不要ですが。
            var args = new string[] { "0" };
            switcher.Run(args); // 走らせる
        }
    }

    public class BenchmarkConfig : ManualConfig {
        public BenchmarkConfig() {
            AddExporter(MarkdownExporter.GitHub); // Markdown形式での出力  
            AddDiagnoser(MemoryDiagnoser.Default);
            AddJob(Job.ShortRun); // テスト回数をデフォルトより少なくする  
        }
    }

}