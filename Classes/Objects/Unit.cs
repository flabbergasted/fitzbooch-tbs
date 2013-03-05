using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TurnBasedStrategy.Classes.Objects.Items;

namespace TurnBasedStrategy
{
    enum Species { Human = 0 };
    class Unit : Object
    {
        public Int32 CurrentMorale;
        public Int32 TotalMorale;
        public Int32 SoldierCount;
        public Int32 TotalSoldierCount;
        public Int32 Loyalty;
        public String CommanderName;
        public Int32 BattlesWon = 0;
        public Int32 BattlesLost = 0;
        public bool IsMounted = false;
        public bool ChargeBonus = false;
        public bool IsRouting = false;
        public Terrain CurrentPosition;
        public Terrain Destination;
        
        private Weapon Weapon;
        private Armor Armor;
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
            Armor = new Armor();
            Weapon = new Weapon();
            CommanderName = NameGenerator.GenerateName(Species.Human);
            ran = new Random();
        }

        public Unit(Int32 iSoldierCount, Int32 iTotalMorale,WeaponType wWeaponType , MetalType iWeaponMetalType, MetalType iArmorMetalType)
        {
            this.SoldierCount = iSoldierCount;
            this.TotalSoldierCount = iSoldierCount;
            this.TotalMorale = iTotalMorale;
            this.CurrentMorale = iTotalMorale;
            Weapon = new Weapon(wWeaponType, new Metal(iWeaponMetalType));
            Armor = new Armor(new Metal(iArmorMetalType));
            CommanderName = NameGenerator.GenerateName(Species.Human);
            ran = new Random();
        }        

        //'heals' casualties by the a specified amount, capped by max size
        public void ReplenishSoldiers(int Replenishment)
        {
            SoldierCount = SoldierCount + Replenishment;

            if (SoldierCount > TotalSoldierCount)
            {
                SoldierCount = TotalSoldierCount;
            }
        }

        //'kills' soldiers by the a specified amount, if the soldier count is brought to <= 0, destroy the unit
        public void KillSoldiers(int Damage)
        {
            SoldierCount = SoldierCount - Damage;

            if (SoldierCount <= 0)
            {
                SoldierCount = 0;
                KillUnit();
            }
        }

        //destroys the unit, recording a victory for any unit it was engaged with
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

        //increments victory counter, which will influence how experienced a unit is (and commanders stats?)
        public void RecordVictory()
        {
            BattlesWon = BattlesWon + 1;
        }

        //increments battles lost counter, which will negatively influence a units experience (and commanders stats?)
        public void RecordLoss()
        {
            BattlesLost = BattlesLost + 1;
        }

        //returns the relevant Charge bonus for the ChargingUnit relative to the DefendingUnit
        public double GetChargeBonus(Unit ChargingUnit, Unit DefendingUnit)
        {
            double result;

            result = ChargingUnit.Weapon.GetChargingDamage();

            if (ChargingUnit.IsMounted)
            {
                result = result + 1;
            }

            //charges are less effective against spears than normal attacks
            if (DefendingUnit.Weapon.DoesStopCharge())
            {
                result = .9;
            }
            ChargingUnit.ChargeBonus = false;
            return result;
        }

        //increases morale of unit by a set amount
        public void RaiseMorale(int MoraleAmountToRaise)
        {
            TotalMorale = TotalMorale + MoraleAmountToRaise;
        }

        //provides a small morale boost to a units morale (usually as a result of positive battle happenings)
        public void MoraleBonus()
        {
            CurrentMorale = CurrentMorale + 1;

            if (CurrentMorale > TotalMorale)
            {
                CurrentMorale = TotalMorale;
            }
        }

        //provides a small decrement to a units morale (usually as a result of negative battle happenings)
        public void MoraleShock()
        {
            CurrentMorale = CurrentMorale - 1;

            if (CurrentMorale < 0)
            {
                CurrentMorale = 0;
            }
            //if the units morale is 10% of it's starting value, start routing (drop weapons, lose battles, etc.)
            if (((double)CurrentMorale / TotalMorale) < .1)
            {
                Route();
            }
            
        }

        //Cause this unit to route
        public void Route()
        {
            Weapon = new Weapon();
            Armor = new Armor(); 
            
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

        //adds to this units opponents list
        public void AddOpponent(Unit Opponent)
        {
            CurrentOpponents.Add(Opponent);
            Opponent.CurrentOpponents.Add(this);
        }

        //calculate morale shocks based on 2 fighting units numbers and equipment
        public void CalculateMoraleShocks(Unit UnitA, Unit UnitB)
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

            if (UnitB.Armor > UnitA.Armor)
            {
                UnitA.MoraleShock();
            }

            if(UnitA.Armor > UnitB.Armor)
            {
                UnitB.MoraleShock();
            }
        }

