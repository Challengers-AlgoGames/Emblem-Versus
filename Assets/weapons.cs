using System.Collections.Generic;
using UnityEngine;
namespace Weapons{
	public class Weapon: MonoBehaviour{
	    	public string Name, Effect, SpecialEffect ;
	    	public int CellRange, ChargeTime ;
	    	public List<string> UnitUsers ;
	    	
	    	public virtual void ApplySpecialEffect(string effect){
	    		SpecialEffect = effect ;
	    		Debug.Log("Special Effect added") ;
	    	}
	}
}
