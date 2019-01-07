using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace ToStringVsGetName
{
    public enum SmallEnum
    {
        This,
        Is,
        Small,
        Enum
    }

    public enum BigEnum
    {
        This, Is, Big, Enum,
        Hello, World, Iam, Artem,
        Red, Green, Blue, White,
        Black, Yellow, Pink, Orange,
        AAA, BBB, CCC, DDD,
        Alpha, Beta, Gamma, Omega,
        Bang, Pop, Squash, Slop,
        Z, X, C, V
    }

    [MinColumn, MaxColumn]
    public class ToStringVsGetNameBenchmark
    {
        private SmallEnum[] _smallEnumValues;

        private BigEnum[] _bigEnumValues;

        private int _smallEnumSize;

        private int _bigEnumSize;

        private Random _rnd;

        [GlobalSetup]
        public void Setup()
        {
            _smallEnumValues = Enum.GetValues(typeof(SmallEnum)).Cast<SmallEnum>().ToArray();
            _bigEnumValues = Enum.GetValues(typeof(BigEnum)).Cast<BigEnum>().ToArray();

            _smallEnumSize = _smallEnumValues.Length;
            _bigEnumSize = _bigEnumValues.Length;

            _rnd = new Random();
        }

        [Benchmark]
        public string SmallEnumToString()
        {
            return _smallEnumValues[_rnd.Next(_smallEnumSize)].ToString();
        }

        [Benchmark]
        public string BigEnumToString()
        {
            return _bigEnumValues[_rnd.Next(_bigEnumSize)].ToString();
        }

        [Benchmark]
        public string SmallEnumGetName()
        {
            return _smallEnumValues[_rnd.Next(_smallEnumSize)].GetName();
        }

        [Benchmark]
        public string BigEnumGetName()
        {
            return _bigEnumValues[_rnd.Next(_bigEnumSize)].GetName();
        }
    }

    public static class Extensions
    {
        public static string GetName<T>(this T t) where T : struct, IConvertible
        {
            return Enum.GetName(typeof(T), t);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ToStringVsGetNameBenchmark>(
            ManualConfig
                .Create(DefaultConfig.Instance)
                .With(new MemoryDiagnoser())
                );

            Console.ReadLine();
        }
    }
}
