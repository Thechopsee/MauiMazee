﻿using MauiMaze.Engine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MauiMaze.Models.ClassicMaze
{
    public class Maze : GameMaze
    {
        public Edge[] Edges { get; set; }
        
        public Maze(int width,int height)
        {
            //Size size=new Size(width,height);
            var edgesToCheck = Generate(width, height);
            var sets = InitializeSets(width, height);

            var mazeEdges = new List<Edge>();
            var random = new Random();

            while (sets.Length > 1 && edgesToCheck.Count > 0)
            {
                var edgeIndex = random.Next(edgesToCheck.Count);
                var edge = edgesToCheck[edgeIndex];

                var set1 = FindSet(sets, edge.Cell1);
                var set2 = FindSet(sets, edge.Cell2);

                if (set1 != set2)
                {
                    UnionSets(sets, set1, set2);
                }
                else
                {
                    mazeEdges.Add(edge);
                }

                edgesToCheck.RemoveAt(edgeIndex);
            }

            Edges = edgesToCheck.Concat(mazeEdges).ToArray();
            this.Width= width;
            this.Height= height;
            //Size = size;
        }

        private static List<Edge> Generate(int Width,int Height)
        {
            var edges = new List<Edge>();

            var cell = 0;
            for (var row = 0; row <Height; row++)
            {
                for (var column = 0; column < Width; column++)
                {
                    if (column != Width - 1)
                    {
                        edges.Add(new Edge(cell, cell + 1));
                    }

                    if (row != Height - 1)
                    {
                        edges.Add(new Edge(cell, cell + Convert.ToInt32(Width)));
                    }

                    cell++;
                }
            }

            return edges;
        }

        private static int[] InitializeSets(int Width, int Height)
        {
            int[] sets = new int[(int)(Width * Height)];
            for (var i = 0; i < sets.Length; i++)
            {
                sets[i] = i;
            }
            return sets;
        }

        private static int FindSet(int[] sets, int element)
        {
            if (sets[element] == element)
            {
                return element;
            }
            else
            {
                sets[element] = FindSet(sets, sets[element]);
                return sets[element];
            }
        }

        private static void UnionSets(int[] sets, int set1, int set2)
        {
            sets[set1] = set2;
        }

    }
}
