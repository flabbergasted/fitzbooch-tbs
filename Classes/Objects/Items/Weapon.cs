using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnBasedStrategy.Classes.Objects.Items
{
    enum WeaponType { None = 0, Sword, Axe, Spear, Bow };
    class Weapon
    {
        private WeaponType _weaponType;
        private Metal _metal;
        private bool _doesStopCharge;

        #region Constructors
        
        public Weapon()
        {
            _weaponType = WeaponType.None;
            _metal = new Metal();
        }
        public Weapon(WeaponType wt) 
        {
            _weaponType = wt;
            _metal = new Metal();
            AssignChargeStop(wt);
        }
        public Weapon(WeaponType wt, Metal mt)
        {
            _weaponType = wt;
            _metal = mt;
            AssignChargeStop(wt);
        }

        #endregion

        #region Methods
        public double GetChargingDamage()
        {
            double result = 0.0;

            if (_weaponType == WeaponType.Axe)
            {
                result = 1.2;
            }
            else
            {
                result = 1.1;
            }

            return result;
        }

        public bool DoesStopCharge()
        {
            return _doesStopCharge;
        }

        public double GetKillsModifier(bool isOpponentMounted)
        {
            double result = 1;
            switch (_weaponType)
            {
                case WeaponType.Sword:
                    break;
                case WeaponType.Axe:
                    result = .9;
                    break;
                case WeaponType.Spear:
                    if (isOpponentMounted) { result = 1.2; }
                    else { result = .75; }
                    break;
                default:
                    result = .5;
                    break;
            }

            return result;
        }

        public double GetDamage()
        {
            double result = 0;

            result = _metal.GetStrength() + 1;

            return result;
        }
        #endregion

        #region Helpers
        private bool AssignChargeStop(WeaponType wt)
        {
            if (wt == WeaponType.Spear)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
