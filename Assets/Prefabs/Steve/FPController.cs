using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPController : MonoBehaviour
{
    public GameObject cam;
    public Animator anim;
    public AudioSource[] footsteps;
    public AudioSource jump;
    public AudioSource land;

    float speed = 0.1f;
    float sensitivity = 2;
    float MinimumX = -90;
    float MaximumX = 90;
    Rigidbody rb;
    CapsuleCollider capsule;
    Quaternion cameraRot;
    Quaternion characterRot;

    bool cursorIsLocked = true;
    bool lockCursor = true;

    float x;
    float z;

    public bool gameEnded = false;
    bool playingWalking = false;
    bool previouslyGrounded = true;

    //[SerializeField] private UiInventory uiInventory;

    //private Inventory inventory;
    //private void Awake()
    //{
    //    inventory = new Inventory();
    //    uiInventory.SetInventory(inventory);
    //}

    // Start is called before the first frame update
    void Start()
    {
        //inventory = new Inventory();
        //uiInventory.SetInventory(inventory);

        rb = this.GetComponent<Rigidbody>();
        capsule = this.GetComponent<CapsuleCollider>();
        cameraRot = cam.transform.localRotation;
        characterRot = this.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(x) > 0 || Mathf.Abs(z) > 0)
        {
            if (!anim.GetBool("walking"))
            {
                anim.SetBool("walking", true);
                InvokeRepeating("PlayFootStepAudio", 0, 0.4f);
            }
        }
        else if (anim.GetBool("walking"))
        {
            anim.SetBool("walking", false);
            CancelInvoke("PlayFootStepAudio");
            playingWalking = false;
        }

        bool grounded = IsGrounded();
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(0, 300, 0);
            jump.Play();
            if (anim.GetBool("walking"))
            {
                CancelInvoke("PlayFootStepAudio");
                playingWalking = false;
            }
        }
        else if (!previouslyGrounded && grounded)
        {
            land.Play();
        }

        previouslyGrounded = grounded;

    }

    void PlayFootStepAudio()
    {
        if (gameEnded == false)
        {
            AudioSource audioSource = new AudioSource();
            int n = Random.Range(1, footsteps.Length);

            audioSource = footsteps[n];
            audioSource.Play();
            footsteps[n] = footsteps[0];
            footsteps[0] = audioSource;
            playingWalking = true;
        }
    }


    void FixedUpdate()
    {
        //Micanje miša lijevo desno. Sensitivity, koliko pokazivač miša brzo respondira prilikom micanja miša
        float yRot = Input.GetAxis("Mouse X") * sensitivity;
        float xRot = Input.GetAxis("Mouse Y") * sensitivity;

        //Središnji fokuser gore dolje
        cameraRot *= Quaternion.Euler(-xRot, 0, 0);
        //Središnji fokuser lijevo desno
        characterRot *= Quaternion.Euler(0, yRot, 0);

        //Zabrani punu rotaciju po x osi središnjeg fokusera
        cameraRot = ClampRotationAroundXAxis(cameraRot);

        this.transform.localRotation = characterRot;
        cam.transform.localRotation = cameraRot;

        //Lijevo Desno na tipkovnici hodanje
        x = Input.GetAxis("Horizontal") * speed;
        //Gore Dolje na tipkovnici hodanje 
        z = Input.GetAxis("Vertical") * speed;

        //Nova pozicija igrača
        transform.position += this.transform.forward * z + cam.transform.right * x;

        //Fokuser miša
        UpdateCursorLock();
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    bool IsGrounded()
    {
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, capsule.radius / 2f, Vector3.down, out hitInfo,
                (capsule.height / 2f) - capsule.radius + 0.1f))
        {
            return true;
        }
        return false;
    }

    void OnCollisionEnter(Collision col)
    {
        if (IsGrounded())
        {
            if (anim.GetBool("walking") && !playingWalking)
            {
                InvokeRepeating("PlayFootStepAudio", 0, 0.4f);
            }
        }
    }

    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        if (lockCursor)
            InternalLockUpdate();
    }

    public void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            cursorIsLocked = false;
        else if (Input.GetMouseButtonUp(0))
            cursorIsLocked = true;

        if (cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

}
