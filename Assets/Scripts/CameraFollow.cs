using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraFollow : MonoBehaviour
    {

        public Transform target;
        public List<GameObject> backgrounds;

        public float damping = 0.125f;
        public float parallax = 0.1f;

        private List<Vector3> startPos = new List<Vector3>();
        private List<float> length = new List<float>();

        private void Start()
        {
            foreach(var b in backgrounds)
            {
                startPos.Add(b.transform.position);
                length.Add(b.GetComponent<SpriteRenderer>().bounds.size.x);
            }
        }

        private void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, target.position, damping * Time.deltaTime);

            for (int i = 0; i < backgrounds.Count; i++)
            {
                float posX = startPos[i].x + transform.position.x * parallax;
                float posY = startPos[i].y + transform.position.y * parallax;

                backgrounds[i].transform.position = new Vector3(posX, posY, transform.position.z);

                if (transform.position.x > posX + length[i])
                {
                    var pos = startPos[i];
                    pos.x += length[i];
                    startPos[i] = pos;
                }
                else if (transform.position.x < posX - length[i])
                {
                    var pos = startPos[i];
                    pos.x -= length[i];
                    startPos[i] = pos;
                }
            }
        }

    }
}