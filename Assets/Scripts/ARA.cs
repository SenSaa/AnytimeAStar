// ARA* 2D (Anytime Repairing A*)
// Based on:
// https://github.com/zhm-real/PathPlanning/blob/master/Search_based_Planning/Search_2D/ARAstar.py
// Original author: huiming zhou
// description: local inconsistency: g-value decreased.
// g(s) decreased introduces a local inconsistency between s and its successors.

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using Enviro;
using Utils;
using System.Threading;

namespace ARAstar
{

    public class AraStar
    {

        private Tuple<int, int> s_start;
        private Tuple<int, int> s_goal;
        private string heuristic_type;
        private Env env;
        private List<Tuple<int,int>> u_set;
        private HashSet<Tuple<int,int>> obs;
        private double e;
        private Dictionary<Tuple<int, int>, double> g;
        private Dictionary<Tuple<int, int>, double> OPEN;
        private HashSet<Tuple<int, int>> CLOSED;
        private Dictionary<Tuple<int, int>, double> INCONS;
        private Dictionary<Tuple<int, int>, Tuple<int,int>> PARENT;
        private List<Tuple<int, int>> path;
        private HashSet<Tuple<int, int>> visited;
        private int plotPathCounter;

        public AraStar(Tuple<int,int> s_start, Tuple<int,int> s_goal, double e, string heuristic_type)
        {
            this.s_start = s_start;
            this.s_goal = s_goal;
            this.heuristic_type = heuristic_type;
            this.env = new Env();
            this.u_set = env.motions;
            this.obs = env.obs;
            this.e = e;
            this.g = new Dictionary<Tuple<int, int>, double>();
            this.OPEN = new Dictionary<Tuple<int,int>, double>();
            this.CLOSED = new HashSet<Tuple<int, int>>();
            this.INCONS = new Dictionary<Tuple<int, int>, double>
            {
            };
            this.PARENT = new Dictionary<Tuple<int, int>, Tuple<int, int>>();
            this.path = new List<Tuple<int, int>>();
            this.visited = new HashSet<Tuple<int, int>>();
        }
        
        public void init()
        {
            this.g[this.s_start] = 0.0;
            this.g[this.s_goal] = double.PositiveInfinity;
            this.OPEN[this.s_start] = this.f_value(this.s_start);
            this.PARENT[this.s_start] = this.s_start;
        }

        public Tuple<List<Tuple<int,int>>,HashSet<Tuple<int,int>>> searching()
        {
            this.init();
            this.ImprovePath();
            this.path = this.extract_path();
            plotPathCounter++;
            Thread.Sleep(1000);
            
            while (this.update_e() > 1)
            {
                this.e -= 0.4;
                foreach (var INCONS_element in INCONS)
                {
                    if (!OPEN.ContainsKey(INCONS_element.Key))
                    {
                        OPEN.Add(INCONS_element.Key, INCONS_element.Value);
                    }
                }
                foreach(var s in OPEN.Keys.ToList())
                {
                    OPEN[s] = f_value(s);
                }
                this.INCONS = new Dictionary<Tuple<int, int>, double>();
                this.CLOSED = new HashSet<Tuple<int, int>>();
                this.ImprovePath();
                this.path = this.extract_path();
                plotPathCounter++;
                Thread.Sleep(1000);
            }
            
            return Tuple.Create(this.path, this.visited);
        }

        // return: e'-suboptimal path
        public void ImprovePath()
        {
            var visited_each = new List<Tuple<int,int>>();
            int iteration = 0;
            while (true)
            {
                var _tup_1 = this.calc_smallest_f();
                var s = _tup_1.Item1;
                var f_small = _tup_1.Item2;
                if (this.f_value(this.s_goal) <= f_small)
                {
                    break;
                }
                this.OPEN.Remove(s);
                this.CLOSED.Add(s);
                foreach (var s_n in this.get_neighbor(s))
                {
                    if (this.obs.Contains(s_n))
                    {
                        continue;
                    }
                    var new_cost = this.g[s] + this.cost(s, s_n);

                    if (!g.ContainsKey(s_n) || new_cost < this.g[s_n])
                    {
                        this.g[s_n] = new_cost;
                        this.PARENT[s_n] = s;
                        visited_each.Add(s_n);
                        if (!this.CLOSED.Contains(s_n))
                        {
                            this.OPEN[s_n] = this.f_value(s_n);
                        }
                        else
                        {
                            this.INCONS[s_n] = 0.0;
                        }
                    }
                }
                iteration++;
            }
            // *
            foreach(var visited_each_element in visited_each)
            {
                this.visited.Add(visited_each_element);
            }
        }

