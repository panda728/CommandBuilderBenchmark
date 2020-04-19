using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using Cysharp.Text;

namespace CommandBuilderBenchmark.Benchmarks {
    [Config(typeof(BenchmarkConfig))]
    public class CmdBuilder {
        List<(int, string, string, string)> cmdList;

        [GlobalSetup]
        public void Setup() {
            cmdList = new List<(int, string, string, string)>();
            Enumerable.Range(0, 1000)
                .ToList()
                .ForEach(i => cmdList.Add((
                    i, Ulid.NewUlid().ToString(), Ulid.NewUlid().ToString(), Ulid.NewUlid().ToString())));
        }

        [Benchmark]
        public string[] Linq() {
            var result = cmdList
                .Select(c => string.Format("{0:00000};{1},{2},{3}", c.Item1, c.Item2, c.Item3, c.Item4))
                .ToArray();
            return result;
        }

        [Benchmark]
        public string[] LinqParallel() {
            var result = cmdList
                .AsParallel()
                .Select(c => (c.Item1, string.Format("{0:00000};{1},{2},{3}", c.Item1, c.Item2, c.Item3, c.Item4)))
                .OrderBy(c => c.Item1)
                .Select(c => c.Item2)
                .ToArray();
            return result;
        }

        [Benchmark]
        public string[] StringFormat() {
            var result = cmdList
                .Select(c => string.Format("{0:00000};{1},{2},{3}", c.Item1, c.Item2, c.Item3, c.Item4))
                .ToArray();
            return result;
        }

        [Benchmark]
        public string[] ZStringFormatA() {
            var result = cmdList
                .Select(c => ZString.Format("{0:00000};{1},{2},{3}", c.Item1, c.Item2, c.Item3, c.Item4))
                .ToArray();
            return result;
        }

        [Benchmark]
        public string[] ZStringFormatB() {
            var result = cmdList
                .Select(c => ZString.Format("{0};{1},{2},{3}", c.Item1.ToString("00000"), c.Item2, c.Item3, c.Item4))
                .ToArray();
            return result;
        }

        [Benchmark]
        public string[] StringBuilder() {
            var result = cmdList
                .Select(c => {
                    var sb = new StringBuilder();
                    sb.AppendFormat("{0:00000}", c.Item1);
                    sb.Append(",");
                    sb.Append(c.Item2);
                    sb.Append(",");
                    sb.Append(c.Item3);
                    sb.Append(",");
                    sb.Append(c.Item4);
                    return sb.ToString();
                }).ToArray();
            return result;
        }
        [Benchmark]
        public string[] ZStringBuilderA() {
            var result = cmdList
                .Select(c => {
                    using var sb = ZString.CreateStringBuilder();
                    sb.AppendFormat("{0:00000}", c.Item1);
                    sb.Append(",");
                    sb.Append(c.Item2);
                    sb.Append(",");
                    sb.Append(c.Item3);
                    sb.Append(",");
                    sb.Append(c.Item4);
                    return sb.ToString();
                }).ToArray();
            return result;
        }
        [Benchmark]
        public string[] ZStringBuilderB() {
            var result = cmdList
                .Select(c => {
                    using var sb = ZString.CreateStringBuilder();
                    sb.Append(c.Item1.ToString("00000"));
                    sb.Append(",");
                    sb.Append(c.Item2);
                    sb.Append(",");
                    sb.Append(c.Item3);
                    sb.Append(",");
                    sb.Append(c.Item4);
                    return sb.ToString();
                }).ToArray();
            return result;
        }
    }
}
