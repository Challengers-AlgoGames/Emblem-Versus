using UnityEngine;

namespace Weapons {
	public class Weapon: MonoBehaviour
	{
		[SerializeField] private string waponName;
		[SerializeField] private int maxRange; // portée
		[SerializeField] private int minRange;
		[SerializeField] private int acuracy; // weapon atk
		[SerializeField] private WeaponCategory category;
		[SerializeField] private WeaponType type;
		[SerializeField] private WeaponAttribute attribute;

		public int MaxRange { get => maxRange; }
		public int MinRange { get => minRange; }
		public int Acuracy { get => acuracy; }
		public WeaponAttribute Attribute { get => attribute; }
		public WeaponCategory Category { get => category; }
		
	}
}
