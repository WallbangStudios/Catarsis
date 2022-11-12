using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//las tres clases te dan la opcion de implementar sus interfaces
public class drag : MonoBehaviour
    , IBeginDragHandler
    , IDragHandler
    , IEndDragHandler
    , IPointerEnterHandler
    , IPointerExitHandler   
    {

    public static GameObject item_arrastrado;
    public static Vector3 posicion_inicial;
    public static Transform padre;

    public static GameObject SlotsHighlighter;

    [HideInInspector]
    public Vector2 Size;
    public Item item;

    [HideInInspector]
    public GameObject Inventario;

    Vector3 offset;

    public Image Highlighter;

    void Start() {
        //Justo cuando se intancien, se actualizaran los datos del slot donde estaran
        Item A = transform.parent.GetComponent<slot>().Localization;
        item = new Item(A.X, A.Y, item.Width, item.Height, transform.name);
        //print(item.Name + " " + transform.name);
        transform.parent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(item.Width,item.Height) * 70;
        slot SLT = transform.parent.GetComponent<slot>();
        Inventario = SLT.transform.parent.gameObject;
        Inv = Inventario.transform.GetComponent<Inventory>();
        for (int y = 0; Mathf.Abs(y) < item.Height; y--)
        {
            for (int x = 0; Mathf.Abs(x) < item.Width; x--)
            {
                //Cada Slot en el que estara el item, estara ocupado
                slot a = Inv.Slots[item.X + x, item.Y + y].GetComponent<slot>();
                a.Occupied = true;
            }
        }
        SLT.Item = gameObject;
        SLT.Localization.Name = item.Name;
        Highlighter.color = new Color(1f, 1f, 1f, 0f);
        transform.parent.transform.parent.transform.GetComponent<Inventory>().Items.Add(item);
        gameObject.GetComponent<RectTransform>().pivot = new Vector2((0.5f / item.Width), ((item.Height - 0.5f) / item.Height));
    }
    Inventory Inv;
    //interfaz de IBeginDragHandler
    public void OnBeginDrag(PointerEventData eventData)
    {

        //el item arrastrado pasa a ser el objeto y agarra de una vez la posicion inicial antes de empezar a moverlo
        item_arrastrado = gameObject;
        posicion_inicial = transform.position;
        
        //El Slot inicial
        padre = transform.parent;

        //Recuperar el Slot anterior y quitamos a este item del slot
        Inventario = padre.transform.parent.gameObject;
        Inv=Inventario.transform.GetComponent<Inventory>();
        //print(item.Width + "x" + item.Height + "- (" + item.X + ", " + item.Y + ")");
        for (int y = 0; Mathf.Abs(y) < item.Height; y--)
        {
            for (int x = 0; Mathf.Abs(x) < item.Width; x--)
            {
                //Cada Slot en el que estara el item, estara desocupado
                slot SLT = Inv.Slots[item.X + x, item.Y + y].GetComponent<slot>();
                SLT.Occupied = false;
                SLT.Item = null;
                //print(SLT.Localization.X + "x" + SLT.Localization.Y);
            }
        }

        for (int i = 0; i < Inv.Items.Count; i++) {
            if (Inv.Items[i].Name != item.Name){
                int X = Inv.Items[i].X;
                int Y = Inv.Items[i].Y;
                Inv.Slots[X, Y].GetComponentInChildren<Image>().raycastTarget = false;
            }
        }

       //Sacamos su parent para que no haya dependencias del Layout
       transform.SetParent(Inventario.transform.parent.transform.parent, false); 

        
        slot SlT = Inv.Slots[item.X, item.Y].GetComponent<slot>();
        GameObject H = (GameObject)Instantiate(Highlighter.gameObject);

        H.transform.SetParent(SlT.gameObject.transform);
        SlT.GetComponent<GridLayoutGroup>().cellSize = new Vector2(item.Width, item.Height) * 70;
        H.GetComponent<RectTransform>().localScale = Vector3.one;
        H.GetComponent<RectTransform>().pivot = new Vector2((0.5f / item.Width), ((item.Height - 0.5f) / item.Height));
        H.GetComponent<Image>().color = new Color(0f, 1f, 0f, 0.23529f);
        SlotsHighlighter = H;


        //tengo entendido que blocksraycast no permite los clicks, por eso se coloca en false cuando se empieza a clickear
        GetComponent<CanvasGroup>().blocksRaycasts = false;


        //print(padre.name + "/" + Inventario.transform.name);
    }


    //interfaz de IDragHandler
    public void OnDrag(PointerEventData eventData)
    {
        //va a la direccion del mouse el objeto en el proceso de drag
        transform.position = Input.mousePosition;
    }


    //interfaz de IEndDragHandler
    public void OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent.name.Contains("Ca")) {
            transform.SetParent(padre);
            Debug.Log("it");
        }

        Destroy(SlotsHighlighter);
        SlotsHighlighter = null;

        for (int i = 0; i < Inv.Items.Count; i++)
        {
            if (Inv.Items[i].Name != item.Name)
            {
                int X = Inv.Items[i].X;
                int Y = Inv.Items[i].Y;
                Inv.Slots[X, Y].GetComponentInChildren<Image>().raycastTarget = true;
            }
        }

        //cuando lo suelta vuelve el objeto a su posicion inicial
        item_arrastrado = null;
        
        //blocksraycast permite arrastrar el objeto en true
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        // SI QUIERE QUE EL OBJETO QUEDE EN EL AIRE, ACTIVAR ESTE IF
        //  if (transform.parent !=padre){

        transform.position = posicion_inicial;

      //  }

        //Cada vez que se suelte que se refrezque la lista de items
        Inventory.RefreshItems(Inventario.transform.GetComponent<Inventory>());

    }

    public void OnPointerEnter(PointerEventData eventData) {
        Highlighter.color = new Color(1f, 1f, 1f, 0.23529f);
    }

    public void OnPointerExit(PointerEventData eventData) {
        Highlighter.color = new Color(1f, 1f, 1f, 0f);
    }
}
