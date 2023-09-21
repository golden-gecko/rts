using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    void Awake()
    {
        CreateSkills();
    }

    public Skill Get(string name)
    {
        return Skills[name].Clone() as Skill;
    }

    private void CreateSkills()
    {
        Skills["Damage_Aura"] = new AuraDamage("Damage_Aura", 10.0f, 3.0f, 20.0f);
        Skills["Health_Aura"] = new AuraHealth("Health_Aura", 10.0f, 3.0f, 20.0f);

        Skills["Damage"] = new Damage("Damage", 10.0f, 3.0f, 20.0f);
        Skills["Repair"] = new Repair("Repair", 10.0f, 3.0f, 20.0f);
    }

    public Dictionary<string, Skill> Skills { get; } = new Dictionary<string, Skill>();
}
