using _Game.Systems.PlatformSystem;

namespace _Game.Interfaces
{
    public interface IPlatformMovement
    {
        float CurrentSpeed { get; }
        public void RegisterPlatform(Platform newPlatform);
    }
}