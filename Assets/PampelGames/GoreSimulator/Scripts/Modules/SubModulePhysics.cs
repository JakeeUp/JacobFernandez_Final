// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------


using System;
using System.Collections.Generic;
using PampelGames.Shared.Utility;
using UnityEngine;

namespace PampelGames.GoreSimulator
{
    public class SubModulePhysics : SubModuleBase
    {
        public override string ModuleName()
        {
            return "Physics";
        }

        public override string ModuleInfo()
        {
            return "Adds physics components to detached objects.";
        }

        public override int imageIndex()
        {
            return 2;
        }

        public override bool CompatibleRagdoll()
        {
            return false;
        }

        public override void ModuleAdded(Type type)
        {
            base.ModuleAdded(type);
#if UNITY_EDITOR
            layer = LayerMask.NameToLayer("Default");
#endif
        }

        /********************************************************************************************************************************/

        public Enums.Collider collider = Enums.Collider.None;
        public int layer;

        public bool rigidbody;

        /********************************************************************************************************************************/

        public override void ExecuteModuleCut(SubModuleClass subModuleClass)
        {
            ExecuteModuleInternalChildren(subModuleClass);

            if (subModuleClass.subRagdoll) return;
            
            ExecuteModuleInternal(subModuleClass);

            if (!rigidbody) return;
            Rigidbody rigidBody = null;
            if (subModuleClass.parent.TryGetComponent<Rigidbody>(out var _rigidbody))
                rigidBody = _rigidbody;
            else
                rigidBody = subModuleClass.parent.AddComponent<Rigidbody>();    
            if(subModuleClass.force != Vector3.zero)
                rigidBody.AddForce(subModuleClass.force);
        }

        public override void ExecuteModuleExplosion(SubModuleClass subModuleClass)
        {
            ExecuteModuleInternal(subModuleClass);
            ExecuteModuleInternalChildren(subModuleClass);

            if (!rigidbody) return;
            for (int i = 0; i < subModuleClass.subModuleObjectClasses.Count; i++)
            {
                var subModuleObjectClass = subModuleClass.subModuleObjectClasses[i];
                if(!subModuleObjectClass.obj.TryGetComponent(out Rigidbody rigid))
                    rigid = subModuleObjectClass.obj.AddComponent<Rigidbody>();

                if (subModuleObjectClass.force != Vector3.zero)
                {
                    rigid.AddForce(subModuleObjectClass.force);
                }
            }
        }

        public override void ExecuteModuleRagdoll(List<GoreBone> goreBones)
        {
            
        }
        

        /********************************************************************************************************************************/

        private void ExecuteModuleInternal(SubModuleClass subModuleClass)
        {
            for (int i = 0; i < subModuleClass.subModuleObjectClasses.Count; i++)
            {
                var subModuleObjectClass = subModuleClass.subModuleObjectClasses[i];
                ExecuteInternal(subModuleObjectClass.obj, subModuleObjectClass.mesh);
            }
        }
        
        private void ExecuteModuleInternalChildren(SubModuleClass subModuleClass)
        {
            for (int i = 0; i < subModuleClass.children.Count; i++)
            {
                var child = subModuleClass.children[i].gameObject;
                Mesh childMesh = null;
                if (child.TryGetComponent<MeshFilter>(out var meshFilter)) childMesh = meshFilter.mesh;
                ExecuteInternalChild(child, childMesh);
            }
        }
        
        private void ExecuteInternal(GameObject obj, Mesh mesh)
        {
            obj.layer = layer;

            if (obj.TryGetComponent<Collider>(out var _collider)) return;
            
            if (collider == Enums.Collider.Box)
            {
                var boxCollider = obj.AddComponent<BoxCollider>();
                boxCollider.material = _goreSimulator.physicMaterial;
            }
            else if (collider == Enums.Collider.Capsule)
            {
                var capsuleCollider = obj.AddComponent<CapsuleCollider>();
                PGMeshUtility.MatchCapsuleColliderToBounds(mesh, capsuleCollider);
                capsuleCollider.material = _goreSimulator.physicMaterial;
            }
            else if (collider == Enums.Collider.Mesh)
            {
                var meshCollider = obj.AddComponent<MeshCollider>();
                meshCollider.convex = true;
                meshCollider.material = _goreSimulator.physicMaterial;
            }
        }

        private void ExecuteInternalChild(GameObject child, Mesh mesh)
        {
            if (!child.TryGetComponent<DetachedChild>(out var detachedChild)) return;

            if (collider != Enums.Collider.None && !child.TryGetComponent<Collider>(out var col))
            {
                detachedChild.layer = child.gameObject.layer;
                child.gameObject.layer = layer;
                    
                if (collider == Enums.Collider.Box)
                {
                    var boxCollider = child.gameObject.AddComponent<BoxCollider>();
                    boxCollider.material = _goreSimulator.physicMaterial;
                    detachedChild.RegisterComponent(boxCollider);
                }
                    
                else if (collider == Enums.Collider.Capsule)
                {
                    var capsuleCollider = child.gameObject.AddComponent<CapsuleCollider>();
                    if(mesh != null) PGMeshUtility.MatchCapsuleColliderToBounds(mesh, capsuleCollider);
                    capsuleCollider.material = _goreSimulator.physicMaterial;
                    detachedChild.RegisterComponent(capsuleCollider);
                }
                    
                else if (collider == Enums.Collider.Mesh)
                {
                    var meshCollider = child.gameObject.AddComponent<MeshCollider>();
                    meshCollider.convex = true;
                    meshCollider.material = _goreSimulator.physicMaterial;
                    detachedChild.RegisterComponent(meshCollider);
                }
            }
            
            if (rigidbody)
            {
                var rigid = child.GetComponent<Rigidbody>();
                if (rigid == null) rigid = child.gameObject.AddComponent<Rigidbody>();
                detachedChild.RegisterComponent(rigid);
            }
            
        }
    }
}