namespace WoosanStudio.ZombieShooter
{
    /// <summary>
    /// 탄창
    /// </summary>
    public class BulletClip
    {
        private int maxCount;
        private int count;

        public BulletClip(int maxCount)
        {
            this.maxCount = maxCount;
        }

        public void Fire()
        {
            Count--;
        }

        public void Recharge()
        {
            Count = MaxCount;
        }

        public int MaxCount { get => maxCount; set => maxCount = value; }
        public int Count { get => count; set => count = value; }
    }
}
