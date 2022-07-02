// GENERATED AUTOMATICALLY FROM 'Assets/InputSystem/PCInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PCInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PCInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PCInputActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""76dfde78-7d99-4623-8410-b91ac5f9c5f6"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""34a32888-d155-495b-8399-7d7c6cda7eee"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""8e31f0dc-ed29-48f9-84ca-2fefce0a3e60"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""5ba20108-e010-4195-8db7-c6d0f7579770"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TouchStart"",
                    ""type"": ""Button"",
                    ""id"": ""057b188d-227a-4f00-bb59-971548f1423a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TouchCanceled"",
                    ""type"": ""PassThrough"",
                    ""id"": ""437b0d8a-6c58-4872-a89f-de46882e6cc8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TouchPos"",
                    ""type"": ""PassThrough"",
                    ""id"": ""81b41110-5c95-4370-8696-ec00e3b7a033"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TouchDelta"",
                    ""type"": ""PassThrough"",
                    ""id"": ""748ef298-fd65-4fd6-9503-7673401e834e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""FireOrChargeWeapon"",
                    ""type"": ""PassThrough"",
                    ""id"": ""686a0fc1-4ffa-446d-854b-9960c04e6780"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""InventoryActivation"",
                    ""type"": ""Button"",
                    ""id"": ""8efca505-6d8e-4f75-bdc1-e54ae1204b66"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EquipmentQuickCast_1"",
                    ""type"": ""Button"",
                    ""id"": ""319b693e-2ead-48dc-818d-0883d495a1bb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EquipmentQuickCast_2"",
                    ""type"": ""Button"",
                    ""id"": ""cef70e07-be39-473d-9ebb-9a883d2c284e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EquipmentQuickCast_3"",
                    ""type"": ""Button"",
                    ""id"": ""fbe0e9ee-2986-44a7-8411-5deae60a8d31"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EquipmentQuickCast_4"",
                    ""type"": ""Button"",
                    ""id"": ""84b16dca-fc40-47c1-8301-62722adb8084"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EquipmentQuickCast_5"",
                    ""type"": ""Button"",
                    ""id"": ""b4581ca5-1aaa-49d6-88be-c60cbb1de48c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EquipmentQuickCast_6"",
                    ""type"": ""Button"",
                    ""id"": ""8d558eec-763d-4e33-a8b6-0756dd2768f1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Respawn"",
                    ""type"": ""Button"",
                    ""id"": ""7730b492-fa32-48e3-8123-9b1b8ae5d07a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""KeyBoard"",
                    ""id"": ""414138fb-5004-4b70-a234-e8d03b59bff8"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6e4e351d-45e9-4d8d-965d-294dbcaae871"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ee8ad947-67cb-4fed-ac5b-617cc1dfc7fe"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""711b1897-187a-4e46-aba9-04d5ad890a9a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""dbe9ef24-2c61-4165-ba82-ba86e830d1b7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Mobile"",
                    ""id"": ""8c261a07-a487-431b-9b4f-d5cb50b88b85"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""29e7742c-ed1a-4028-86c2-b5afd7721001"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""dd9a8bc2-eb0b-41fb-84fe-8181b610d154"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4067e726-51c8-4312-8a09-0340f935c5ca"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""85848212-004c-436b-b00e-6123b5a65354"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""fa77c7c9-037e-4f9f-ba24-2be958a7cc6f"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""065d7213-13e3-48e2-820c-583136995e73"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6db0bfad-cf04-4bce-b6a8-679edd33f962"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InventoryActivation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8657b0a2-8908-4022-8bfd-d828680900a6"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InventoryActivation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a6373f95-c436-4da4-9015-6a3e3007a46a"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EquipmentQuickCast_1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""572fc477-183a-4de6-8bea-d49600a9f65d"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EquipmentQuickCast_2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f2caf223-daab-404f-abaa-73dd517476f3"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EquipmentQuickCast_3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1669cad6-c0b8-4035-b349-e8f896c0417a"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EquipmentQuickCast_4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""191fed95-b06d-436f-8d78-e6391d1c70bd"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EquipmentQuickCast_5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""54039280-973e-48bd-a1a7-bb92d88dd26b"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EquipmentQuickCast_6"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e84b5889-c9fe-4f75-81a1-98aed2222310"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FireOrChargeWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""486eda76-85a1-4b16-b500-a6b332165098"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FireOrChargeWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Mobile"",
                    ""id"": ""a9b917f6-ed86-46bb-be22-4b4d7b73b74b"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""741e6099-1ee8-43f8-959e-aaaaaee38712"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""dba7bee7-2ac9-4c1f-9a63-f96f693cf552"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d7dc93c8-c4ec-4cfc-b1b5-a203a23d9989"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""aa029681-bf3f-41e0-92ed-406ba79a9047"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f072a435-3c35-4fdd-a7b5-e9038f8a7ed1"",
                    ""path"": ""<Touchscreen>/position"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchPos"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4cb4c30c-7dc1-46ab-a5c8-ae67dfc00193"",
                    ""path"": ""<Touchscreen>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""891c24fd-2dcd-47d3-9b89-c43cb4948cf0"",
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0dacf2a8-db17-4141-8eeb-4112db81426e"",
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchCanceled"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""29f836ec-b43c-4504-a8d7-c8fc6bf3b654"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Respawn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        m_Player_Aim = m_Player.FindAction("Aim", throwIfNotFound: true);
        m_Player_TouchStart = m_Player.FindAction("TouchStart", throwIfNotFound: true);
        m_Player_TouchCanceled = m_Player.FindAction("TouchCanceled", throwIfNotFound: true);
        m_Player_TouchPos = m_Player.FindAction("TouchPos", throwIfNotFound: true);
        m_Player_TouchDelta = m_Player.FindAction("TouchDelta", throwIfNotFound: true);
        m_Player_FireOrChargeWeapon = m_Player.FindAction("FireOrChargeWeapon", throwIfNotFound: true);
        m_Player_InventoryActivation = m_Player.FindAction("InventoryActivation", throwIfNotFound: true);
        m_Player_EquipmentQuickCast_1 = m_Player.FindAction("EquipmentQuickCast_1", throwIfNotFound: true);
        m_Player_EquipmentQuickCast_2 = m_Player.FindAction("EquipmentQuickCast_2", throwIfNotFound: true);
        m_Player_EquipmentQuickCast_3 = m_Player.FindAction("EquipmentQuickCast_3", throwIfNotFound: true);
        m_Player_EquipmentQuickCast_4 = m_Player.FindAction("EquipmentQuickCast_4", throwIfNotFound: true);
        m_Player_EquipmentQuickCast_5 = m_Player.FindAction("EquipmentQuickCast_5", throwIfNotFound: true);
        m_Player_EquipmentQuickCast_6 = m_Player.FindAction("EquipmentQuickCast_6", throwIfNotFound: true);
        m_Player_Respawn = m_Player.FindAction("Respawn", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Interact;
    private readonly InputAction m_Player_Aim;
    private readonly InputAction m_Player_TouchStart;
    private readonly InputAction m_Player_TouchCanceled;
    private readonly InputAction m_Player_TouchPos;
    private readonly InputAction m_Player_TouchDelta;
    private readonly InputAction m_Player_FireOrChargeWeapon;
    private readonly InputAction m_Player_InventoryActivation;
    private readonly InputAction m_Player_EquipmentQuickCast_1;
    private readonly InputAction m_Player_EquipmentQuickCast_2;
    private readonly InputAction m_Player_EquipmentQuickCast_3;
    private readonly InputAction m_Player_EquipmentQuickCast_4;
    private readonly InputAction m_Player_EquipmentQuickCast_5;
    private readonly InputAction m_Player_EquipmentQuickCast_6;
    private readonly InputAction m_Player_Respawn;
    public struct PlayerActions
    {
        private @PCInputActions m_Wrapper;
        public PlayerActions(@PCInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputAction @Aim => m_Wrapper.m_Player_Aim;
        public InputAction @TouchStart => m_Wrapper.m_Player_TouchStart;
        public InputAction @TouchCanceled => m_Wrapper.m_Player_TouchCanceled;
        public InputAction @TouchPos => m_Wrapper.m_Player_TouchPos;
        public InputAction @TouchDelta => m_Wrapper.m_Player_TouchDelta;
        public InputAction @FireOrChargeWeapon => m_Wrapper.m_Player_FireOrChargeWeapon;
        public InputAction @InventoryActivation => m_Wrapper.m_Player_InventoryActivation;
        public InputAction @EquipmentQuickCast_1 => m_Wrapper.m_Player_EquipmentQuickCast_1;
        public InputAction @EquipmentQuickCast_2 => m_Wrapper.m_Player_EquipmentQuickCast_2;
        public InputAction @EquipmentQuickCast_3 => m_Wrapper.m_Player_EquipmentQuickCast_3;
        public InputAction @EquipmentQuickCast_4 => m_Wrapper.m_Player_EquipmentQuickCast_4;
        public InputAction @EquipmentQuickCast_5 => m_Wrapper.m_Player_EquipmentQuickCast_5;
        public InputAction @EquipmentQuickCast_6 => m_Wrapper.m_Player_EquipmentQuickCast_6;
        public InputAction @Respawn => m_Wrapper.m_Player_Respawn;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Interact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Aim.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @TouchStart.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouchStart;
                @TouchStart.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouchStart;
                @TouchStart.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouchStart;
                @TouchCanceled.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouchCanceled;
                @TouchCanceled.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouchCanceled;
                @TouchCanceled.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouchCanceled;
                @TouchPos.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouchPos;
                @TouchPos.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouchPos;
                @TouchPos.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouchPos;
                @TouchDelta.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouchDelta;
                @TouchDelta.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouchDelta;
                @TouchDelta.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTouchDelta;
                @FireOrChargeWeapon.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFireOrChargeWeapon;
                @FireOrChargeWeapon.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFireOrChargeWeapon;
                @FireOrChargeWeapon.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFireOrChargeWeapon;
                @InventoryActivation.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInventoryActivation;
                @InventoryActivation.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInventoryActivation;
                @InventoryActivation.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInventoryActivation;
                @EquipmentQuickCast_1.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipmentQuickCast_1;
                @EquipmentQuickCast_1.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipmentQuickCast_1;
                @EquipmentQuickCast_1.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipmentQuickCast_1;
                @EquipmentQuickCast_2.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipmentQuickCast_2;
                @EquipmentQuickCast_2.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipmentQuickCast_2;
                @EquipmentQuickCast_2.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipmentQuickCast_2;
                @EquipmentQuickCast_3.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipmentQuickCast_3;
                @EquipmentQuickCast_3.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipmentQuickCast_3;
                @EquipmentQuickCast_3.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipmentQuickCast_3;
                @EquipmentQuickCast_4.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipmentQuickCast_4;
                @EquipmentQuickCast_4.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipmentQuickCast_4;
                @EquipmentQuickCast_4.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipmentQuickCast_4;
                @EquipmentQuickCast_5.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipmentQuickCast_5;
                @EquipmentQuickCast_5.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipmentQuickCast_5;
                @EquipmentQuickCast_5.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipmentQuickCast_5;
                @EquipmentQuickCast_6.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipmentQuickCast_6;
                @EquipmentQuickCast_6.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipmentQuickCast_6;
                @EquipmentQuickCast_6.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEquipmentQuickCast_6;
                @Respawn.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRespawn;
                @Respawn.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRespawn;
                @Respawn.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRespawn;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @TouchStart.started += instance.OnTouchStart;
                @TouchStart.performed += instance.OnTouchStart;
                @TouchStart.canceled += instance.OnTouchStart;
                @TouchCanceled.started += instance.OnTouchCanceled;
                @TouchCanceled.performed += instance.OnTouchCanceled;
                @TouchCanceled.canceled += instance.OnTouchCanceled;
                @TouchPos.started += instance.OnTouchPos;
                @TouchPos.performed += instance.OnTouchPos;
                @TouchPos.canceled += instance.OnTouchPos;
                @TouchDelta.started += instance.OnTouchDelta;
                @TouchDelta.performed += instance.OnTouchDelta;
                @TouchDelta.canceled += instance.OnTouchDelta;
                @FireOrChargeWeapon.started += instance.OnFireOrChargeWeapon;
                @FireOrChargeWeapon.performed += instance.OnFireOrChargeWeapon;
                @FireOrChargeWeapon.canceled += instance.OnFireOrChargeWeapon;
                @InventoryActivation.started += instance.OnInventoryActivation;
                @InventoryActivation.performed += instance.OnInventoryActivation;
                @InventoryActivation.canceled += instance.OnInventoryActivation;
                @EquipmentQuickCast_1.started += instance.OnEquipmentQuickCast_1;
                @EquipmentQuickCast_1.performed += instance.OnEquipmentQuickCast_1;
                @EquipmentQuickCast_1.canceled += instance.OnEquipmentQuickCast_1;
                @EquipmentQuickCast_2.started += instance.OnEquipmentQuickCast_2;
                @EquipmentQuickCast_2.performed += instance.OnEquipmentQuickCast_2;
                @EquipmentQuickCast_2.canceled += instance.OnEquipmentQuickCast_2;
                @EquipmentQuickCast_3.started += instance.OnEquipmentQuickCast_3;
                @EquipmentQuickCast_3.performed += instance.OnEquipmentQuickCast_3;
                @EquipmentQuickCast_3.canceled += instance.OnEquipmentQuickCast_3;
                @EquipmentQuickCast_4.started += instance.OnEquipmentQuickCast_4;
                @EquipmentQuickCast_4.performed += instance.OnEquipmentQuickCast_4;
                @EquipmentQuickCast_4.canceled += instance.OnEquipmentQuickCast_4;
                @EquipmentQuickCast_5.started += instance.OnEquipmentQuickCast_5;
                @EquipmentQuickCast_5.performed += instance.OnEquipmentQuickCast_5;
                @EquipmentQuickCast_5.canceled += instance.OnEquipmentQuickCast_5;
                @EquipmentQuickCast_6.started += instance.OnEquipmentQuickCast_6;
                @EquipmentQuickCast_6.performed += instance.OnEquipmentQuickCast_6;
                @EquipmentQuickCast_6.canceled += instance.OnEquipmentQuickCast_6;
                @Respawn.started += instance.OnRespawn;
                @Respawn.performed += instance.OnRespawn;
                @Respawn.canceled += instance.OnRespawn;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnTouchStart(InputAction.CallbackContext context);
        void OnTouchCanceled(InputAction.CallbackContext context);
        void OnTouchPos(InputAction.CallbackContext context);
        void OnTouchDelta(InputAction.CallbackContext context);
        void OnFireOrChargeWeapon(InputAction.CallbackContext context);
        void OnInventoryActivation(InputAction.CallbackContext context);
        void OnEquipmentQuickCast_1(InputAction.CallbackContext context);
        void OnEquipmentQuickCast_2(InputAction.CallbackContext context);
        void OnEquipmentQuickCast_3(InputAction.CallbackContext context);
        void OnEquipmentQuickCast_4(InputAction.CallbackContext context);
        void OnEquipmentQuickCast_5(InputAction.CallbackContext context);
        void OnEquipmentQuickCast_6(InputAction.CallbackContext context);
        void OnRespawn(InputAction.CallbackContext context);
    }
}
