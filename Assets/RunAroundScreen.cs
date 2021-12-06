
// =================================	
// Namespaces.
// =================================

using UnityEngine;

// =================================	
// Classes.
// =================================

public class RunAroundScreen : MonoBehaviour
{
    // =================================	
    // Nested classes and structures.
    // =================================

    // ...

    // =================================	
    // Variables.
    // =================================

    // ...

    public float speed = 8.0f;
    public float distanceFromCamera = 5.0f;

    public bool ignoreTimeScale;

    // =================================	
    // Functions.
    // =================================

    // ...

    void Awake()
    {

    }

    // ...

    void Start()
    {

    }

    public int CurrentPosition = 0;
    public int MaxPosition = 20; // equivalent to 0
    public int Direction = 1;
    public bool Latched = true;

    public void BumpPosition()
    {
        CurrentPosition = (CurrentPosition + Direction + MaxPosition) % MaxPosition;
        Latched = false;
    }

    public void ReverseDirection()
    {
        Direction = -Direction;
    }

    void Update()
    {
        int perimeter = Screen.width * 2 + Screen.height * 2;
        int interval = perimeter / MaxPosition;
        int currentPerimeterPosition = interval * CurrentPosition;
        int line = currentPerimeterPosition < Screen.width ? 0
            : currentPerimeterPosition < Screen.width + Screen.height ? 1
            : currentPerimeterPosition < Screen.width + Screen.height + Screen.width ? 2
            : 3;
        Vector3 mousePosition = new Vector3(
            line == 0 ? currentPerimeterPosition
            : line == 1 ? Screen.width - 30
            : line == 2 ? Screen.width - (currentPerimeterPosition - Screen.width - Screen.height)
            : 30,
            line == 0 ? Screen.height - 30
            : line == 1 ? Screen.height - (currentPerimeterPosition - Screen.width)
            : line == 2 ? 30
            : currentPerimeterPosition - Screen.width - Screen.height - Screen.width
        );
        //Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = distanceFromCamera;

        Vector3 mouseScreenToWorld = Camera.main.ScreenToWorldPoint(mousePosition);

        float deltaTime = !ignoreTimeScale ? Time.deltaTime : Time.unscaledDeltaTime;

        Vector3 targetPos = Vector3.Lerp(transform.position, mouseScreenToWorld, 1.0f - Mathf.Exp(-speed * deltaTime));
        Vector3 position = !Latched
            ? targetPos
            : Vector3.Lerp(transform.position, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, distanceFromCamera)), 1.0f - Mathf.Exp(-speed * deltaTime * 0.25f));

        transform.position = position;
        if (!Latched && Mathf.Abs((mouseScreenToWorld - targetPos).magnitude) < 0.3f)
        {
            Latched = true;
        }
    }

    // ...

    void LateUpdate()
    {

    }

    // =================================	
    // End functions.
    // =================================

}

// =================================	
// --END-- //
// =================================
