using UnityEngine;
namespace Weapons{
	public class SpiritualWeapon : Weapon
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
		public virtual void UseWeapon(){
			if(Charges) {
				Charges=false ;
				return ;
			}
			Debug.Log("Operation Failed") ;
		}
	}
}
