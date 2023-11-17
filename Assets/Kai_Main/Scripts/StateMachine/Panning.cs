using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kaicita
{
    public class Panning : State
    {
        Vector2 worldStart = Vector2.zero;

        public Panning(GameObject obj) : base(obj) { }

        public override void OnEnter()
        {
            worldStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        public override void OnUpdate()
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var diff = worldStart - mouseWorldPos;
            Camera.main.transform.Translate(diff);
        }

        public override void OnExit() { }
    }
}