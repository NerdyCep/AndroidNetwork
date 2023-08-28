using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Unity.Netcode;

public class PlayerMoovoment : NetworkBehaviour
{
    private FixedJoystick _joystick;

    private void Start()
    {
        // Найти джойстик в сцене
        _joystick = FindObjectOfType<FixedJoystick>();
    }
    private void Update()
    {
        if (!IsOwner) return;

        Vector3 moveDir = new Vector3(0, 0, 0);

        // Использовать значения джойстика для движения
        moveDir.x = _joystick.Horizontal;
        moveDir.z = _joystick.Vertical;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

}
