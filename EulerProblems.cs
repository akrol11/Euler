﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euler1
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Enter the Euler project number to solve. Enter 0 to exit");
            
            string str;
            

            bool done = false;

            int param = 0;
            while (!done)
            {
                str = Console.ReadLine();
                while (!Int32.TryParse(str, out param))
                {
                    Console.WriteLine("Input is not an integer. Try again.");
                    Console.ReadLine();
                }

                if (param == 0)
                    return;

                switch (param)
                {
                    case 1:
                       CalculateEuler1(1000);
                        
                        break;
                    case 201:
                        //CalculateEuler201();
                        CycleThroughSubsets();
                        break;
                    default:
                        Console.WriteLine("Problem {0} has not been solved.", param);
                        break;
                }
                Console.WriteLine("Want to solve another one?");
            }

           
        }

        static void  CalculateEuler1(int param)
        {
            int sum = 0;
            for (int i = 1; i < param; i++)
            {
                if (i % 3 == 0 || i % 5 == 0)
                    sum += i;
            }
            Console.WriteLine("Result = {0}", sum);
        }
        
        static void CalculateEuler201()
        {
            /* S = {1^2, 2^2, ..., 100^2}
             * U(S,50) - unique!(duplicate not included) sums of 50-element subsets of S
             * Question to find sum(U(S,50))
             * Idea: 1. Consider a 50 branch tree (breadth first construction)
             *   Root at level n, is a sum of n-element nodes in S
             *   If there are the same sums across the level then the whole branch can be chopped off.
             *   If we don't run out of memory :), add up the nodes at the bottom of a 50 node tree
             *   
             *  How to trim: Need to remember combinations that can be duplicated
             * 
            */

            var ourSet = new int[] {1, 3, 6, 8, 10, 11};

            int sizeSet = ourSet.Length;
            int sizeSubset = 3;
            
            long minSum = 0;
            for (long i = 0; i < sizeSubset; ++i)
                minSum += ourSet[i];
            Console.WriteLine("The min sum is {0}", minSum);

            long maxSum = 0;
            for (long i = sizeSet; i > sizeSubset-1; --i)
                maxSum += ourSet[i-1];
            Console.WriteLine("The max sum is {0}", maxSum);

            //Min sum = 42925
            //Max sum = 297925

            //var originalSet = new int[100];
            //for (int i = 1; i < 101; i++)
            //{
            //    originalSet[i - 1] = i*i;
            //    Console.Write("a[{0}]={1},", i-1, originalSet[i-1]);
            //}

            // brute force: array of 300000 fill up with 0's
            var summands = new bool[sizeSet];
            var sums = new long[maxSum];
            
            // numOfOperand
            int curSubsetSize = 0;
            
            int foundSum = 0;
            for (long i = minSum; i < maxSum+1; i++)
            {
                // start looking
                foundSum = 0;
                int curInd = 0;
                int curSum = 0;
                int indUsed = 1;

                summands[curInd] = true;
                
                int numOfTimesFound = 0;
                while (numOfTimesFound < 2)
                {
                    curSum += ourSet[i];
                    Console.Write("a[{0}]={1},", i - 1, sums[i - 1]);
                    if (indUsed == sizeSubset)
                        numOfTimesFound += (curSum == i ? 1 : 0);
                    else
                    {
                        if ((curInd + indUsed < sizeSet) && (curSum < maxSum))
                        {
                            //curInd = pickNextIndex(i);
                            ++indUsed;
                        }
                    }
                } 
            }

        }

        static void CycleThroughSubsets()
        {
            const int ArraySize = 40;
            var ourSet = new int[ArraySize];
            for (int j = 0; j < ArraySize; j++)
                ourSet[j] = (j + 1) * (j + 1);

   //         var ourSet = new int[] { 1, 3, 6, 8, 10, 11 }; 
            
            const int sizeOfSubset = 20;
            int sizeOfSet = ourSet.Length;

            //build min max arrays
            var minArray = new int[sizeOfSet + 1,sizeOfSet + 1];
            var maxArray = new int[sizeOfSet + 1,sizeOfSet + 1];

            for (int i = 1; i < sizeOfSet + 1; i++)
            {
                //minArray[i,1] = ourSet[i-1];
                //maxArray[i,1] = ourSet[i-1];

                for (int j = 1; j < sizeOfSet + 1 - (i - 1); j++)
                {
                    minArray[i,j] += minArray[i,j-1] + ourSet[j-1];
                    maxArray[i,j] += maxArray[i,j-1] + ourSet[sizeOfSet - j];
                }
            }

            var sumArray = new int[maxArray[1,sizeOfSet]];

            for (int sum = minArray[1,sizeOfSubset]; sum <= maxArray[1,sizeOfSubset]; sum++)
            {
                var subsetIndArray = new int[sizeOfSubset + 1];

                // start
                subsetIndArray[1] = 1;
                int curSubsetCount = 1;
                int curLargestElem = 1;
                int curSum = ourSet[curLargestElem - 1];
                bool done = false;

                Console.WriteLine("Looking for sum = {0}", sum); 
                while (!done)
                {
                    //building up summands
                    while (curSubsetCount < sizeOfSubset && subsetIndArray[curSubsetCount] < sizeOfSet)
                    {
                        ++curLargestElem;
                        ++curSubsetCount;
                        subsetIndArray[curSubsetCount] = curLargestElem;

                        curSum += ourSet[curLargestElem - 1];
                    }

                    for (int j = 1; j < sizeOfSubset + 1; j++)
                    {
                        Console.Write("{0} + ", ourSet[subsetIndArray[j] - 1]);
                    }
                    Console.WriteLine(" = {0}", curSum);

                    if (curSum == sum)
                    {
                        //found
                        //for (int j = 1; j < sizeOfSubset + 1; j++)
                        //{
                        //    Console.Write("{0} + ", ourSet[subsetIndArray[j] - 1]);
                        //}
                        //Console.WriteLine(" = {0}", curSum);
                        
                        ++sumArray[sum];
                        if (sumArray[sum] > 1)
                            break;
                    }

                    // retreat
                    while (subsetIndArray[curSubsetCount] >= sizeOfSet ||
                           sizeOfSet - subsetIndArray[curSubsetCount] < sizeOfSubset - curSubsetCount ||
                           curSum > sum ||
                           (minArray[curSubsetCount, sizeOfSubset - curSubsetCount] > sum - curSum) ||
                           (maxArray[curSubsetCount, sizeOfSubset - curSubsetCount] < sum - curSum))
                    {
                        //nowhere to retreat
                        if (curSubsetCount == 1)
                        {
                            done = true;
                            break;
                        }

                        // remove current level summand
                        curSum -= ourSet[subsetIndArray[curSubsetCount] - 1];
                        subsetIndArray[curSubsetCount] = 0;
                        
                        // go back one level
                        --curSubsetCount;

                        // up the number for the current
                        curSum -= ourSet[subsetIndArray[curSubsetCount] - 1];
                        curLargestElem = subsetIndArray[curSubsetCount] + 1;

                       
                        curSum += ourSet[curLargestElem - 1];

                        while (maxArray[curSubsetCount, sizeOfSubset - curSubsetCount] < sum - curSum)
                        {
                            curSum -= ourSet[curLargestElem - 1];
                            ++curLargestElem;
                            curSum += ourSet[curLargestElem - 1];
                        }

                        subsetIndArray[curSubsetCount] = curLargestElem;
                    }

                    if (done)
                        break;

                    // last element
                    if (curSubsetCount == sizeOfSubset)
                    {
                        curSum -= ourSet[subsetIndArray[curSubsetCount] - 1];
                        ++curLargestElem;
                        subsetIndArray[curSubsetCount] = curLargestElem;
                        curSum += ourSet[curLargestElem - 1];
                    }


                }
            }

            int finalSum = 0;
            for(int i = 0; i< sumArray.Length; i++)
                finalSum += (sumArray[i] == 1) ? i : 0;

            Console.WriteLine("Final result = {0}", finalSum);

        }
    }
}
    

