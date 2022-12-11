using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Utils
{
    public static class MathHelpers
    {

        // Find hypotenuse of triangles, when given the remaining two sides.
        // List<double> as Input and Output.
        public static List<double> Hypotenuse(List<double> side1, List<double> side2)
        {
            List<double> hypot = new List<double>();
            for (int i = 0; i < side1.Count; i++)
            {
                hypot.Add(Math.Sqrt(Math.Pow(side1[i], 2) + Math.Pow(side2[i], 2)));
            }
            return hypot;
        }
        // double data type as Input & Output. 
        public static double Hypotenuse(double side1, double side2)
        {
            double hypot = 0;
            {
                hypot = Math.Sqrt(Math.Pow(side1, 2) + Math.Pow(side2, 2));
            }
            return hypot;
        }

        // Sum Tuples of double.
        public static Tuple<double,double> SumDoubleTuples(Tuple<double,double> tpl1, Tuple<double,double> tpl2)
        {
            double item1 = tpl1.Item1 + tpl2.Item1;
            double item2 = tpl1.Item2 + tpl2.Item2;
            return Tuple.Create(item1, item2);
        }
        // Sum Tuples of int.
        public static Tuple<int,int> SumIntTuples(Tuple<int,int> tpl1, Tuple<int,int> tpl2)
        {
            int item1 = tpl1.Item1 + tpl2.Item1;
            int item2 = tpl1.Item2 + tpl2.Item2;
            return Tuple.Create(item1, item2);
        }

        public static T MinBy<T, C>(this IEnumerable<T> items, Func<T, C> projection) where C : IComparable<C>
        {
            return items.Aggregate((acc, e) => projection(acc).CompareTo(projection(e)) <= 0 ? acc : e);
        }

    }
}
