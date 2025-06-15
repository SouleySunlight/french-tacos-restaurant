using UnityEngine;

[CreateAssetMenu(fileName = "Worker", menuName = "Scriptable Objects/Worker")]
public class Worker : ScriptableObject
{
    public string id;
    public int level = 1;
    public float secondsBetweenTasks;
    public int pricePerDay;
    public WorkersRole role = WorkersRole.GRILL;

}

public enum WorkersRole
{
    GRILL,
    CHECKOUT,
    HOTPLATE,
    FRYER
}