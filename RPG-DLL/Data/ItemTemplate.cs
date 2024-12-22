using RPG_DLL.Entities;
using RPG_DLL.Systems;

public class ItemTemplate
{
    public string Name { get; set; }
    public ItemType Type { get; set; }
    public Stats Stats { get; set; }
    public Skill Skill { get; set; } // Pour les équipements
}
