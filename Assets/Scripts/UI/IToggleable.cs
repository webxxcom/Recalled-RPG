public interface IToggleable
{
    bool IsActive { get; }
    void SetActive(bool value);
}
