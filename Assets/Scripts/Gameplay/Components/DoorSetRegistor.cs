using UnityEngine;

[RequireComponent(typeof(Door))]
public class DoorSetRegistor : AutoSetRegistor<Door>
{
    protected override Door Data => GetComponent<Door>();
}
