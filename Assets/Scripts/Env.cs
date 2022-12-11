using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Enviro
{
    public class Env
    {

        public int x_range;
        public int y_range;
        public List<Tuple<int, int>> motions;
        public HashSet<Tuple<int, int>> obs;

        public Env()
        {
            this.x_range = 51;
            this.y_range = 31;
            this.motions = new List<Tuple<int, int>> {
                new Tuple<int, int>(-1, 0),
                new Tuple<int, int>(-1, 1),
                new Tuple<int, int>(0, 1),
                new Tuple<int, int>(1, 1),
                new Tuple<int, int>(1, 0),
                new Tuple<int, int>(1, -1),
                new Tuple<int, int>(0, -1),
                new Tuple<int, int>(-1, -1)
            };
            this.obs = this.obs_map();
        }

        public void update_obs(HashSet<Tuple<int, int>> obs)
        {
            this.obs = obs;
        }

        // Initialize obstacles' positions
        public HashSet<Tuple<int, int>> obs_map()
        {
            var x = this.x_range;
            var y = this.y_range;
            var obs = new HashSet<Tuple<int, int>>();
            foreach (var i in Enumerable.Range(0, x))
            {
                obs.Add(Tuple.Create(i, 0));
            }
            foreach (var i in Enumerable.Range(0, x))
            {
                obs.Add(Tuple.Create(i, y - 1));
            }
            foreach (var i in Enumerable.Range(0, y))
            {
                obs.Add(Tuple.Create(0, i));
            }
            foreach (var i in Enumerable.Range(0, y))
            {
                obs.Add(Tuple.Create(x - 1, i));
            }
            foreach (var i in Enumerable.Range(10, 21 - 10))
            {
                obs.Add(Tuple.Create(i, 15));
            }
            foreach (var i in Enumerable.Range(0, 15))
            {
                obs.Add(Tuple.Create(20, i));
            }
            foreach (var i in Enumerable.Range(15, 30 - 15))
            {
                obs.Add(Tuple.Create(30, i));
            }
            foreach (var i in Enumerable.Range(0, 16))
            {
                obs.Add(Tuple.Create(40, i));
            }
            return obs;
        }
    }
}