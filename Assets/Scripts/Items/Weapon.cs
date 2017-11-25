using UnityEngine;

public class Weapon : Item {

    [Header("Weapon Attributes")]
    public int APCost;
    public WeaponType WeaponType;
    public int Damage;
    public float HitRate;
    public int MinimumRange;
    public int MaximumRange;
    public Range WeaponRange;
    public RangefinderMap Rangefinder;

    protected override void OnValidate()
    {
        base.OnValidate();
        Type = ItemTypes.Weapon;
    }
    void OnEnable()
    {
        WeaponRange = new Range(MinimumRange, MaximumRange);
    }

    public override void OnOwnerChanged()
    {
        Rangefinder = new RangefinderMap(this, Owner);
    }

    public void VisualizeAttack()
    {
        Rangefinder.CreateMap(Owner.OccupiedHex);
        Rangefinder.VisualizeMap();
    }
    public void EndVisualization()
    {
        Rangefinder.EndVisualization();
    }
}

public enum WeaponType
{
    Tactical,
    Precise,
    Powerful
}
