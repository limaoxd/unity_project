using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIWeapon : MonoBehaviour
{
    private Image spriteImage;
    public Weapon_art weapon;

    private void Awake()
    {
        spriteImage = GetComponent<Image>();
    }

    private void Update()
    {
        spriteImage.sprite = weapon.item.item.icon;
    }
}