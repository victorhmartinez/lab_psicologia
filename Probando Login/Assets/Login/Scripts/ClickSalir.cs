using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSalir : MonoBehaviour
{
    RegistrarUsuario ru = new RegistrarUsuario();

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if(hit.transform != null)
                {
                    Rigidbody rb;
                    
                    if (hit.transform.GetComponent<Rigidbody>().name.Equals("Salir"))
                    {
                        ru.Send_User();
                    }
                    
                }
            }
                    
        }
    }


}