        // return node with smallest f_value in OPEN set.
        public Tuple<Tuple<int,int>,double> calc_smallest_f()
        {
            var s_small = OPEN.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            return Tuple.Create(s_small, this.OPEN[s_small]);
        }

        // find neighbors of current state.
        public List<Tuple<int,int>> get_neighbor(Tuple<int, int> s)
        {
            return (from u in this.u_set
                    select Tuple.Create((s.Item1 + u.Item1), (s.Item2 + u.Item2))).ToList();
        }

        public double update_e()
        {
            var v = double.PositiveInfinity;
            if (OPEN != null && OPEN.Count > 0)
            {
                v = (from s in this.OPEN.Keys
                        select this.g[s] + this.h(s)).Min();
            }
            if (INCONS != null && INCONS.Count > 0)
            {
                v = Math.Min(v, (from s in this.INCONS.Keys
                                select this.g[s] + this.h(s)).Min());
            }
            return Math.Min(this.e, this.g[this.s_goal] / v);
        }

        // f = g + e * h
        // f = cost-to-come + weight * cost-to-go
        public double f_value(Tuple<int,int> x)
        {
            return this.g[x] + this.e * this.h(x);
        }

        // Extract the planning path based on the PARENT set.
        public List<Tuple<int, int>> extract_path()
        {
            var path = new List<Tuple<int,int>> {
                this.s_goal
            };
            var s = this.s_goal;

            string s_str = "";
            double pathCost = 0;

            while (true)
            {
                s = this.PARENT[s];
                path.Add(s);

                s_str += s;
                pathCost += g[s];

                if (s == this.s_start)
                {
                    break;
                }
            }
            Debug.Log("pathCost -> " + pathCost);
            Debug.Log("s_str " + s_str);
            return path.ToList();
        }

        // Calculate heuristic.
        public double h(Tuple<int, int> s)
        {
            var heuristic_type = this.heuristic_type;
            var goal = this.s_goal;
            if (heuristic_type == "manhattan")
            {
                return Math.Abs(goal.Item1 - s.Item1) + Math.Abs(goal.Item2 - s.Item2);
            }
            else
            {
                return MathHelpers.Hypotenuse(goal.Item1 - s.Item1, goal.Item2 - s.Item2);
            }
        }

        // Calculate Cost for this motion.
        public double cost(Tuple<int, int> s_start, Tuple<int, int> s_goal)
        {
            if (this.is_collision(s_start, s_goal))
            {
                return double.PositiveInfinity;
            }
            return MathHelpers.Hypotenuse(s_goal.Item1 - s_start.Item1, s_goal.Item2 - s_start.Item2);
        }

        // check if edge between nodes (s_start, s_end) is in collision.
        public bool is_collision(Tuple<int, int> s_start, Tuple<int, int> s_end)
        {
            object s2;
            object s1;
            if (this.obs.Contains(s_start) || this.obs.Contains(s_end))
            {
                return true;
            }
            if (s_start.Item1 != s_end.Item1 && s_start.Item2 != s_end.Item2)
            {
                if (s_end.Item1 - s_start.Item1 == s_start.Item2 - s_end.Item2)
                {
                    s1 = (Math.Min(s_start.Item1, s_end.Item1), Math.Min(s_start.Item2, s_end.Item2));
                    s2 = (Math.Max(s_start.Item1, s_end.Item1), Math.Max(s_start.Item2, s_end.Item2));
                }
                else
                {
                    s1 = (Math.Min(s_start.Item1, s_end.Item1), Math.Max(s_start.Item2, s_end.Item2));
                    s2 = (Math.Max(s_start.Item1, s_end.Item1), Math.Min(s_start.Item2, s_end.Item2));
                }
                if (this.obs.Contains(s1) || this.obs.Contains(s2))
                {
                    return true;
                }
            }
            return false;
        }


        public List<Tuple<int,int>> getPath()
        {
            return path;
        }

        public HashSet<Tuple<int, int>> getObs()
        {
            return obs;
        }

        public HashSet<Tuple<int,int>> getVisited()
        {
            return visited;
        }

        public int getPlotPathCounter()
        {
            return plotPathCounter;
        }

    }

}