namespace PathFinding.AStar
{
    public class Node
    {
        public int X { get; }
        public int Y { get; }

        public Node Parent { get; set; }
        public bool Walkable { get; set; }

        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }

        /*
         * (A) ._______(C)______________.(B)
         * AC = GCost
         * CB = HCost
         * AB = FCost
         */
        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost => HCost + GCost;
    }
}