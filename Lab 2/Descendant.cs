using System;

namespace Lab2
{
    public class Descendant: IComparable
    {
        //Сравнения двух потомков для сортировки
        public double X, Y;
        public int CompareTo(object obj)
        {
            var y = obj as Descendant;
            var delta = Y - y.Y;
            if (delta > 0)
                return 1;
            if (delta < 0)
                return -1;
            return 0;
        }
    }
}
