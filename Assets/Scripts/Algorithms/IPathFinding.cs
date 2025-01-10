namespace PathFinding
{
public interface IPathFinding
{
    Grid Grid { get; }
    bool TryFindPath(Node start, Node end, out Node[] path);
}
}