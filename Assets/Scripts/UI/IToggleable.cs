public interface IToggleable
{
    bool IsActive { get; }
    void SetIsActive(bool value);
}
