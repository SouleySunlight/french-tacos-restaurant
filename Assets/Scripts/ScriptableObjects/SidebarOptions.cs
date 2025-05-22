using UnityEngine;

[CreateAssetMenu(fileName = "SidebarOptions", menuName = "Scriptable Objects/SidebarOptions")]
public class SidebarOptions : ScriptableObject
{
    public new string name;

    public ViewToShowEnum viewToShow;

}
