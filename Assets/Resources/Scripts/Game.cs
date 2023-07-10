using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    private void Awake()
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

    private void Start()
    {
        // TODO: Refactor. Move to player class.
        Player cpu = GameObject.Find("CPU").GetComponent<Player>();
        Player gaia = GameObject.Find("Gaia").GetComponent<Player>();
        Player human = GameObject.Find("Human").GetComponent<Player>();

        Diplomacy[cpu] = new Dictionary<Player, DiplomacyState>();
        Diplomacy[gaia] = new Dictionary<Player, DiplomacyState>();
        Diplomacy[human] = new Dictionary<Player, DiplomacyState>();

        Diplomacy[cpu][cpu] = DiplomacyState.Ally;
        Diplomacy[cpu][gaia] = DiplomacyState.Neutral;
        Diplomacy[cpu][human] = DiplomacyState.Enemy;

        Diplomacy[gaia][cpu] = DiplomacyState.Neutral;
        Diplomacy[gaia][gaia] = DiplomacyState.Ally;
        Diplomacy[gaia][human] = DiplomacyState.Neutral;

        Diplomacy[human][cpu] = DiplomacyState.Enemy;
        Diplomacy[human][gaia] = DiplomacyState.Neutral;
        Diplomacy[human][human] = DiplomacyState.Ally;
    }

    public Dictionary<Player, Dictionary<Player, DiplomacyState>> Diplomacy { get; } = new Dictionary<Player, Dictionary<Player, DiplomacyState>>();
}
