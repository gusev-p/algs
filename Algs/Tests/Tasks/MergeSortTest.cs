using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Algs.Tasks.Sorting;
using Algs.TestUtilities;
using NUnit.Framework;

namespace Algs.Tests.Tasks
{
    [TestFixture]
    public class MergeSortTest
    {
        [Test]
        public void TestSimple()
        {
            CheckMergeSort(5, 3, 8, 1);
        }

        [Test]
        public void TestSimple2()
        {
            CheckMergeSort(2, 1, 3, 1, 2);
        }

        [Test]
        public void TestRandom()
        {
            const int testCount = 100;
            const int maxArrayLength = 10000;
            const int maxValue = 1000;

            var random = new Random();
            for (var i = 0; i < testCount; i++)
            {
                var arrayLength = random.Next(maxArrayLength);
                var testArray = new int[arrayLength];
                random.NextInts(testArray, maxValue);
                CheckSort(a => QuickSorter2.QuickSort(testArray), testArray);
            }
        }
        
        [Test]
        public void TestRandomBubbleSort()
        {
            const int testCount = 1000;
            const int arrayLength = 100;
            const int maxValue = 1000;

            var random = new Random();
            var testArray = new int[arrayLength];
            for (var i = 0; i < testCount; i++)
            {
                random.NextInts(testArray, maxValue);
                CheckBubbleSort(testArray);
            }
        }
        
        [Test]
        public void TestRandomShit()
        {
            const int testCount = 10000;
            const int arrayLength = 100;
            const int maxValue = 1000;

            var random = new Random();
            var testArray = new int[arrayLength];
            for (var i = 0; i < testCount; i++)
            {
                random.NextInts(testArray, maxValue);

                var etalon = testArray.Copy();
                Array.Sort(etalon);
                var countingInversions = new CountingInversions();
                var sorted = countingInversions.SortAndCount(new List<int>(testArray))
                    .Item2.ToArray();
                Assert.That(sorted, Is.EqualTo(etalon));
            }
        }

        [Test]
        public void ЗаебалоБЛЯТЬЭТОДРОЧЕВОСУКА()
        {
            const string sss = "139 182 536 133 169 655 715 786 973 718 965 132 152 706 848 95 468 343 281 539 838 610 27 166 420 523 87 841 683 849 244 154 182 838 806 495 134 480 96 526 412 128 592 865 5 25 866 343 89 982 691 88 985 828 745 530 156 371";
            var data = sss.Split(' ').Select(int.Parse).ToArray();

            var etalon = data.Copy();
            //var countingInversions = new CountingInversions();
            //countingInversions.SortAndCount(new List<int>(etalon));

            var mergeSorter = new MergeSorter(data);
            mergeSorter.Sort();
        }

        [Test]
        public void Safdasdf()
        {
            const string filePath =
                @"C:\sources\knopka.statistics\_Modules\ConsoleApplication1\ConsoleApplication1\bin\Debug\input.txt";
            var numberStrings = File.ReadAllLines(filePath);
            var numbers = Array.ConvertAll(numberStrings, int.Parse);
            var mergeSorter = new MergeSorter(numbers);
            mergeSorter.Sort();
            Console.Out.WriteLine(mergeSorter.InversionsCount);
        }

