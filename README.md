# CommandBuilderBenchmark
##### 概要  
  コマンド用の文字列作る方法の検証  
  Shift-jisでエンコードしたデータを作って、その前後に、区切り文字を入れる  
  バイナリ配列を作ったら完成という仕様  
  .net core 3.1環境で速度比較。  

##### #0 CmdBuilder　コマンド文字列組み立て
|          Method |     Mean |     Error |   StdDev |    Gen 0 |   Gen 1 | Gen 2 | Allocated |
|---------------- |---------:|----------:|---------:|---------:|--------:|------:|----------:|
|            Linq | 491.2 us | 381.66 us | 20.92 us |  53.2227 | 26.3672 |     - | 281.37 KB |
|    LinqParallel | 575.5 us | 574.78 us | 31.51 us |  67.3828 | 33.2031 |     - | 343.13 KB |
|    StringFormat | 474.8 us |  69.20 us |  3.79 us |  53.2227 | 26.3672 |     - | 281.37 KB |
|  ZStringFormatA | 620.3 us | 106.85 us |  5.86 us |  39.0625 | 18.5547 |     - | 203.24 KB |
|  ZStringFormatB | 658.2 us |  86.96 us |  4.77 us |  44.9219 | 19.5313 |     - | 234.49 KB |
|   StringBuilder | 541.3 us |  82.01 us |  4.50 us | 145.5078 | 60.5469 |     - | 757.93 KB |
| ZStringBuilderA | 418.7 us |  44.84 us |  2.46 us |  40.0391 | 19.5313 |     - | 203.24 KB |
| ZStringBuilderB | 346.6 us | 100.34 us |  5.50 us |  44.9219 | 20.0195 |     - | 234.49 KB |

負荷が軽すぎるのか並列処理では効果なし  
ZString.Formatはメモリ使用用少ない。でも速度的に微妙
ZStringのStringBuilderはメモリ使用量も速度も優秀

##### #1 CmdConverter　文字列の配列の前後に区切り用データをつけてバイナリデータに変換
|                 Method |       Mean |    Error |   StdDev |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|----------------------- |-----------:|---------:|---------:|---------:|---------:|---------:|----------:|
|                   List | 2,814.7 us | 165.6 us |  9.08 us | 558.5938 | 234.3750 | 109.3750 |   3.16 MB |
|           MemoryStream |   742.6 us | 224.6 us | 12.31 us | 680.6641 | 347.6563 | 291.9922 |   1.81 MB |
| MemoryStreamEncodeSpan |   701.9 us | 125.8 us |  6.90 us | 375.0000 | 293.9453 | 290.0391 |   1.36 MB |

最後にSpan使って頑張ったわりに処理速度的には効果薄い。  
メモリの使用効率は良い
MemoryStream以外の方法があったら試したいけど要調査

##### #2 FormatBenchmark
|        Method |     Mean |    Error |  StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |---------:|---------:|--------:|-------:|------:|------:|----------:|
|  StringFormat | 177.8 ns | 16.77 ns | 0.92 ns | 0.0608 |     - |     - |      96 B |
| ZStringFormat | 164.1 ns | 13.11 ns | 0.72 ns | 0.0305 |     - |     - |      48 B |

#0 が処理が重かったZString.Format
単純なformat対決してみたらZStringが軽い
#0 で遅くなる原因はなにか別のあるのかな


BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.778 (1909/November2018Update/19H2)  
Intel Core i5-7200U CPU 2.50GHz (Kaby Lake), 1 CPU, 4 logical and 2 physical cores  
.NET Core SDK=3.1.201  
  [Host]   : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT  
  ShortRun : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT  
