using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ComponentInitializer : MonoBehaviour
{

    public PlayerManager playerManager;
    public AnimatorManager animatorManager;
    public Animator animator;

    public InputManager inputManager;
    public CameraManager cam;
    public JumpComponent playerJump;

    public Transform cameraObject;
    public Rigidbody rb;

    private void Awake()
    {
        ComponentReflection();
        AutoAssignComponents();
    }

    private void Init()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        cam = FindObjectOfType<CameraManager>();
        rb = GetComponent<Rigidbody>();
        playerJump = GetComponent<JumpComponent>();
        cameraObject = Camera.main.transform;
        animator = GetComponent<Animator>();
    }
    public delegate void ComponentAssigner();
    public event ComponentAssigner componentReflection;
    public void ComponentReflection()
    {
        Init();
        componentReflection?.Invoke();
    }

    private void AutoAssignComponents()
    {
        BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

        foreach (FieldInfo field in GetType().GetFields(flags))
        {
            if (typeof(Component).IsAssignableFrom(field.FieldType))
            {
                Component component = GetComponent(field.FieldType);
                if (component != null)
                {
                    field.SetValue(this, component);
                }
                else
                {
                    Debug.LogError($"Component of type {field.FieldType.Name} not found on {gameObject.name}");
                }
            }
        }
    }
}