        [Test]
        public void TestRandomInversions()
        {
            const int testCount = 10;
            const int maxArrayLength = 10000;
            const int maxValue = 1000;

            var random = new Random();
            for (var i = 0; i < testCount; i++)
            {
                var arrayLength = random.Next(maxArrayLength);
                var testArray = new int[arrayLength];
                random.NextInts(testArray, maxValue);

                //var countingInversions = new CountingInversions();
                //var etalon = testArray.Copy();
                //var anotherCopy = testArray.Copy();
                //var etalonShit = countingInversions.SortAndCount(new List<int>(etalon));
                var etalon = testArray.Copy();
                var bubbleSorter = new BubbleSortInversionsCounter(etalon);
                bubbleSorter.Sort();

                var mergeSorter = new MergeSorter(testArray);
                mergeSorter.Sort();
                Assert.That(mergeSorter.InversionsCount, Is.EqualTo(bubbleSorter.InversionsCount));
                Console.Out.WriteLine("test {0} done", i);
                //Assert.That(testArray, Is.EqualTo(etalonShit.Item2.ToArray()));
                //if (mergeSorter.InversionsCount != etalonShit.Item1)
                //{
                //    var dump = string.Join(" ", anotherCopy);
                //    Console.Out.WriteLine("SHIT!!!");
                    
                //    Console.Out.WriteLine("data:");
                //    Console.Out.WriteLine(dump);

                //    Console.Out.WriteLine("etalon fucking inversions:");
                //    Console.Out.WriteLine(string.Join(" ", countingInversions.FuckingInversions.ToArray()));
                    
                //    Console.Out.WriteLine("merge fucking inversions:");
                //    Console.Out.WriteLine(string.Join(" ", mergeSorter.FuckingInversions.ToArray()));
                //    Assert.Fail();
                //}
            }
        }

        private class BubbleSortInversionsCounter
        {
            private readonly int[] data;

            public BubbleSortInversionsCounter(int[] data)
            {
                this.data = data;
            }

            public int InversionsCount { get; private set; }

            public void Sort()
            {
                var sorted = false;
                while (!sorted)
                {
                    sorted = true;
                    for (var i = 0; i < data.Length - 1; i++)
                        if (data[i] > data[i + 1])
                        {
                            var t = data[i];
                            data[i] = data[i + 1];
                            data[i + 1] = t;
                            sorted = false;
                            InversionsCount++;
                        }
                }
            }
        }

        public class CountingInversions
        {
            public CountingInversions()
            {
                FuckingInversions = new List<int>();
            }

            public List<int> FuckingInversions { get; set; }
            public int totalInversions;
            public Tuple<int, List<int>> SortAndCount(List<int> list)
            {
                if (list.Count <= 1)
                    return new Tuple<int, List<int>>(0, list);

                // divide
                int middle = list.Count / 2;
                var leftList = list.GetRange(0, middle);
                var rightList = list.GetRange(middle, list.Count - leftList.Count);

                // process recursively
                Tuple<int, List<int>> leftResult = SortAndCount(leftList);
                Tuple<int, List<int>> rightResult = SortAndCount(rightList);

                // merge
                Tuple<int, List<int>> mergeResult = MergeAndCount(leftResult.Item2, rightResult.Item2);

                var result = new Tuple<int, List<int>>(leftResult.Item1 + rightResult.Item1 + mergeResult.Item1,
                    mergeResult.Item2);
                totalInversions += mergeResult.Item1;
                FuckingInversions.Add(totalInversions);
                return result;
            }

            private Tuple<int, List<int>> MergeAndCount(List<int> leftList, List<int> rightList)
            {
                int inversions = 0;
                var outputList = new List<int>();
                int i = 0, j = 0;

                while (i < leftList.Count && j < rightList.Count)
                {
                    if (leftList[i] < rightList[j])
                    {
                        outputList.Add(leftList[i]);
                        i++;
                    }
                    else
                    {
                        outputList.Add(rightList[j]);
                        j++;
                        inversions += leftList.Count - i;
                    }
                }

                // we still have values in one of lists 
                if (i < leftList.Count)
                    outputList.AddRange(leftList.GetRange(i, leftList.Count - i));
                else if (j < rightList.Count)
                    outputList.AddRange(rightList.GetRange(j, rightList.Count - j));

                return new Tuple<int, List<int>>(inversions, outputList);
            }
        }

        private static void CheckMergeSort(params int[] sample)
        {
            CheckSort(a => new MergeSorter(a).Sort(), sample);
        }
        
        private static void CheckBubbleSort(params int[] sample)
        {
            CheckSort(a => new BubbleSortInversionsCounter(a).Sort(), sample);
        }

        private static void CheckSort(Action<int[]> sorter, params int[] sample)
        {
            var etalon = sample.Copy();
            Array.Sort(etalon);
            sorter(sample);
            Assert.That(sample, Is.EqualTo(etalon));
        }
    }
}