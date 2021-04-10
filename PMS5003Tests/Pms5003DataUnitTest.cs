using System;
using PMS5003;
using PMS5003.Exceptions;
using Xunit;
using Xunit.Abstractions;

namespace PMS5003Tests
{
    public class Pms5003DataUnitTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Pms5003DataUnitTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Test_FromBytes_Ok()
        {
            var tests = new byte[][]
            {
                new byte[]
                {
                    66, 77, 0, 28, 0, 8, 0, 13, 0, 15, 0, 8, 0, 13, 0, 15, 7, 17, 1, 249, 0, 82, 0, 6, 0, 2, 0, 2, 151,
                    0, 2, 248
                },
                new byte[]
                {
                    66, 77, 0, 28, 0, 9, 0, 15, 0, 16, 0, 9, 0, 15, 0, 16, 6, 246, 2, 11, 0, 104, 0, 4, 0, 2, 0, 2, 151,
                    0,
                    3, 11
                },
                new byte[]
                {
                    66, 77, 0, 28, 0, 7, 0, 12, 0, 13, 0, 7, 0, 12, 0, 13, 5, 226, 1, 152, 0, 66, 0, 12, 0, 2, 0, 2,
                    151, 0,
                    3, 84
                }
            };

            // Assert that no exception is thrown
            foreach (var subTest in tests)
            {
                Pms5003Data.FromBytes(subTest);
            }
        }

        [Fact]
        public void Test_FromBytes_ChecksumMismatchException()
        {
            var tests = new byte[][]
            {
                new byte[]
                {
                    66, 77, 0, 28, 0, 8, 0, 13, 0, 16, 0, 8, 0, 13, 0, 16, 6, 45, 1, 167, 0, 101, 0, 10, 0, 8, 0, 151,
                    0, 144, 255, 0
                },
                new byte[]
                {
                    66, 77, 0, 28, 0, 7, 0, 11, 0, 13, 0, 7, 0, 11, 0, 13, 5, 193, 1, 155, 8, 1, 6, 0, 2, 0, 2, 151, 0,
                    3, 44, 0
                },
                new byte[]
                {
                    66, 77, 0, 28, 0, 8, 0, 14, 0, 18, 0, 8, 0, 14, 0, 18, 6, 186, 1, 208, 0, 100, 0, 8, 0, 6, 0, 6,
                    151, 3, 155, 0
                },
                new byte[]
                {
                    66, 77, 0, 28, 0, 7, 0, 10, 0, 10, 232, 0, 10, 0, 33, 208, 10, 165, 0, 70, 0, 4, 0, 0, 0, 0, 151, 0,
                    2, 161, 0, 0,
                }
            };

            foreach (var subTest in tests)
            {
                Assert.Throws<ChecksumMismatchException>(() => Pms5003Data.FromBytes(subTest));
            }
        }

        [Fact]
        public void Test_FromBytes_InvalidStartByteException()
        {
            var tests = new byte[][]
            {
                new byte[]
                {
                    66, 00, 0, 28, 0, 8, 0, 13, 0, 16, 0, 8, 0, 13, 0, 16, 6, 45, 1, 167, 0, 101, 0, 10, 0, 8, 0, 151,
                    0, 144, 255, 0
                },
                new byte[]
                {
                    00, 77, 0, 28, 0, 7, 0, 11, 0, 13, 0, 7, 0, 11, 0, 13, 5, 193, 1, 155, 8, 1, 6, 0, 2, 0, 2, 151, 0,
                    3, 44, 0
                },
            };

            foreach (var subTest in tests)
            {
                Assert.Throws<InvalidStartByteException>(() => Pms5003Data.FromBytes(subTest));
            }
        }

        [Fact]
        public void Test_ToString()
        {
            var pmsData = Pms5003Data.FromBytes(new byte[]
            {
                66, 77, 0, 28, 0, 7, 0, 12, 0, 13, 0, 7, 0, 12, 0, 13, 5, 226, 1, 152, 0, 66, 0, 12, 0, 2, 0, 2,
                151, 0,
                3, 84
            });

            _testOutputHelper.WriteLine(pmsData.ToString());
            Assert.Equal(@"Pms5003Data[
Pm1Standard=7,
Pm2Dot5Standard=12,
Pm10Standard=13,
Pm1Atmospheric=7,
Pm2Dot5Atmospheric=12,
Pm10Atmospheric=13,
ParticlesDiameterBeyond0Dot3=1506,
ParticlesDiameterBeyond0Dot5=408,
ParticlesDiameterBeyond1Dot0=66,
ParticlesDiameterBeyond2Dot5=12,
ParticlesDiameterBeyond5Dot0=2,
ParticlesDiameterBeyond10Dot0=2,
Reserved=38656,
Checksum=852
]
", pmsData.ToString());
        }
    }
}