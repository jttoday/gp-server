using System;
using System.Collections.Generic;
using System.Collections;

namespace GPClient
{

    public class DirectionArray
    {
        public static readonly PositionData[] DIRS = new[]
        {
            new PositionData(1, 0),
            new PositionData(0, 1),
            new PositionData(-1, 0),
            new PositionData(0, -1)
        };
    }

    public interface WeightedGraph
    {
        int Cost(PositionData a, PositionData b);
        IEnumerable<PositionData> Neighbors(PositionData id);

    }

    public class RoomListGraph : WeightedGraph
    {

        public RoomInfo[] rooms;
        public Dictionary<PositionData, RoomInfo> roomMap;

        public RoomListGraph(RoomInfo[] rooms)
        {
            this.rooms = rooms;
            foreach (var room in this.rooms)
            {
                this.roomMap.Add(room.pos, room);
            }
        }

        public void AddRoom(RoomInfo room)
        {
            this.roomMap.Add(room.pos, room);
        }

        public RoomInfo RoomAtPos(PositionData pos)
        {
            if (!this.roomMap.ContainsKey(pos))
            {
                return null;
            }
            return roomMap[pos];
        }
		public List<PositionData> stairPos(int currentLevel, int targetLevel)
        {
            return null;
        }

        public int Cost(PositionData a, PositionData b)
        {
            return 1;
        }

        public bool Passable(RoomInfo r1, RoomInfo r2, int dir)
        {
            if (r1 == null || r2 == null)
            {
                return false;
            }
            int r1Door = dir - r1.rotation;
            r1Door = (r1Door + 4) % 4;
            int r2Door = (4 - dir) - r2.rotation;
            r2Door = (r2Door + 4) % 4;
            return r1.openDoors[r1Door] && r2.openDoors[r2Door];
        }

        public IEnumerable<PositionData> Neighbors(PositionData current)
        {
            int[] DirIdx = { 0, 1, 2, 3 };
            RoomInfo currnetRoom = RoomAtPos(current);
            foreach (var idx in DirIdx)
            {
                var dir = DirectionArray.DIRS[idx];
                PositionData next = new PositionData(current.level, current.x + dir.x, current.y + dir.y);
                RoomInfo nextRoom = RoomAtPos(next);
                if (Passable(currnetRoom, nextRoom, idx))
                {
                    yield return next;

                }
            }
        }
    }

    public class GripGraph : WeightedGraph
    {
        public int[][] map;
        public int width;
        public int height;

        public GripGraph(int[][] map)
        {
            this.map = map;
            this.width = map.Length;
            this.height = map[0].Length;
        }

        public bool InBounds(PositionData id)
        {
            return 0 <= id.x && id.x < width
                && 0 <= id.y && id.y < height;
        }

        public bool Passable(PositionData id)
        {
            return map[id.x][id.y] == 0;
        }

		public int Cost(PositionData a, PositionData b)
        {
            return 1;
        }

        public IEnumerable<PositionData> Neighbors(PositionData current)
        {
            foreach (var dir in DirectionArray.DIRS)
            {
                PositionData next = new PositionData(current.level, current.x + dir.x, current.y + dir.y);
                if (InBounds(next) && Passable(next))
                {
                    yield return next;

                }
            }
        }

    }

	public struct Node<A, B>
    {
        public A Item1;
        public B Item2;
        public Node(A a, B b)
        {
            Item1 = a;
            Item2 = b;
        }
    }


    public class PriorityQueue<T>
    {
        private List<Node<T, int>> elements = new List<Node<T, int>>();

        public int Count
        {
            get { return elements.Count; }
        }

        public void Enqueue(T item, int priority)
        {
            elements.Add(new Node<T, int>(item, priority));
        }

