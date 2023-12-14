public interface IBurnable {
    public bool IsBurning { get; set; }
    public void StartBurning(int damagePerSecond);
    public void StopBurning();
}