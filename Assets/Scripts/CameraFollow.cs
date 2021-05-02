using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraFollow : MonoBehaviour
    {

        public Transform target;

        public float damping = 0.125f;

        private void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, target.position, damping * Time.deltaTime);
        }

    }
}