        public T Dequeue()
        {
            int bestIndex = 0;

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].Item2 < elements[bestIndex].Item2)
                {
                    bestIndex = i;
                }
            }

            T bestItem = elements[bestIndex].Item1;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }
    }


    public class AStarSearch
    {
        static public int Heuristic(PositionData a, PositionData b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        static public List<PositionData> findPath(WeightedGraph graph, PositionData start, PositionData goal)
        {
            var cameFrom = new Dictionary<PositionData, PositionData>();
            var costSoFar = new Dictionary<PositionData, int>();
            var frontier = new PriorityQueue<PositionData>();
            bool found = false;

            frontier.Enqueue(start, 0);
            cameFrom[start] = start;
            costSoFar[start] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (current.Equals(goal))
                {
                    found = true;
                    break;
                }

                foreach (var next in graph.Neighbors(current))
                {
                    int newCost = costSoFar[current]
                        + graph.Cost(current, next);
                    if (!costSoFar.ContainsKey(next)
                        || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        int priority = newCost + Heuristic(next, goal);
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
						if (next.Equals(goal))
                        {
                            found = true;
                        }
                    }
                }
            }
            if (!found)
            {
                return null;
            }
            var path = new List<PositionData>();
            path.Add(goal);
            do
            {
                goal = cameFrom[goal];
                path.Add(goal);
            }
            while (!goal.Equals(start));

            path.Reverse();
            return path;
        }
    }


    class PathFinder
    {
        private static List<List<PositionData>> concatPath(List<List<List<PositionData>>> pathArray)
        {
            var n = pathArray.Count;
            var max = 0;
            // find the number of pathes
            // max should only be 1 or 2, otherwise, there maybe some problem
            foreach (var path in pathArray)
            {
                if (path.Count > max)
                {
                    max = path.Count;
                }
            }

            var result = new List<List<PositionData>>();
            for (int j = 0; j < max; j++)
            {
                var path = new List<PositionData>();
                for (int i = 0; i < n; i++)
                {
                    List<PositionData> current = null;
                    if (j > pathArray[i].Count)
                    {
                        current = pathArray[i][0];
                    }
                    else
                    {
                        current = pathArray[i][j];
                    }
                    path.AddRange(current);
                }
                result.Add(path);
            }
            return result;
        }

        private static List<PositionData> minPath(List<List<PositionData>> pathArray)
        {
            var min = pathArray[0].Count;
            var idx = 0;
            for (int i = 0; i < pathArray.Count; i++)
            {
                if (min > pathArray[i].Count)
                {
                    min = pathArray[i].Count;
                    idx = i;
                }
            }
            return pathArray[idx];
        }
		private static List<List<PositionData>> reversePath(List<List<PositionData>> pathArray)
        {
            foreach (var path in pathArray)
            {
                path.Reverse();
            }
            return pathArray;
        }

		private static List<List<PositionData>> findPathToStair(PositionData pos, int level, RoomListGraph gf)
        {
            var stairPos = gf.stairPos(pos.level, level);
            var pathResult = new List<List<PositionData>>();
            foreach (var stair in stairPos)
            {
                var path = AStarSearch.findPath(gf, pos, stair);
                pathResult.Add(path);
            }
            return pathResult;
        }

		private static List<List<PositionData>> pathBetweenStairs(RoomListGraph gf)
        {
            var pathResult = new List<List<PositionData>>();
            var downStair = gf.stairPos(1, 0)[0];
            var upStair = gf.stairPos(1, 2);
            foreach (var stair in upStair)
            {
                var path = AStarSearch.findPath(gf, downStair, stair);
                pathResult.Add(path);
            }
            return pathResult;
        }

        static public List<PositionData> findPathAcrossRoom(PositionData start, PositionData end, RoomInfo[] rooms)
        {
            var gf = new RoomListGraph(rooms);
            var sl= start.level;
            var el = end.level;
				// case 1 : at the same level
			if (start.level == end.level)
            {
                return AStarSearch.findPath(gf, start, end);
            }

            var tmp = new List<List<List<PositionData>>>();

            // case 2 : at 0 level and at 2 level
            if (Math.Abs(sl - el) == 2)
            {

                var path1 = findPathToStair(start, sl, gf);
                var path2 = findPathToStair(end, el, gf);
                path2 = reversePath(path2);
                
                var middlePath = pathBetweenStairs(gf);
				if (sl > el)
                {
                    middlePath = reversePath(middlePath);
                }
                tmp.Add(path1);
                tmp.Add(middlePath);
                tmp.Add(path2);

            }
            else
            // case 3 : abs(sl - el) == 1
            // one among sl and el equals 1
            {
                if (sl == 1)
                {
                    sl = el;
                }
                else if (el == 1)
                {
                    el = sl;
                }
                var path1 = findPathToStair(start, sl, gf);
                var path2 = findPathToStair(end, el, gf);
                path2 = reversePath(path2);

                tmp.Add(path1);
                tmp.Add(path2);
            }
            var path = concatPath(tmp);
            return minPath(path);
        }

		static public List<PositionData> findPathInRoom(PositionData start, PositionData end, int[][] map)
        {
            GripGraph graph = new GripGraph(map);
            return AStarSearch.findPath(graph, start, end);
        }

		static public int[][] errosion(int[][] raw, int radius)
        {
            int width = raw.Length;
            int height = raw[0].Length;
            int[][] result = new int[width][];
            for (int i = 0; i < width; i++)
            {
                result[i] = new int[height];
                for (int j = 0; j < height; j++)
                {

                    result[i][j] = raw[i][j];
					if (raw[i][j] == 0)
                    {
                        bool nearby = false;
						// check if it is in the radius
                        for (int ii = -radius; ii <= radius; ii++)
                        {
                            for (int jj = -radius; jj <= radius; jj++)
                            {
                                if (i + ii< 0 || i + ii >= width || j + jj < 0 || j + jj >= height)
                                {
                                    nearby = true;
                                    break;
                                }
                                if (raw[i + ii][j + jj] != 0)
                                {
                                    nearby = true;
                                    break;
                                }
                            }
                            if (nearby)
                            {
                                break;
                            }
                        }

                        if (nearby)
                        {
                            result[i][j] = 0;
                        }
                    }
                }
            }
            return raw;
        }

        static void Main(String[] args)
        {
            PositionData start = new PositionData(1, 1);
            PositionData end = new PositionData(4, 4);
            int[,] raw = new int[,]{
				{ 0, 0, 0, 0, 0, 0},
				{ 0, 0, 0, 0, 1, 0},
				{ 0, 0, 0, 1, 1, 0},
				{ 0, 0, 0, 0, 0, 0},
				{ 0, 0, 0, 0, 0, 0},
                { 0, 0, 0, 0, 0, 0}
			};
            int[][] map = new int[6][];
            for (int i = 0; i < 6; i++)
            {
                int[] row = new int[6];
                for (int j = 0; j < 6; j++)
                {
                    row[j] = raw[i, j];
                }
                map[i] = row;
            }
            map = errosion(map, 1);
            var path = PathFinder.findPathInRoom(start, end, map);
            foreach (var pos in path)
            {
                Console.WriteLine("X:"+pos.x+ "Y:" + pos.y);
            }
            Console.ReadLine();
        }

    }

}
