using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public struct OnGoalCompleted : IEvent { }
    public struct OnLevelCompleted : IEvent { }
    public struct OnLevelFailed : IEvent { }
    public struct OnLevelGiveUp : IEvent { }
    public struct OnLevelEnded : IEvent { }
    public struct OnLevelStarted : IEvent { }

    public struct OnLevelLoaded : IEvent
    {
        public int level;
        public LevelData levelData;

        public OnLevelLoaded(int level,LevelData levelData)
        {
            this.level = level;
            this.levelData = levelData;
        }
    }
    public struct OnCoinChanged : IEvent {

        public int coin;

        public OnCoinChanged(int coin)
        {
            this.coin = coin;
        }
    }

    public struct OnStateChanged : IEvent
    {
        public GameState state;

        public OnStateChanged(GameState state)
        {
            this.state = state;
        }
    }

    public struct OnItemCollected : IEvent
    {
        public Item item;

        public OnItemCollected(Item item)
        {
            this.item = item;
        }
    }
}
