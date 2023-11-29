using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kaicita
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField]
        private float m_cameraSpeed;
        [SerializeField]
        private GameObject m_pauseMenu;

        private Vector2 m_cameraVel = Vector2.zero;
        private GameObject m_selected = null;
        private StateMachine m_panSM = null;
        private Panning s_panning;
        private NotPanning s_not_panning;

        private void Awake()
        {
            s_panning = new Panning(gameObject);
            s_not_panning = new NotPanning(gameObject);
            m_panSM = new StateMachine(s_not_panning);

            Time.timeScale = 1.0f;
        }

        private void Update()
        {
            Camera.main.transform.Translate
                (m_cameraVel * m_cameraSpeed * Time.deltaTime);
            m_panSM.Update();
        }

        public void OnPan(InputAction.CallbackContext ctx)
        {
           
            if (ctx.ReadValueAsButton())
            {
                if (m_panSM.GetState() != s_panning)
                    m_panSM.SetState(s_panning);
            }
            else
            {
                if (m_panSM.GetState() != s_not_panning)
                    m_panSM.SetState(s_not_panning);
            }
        }

        public void OnLook(InputAction.CallbackContext ctx)
        {
            m_cameraVel =
                ctx.ReadValue<Vector2>() *
                m_cameraSpeed;
        }

        public void OnClick(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed)
                return;

            if (m_selected == null)
            {
                var maybe = Physics2D.OverlapPoint(
                    Camera.main.ScreenToWorldPoint(Input.mousePosition)
                    );

                if (maybe == null)
                    return;
                if (!maybe.CompareTag("Player"))
                    return;

                m_selected = maybe.gameObject;
            }
            else
            {
                if (m_selected == null)
                    return;

                Transform dest = m_selected.transform.parent.GetChild(0);
                dest.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                m_selected = null;
            }
        }

        public void OnPause(InputAction.CallbackContext ctx)
        {
            if (ctx.phase == InputActionPhase.Performed)
            {
                if (m_pauseMenu.activeSelf)
                    Time.timeScale = 1f;
                else
                    Time.timeScale = 0f;

                m_pauseMenu.SetActive(!m_pauseMenu.activeSelf);
            }
        }
    }
}