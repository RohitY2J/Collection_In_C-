using System;
using System.Collections.Generic;

namespace Collection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            RkArrayList<int> list = new RkArrayList<int>();
            for (int i = 0; i < 20; ++i)
            {
                list.Add(i);
            }


        }        
    }
}
