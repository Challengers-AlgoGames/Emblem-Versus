using UnityEngine;

namespace Weapons {
	public class Weapon: MonoBehaviour
	{
		[SerializeField] private string waponName;
		[SerializeField] private int range; // portée
		[SerializeField] private int acuracy; // weapon atk
		public int Acuracy { get => acuracy; }

		[SerializeField] private WeaponCategory category;
		public WeaponCategory Category { get => category; }

		[SerializeField] private WeaponType type;
		[SerializeField] private WeaponAttribute attribute;
		
	}
}
