using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePlay;

namespace Units
{
    public class UnitMovement : MonoBehaviour
    {
        public float moveSpeed = 5f;
        private Queue<Vector3Int> path = new Queue<Vector3Int>();
        [SerializeField] private TileSystem tileSystem;

        public void SetPath(List<Vector3Int> newPath)
        {
            path.Clear();
            foreach (var node in newPath)
            {
                path.Enqueue(node);
            }
        }

        private void Update()
        {
            if (path.Count == 0)
                return;

            Vector3Int targetTile = path.Peek();
            Vector3 targetPosition = tileSystem.ConvertCellToWorldPosition(targetTile);

            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                transform.position = targetPosition;
                path.Dequeue();
            }
        }
    }
}