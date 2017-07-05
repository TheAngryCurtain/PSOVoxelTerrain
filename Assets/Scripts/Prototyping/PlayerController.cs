using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform cameraTransform;

    private float moveSpeed = 10f;
    private float jumpForce = 5f;

    private Vector3 forward;
    private Vector3 right;
    private Vector3 moveDir = Vector3.zero;

    private RaycastHit hit;
    private Ray mouseRay;
    private float rayDistance = 100f;

    private void Update()
    {
        // block removing
        if (Input.GetMouseButtonDown(0))
        {
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out hit, rayDistance))
            {
                TerrainEditor.SetBlock(hit, new BlockAir());
            }
        }

        // block placing
        else if (Input.GetMouseButtonDown(1))
        {
            mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out hit, rayDistance))
            {
                TerrainEditor.SetBlock(hit, new BlockWood(), true); // test wood!
            }
        }

        // movement
        if (rb != null)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up);
            right = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up);

            Vector3 targetDir = forward * verticalInput + right * horizontalInput;
            moveDir = Vector3.RotateTowards(moveDir, targetDir, 200 * Mathf.Deg2Rad * Time.deltaTime, 1000);

            if (moveDir != Vector3.zero)
            {
                transform.LookAt(transform.position + moveDir.normalized);
                rb.MovePosition(transform.position + moveDir.normalized * Time.deltaTime * moveSpeed);
            }

            // notify player position
            VSEventManager.Instance.TriggerEvent(new GameEvents.PlayerPositionUpdateEvent(transform.position));

            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }
        }
    }
}
