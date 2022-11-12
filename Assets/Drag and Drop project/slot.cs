using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class slot : MonoBehaviour
    , IDropHandler
    , IPointerEnterHandler
    , IPointerExitHandler
    {

    //Para regular el tamaño del Cellsize de Este Slot
    public GridLayoutGroup GLG;

    //El item que acumulara el Slot
    public GameObject Item;

    //El item que tendra guardado en este slot
    public Item Localization;
    //Si esta ocupado o no
    public bool Occupied;

    void Awake() {
        //Se va a cambiar el tamaño de cada slot
        GLG = GetComponent<GridLayoutGroup>();
    }

    //funcion para saber si se puede poner el Item en este slot
    public bool CanDrop(Item I) {
        //Recorre cada Slot el cual el item va a ocupar
        for (int y = 0; Mathf.Abs(y) < I.Height; y--) {
            for (int x = 0; Mathf.Abs(x) < I.Width; x--) {
                //sumarle el offset calculado
                int offsetx = Localization.X + x;
                int offsety = Localization.Y + y;
                //Si esta fuera de los limites pues claramente no se puede
                if (0 > offsetx || 0 > offsety)
                    return false;
                //obtener cada slot y analizar si estan ocupados por otro item
                slot TempSlot = transform.parent.GetComponent<Inventory>().Slots[offsetx, offsety].GetComponent<slot>();
                if (TempSlot.Item != null || TempSlot.Occupied)
                    return false;
            }
        }
        return true;
    }



    //empieza la interfaz del idrophandler
    public void OnDrop(PointerEventData eventData)
    {
        //Obtener el item que estuvo siendo agarrado
        Item B = drag.item_arrastrado.GetComponent<drag>().item;
        Inventory Inv = transform.parent.transform.GetComponent<Inventory>();
        //Reubicar donde se pondra el item
        slot SL = Inv.Slots[Localization.X, Localization.Y].GetComponent<slot>();

        //si se puede Poner el item entonces
        if (CanDrop(B))
        {
            //se pone el CellSize del tamaño del item correspondiente
            SL.GLG.cellSize = new Vector2(B.Width, B.Height) * 70;
            //se le asigna el item que estuvo siendo agarrado a este slot
            SL.Item = drag.item_arrastrado;
            SL.Localization.Name = B.Name;
            B = new Item(Localization.X, Localization.Y, B.Width, B.Height, B.Name);
            drag.item_arrastrado.GetComponent<drag>().item = B;
            //drag es el sript donde tenemos los items
            drag.item_arrastrado.transform.SetParent(SL.transform);
            //Le ponemos de Parent el Slot
            for (int y = 0; Mathf.Abs(y) < B.Height; y--)
            {
                for (int x = 0; Mathf.Abs(x) < B.Width; x--)
                {
                    //offset calculado
                    int offsetx = Localization.X + x;
                    int offsety = Localization.Y + y;
                    //Cada Slot en el que estara el item, estara Ocupado
                    Inv.Slots[offsetx, offsety].GetComponent<slot>().Occupied = true;
                }
            }
        }//Si No
        else {
            //Obtener el slot que estuvo antes
            slot Ant = drag.padre.GetComponent<slot>();
            //Y se devuelven los valores a como antes
            Ant.Item = gameObject;
            Ant.Occupied = true;
            drag.item_arrastrado.transform.SetParent(drag.padre);
        }
        //Cada vez que se suelte que se refrezque la lista de items
        Inventory.RefreshItems(Inv);
    }

    void Update() {
        
    }

    public bool Hover { get; private set; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (drag.item_arrastrado != null && Item == null){
            drag.SlotsHighlighter.SetActive(true);
            Item B = drag.item_arrastrado.GetComponent<drag>().item;
            drag.SlotsHighlighter.transform.SetParent(transform);
            GLG.cellSize = new Vector2(B.Width, B.Height) * 70;
            if (CanDrop(B))
            {
                drag.SlotsHighlighter.GetComponent<Image>().color = new Color(0f, 1f, 0f, 0.23529f);
            }
            else {
                drag.SlotsHighlighter.GetComponent<Image>().color = new Color(1f, 0f, 0f, 0.23529f);
            }
        }
        else if (Item != null && drag.item_arrastrado != null){
            drag.SlotsHighlighter.SetActive(false);
            Item.GetComponent<drag>().Highlighter.color = new Color(1f, 0f, 0f, 0.23529f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
