// ----------------------------------------------------
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PampelGames.Shared.Utility
{
    public static class PGMeshUtility
    {
        
        
        /// <summary>
        ///     Combines an array of meshes into one new single mesh.
        /// </summary>
        public static Mesh CombineMeshes(Mesh[] meshes, bool mergeSubmeshes)
        {
            return CombineMeshesInternal(meshes, mergeSubmeshes);
        }

        /// <summary>
        ///     Get all indexes that are within the radius from the pivot point.
        /// </summary>
        /// <param name="indexesInRadius">All indexes within the radius.</param>
        /// <param name="closestIndex">The closest index to the pivot point.</param>
        public static void GetIndexesInRadius(Mesh mesh, Vector3 pivot, float radius, out int[] indexesInRadius, out int closestIndex)
        {
            GetIndexesInRadiusInternal(mesh, pivot, radius, out indexesInRadius, out closestIndex);
        }

        /// <summary>
        ///     Equalizes the normals of vertices that share the same position in the mesh.
        /// </summary>
        public static void EqualizeNormals(Mesh mesh)
        {
            EqualizeNormalsInternal(mesh);
        }
        
        /// <summary>
        /// Adapts a capsule collider to the mesh bounds.
        /// </summary>
        public static void MatchCapsuleColliderToBounds(Mesh mesh, CapsuleCollider capsuleCollider)
        {
            MatchCapsuleColliderToBoundsInternal(mesh, capsuleCollider);
        }
        
        
        /* Vertices **********************************************************************************************************************/
        
        
        /// <summary>
        ///     Creates a new list from the current vertices which can be used for the transformation methods provided below.
        ///     Remember to use mesh.SetVertices(vertices); when done.
        /// </summary>
        public static List<Vector3> CreateVertexList(Mesh mesh)
        {
            var vertices = new List<Vector3>(mesh.vertexCount);
            mesh.GetVertices(vertices);
            return vertices;
        }
        
        /// <summary>
        ///     Translates all vertices.
        /// </summary>
        /// <param name="translation">Delta position of the vertices.</param>
        public static void PGTranslateVertices(List<Vector3> vertices, Vector3 translation)
        {
            for (var i = 0; i < vertices.Count; i++) vertices[i] += translation;
        }
        
        /// <summary>
        ///     Rotates all vertices around a pivot point.
        /// </summary>
        /// <param name="pivot">The pivot point around which the rotation occurs. For example mesh.bounds.center</param>
        public static void PGRotateVertices(List<Vector3> vertices, Quaternion rotation, Vector3 pivot)
        {
            for (var i = 0; i < vertices.Count; i++) vertices[i] = rotation * (vertices[i] - pivot) + pivot;
        }
        
        /// <summary>
        ///     Scales the vertices of the mesh by the specified scaleFactor around the given center point.
        /// </summary>
        /// <param name="scaleFactor">The scale factor to apply to the vertices.</param>
        /// <param name="center">The center point around which the scaling is performed.</param>
        public static void PGScaleVertices(List<Vector3> vertices, Vector3 scaleFactor, Vector3 center)
        {
            for (var i = 0; i < vertices.Count; i++)
            {
                var scaledPosition = vertices[i] - center;
                scaledPosition = new Vector3(scaledPosition.x * scaleFactor.x, scaledPosition.y * scaleFactor.y, scaledPosition.z * scaleFactor.z);
                vertices[i] = scaledPosition + center;
            }
        }
        
        /********************************************************************************************************************************/
        /********************************************************************************************************************************/


        private static Mesh CombineMeshesInternal(Mesh[] meshes, bool mergeSubmeshes)
        {
            var combinedInstances = new CombineInstance[meshes.Length];
            for (var i = 0; i < combinedInstances.Length; i++) combinedInstances[i].transform = Matrix4x4.identity;
            for (var i = 0; i < combinedInstances.Length; i++) combinedInstances[i].mesh = meshes[i];

            var combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(combinedInstances, mergeSubmeshes, true);
            return combinedMesh;
        }

        private static void GetIndexesInRadiusInternal(Mesh mesh, Vector3 pivot, float radius, out int[] indexesInRadius, out int closestIndex)
        {
            HashSet<int> indexesWithinRadiusSet = new HashSet<int>();
            float closestDistanceSqr = Mathf.Infinity;
            closestIndex = -1;

            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;

            for (int i = 0; i < triangles.Length; i += 3)
            {
                int vertexIndex1 = triangles[i];
                int vertexIndex2 = triangles[i + 1];
                int vertexIndex3 = triangles[i + 2];

                Vector3 vertex1 = vertices[vertexIndex1];
                Vector3 vertex2 = vertices[vertexIndex2];
                Vector3 vertex3 = vertices[vertexIndex3];

                float distanceSqr1 = (vertex1 - pivot).sqrMagnitude;
                float distanceSqr2 = (vertex2 - pivot).sqrMagnitude;
                float distanceSqr3 = (vertex3 - pivot).sqrMagnitude;

                if (distanceSqr1 <= radius * radius)
                {
                    indexesWithinRadiusSet.Add(vertexIndex1);
                    if (distanceSqr1 < closestDistanceSqr)
                    {
                        closestDistanceSqr = distanceSqr1;
                        closestIndex = vertexIndex1;
                    }
                }

                if (distanceSqr2 <= radius * radius)
                {
                    indexesWithinRadiusSet.Add(vertexIndex2);
                    if (distanceSqr2 < closestDistanceSqr)
                    {
                        closestDistanceSqr = distanceSqr2;
                        closestIndex = vertexIndex2;
                    }
                }

                if (distanceSqr3 <= radius * radius)
                {
                    indexesWithinRadiusSet.Add(vertexIndex3);
                    if (distanceSqr3 < closestDistanceSqr)
                    {
                        closestDistanceSqr = distanceSqr3;
                        closestIndex = vertexIndex3;
                    }
                }
            }

            indexesInRadius = indexesWithinRadiusSet.ToArray();
        }
        
        
        private static void EqualizeNormalsInternal(Mesh mesh)
        {
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;
            int vertexCount = mesh.vertexCount;

            Dictionary<Vector3, List<int>> vertexMap = new Dictionary<Vector3, List<int>>();

            for (int i = 0; i < vertexCount; i++)
            {
                Vector3 vertexPosition = vertices[i];

                if (!vertexMap.ContainsKey(vertexPosition))
                    vertexMap[vertexPosition] = new List<int>();

                vertexMap[vertexPosition].Add(i);
            }

            foreach (KeyValuePair<Vector3, List<int>> entry in vertexMap)
            {
                List<int> indices = entry.Value;
                int count = indices.Count;
                if (count <= 1) continue;

                Vector3 averageNormal = Vector3.zero;
                foreach (int index in indices) averageNormal += normals[index];
                averageNormal /= count;

                foreach (int index in indices) normals[index] = averageNormal;
            }

            mesh.normals = normals;
        }
        
        private static void MatchCapsuleColliderToBoundsInternal(Mesh mesh, CapsuleCollider capsuleCollider)
        {
            Bounds bounds = mesh.bounds;
            Vector3 size = bounds.size;
            var lengths = new List<float>{ size.x, size.y, size.z };
            lengths.Sort();
            capsuleCollider.radius = lengths[1] / 2;
            capsuleCollider.height = lengths[2];
            capsuleCollider.center = bounds.center;
            int direction = 0;
            float maxLength = size.x;
            if (size.y > maxLength)
            {
                direction = 1;
                maxLength = size.y;
            }
            if (size.z > maxLength) direction = 2;
                    
            capsuleCollider.direction = direction;
        }
        
        
    }
}