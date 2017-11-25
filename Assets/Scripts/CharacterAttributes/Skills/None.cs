public class None : Skill
{
    public None()
    {
        Name = "None";
        Description = "Always room to grow!";
        Type = SkillTypes.None;

    }

    public override void ActivateSkill(object[] args) { }
}
