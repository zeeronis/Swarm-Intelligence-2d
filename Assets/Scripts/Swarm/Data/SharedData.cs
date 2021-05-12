
public struct SharedData
{
    public int pathID;
    public int dist;

    public SharedData(int id)
    {
        this.pathID = id;
        dist = int.MaxValue / 2;
    }
}