﻿using LiteDB;
using LiteDB.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LiteDB.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();

            sw.Start();

            //TestMemoryFile.Run(sw);
            //TestAesEncryption.CreateEncryptedFile(sw);
            //TestAesEncryption.ReadEncryptedFile(sw);
            //TestDataPage.Run(sw);
            TestEngine.Run(sw);




            Console.WriteLine("===============================");
            Console.WriteLine("Elapsed: " + sw.ElapsedMilliseconds);
            Console.ReadKey();

        }
    }

}
