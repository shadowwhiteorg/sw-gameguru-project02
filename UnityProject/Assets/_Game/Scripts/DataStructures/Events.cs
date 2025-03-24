namespace _Game.DataStructures
{
    public struct OnLevelInitializeEvent { }
    public struct OnLevelStartEvent { }
    public struct OnLevelFailEvent { }
    public struct OnLevelWinEvent { }

    public struct OnComboEvent
    {
        public int ComboCount { get; }

        public OnComboEvent(int comboCount)
        {
            ComboCount = comboCount;
        }
    }
    public struct OnButtonClickedEvent{}
    public struct OnStopPlatformEvent{}
    public struct OnPlayerChangedPlatformEvent{}
    
}