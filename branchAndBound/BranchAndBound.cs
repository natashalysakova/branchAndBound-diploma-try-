using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace branchAndBound
{
    public class BranchAndBound
    {

        static List<char> _chars = new List<char>()
            {
                'A',
                'B',
                'C',
                'D',
                'E',
                'F'
            };

        private const int M = -1;


        public static string Calculate(List<List<int>> data)
        {
            List<string> answer = new List<string>();
            Dictionary<Point, int> lowHamiltonBound = new Dictionary<Point, int>();

            int[] di = new int[data.Count];
            int[] dj = new int[data.Count];
            di = GetDi(data);

            do
            {

                for (int i = 0; i < data.Count; i++)
                {
                    for (int j = 0; j < data[i].Count; j++)
                    {
                        if (data[i][j] < 0)
                            continue;
                        data[i][j] -= di[i];
                    }
                }

                dj = GetDj(data);

                for (int i = 0; i < data.Count; i++)
                {
                    for (int j = 0; j < data.Count; j++)
                    {
                        if (data[j][i] < 0)
                            continue;
                        data[j][i] -= dj[i];
                    }
                }

                Dictionary<Point, int> reductionConsts = new Dictionary<Point, int>();
                int H = di.Sum() + dj.Sum();


                di = GetDi(data, false);
                dj = GetDj(data, false);
                List<List<int>> tmp = new List<List<int>>();
                for (int i = 0; i < data.Count; i++)
                {
                    tmp.Add(new List<int>());
                    for (int j = 0; j < data[i].Count; j++)
                    {
                        if (data[i][j] == 0)
                        {
                            int summ = di[i] + dj[j];
                            tmp[i].Add(summ);
                            reductionConsts.Add(new Point(i, j), summ);
                        }
                        else
                        {
                            tmp[i].Add(data[i][j]);
                        }
                    }
                }

                if (reductionConsts.Count == 0)
                {
                    //foreach (char c in _chars)
                    //{
                    //    if (!answer.Contains(c.ToString()) && answer.Count < data.Count)
                    //    {
                    //        answer.Add(c.ToString());
                    //    }
                    //}
                    //answer.Add(answer[0]);

                    break;
                }


                int max = reductionConsts.Max(x => x.Value);
                KeyValuePair<Point, int> normal = reductionConsts.FirstOrDefault(x => x.Value == max);

                //Нижняя граница гамильтоновых циклов этого подмножества:
                int normalValue = H + normal.Value;

                //Исключение ребра проводим путем замены элемента dij = 0 на M, после чего осуществляем очередное приведение матрицы расстояний для образовавшегося подмножества(1 *, 4 *), в результате получим редуцированную матрицу.

                data[normal.Key.Row][normal.Key.Col] = M;
                di = GetDi(data);
                dj = GetDj(data);

                for (int i = 0; i < data.Count; i++)
                {
                    for (int j = 0; j < data[i].Count; j++)
                    {
                        if (i == normal.Key.Row || j == normal.Key.Col)
                            data[i][j] = M;
                    }
                }

                int otherValue = H + di.Sum() + dj.Sum();
                Debug.WriteLine("H = " + normalValue);
                Debug.WriteLine("H' = " + otherValue);

                data[normal.Key.Col][normal.Key.Row] = M;
                di = GetDi(data);
                dj = GetDj(data);


                if (otherValue <= normalValue)
                {
                    lowHamiltonBound.Add(normal.Key, otherValue);
                }
                else
                {
                    lowHamiltonBound.Add(normal.Key, normalValue);
                }

                PrintMatrix(di, dj, data);

            } while (true);


            //for (int i = 0; answer.Count < lowHamiltonBound.Count; i++)
            //{
            //    var pair = lowHamiltonBound.ToList()[i];
            //    answer.Add($"({pair.Key.Row}, {pair.Key.Col})");

            //    KeyValuePair<Point, int> c = lowHamiltonBound.ToList().Where(x => x.Key.Row == pair.Key.Col).First();
            //    i = lowHamiltonBound.ToList().IndexOf(c)-1;
            //}

            for (int i = 0; answer.Count < lowHamiltonBound.Count; i++)
            {
                var pair = lowHamiltonBound.ToList()[i];

                if (i == 0)
                {
                    answer.Add($"{_chars[pair.Key.Row]}");
                    answer.Add($"{_chars[pair.Key.Col]}");
                }
                else
                {
                    answer.Add($"{_chars[pair.Key.Col]}");
                }

                KeyValuePair<Point, int> c = lowHamiltonBound.ToList().First(x => x.Key.Row == pair.Key.Col);
                i = lowHamiltonBound.ToList().IndexOf(c) - 1;
            }

            answer.Add(answer[0]);

            return string.Join(" => ", answer);
        }


        private static int[] GetDi(List<List<int>> data, bool withZero = true)
        {
            int[] di = new int[data.Count];
            for (int i = 0; i < data.Count; i++)
            {
                di[i] = GetMinimum(data[i], withZero);
            }
            return di;
        }


        private static int[] GetDj(List<List<int>> data, bool withZero = true)
        {
            List<List<int>> transpData = TransData(data);
            int[] dj = new int[data.Count];

            for (int i = 0; i < data.Count; i++)
            {
                dj[i] = GetMinimum(transpData[i], withZero);
            }
            return dj;
        }

        private static int GetMinimum(List<int> array, bool withZero)
        {
            int min = int.MaxValue;
            if (withZero)
            {
                foreach (int i in array)
                {
                    if (i < min && i >= 0)
                        min = i;
                }
            }
            else
            {
                int countOfZero = 0;
                foreach (int i in array)
                {
                    if (i == 0)
                        countOfZero++;
                    if (countOfZero > 1)
                        return 0;

                    if (i < min && i > 0)
                        min = i;
                }

            }

            if (min == int.MaxValue)
                return 0;

            return min;
        }

        private static List<List<int>> TransData(List<List<int>> data)
        {
            List<List<int>> trans = new List<List<int>>();
            for (int i = 0; i < data.Count; i++)
            {
                trans.Add(new List<int>());
                for (int j = 0; j < data[i].Count; j++)
                {
                    trans[i].Add(data[j][i]);
                }
            }

            return trans;
        }

        private static void PrintMatrix(int[] di, int[] dj, List<List<int>> data)
        {
            Debug.WriteLine("========================");
            for (int i = 0; i <= data.Count; i++)
            {
                Debug.Write(i + "\t");
            }
            Debug.Write("\n");
            for (int i = 0; i < data.Count; i++)
            {
                Debug.Write((i + 1) + " |\t");
                for (int j = 0; j < data[i].Count; j++)
                {
                    if (data[i][j] == M)
                        Debug.Write(" \t");
                    else
                        Debug.Write(data[i][j] + "\t");
                }
                Debug.WriteLine("|" + di[i]);
            }
            Debug.Write("\t");
            for (int i = 0; i < data.Count; i++)
            {
                Debug.Write(dj[i] + "\t");
            }
            Debug.Write(di.Sum() + dj.Sum());

            Debug.Write("\n");
            Debug.WriteLine("========================");

        }

        private static void PrintMatrix(List<List<int>> data)
        {
            Debug.WriteLine("========================");
            for (int i = 0; i <= data.Count; i++)
            {
                Debug.Write(i + "\t");
            }
            Debug.Write("\n");
            for (int i = 0; i < data.Count; i++)
            {
                Debug.Write((i + 1) + " |\t");
                for (int j = 0; j < data[i].Count; j++)
                {
                    if (data[i][j] == M)
                        Debug.Write(" \t");
                    else
                        Debug.Write(data[i][j] + "\t");
                }
            }
            Debug.Write("\t");
            Debug.Write("\n");
            Debug.WriteLine("========================");

        }


        private class Point
        {
            public int Row { get; set; }
            public int Col { get; set; }

            public Point(int row, int col)
            {
                Row = row;
                Col = col;
            }
        }
    }
}
