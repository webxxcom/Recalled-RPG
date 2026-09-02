using UnityEngine.UI;

public interface ISettingsConfigField<T> where T : Selectable
{
    T Serialized { get; set; }
    bool Changed { get; set; }
}
