using UnityEngine;


namespace Weapons
{
	public abstract class Weapon: MonoBehaviour
	{
		[SerializeField] protected string title;
		[SerializeField] protected int range;
		[SerializeField] protected int maxCharges;
		[SerializeField] protected Attribute effect;
		[SerializeField] protected SpecialAttribute specialEffect;

		protected int leftCharges;
		public int LeftCharges { get => leftCharges; }

		public abstract void ChargeWeapon();
	}
}
