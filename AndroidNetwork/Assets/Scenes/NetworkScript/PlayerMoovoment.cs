using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Unity.Netcode;

public class PlayerMoovoment : NetworkBehaviour
{
    private FixedJoystick _joystick;

    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (int previosValue, int newValue) => {
            Debug.Log(OwnerClientId + "; randomNumber:" + randomNumber.Value);

        };
    }

    private void Start()
    {
        // Найти джойстик в сцене
        _joystick = FindObjectOfType<FixedJoystick>();
    }
    private void Update()
    {
        Debug.Log(OwnerClientId + "; randomNumber:" + randomNumber.Value);

        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            randomNumber.Value = Random.Range(0, 10);
        }

        Vector3 moveDir = new Vector3(0, 0, 0);

        // Использовать значения джойстика для движения
        moveDir.x = _joystick.Horizontal;
        moveDir.z = _joystick.Vertical;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

}
