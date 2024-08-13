using UnityEngine;
namespace Weapons{
	public class MeleeWeapon : Weapon
	{
		public bool Charges=true ;
		public virtual void ChargeWeapon(){
			if(!Charges) {
				Charges=true ;
				Debug.Log("Weapon charged successfuly") ;
				return ;
			}
			Debug.Log("Weapon can't be charged") ;
		}
		public virtual void Fire(){
			if(Charges) {
				Charges=false ;
				return ;
			}
			Debug.Log("Operation Failed") ;
		}
	}
}
