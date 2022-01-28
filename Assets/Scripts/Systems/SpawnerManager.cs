using UnityEngine;
using LastBastion.Waves;
public class SpawnerManager : MonoBehaviour
{
    public static event System.Action<IDamagable[]> OnBossSpawned;
    private BoxCollider2D square;
    [SerializeField] private WaveProps[] waves;
    private void Start()
    {
        square = GetComponent<BoxCollider2D>();
    }
    public void Spawn()
    {
        WaveProps wave = WavesUtils.FindWave(waves);
        if (!wave.IsBoss)
        {
            GameObject[] enemies = wave.WaveEnemies;
            int w = WavesUtils.WaveNumber % 30;
            int quantity = Mathf.RoundToInt(((float)w * 1.01f) * 1.25f);
            for (int i = 0; i < quantity; i++)
            {
                Instantiate(enemies[Random.Range(0, enemies.Length)], PositionInside(square), Quaternion.identity);
            }
        }
        else
        {
            IDamagable[] damagables = new IDamagable[wave.Bosses.Length];
            for (int i = 0; i < wave.Bosses.Length; i++)
            {
                damagables[i] = Instantiate(wave.Bosses[i], PositionInside(square), Quaternion.identity).GetComponent<IDamagable>();
            }

            OnBossSpawned?.Invoke(damagables);
        }
        WavesUtils.SetIncoming();
    }
    public static Vector2 PositionInside(BoxCollider2D square)
    {
        Vector2 size = square.bounds.size;
        Vector2 topLeft = (Vector2)square.bounds.center - size / 2;
        Vector2 bottomRight = (Vector2)square.bounds.center + size / 2;
        // Debug.Log((Vector2)transform.position - (topLeft * Random.value - bottomRight * Random.value));
        return new Vector2(
        Random.Range(square.bounds.min.x, square.bounds.max.x),
        Random.Range(square.bounds.min.y, square.bounds.max.y));
    }
    private void Update()
    {
        TimerUtils.AddTimer(1f, () =>
        {
            if (WavesUtils.TimeRemaining <= 0 && !WavesUtils.AreIncoming)
            {

                Spawn();
                return;
            }
            else if (WavesUtils.AreIncoming)
            {
                return;
            }
            else
            {
                WavesUtils.DecrementTime();
            }


        });
    }
}