        //gets the modifier to apply to your damage (1 means no affect, .8 means 80% effective, .9 means 90% effective)
        public double GetMoraleEffect()
        {
            double moraleaffect = 1;

            if (((double)CurrentMorale / TotalMorale) < .25)
            {
                moraleaffect = .8;
            }
            else if (((double)CurrentMorale / TotalMorale) < .5)
            {
                moraleaffect = .9;
            }

            return moraleaffect;
        }

        //calculates the morale shocks to be administered to the losing side of one 'round' of combat
        //decided by who did more damage as a percent of total soldiers.
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

                //Battle damage is calculated using 2 numbers myKillsInflicted and damage
                //myKillsInflicted: this is the actual number of enemy soldiers you will kill.  
                //        -This number is affected by luck (random number), weapon type, being mounted and charge bonus.
                //damage: this number is a modifier that gets applied to your myKillsInflicted in the form 
                //of a percentage represented by a floating point number.
                //        -This number is affected by 
                //            -what metal your weapon is made of vs what metal your opponents armor is made of
                //            -your current morale relative to your max morale
                randDamage = getRandomBaseDamage();
                double enemyKillsInflicted = (double)uOpponent.SoldierCount * randDamage;
                randDamage = getRandomBaseDamage();
                double myKillsInflicted = (double)SoldierCount * randDamage;
                
                CalculateMoraleShocks(this, uOpponent);

                if ((this.IsRouting ) || (uOpponent.IsRouting))
                {
                    return;
                }

                if (ChargeBonus)
                {
                    myKillsInflicted = myKillsInflicted * GetChargeBonus(this, uOpponent);
                }
                if (uOpponent.ChargeBonus)
                {
                    enemyKillsInflicted = enemyKillsInflicted * uOpponent.GetChargeBonus(uOpponent, this);
                }

                //The type of weapon you wield determines how effective you are in combat
                //Swords have no +/- effect, while axes are do 10% less damage, but have a better charge bonus.
                //Spears perform the worst in melee combat, unless against mounted opponents
                //no weapon means you are half as effective.
                myKillsInflicted = myKillsInflicted * Weapon.GetKillsModifier(uOpponent.IsMounted);
                enemyKillsInflicted = enemyKillsInflicted * uOpponent.Weapon.GetKillsModifier(IsMounted);

                resist = Armor.GetResist();
                enemyresist = uOpponent.Armor.GetResist();

                damage = Weapon.GetDamage();
                enemydamage = uOpponent.Weapon.GetDamage();
                
                //your max damage is affected by how good enemy armor is, same vs same metal type
                //results in a damage modifier of 1, which means no damage change. (Number * 1 = Number)
                //If you have a step above (EX Steel weapon vs Iron armor) you get a 10% boost per step up.
                damage = damage - enemyresist;
                enemydamage = enemydamage - resist;

                //If you have less than 25% of your total morale, then your damage is only 80% effective (.8)
                //which means 20% less damage
                //If you have less than 50% of your total morale, then your damage is only 90% effective (.9)
                //which means 10% less damage
                //same calculation for opponents
                moraleaffect = GetMoraleEffect();
                enemymoraleaffect = uOpponent.GetMoraleEffect();
                
                //your current morale affects your max damage
                damage = moraleaffect * damage;
                enemydamage = enemymoraleaffect * enemydamage;

                //myKillsInflicted represents how many soldiers you can kill
                //damage represents how much to increase or decrease this base amount based on morale and equipment differences.
                //EX. your myKillsInflicted is 20 soldiers killed per round, you are charging so you get a 20% boost (20 * 1.2)
                //so your myKillsInflicted becomes 24 soldiers killed in this round.
                //your total number of soldiers killed is spread evenly between all you're current opponents.
                myKillsInflicted = (myKillsInflicted * damage)/ CurrentOpponents.Count;
                enemyKillsInflicted = (enemyKillsInflicted * enemydamage)/ uOpponent.CurrentOpponents.Count;

                CalculatePostBattleMorale(myKillsInflicted, enemyKillsInflicted, this, uOpponent);

                uOpponent.KillSoldiers((int)myKillsInflicted);
                KillSoldiers((int)enemyKillsInflicted);
            }

        }


    }
}
