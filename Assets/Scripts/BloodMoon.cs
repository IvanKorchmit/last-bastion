using UnityEngine;
using LastBastion.Waves;

[CreateAssetMenu(fileName = "Blood Moon", menuName = "Events/Weather/Blood Moon")]
public class BloodMoon : GameEvent, IEventEndable
{
    public enum BloodMoonStatus
    {
        Begin, End
    }
    public static event System.Action<BloodMoonStatus> OnBloodMoonChange;
    public override void Launch()
    {
        AIBase.OnEnemySpawn += AIBase_OnEnemySpawn;
        WavesUtils.OnDayChanged += WavesUtils_OnDayChanged;
    }

    private void AIBase_OnEnemySpawn()
    {
        OnBloodMoonChange?.Invoke(BloodMoonStatus.Begin);
    }

    private void WavesUtils_OnDayChanged(WavesUtils.DayTime obj)
    {
        OnBloodMoonChange?.Invoke(BloodMoonStatus.Begin);
        TimerUtils.AddTimer(10, EndByTime);
    }

    public void End()
    {
        TimerUtils.Cancel(EndByTime);
        OnBloodMoonChange?.Invoke(BloodMoonStatus.End);
        WavesUtils.OnDayChanged -= WavesUtils_OnDayChanged;
        AIBase.OnEnemySpawn -= AIBase_OnEnemySpawn;
    }
    public void EndByTime()
    {
        OnBloodMoonChange?.Invoke(BloodMoonStatus.End);
        WavesUtils.OnDayChanged -= WavesUtils_OnDayChanged;
        AIBase.OnEnemySpawn -= AIBase_OnEnemySpawn;
    }
}