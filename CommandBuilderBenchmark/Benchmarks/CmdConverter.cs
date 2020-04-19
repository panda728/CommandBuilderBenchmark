using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace CommandBuilderBenchmark.Benchmarks {
    // ベンチマーク本体
    [Config(typeof(BenchmarkConfig))]
    public class CmdConverter {
        List<(int, string)> cmdList;
        static readonly byte[] STX = new byte[] { 0x01 };
        static readonly byte[] ETX = new byte[] { 0x02, 0x03 };
        private MemoryStream _ms;

        [GlobalSetup]
        public void Setup() {
            cmdList = new List<(int, string)>();
            var cmds = Enumerable.Range(0, 5000)
                .Select(i => (i % 2 == 0) ? $"{i:00000};X" : $"{i:00000};{Ulid.NewUlid()}{Ulid.NewUlid()}{Ulid.NewUlid()}{Ulid.NewUlid()}{Ulid.NewUlid()}")
                .Select((c, i) => (i, c))
                .ToArray();
            cmdList.AddRange(cmds);
        }

        [Benchmark]
        public byte[] List() {
            var result = cmdList
                .Select(c => {
                    var bytes = new List<byte>();
                    bytes.AddRange(STX);
                    bytes.AddRange(Encoding.ASCII.GetBytes(c.Item2));
                    bytes.AddRange(ETX);
                    return bytes.ToArray();
                })
                .SelectMany(b => b)
                .ToArray();
            return result;
        }

        [Benchmark]
        public byte[] MemoryStream() {
            _ms = new MemoryStream();
            cmdList.ForEach(c => {
                _ms.Write(STX);
                _ms.Write(Encoding.ASCII.GetBytes(c.Item2));
                _ms.Write(ETX);
            });
            return _ms.ToArray();
        }

        [Benchmark]
        public byte[] MemoryStreamEncodeSpan() {
            _ms = new MemoryStream();
            cmdList.ForEach(c => Append(c.Item2, 128));
            return _ms.ToArray();
        }

        private void Append(ReadOnlySpan<char> values, int allocSize) {
            var dataSize = Encoding.ASCII.GetByteCount(values);
            var finalSize = dataSize + STX.Length + ETX.Length;
            byte[] rentByte = null;
            try {
                if (finalSize >= allocSize)
                    rentByte = ArrayPool<byte>.Shared.Rent(finalSize);

                var bytes = (rentByte == null)
                    ? stackalloc byte[finalSize]
                    : rentByte.AsSpan().Slice(0, finalSize);

                STX.CopyTo(bytes);
                Encoding.ASCII.GetBytes(values, bytes.Slice(STX.Length));
                ETX.CopyTo(bytes.Slice(STX.Length + dataSize));

                _ms.Write(bytes.Slice(0, finalSize));
            } finally {
                if (rentByte != null)
                    ArrayPool<byte>.Shared.Return(rentByte);
            }
        }
    }
}
