using UnityEngine;

[CreateAssetMenu(fileName = "Meteor Rain", menuName = "Events/Weather/Meteor Rain")]
public class MeteorRain : GameEvent, IEventEndable
{
    [SerializeField] private GameObject meteorPrefab;
    public GameObject MeteorPrefab => meteorPrefab;
    public override void Launch()
    {
        WeatherUtils.status = WeatherUtils.Status.MeteorRain;
    }
    public void End()
    {
        WeatherUtils.status = WeatherUtils.Status.None;
    }
}

