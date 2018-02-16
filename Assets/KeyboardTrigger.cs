using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardTrigger : MonoBehaviour {

    public KeyCode trigger;
    public DadCell cell;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(trigger))
        {
            DadCell cell2 = this.GetComponentInParent<DadCell>();
            ClickItem item = cell2.GetComponentInChildren<ClickItem>();
            if (item != null)
            {
                if (item.type == ClickItem.Type.consumable)
                {
                    DadItem.eat(cell, item);
                }
                if (item.type == ClickItem.Type.weapon)
                {
                    if (item.value == 1)
                        DadItem.attack(cell, item);
                    if (item.value == 2)
                        DadItem.burn(cell, item);
                }
            }
        }
    }
}
