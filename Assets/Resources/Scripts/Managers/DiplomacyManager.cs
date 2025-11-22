using UnityEngine;

public class DiplomacyManager : MonoBehaviour
{
    public static DiplomacyManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        Player cpu = GameObject.Find("CPU").GetComponent<Player>();
        Player gaia = GameObject.Find("Gaia").GetComponent<Player>();
        Player human = GameObject.Find("Human").GetComponent<Player>();

        cpu.SetDiplomacy(cpu, DiplomacyState.Ally);
        cpu.SetDiplomacy(gaia, DiplomacyState.Neutral);
        cpu.SetDiplomacy(human, DiplomacyState.Enemy);

        gaia.SetDiplomacy(cpu, DiplomacyState.Neutral);
        gaia.SetDiplomacy(gaia, DiplomacyState.Neutral);
        gaia.SetDiplomacy(human, DiplomacyState.Neutral);

        human.SetDiplomacy(cpu, DiplomacyState.Enemy);
        human.SetDiplomacy(gaia, DiplomacyState.Neutral);
        human.SetDiplomacy(human, DiplomacyState.Ally);
    }
}
