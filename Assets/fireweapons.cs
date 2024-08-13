using UnityEngine;
namespace Weapons{
	public class FireWeapon : Weapon
	{
		public int Capacity, BulletNumb, Charges ;
		public virtual void ChargeWeapon(){
			if(	Charges > 0 ) {
				BulletNumb = Capacity ;
				Charges -- ;
				Debug.Log("Weapon charged successfuly") ;
				return ;
			}
			Debug.Log("Weapon can't be charged") ;
		}
		public virtual void Fire(){
			if(	BulletNumb > 0 ) {
				BulletNumb -- ;
				return ;
			}
			Debug.Log("Operation Failed") ;
		}
	}
}
