using System;

public class BookView : ToggleableObject
{
    public override event Action<ToggleableObject> Toggled;
}
