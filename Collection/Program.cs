using Collection.DataStructures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Collection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //RkArrayList<int> list = new RkArrayList<int>();
            //for (int i = 0; i < 20; ++i)
            //{
            //    list.Add(i);
            //}
            Random rnd = new Random();
            RkDoubleLinkedList<int> list = new RkDoubleLinkedList<int>();
            Console.WriteLine("Adding to the list...");
            for (int i = 0; i < 10; ++i)
            {
                // Get the value to add
                //int nextValue = rnd.Next(100);
                Console.Write("{0} ", i);
                //bool added = false;
                list.AddToEnd(i);
            }
            Console.WriteLine();
            Console.WriteLine("The list is");
            Console.WriteLine(ArrayToString(list.ToArray()));
            Console.WriteLine(list.Head.Data);
            Console.WriteLine(list.Tail.Data);
            Console.WriteLine(list.Count);
            Console.WriteLine(list.AddToBeginning(99).Data);
            Console.WriteLine(list.Remove(0));
            Console.WriteLine(list.Find(5).Data);
            Console.WriteLine(list.AddAfter(list.Find(5), 100));
            Console.WriteLine(list.Remove(5));
            Console.WriteLine(ArrayToString(list.ToArray()));
        }

        static string ArrayToString(Array array)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            if (array.Length > 0)
            {
                sb.Append(array.GetValue(0));
            }
            for (int i = 1; i < array.Length; ++i)
            {
                sb.AppendFormat(",{0}", array.GetValue(i));
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}
