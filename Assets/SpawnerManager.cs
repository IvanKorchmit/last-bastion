using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    private BoxCollider2D square;
    [SerializeField] private WaveProps[] waves;
    private void Start()
    {
        square = GetComponent<BoxCollider2D>();
    }
    public void Spawn()
    {
        GameObject[] enemies = WavesUtils.FindWave(waves).WaveEnemies;
        int w = WavesUtils.waveNumber;
        int quantity = Mathf.RoundToInt(((float)w * 1.1f) * 10f);
        WavesUtils.areIncoming = true;
        for (int i = 0; i < quantity; i++)
        {
            Instantiate(enemies[Random.Range(0, enemies.Length)], PositionInside(), Quaternion.identity);
        }
        
    }
    private Vector2 PositionInside()
    {
        Vector2 size = square.size;
        return (Vector2)square.bounds.center + new Vector2(
                   (Random.value - 0.5f) * size.x,
                   (Random.value - 0.5f) * size.y);
    }
    private void Update()
    {
        TimerUtils.AddTimer(1f, () =>
        {
            if (WavesUtils.timeRemaining <= 0 && !WavesUtils.areIncoming)
            {
                Spawn();
                return;
            }
            else if (WavesUtils.areIncoming)
            {
                return;
            }
            else
            {
                WavesUtils.timeRemaining--;
            }


        });
    }
}
