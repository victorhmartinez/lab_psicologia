using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[SerializeField]
public class Usuario : MonoBehaviour
{
    public string Name;
    public string Email;
    public string Age;
    public string City;

    public Usuario()
    {
        Name = RegistrarUsuario.User_Name;
        Email = RegistrarUsuario.User_Email;
        Age = RegistrarUsuario.User_Age;
        City = RegistrarUsuario.User_City;
    }
}
