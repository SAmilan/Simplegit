using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GamepadManager : MonoBehaviour
{
    public static GamepadManager Instance;
    public string[] GamepadNames;
    public Gamepad m_Gamepad1;
    public Gamepad m_Gamepad2;
    private void Awake()
    {
        Instance = this;
       
    }
    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnInputDeviceChanges;
        var gamepads = Gamepad.all;
        if(gamepads.Count >= 2)
        {
            m_Gamepad1 = gamepads[0];
            m_Gamepad2 = gamepads[1];
        }
        if(gamepads.Count == 1)
        {
            m_Gamepad1 = gamepads[0];
            m_Gamepad2 = null;
        }

    }
    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnInputDeviceChanges;
    }

    private void OnInputDeviceChanges(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                var gamepads = Gamepad.all;
                if(gamepads.Count >= 2)
                {
                    m_Gamepad1 = gamepads[0];
                    m_Gamepad2 = gamepads[1];
                }
                if(gamepads.Count == 1)
                {
                    m_Gamepad1 = gamepads[0];
                    m_Gamepad2 = null;
                }
                Debug.Log("Input Device Added");
                // New Device.
                break;
            case InputDeviceChange.Disconnected:
                Debug.Log("Input Device Disconnected");

                // Device got unplugged.
                break;
            case InputDeviceChange.Reconnected:
                Debug.Log("Input Device Reconnected");

                // Plugged back in.
                break;
            case InputDeviceChange.Removed:
                var gamepadss = Gamepad.all;

                if (gamepadss.Count == 1)
                {
                    m_Gamepad1 = gamepadss[0];
                    m_Gamepad2 = null;
                }
                Debug.Log("Input Device Removed");

                // Remove from Input System entirely; by default, Devices stay in the system once discovered.
                break;
            default:
                // See InputDeviceChange reference for other event types.
                break;
        }
    }

    public void StopVibration()
    {
        if(m_Gamepad1 != null)
        {
            m_Gamepad1.SetMotorSpeeds(0f, 0f);
        }
        if(m_Gamepad2 != null)
        {
            m_Gamepad2.SetMotorSpeeds(0f, 0f);
        }
    }


}
