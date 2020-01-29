using System;
using System.Collections.Generic;

namespace Task_11
{
    enum Level { Easy, Medium, Hard }

    class Sudoku
    {
        public List<List<int>> ArrData { get; set; } = new List<List<int>>();
        public Level GameLevel { get; set; } = Level.Easy;

        public List<int> Generate(int start)
        {
            List<int> tmp = new List<int>();

            for (int i = 0; i < 9; i++)
            {
                tmp.Add((start % 9) + 1);
                start++;
            }

            ArrData.Add(tmp);

            return tmp;
        }

        public void SortRow(int first, int second)
        {
            List<int> tmp = new List<int>();
            tmp = ArrData[first];
            ArrData[first] = ArrData[second];
            ArrData[second] = tmp;
        }
        public void SortCol(int first, int second)
        {
            int tmp = 0;

            for (int i = 0; i < ArrData[0].Count; i++)
            {
                tmp = ArrData[i][first];
                ArrData[i][first] = ArrData[i][second];
                ArrData[i][second] = tmp;
            }
        }
        public void ReverseRow()
        {
            ArrData.Reverse();
        }

        public void NullGrid()
        {
            Random rnd = new Random();
            int max = 0;

            switch (GameLevel)
            {
                case Level.Easy:
                    max = 16;
                    break;

                case Level.Medium:
                    max = 37;
                    break;

                case Level.Hard:
                    max = 64;
                    break;
            }

            for (int i = 0; i < max; i++)
            {
                ArrData[rnd.Next(0, ArrData.Count)][rnd.Next(0, ArrData.Count)] = 0;
            }
        }
    }
}