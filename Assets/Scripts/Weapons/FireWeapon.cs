using UnityEngine;


namespace Weapons
{
	public class FireWeapon : Weapon
	{
		[SerializeField] private int damages;

        public override void ChargeWeapon()
        {
            if(leftCharges < maxCharges)
				leftCharges = maxCharges; // se charge en 1 tour
        }
	}
}
