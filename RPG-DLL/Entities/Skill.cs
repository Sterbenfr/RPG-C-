using RPG_DLL.Systems;

public enum SkillEffectType
{
    Damage,
    Heal
}

public class Skill
{
    public string Name { get; set; }
    public Stats Stats { get; set; }
    public SkillEffectType EffectType { get; set; }
    public Skill() { }

    public Skill(string name, Stats stats, SkillEffectType effectType)
    {
        Name = name;
        Stats = stats;
        EffectType = effectType;
    }
}
