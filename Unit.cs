using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TurnBasedStrategy
{
    enum MetalType {None = 0, Copper, Bronze, Iron, Steel };
    enum Species { Human = 0 };
    enum WeaponType { None = 0, Sword, Axe, Spear, Bow };
    class Unit : Object
    {
        public Int32 CurrentMorale;
        public Int32 TotalMorale;
        public Int32 SoldierCount;
        public Int32 TotalSoldierCount;
        public MetalType ArmorMetalType;
        public MetalType WeaponMetalType;
        public Int32 Loyalty;
        public String CommanderName;
        public Int32 BattlesWon = 0;
        public Int32 BattlesLost = 0;
        public bool IsMounted = false;
        public bool ChargeBonus = false;
        public bool IsRouting = false;
        public WeaponType Weapon = WeaponType.None;
        public Vector2 Position;

        private List<Unit> CurrentOpponents = new List<Unit>();
        private Int32 DefaultSoldierCount = 100;
        private Int32 DefaultMorale = 10;
        private Random ran;

        public Unit()
        {
            SoldierCount = DefaultSoldierCount;
            TotalSoldierCount = DefaultSoldierCount;
            TotalMorale = DefaultMorale;
            CurrentMorale = DefaultMorale;
            ArmorMetalType = MetalType.None;
            WeaponMetalType = MetalType.None;
            CommanderName = NameGenerator.GenerateName(Species.Human);
            ran = new Random();
        }

        public Unit(Int32 iSoldierCount, Int32 iTotalMorale,WeaponType wWeaponType , MetalType iWeaponMetalType, MetalType iArmorMetalType)
        {
            this.SoldierCount = iSoldierCount;
            this.TotalSoldierCount = iSoldierCount;
            this.TotalMorale = iTotalMorale;
            this.CurrentMorale = iTotalMorale;
            this.WeaponMetalType = iWeaponMetalType;
            this.ArmorMetalType = iArmorMetalType;
            this.Weapon = wWeaponType;
            CommanderName = NameGenerator.GenerateName(Species.Human);
            ran = new Random();
        }

        public void ReplenishSoldiers(int Replenishment)
        {
            SoldierCount = SoldierCount + Replenishment;

            if (SoldierCount > TotalSoldierCount)
            {
                SoldierCount = TotalSoldierCount;
            }
        }

        public void KillSoldiers(int Damage)
        {
            SoldierCount = SoldierCount - Damage;

            if (SoldierCount <= 0)
            {
                SoldierCount = 0;
                KillUnit();
            }
        }

        public void KillUnit()
        {
            for (int i = 0; i < CurrentOpponents.Count; i++)
            {
                Unit uUnit;
                uUnit = CurrentOpponents.ElementAt(i);

                uUnit.CurrentOpponents.Remove(this);
                uUnit.RecordVictory();
                CurrentOpponents.Clear();
            }
        }


        public void RecordVictory()
        {
            BattlesWon = BattlesWon + 1;
        }

        public void RecordLoss()
        {
            BattlesLost = BattlesLost + 1;
        }

        public double GetChargeBonus(Unit ChargingUnit, Unit DefendingUnit)
        {
            double result;

            if (ChargingUnit.Weapon == WeaponType.Axe)
            {
                result = 1.2;
            }
            else
            {
                result = 1.1;
            }
            if (ChargingUnit.IsMounted)
            {
                result = result + 1;
            }

            if (DefendingUnit.Weapon == WeaponType.Spear)
            {
                result = .9;
            }
            ChargingUnit.ChargeBonus = false;
            return result;
        }

        public void RaiseMorale(int MoraleAmountToRaise)
        {
            TotalMorale = TotalMorale + MoraleAmountToRaise;
        }
        public void MoraleBonus()
        {
            CurrentMorale = CurrentMorale + 1;

            if (CurrentMorale > TotalMorale)
            {
                CurrentMorale = TotalMorale;
            }
        }

        public void MoraleShock()
        {
            CurrentMorale = CurrentMorale - 1;

            if (CurrentMorale < 0)
            {
                CurrentMorale = 0;
            }

            if (((double)CurrentMorale / TotalMorale) < .1)
            {
                Weapon = WeaponType.None;
                WeaponMetalType = MetalType.None;
                ArmorMetalType = MetalType.None;

                if (!IsRouting)
                {
                    RecordLoss();
                }

                IsRouting = true;
                for (int i = 0; i < CurrentOpponents.Count; i++)
                {
                    Unit uUnit;
                    uUnit = CurrentOpponents.ElementAt(i);
                    uUnit.MoraleBonus();
                    uUnit.RecordVictory();
                    uUnit.CurrentOpponents.Remove(this);
                }
                CurrentOpponents.Clear();
            }
        }

        public void AddOpponent(Unit Opponent)
        {
            CurrentOpponents.Add(Opponent);
            Opponent.CurrentOpponents.Add(this);
        }

        public void CalculateMorale(Unit UnitA, Unit UnitB)
        {
            if (UnitA.SoldierCount > ((double)UnitB.SoldierCount * 1.2))
            {
                UnitB.MoraleShock();
            }

            if (UnitB.SoldierCount > ((double)UnitA.SoldierCount * 1.2))
            {
                UnitA.MoraleShock();
            }

            if (UnitA.SoldierCount > ((double)UnitB.SoldierCount * 2))
            {
                UnitB.MoraleShock();
            }

            if (UnitB.SoldierCount > ((double)UnitA.SoldierCount * 2))
            {
                UnitA.MoraleShock();
            }

            if (UnitB.ArmorMetalType > UnitA.ArmorMetalType)
            {
                UnitA.MoraleShock();
            }

            if(UnitA.ArmorMetalType > UnitB.ArmorMetalType)
            {
                UnitB.MoraleShock();
            }
        }

        public void CalculatePostBattleMorale(double UnitADmg, double UnitBDmg, Unit UnitA, Unit UnitB)
        {
            double Apercentdmg;
            double Bpercentdmg;

            Apercentdmg = UnitBDmg / UnitA.SoldierCount;
            Bpercentdmg = UnitADmg / UnitB.SoldierCount;

            if (Apercentdmg > Bpercentdmg)
            {
                UnitA.MoraleShock();
            }
            else if (Bpercentdmg > Apercentdmg)
            {
                UnitB.MoraleShock();
            }
        }

        //returns a randomized value between .2 and .25 
        private double getRandomBaseDamage()
        {

            double ran1 = ran.Next(20, 25);

            return ran1/100;
            
        }

        //Processes the battles this unit is a part of.
        public void ProcessBattles()
        {
            double damage = 0;
            double enemydamage = 0;
            double moraleaffect = 1;
            double enemymoraleaffect = 1;
            double resist = 0;
            double enemyresist = 0;
            double randDamage = .2;
            Unit uOpponent;

            for (int i = 0; i < CurrentOpponents.Count; i++)
            {
                uOpponent = CurrentOpponents.ElementAt(i);

                //base damage reflects how many soldiers total you're going to kill
                //it's randomized between .2 and .25
                randDamage = getRandomBaseDamage();
                double enemybasedamage = (double)uOpponent.SoldierCount * randDamage;
                randDamage = getRandomBaseDamage();
                double basedamage = (double)SoldierCount * randDamage;

                //the 'damage' variable represents the modifier to your basedamage.  This 'damage' number
                //incorporates morale, equipment metal type, and charge bonus

                CalculateMorale(this, uOpponent);

                if ((this.IsRouting ) || (uOpponent.IsRouting))
                {
                    return;
                }

                if (ChargeBonus)
                {
                    basedamage = basedamage * GetChargeBonus(this, uOpponent);
                }
                if (uOpponent.ChargeBonus)
                {
                    enemybasedamage = enemybasedamage * uOpponent.GetChargeBonus(uOpponent, this);
                }
                switch (Weapon)
                {
                    case WeaponType.Sword:
                        break;
                    case WeaponType.Axe:
                        basedamage = basedamage * .9;
                        break;
                    case WeaponType.Spear:
                        if (uOpponent.IsMounted) { basedamage = basedamage * 1.2; }
                        else{basedamage = basedamage * .75;}                        
                        break;
                    default:
                        basedamage = basedamage * .5;
                        break;
                }

                switch (uOpponent.Weapon)
                {
                    case WeaponType.Sword:
                        break;
                    case WeaponType.Axe:
                        enemybasedamage = enemybasedamage * .9;
                        break;
                    case WeaponType.Spear:
                        if (IsMounted) { enemybasedamage = enemybasedamage * 1.2; }
                        else { enemybasedamage = enemybasedamage * .75; }
                        break;
                    default:
                        enemybasedamage = enemybasedamage * .5;
                        break;
                }

                switch (ArmorMetalType)
                {
                    case MetalType.Copper:
                        resist = .1;
                        break;
                    case MetalType.Bronze:
                        resist = .2;
                        break;
                    case MetalType.Iron:
                        resist = .3;
                        break;
                    case MetalType.Steel:
                        resist = .4;
                        break;
                    default:
                        resist = 0;
                        break;
                }

                switch (uOpponent.ArmorMetalType)
                {
                    case MetalType.Copper:
                        enemyresist = .1;
                        break;
                    case MetalType.Bronze:
                        enemyresist = .2;
                        break;
                    case MetalType.Iron:
                        enemyresist = .3;
                        break;
                    case MetalType.Steel:
                        enemyresist = .4;
                        break;
                    default:
                        enemyresist = 0;
                        break;
                }

                switch (uOpponent.WeaponMetalType)
                {
                    case MetalType.Copper:
                        enemydamage = 1.1;
                        break;
                    case MetalType.Bronze:
                        enemydamage = 1.2;
                        break;
                    case MetalType.Iron:
                        enemydamage = 1.3;
                        break;
                    case MetalType.Steel:
                        enemydamage = 1.4;
                        break;
                    default:
                        enemydamage = 1;
                        break;
                }

                switch (WeaponMetalType)
                {
                    case MetalType.Copper:
                        damage = 1.1;
                        break;
                    case MetalType.Bronze:
                        damage = 1.2;
                        break;
                    case MetalType.Iron:
                        damage = 1.3;
                        break;
                    case MetalType.Steel:
                        damage = 1.4;
                        break;
                    default:
                        damage = 1;
                        break;
                }

                //your max damage is affected by how good enemy armor is, same vs same metal 
                //type results in a damage of 1 (which represents 100%).  
                //If you have a step above you get a 10% boost per step up
                damage = damage - enemyresist;
                enemydamage = enemydamage - resist;


                //If you have less than 25% of your total morale, then your morale effect is only 80% effective (.8)
                //If you have less than 50% of your total morale, then your morale effect is only 90% effective (.9)
                //same calculation for opponents
                if (((double)CurrentMorale / TotalMorale) < .25)
                {
                    moraleaffect = .8;
                }
                else if (((double)CurrentMorale / TotalMorale) < .5)
                {
                    moraleaffect = .9;
                }

                if (((double)uOpponent.CurrentMorale / uOpponent.TotalMorale) < .25)
                {
                    enemymoraleaffect = .8;
                }
                else if (((double)uOpponent.CurrentMorale / uOpponent.TotalMorale) < .5)
                {
                    enemymoraleaffect = .9;
                }
                
                //damage = ((double)SoldierCount / TotalSoldierCount) * moraleaffect * damage;
                //enemydamage = ((double)uOpponent.SoldierCount / uOpponent.TotalSoldierCount) * enemymoraleaffect * enemydamage;

                //your current morale affects your max damage
                damage = moraleaffect * damage;
                enemydamage = enemymoraleaffect * enemydamage;

                basedamage = (basedamage * damage)/ CurrentOpponents.Count;
                enemybasedamage = (enemybasedamage * enemydamage)/ uOpponent.CurrentOpponents.Count;

                CalculatePostBattleMorale(basedamage, enemybasedamage, this, uOpponent);

                uOpponent.KillSoldiers((int)basedamage);
                KillSoldiers((int)enemybasedamage);

            }

        }


    }
}
