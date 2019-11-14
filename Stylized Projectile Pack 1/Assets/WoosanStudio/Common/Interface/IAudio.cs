using System;

namespace WoosanStudio.Common.Audio
{
    public interface IAudio
    {
        /// <summary>
        /// 음소거
        /// </summary>
        void Mute();
        /// <summary>
        /// 슬로우 효과시 사용
        /// </summary>
        /// <param name="value">TimeScale 값</param>
        void Pitch(float value);
    }
}
