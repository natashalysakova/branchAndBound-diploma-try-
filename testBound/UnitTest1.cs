using System;
using System.Collections.Generic;
using System.Diagnostics;
using branchAndBound;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace testBound
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            List<List<int>> arr = new List<List<int>>()
            {
                new List<int>() {  -1, 90, 80, 40, 100},
                new List<int>() { 60, -1, 40, 50, 70},
                new List<int>() { 50, 30, -1, 60, 20},
                new List<int>() { 10, 70, 20, -1, 50},
                new List<int>() { 20, 40, 50, 20, -1}

            };

            BranchAndBound algorythm = new BranchAndBound(arr);

            string s = algorythm.Calculate();
            Debug.WriteLine(s);

        }
    }
}
