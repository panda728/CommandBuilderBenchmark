# CommandBuilderBenchmark
-概要
コマンド用の文字列作る
Shift-jisでエンコードしたデータを作って、その前後に、区切り文字を入れる
バイナリ配列を作ったら完成

参考結果

#0 CmdBuilder　コマンド文字列組み立て
|              Method |     Mean |     Error |   StdDev |   Gen 0 |   Gen 1 | Gen 2 | Allocated |
|-------------------- |---------:|----------:|---------:|--------:|--------:|------:|----------:|
|                Linq | 477.2 us |  47.21 us |  2.59 us | 53.7109 | 26.8555 |     - | 281.37 KB |
|        LinqParallel | 437.0 us | 127.36 us |  6.98 us | 66.4063 | 32.7148 |     - | 343.12 KB |
|  LinqZStringFormatA | 629.9 us |  90.67 us |  4.97 us | 39.0625 | 18.5547 |     - | 203.24 KB |
|  LinqZStringFormatB | 666.6 us | 315.01 us | 17.27 us | 44.9219 | 19.5313 |     - | 234.49 KB |
| LinqZStringBuilderA | 421.1 us | 158.58 us |  8.69 us | 39.0625 | 19.0430 |     - | 203.24 KB |
| LinqZStringBuilderB | 349.7 us |  44.27 us |  2.43 us | 44.9219 | 20.0195 |     - | 234.49 KB |

#1 CmdConverter　文字列の配列の前後に区切り用データをつけてバイナリデータに変換
|                 Method |       Mean |    Error |   StdDev |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|----------------------- |-----------:|---------:|---------:|---------:|---------:|---------:|----------:|
|                   List | 2,814.7 us | 165.6 us |  9.08 us | 558.5938 | 234.3750 | 109.3750 |   3.16 MB |
|           MemoryStream |   742.6 us | 224.6 us | 12.31 us | 680.6641 | 347.6563 | 291.9922 |   1.81 MB |
| MemoryStreamEncodeSpan |   701.9 us | 125.8 us |  6.90 us | 375.0000 | 293.9453 | 290.0391 |   1.36 MB |

#2 FormatBenchmark
|        Method |     Mean |    Error |  StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |---------:|---------:|--------:|-------:|------:|------:|----------:|
|  StringFormat | 177.8 ns | 16.77 ns | 0.92 ns | 0.0608 |     - |     - |      96 B |
| ZStringFormat | 164.1 ns | 13.11 ns | 0.72 ns | 0.0305 |     - |     - |      48 B |
