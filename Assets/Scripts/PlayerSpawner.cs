using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject pValkyrie;
    [SerializeField] GameObject pWarrior;
    [SerializeField] GameObject pElf;
    [SerializeField] GameObject pWizard;
    Vector2 v = Vector2.zero;
    PlayerInput playerInput;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        v = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (!v.Equals(Vector2.zero))
        {
            // determine which hero the new player will be
            GameObject p;

            if      (v.Equals(Vector2.up))   p = pValkyrie;
            else if (v.Equals(Vector2.down)) p = pWarrior;
            else if (v.Equals(Vector2.left)) p = pElf;
            else                             p = pWizard;

            //playerInput.user.UnpairDevicesAndRemoveUser();
            InputSystem.FlushDisconnectedDevices(); // TODO start here

            // spawn the hero at the spawner's position
            Instantiate(p, transform.position, transform.rotation);

            // remove spawner
            Destroy(gameObject);
        }
    }
}
