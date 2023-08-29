using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Unity.Netcode;

public class PlayerMoovoment : NetworkBehaviour
{
    private FixedJoystick _joystick;

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(new MyCustomData
    {
        _int = 56, 
         _bool = true,
    }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
        }
    }
    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previosValue, MyCustomData newValue) => {
            Debug.Log(OwnerClientId + "; " + newValue._int + ";" + newValue._bool);

        };
    }

    private void Start()
    {
        // Найти джойстик в сцене
        _joystick = FindObjectOfType<FixedJoystick>();
    }
    private void Update()
    {
       // Debug.Log(OwnerClientId + "; randomNumber:" + randomNumber.Value);

        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.T)) {
            TestClientRpc();
        }
        /*
        {
            randomNumber.Value = new MyCustomData
            {
                _int = 10, 
                _bool = false,
            };
        }
        */

        Vector3 moveDir = new Vector3(0, 0, 0);

        // Использовать значения джойстика для движения
        moveDir.x = _joystick.Horizontal;
        moveDir.z = _joystick.Vertical;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    [ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcParams)
    {
        Debug.Log("TestServerRpc " + OwnerClientId + "; " + serverRpcParams.Receive.SenderClientId);
    }
    [ClientRpc]
    private void TestClientRpc()
    {
        Debug.Log("TestClientRpc");
    }
}
