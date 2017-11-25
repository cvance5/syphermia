using UnityEngine;

public class CombatManager : Singleton<CombatManager> {

    public static void SimulateCombat(CharacterData attacker, CharacterData defender)
    {
        if(!defender.AttemptDodge(attacker.CalculateHitChance()))
        { 
            defender.CalculateDamageRecieved(attacker.CalculateDamageDealt());
            print(defender.Name + " has " + defender.CurrentHealth + " health remaining!");
        }
        else
        {
            print(defender.Name + " has dodged!");
        }

        if(defender.CurrentHealth <= 0)
        {
            TurnManager.RemoveCharacter(defender);
            if(defender.CurrentArmy.ArmySize - 1 == 0)
            {
                BattleManager.EndBattle();
            }
            defender.LeaveArmy();
            defender.Kill();
        }
    }

    public const int MAX_DODGE_ATTEMPTS = 100;
}
