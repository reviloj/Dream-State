using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    public bool canOpen = true;

    public void Open()
    {
        if (canOpen)
        {
            this.GetComponent<Animation>().Play();
            canOpen = false;
            StartCoroutine(constructCrateItems());
        }
    }
    private IEnumerator constructCrateItems()
    {
        DummyInventoryControl ui = GameObject.FindObjectOfType<DummyInventoryControl>();
        ui.chestPos = this.transform.position;
        yield return new WaitForSeconds(1);
        int numItems = Random.Range(1, 65);
        numItems =  9 - (int)Mathf.Sqrt(numItems);
        int[] items = new int[numItems];
        bool scroll = false;
        for (int i = 0; i < numItems; i++)
        {
            int item = Random.Range(1, 35);
            item = 5 - (int)Mathf.Sqrt(item);
            items[i] = (item == 4 && scroll == true) ? 3 : item;
            if (item == 4)
                scroll = true;
        }
        ui.setUpCrate(items);
        GameObject.FindObjectOfType<TimeController>().freeze(false, true);
    }
}