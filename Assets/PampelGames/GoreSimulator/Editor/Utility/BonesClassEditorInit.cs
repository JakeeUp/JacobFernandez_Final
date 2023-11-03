// ----------------------------------------------------
// Gore Simulator
// Copyright (c) Pampel Games e.K. All Rights Reserved.
// https://www.pampelgames.com
// ----------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PampelGames.GoreSimulator.Editor
{
    internal static class BonesClassEditorInit
    {
        /********************************************************************************************************************************/
        // Verify

        public static bool VerifyFillBonesClasses(GoreSimulator _goreSimulator)
        {
            if (!VerifyBoneReferences(_goreSimulator))
            {
                EditorUtility.DisplayDialog("Missing References", "Please assign the storage and the skinned mesh renderer.", "Ok");
                return false;
            }

            for (var i = 0; i < _goreSimulator.bonesListClasses.Count; i++)
                if (_goreSimulator.bonesListClasses[i].bone == null)
                {
                    EditorUtility.DisplayDialog("Missing Bones", "There are empty references in the bones list.", "Ok");
                    return false;
                }

            return true;
        }

        private static bool VerifyBoneReferences(GoreSimulator _goreSimulator)
        {
            if (_goreSimulator.storage == null) return false;
            if (_goreSimulator.smr == null) return false;
            return true;
        }


        /********************************************************************************************************************************/
        // Fill

        public static bool FillBonesClasses(GoreSimulator _goreSimulator, Mesh bakedMesh, bool testMeshes)
        {
            _goreSimulator.bones = new List<Transform>();
            for (var i = 0; i < _goreSimulator.bonesListClasses.Count; i++)
                _goreSimulator.bones.Add(_goreSimulator.bonesListClasses[i].bone);

            _goreSimulator.center = _goreSimulator.bonesListClasses[0].bone;


            EditorUtility.DisplayProgressBar("Hold on...", "Initializing component.", 0f);

            _goreSimulator.bonesClasses = new List<BonesClass>();
            _goreSimulator.storage.bonesStorageClasses = new List<BonesStorageClass>();
            var bonesClassesAll = new List<BonesClass>();

            _goreSimulator.storage.meshDataClass = new MeshDataClass();
            _goreSimulator.storage.meshDataClass.InitializeEditorMeshData(_goreSimulator.smr.sharedMesh);
            _goreSimulator.storage.meshDataClass.InitializeRuntimeMeshData(_goreSimulator.smr.sharedMesh);
            var meshDataClass = _goreSimulator.storage.meshDataClass;
            var meshNativeDataClass = new MeshNativeDataClass();
            meshNativeDataClass.InitializeRuntimeMeshData(meshDataClass.serializableMesh);

            var vertices = bakedMesh.vertices;
            var triangles = bakedMesh.triangles;
            var indexes = Enumerable.Range(0, vertices.Length).ToArray();


            var adjacencyDict = ObjectCreationUtility.BuildAdjacencyDict(bakedMesh);
            
            List<Transform> uniqueBones = _goreSimulator.smr.bones.Distinct().ToList();

            
            var orderedBones = BonesUtility.GetOrderedBones(uniqueBones);
            orderedBones.Reverse();

            
            /********************************************************************************************************************************/
            // Bone Indexes
            for (var i = 0; i < orderedBones.Count; i++)
            {
                bonesClassesAll.Add(new BonesClass
                {
                    bone = orderedBones[i],
                });
            }

            for (var i = 0; i < bonesClassesAll.Count; i++)
            {
                var bonesClass = bonesClassesAll[i];
                if (!_goreSimulator.bones.Contains(bonesClass.bone)) continue;
                _goreSimulator.bonesClasses.Add(bonesClass);
                _goreSimulator.storage.bonesStorageClasses.Add(new BonesStorageClass
                {
                    boneName = bonesClass.bone.name
                });
            }

            for (var i = 0; i < orderedBones.Count; i++)
            {
                var bonesClass = bonesClassesAll[i];
                bonesClass.firstParent = BonesUtility.GetFirstParentBone(bonesClass.bone, _goreSimulator.bones);
                if (bonesClass.firstParent != null) bonesClass.parentExists = true;
                bonesClass.directBoneChildren = BonesUtility.GetDirectChildBones(bonesClass.bone, _goreSimulator.smr);
                bonesClass.boneChildrenSel = BonesUtility.GetAllChildBones(bonesClass.bone, _goreSimulator.bones);
                bonesClass.firstChildren = BonesUtility.GetDirectBoneChildrenSel(bonesClass.bone, bonesClass.boneChildrenSel);

                bonesClass.indexesStorageClass = new IndexesStorageClass();

                bonesClass.boneIndexes = new List<int>();
                var boneIndexesToAdd = MeshEditorUtility.GetBoneIndexes(_goreSimulator.smr, bonesClass.bone, 0.25f);
                bonesClass.boneIndexes.AddRange(boneIndexesToAdd.Except(bonesClass.boneIndexes));
                if (bonesClass.bone == _goreSimulator.center)
                {
                    _goreSimulator.centerBonesClass = bonesClass;
                }
            }

            /********************************************************************************************************************************/
            // Add all child bones until the next bones class.
            for (var i = 0; i < _goreSimulator.bonesClasses.Count; i++)
            {
                var bonesClass = _goreSimulator.bonesClasses[i];
                TraverseChildren(bonesClass.bone, _goreSimulator.smr);

                void TraverseChildren(Transform bone, SkinnedMeshRenderer smr)
                {
                    foreach (Transform childBone in bone)
                    {
                        if (!smr.bones.Contains(childBone)) continue;
                        if (_goreSimulator.bonesClasses.Any(b => b.bone == childBone)) continue;

                        var boneIndexesToAdd = MeshEditorUtility.GetBoneIndexes(_goreSimulator.smr, childBone, 0.25f);
                        
                        bonesClass.boneIndexes.AddRange(boneIndexesToAdd.Except(bonesClass.boneIndexes));

                        TraverseChildren(childBone, smr);
                    }
                }
            }

            // Also parents, if center is first parent.
            for (var i = 0; i < _goreSimulator.bonesClasses.Count; i++)
            {
                var bonesClass = _goreSimulator.bonesClasses[i];
                if (bonesClass.firstParent != _goreSimulator.center) continue;

                TraverseParents(bonesClass.bone, _goreSimulator.smr);

                void TraverseParents(Transform bone, SkinnedMeshRenderer smr)
                {
                    var parentBone = bone.parent;
                    if (parentBone == null) return;
                    if (parentBone == _goreSimulator.center) return;

                    var _parentBone = parentBone;
                    if (smr.bones.Contains(parentBone) && _goreSimulator.bonesClasses.All(b => b.bone != _parentBone))
                    {
                        var boneIndexesToAdd = MeshEditorUtility.GetBoneIndexes(_goreSimulator.smr, parentBone, 0.25f);
                        bonesClass.boneIndexes.AddRange(boneIndexesToAdd.Except(bonesClass.boneIndexes));
                    }

                    TraverseParents(parentBone, smr);
                }
            }

            /********************************************************************************************************************************/

            for (var i = 0; i < _goreSimulator.bonesClasses.Count; i++)
                _goreSimulator.bonesClasses[i].centralBone = BonesUtility.DetermineCentralBone(_goreSimulator.bonesClasses[i]);


            var bonesClasses = _goreSimulator.bonesClasses;
            var bonesStorageClasses = _goreSimulator.storage.bonesStorageClasses;

            for (int i = 0; i < bonesClasses.Count; i++)
            {
                if(bonesClasses[i].boneIndexes.Count != 0) continue;
                Debug.LogError("The bone: " + bonesClasses[i].bone.name + " cannot be used because it either has no bone weights " +
                               "or it belongs to a different Skinned Mesh Renderer.\n" + 
                               "In the case of the latter, you have the option to combine the meshes:\n" +
                               "Tools -> PampelGames -> Gore Simulator -> Combine Skinned Meshes");
                EditorUtility.ClearProgressBar();
                return false;
            }
            
            
            /********************************************************************************************************************************/
            // Cut Side Indexes
            for (var i = 0; i < bonesClasses.Count; i++)
            {
                var progress = (float) i / bonesClasses.Count();
                EditorUtility.DisplayProgressBar("Hold on...", "Initializing mesh.", progress);

                var bonesClass = _goreSimulator.bonesClasses[i];
                var bonesStorageClass = _goreSimulator.storage.bonesStorageClasses[i];

                if (bonesClass.bone == _goreSimulator.center) continue;

                CutSideIndexes(bonesClass, bonesStorageClass);
            }

            void CutSideIndexes(BonesClass bonesClass, BonesStorageClass bonesStorageClass)
            {
                var childBoneClasses =
                    BonesUtility.GetAllChildBoneClasses(bonesClass.bone, _goreSimulator.smr.bones.ToList(), bonesClassesAll);
                var avgChildPos = MathUtility.GetAveragePosition(bonesClass.directBoneChildren);
                if (bonesClass.firstChildren.Count > 0) avgChildPos = MathUtility.GetAveragePosition(bonesClass.firstChildren);

                bonesStorageClass.cutIndexClasses = new List<CutIndexClass>(_goreSimulator.meshesPerBone);


                for (var i = 0; i < _goreSimulator.meshesPerBone; i++)
                {
                    // Only continue if there is a BonesClass in hierarchy.
                    // if (j > 0 && bonesClass.boneChildrenSel.Count == 0) continue;
                    if (i > 0 && bonesClass.bone == _goreSimulator.center) continue;

                    bonesStorageClass.cutIndexClasses.Add(new CutIndexClass());

                    var cutIndexClass = bonesStorageClass.cutIndexClasses[i];

                    /********************************************************************************************************************************/
                    var position = bonesClass.bone.position;

                    // See also TraverseParents on top, adding all indexes until the center.
                    if (bonesClass.firstParent == _goreSimulator.center)
                    {
                        TraverseParents(bonesClass.bone, _goreSimulator.smr);

                        void TraverseParents(Transform bone, SkinnedMeshRenderer smr)
                        {
                            var parentBone = bone.parent;
                            if (parentBone == null) return;
                            if (parentBone == _goreSimulator.center) return;
                            if (smr.bones.Contains(parentBone)) position = parentBone.position;
                            TraverseParents(parentBone, smr);
                        }
                    }


                    if (i > 0)
                    {
                        var percent = (float) i / _goreSimulator.meshesPerBone;
                        position = Vector3.Lerp(position, avgChildPos, percent);
                    }

                    var childDirection = false;
                    var normalParent = bonesClass.bone.parent.position;
                    if (bonesClass.firstChildren.Count > 0)
                    {
                        normalParent = avgChildPos;
                        childDirection = true;
                    }
                    else if (bonesClass.firstParent != null && bonesClass.firstParent != _goreSimulator.center)
                    {
                        normalParent = bonesClass.firstParent.position;
                    }

                    var planeNormal = (position - normalParent).normalized;
                    if (childDirection) planeNormal *= -1;


                    /********************************************************************************************************************************/
                    // Center Only (Not needed currently).
                    var startPoint = Vector3.zero;
                    var endPoint = Vector3.zero;
                    if (bonesClass.bone == _goreSimulator.center)
                    {
                        var centerMesh = new Mesh();
                        var centerPositions = new List<Vector3>();
                        for (var j = 0; j < bonesClass.boneIndexes.Count; j++)
                        {
                            var worldPosition = _goreSimulator.smr.transform.TransformPoint(bakedMesh.vertices[bonesClass.boneIndexes[j]]);
                            centerPositions.Add(worldPosition);
                        }

                        centerMesh.vertices = centerPositions.ToArray();
                        centerMesh.RecalculateBounds();

                        var bounds = centerMesh.bounds;
                        var boundsSize = bounds.size;

                        if (boundsSize.x >= boundsSize.y && boundsSize.x >= boundsSize.z)
                        {
                            startPoint = new Vector3(bounds.min.x, bounds.center.y, bounds.center.z);
                            endPoint = new Vector3(bounds.max.x, bounds.center.y, bounds.center.z);
                        }
                        else if (boundsSize.y >= boundsSize.x && boundsSize.y >= boundsSize.z)
                        {
                            startPoint = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
                            endPoint = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
                        }
                        else
                        {
                            startPoint = new Vector3(bounds.center.x, bounds.center.y, bounds.min.z);
                            endPoint = new Vector3(bounds.center.x, bounds.center.y, bounds.max.z);
                        }

                        planeNormal = (endPoint - startPoint).normalized;

                        var proportion = (float) (i + 1) / (_goreSimulator.meshesPerBone + 1);
                        position = Vector3.Lerp(startPoint, endPoint, proportion);

                        Object.DestroyImmediate(centerMesh);
                    }
                    /********************************************************************************************************************************/

                    var AllIndexesCutSide = new List<int>(bonesClass.boneIndexes);

                    // First adding all children
                    if (bonesClass.bone != _goreSimulator.center)
                        for (var k = 0; k < childBoneClasses.Count; k++)
                            AllIndexesCutSide.AddRange(childBoneClasses[k].boneIndexes.Except(AllIndexesCutSide));
                    
                    

                    MeshCutJobs.GetVerticesCutSides(AllIndexesCutSide, bakedMesh, position, planeNormal, _goreSimulator.smr.transform,
                        out var indexesCutSide, out var indexesRemaining);
                    

                    /********************************************************************************************************************************/
                    if (bonesClass.bone == _goreSimulator.center)
                    {
                        var centerPosition = Vector3.Lerp(startPoint, endPoint, 0.5f);

                        for (var j = 0; j < bonesClass.firstChildren.Count; j++)
                        {
                            var j1 = j;
                            var childBonesClass = bonesClasses.FirstOrDefault(b => b.bone == bonesClass.firstChildren[j1]);
                            var childBonesStorage = bonesStorageClasses.FirstOrDefault(b => b.boneName == bonesClass.firstChildren[j].name);
                            
                            var directionFromPointToBone = childBonesClass.bone.position - centerPosition;
                            var isInPositiveDirection = Vector3.Dot(directionFromPointToBone, planeNormal) > 0;

                            if (isInPositiveDirection && childBonesStorage.cutIndexClasses.Count > 0)
                                indexesCutSide.AddRange(childBonesStorage.cutIndexClasses[0].indexesCutSide
                                    .Except(indexesCutSide));

                            if (i == 0) indexesCutSide.AddRange(bonesClass.boneIndexes.Except(indexesCutSide));
                        }
                    }
                    else
                    {
                        /********************************************************************************************************************************/
                        // Adding children again.
                        
                        for (var k = 0; k < childBoneClasses.Count; k++)
                            indexesCutSide.AddRange(childBoneClasses[k].boneIndexes.Except(indexesCutSide));
                    }
                    
                    
                    /********************************************************************************************************************************/


                    // Now removing parent indexes if they are not in the boneIndexes (removing parent indexes from the children).
                    var parentName = bonesClass.bone.parent;
                    var parentBoneClass = bonesClassesAll.FirstOrDefault(b => b.bone == parentName);
                    
                    
                    indexesCutSide.RemoveAll(item => parentBoneClass != null && parentBoneClass.boneIndexes.Contains(item) &&
                                                     !bonesClass.boneIndexes.Contains(item));
                    
                    var indexesParentSide = MeshCutJobs.GetOppositeIndexes(meshNativeDataClass, indexesCutSide);


                    /********************************************************************************************************************************/
                    // Clean-Up: Getting direct connections and move them over if they are not on the correct side.

                    // REMOVED: thigh sew indexes to center wont work correctly. 

                    // // Cut Side
                    // MeshCutJobs.GetDirectConnections(indexesCutSide, indexesParentSide, adjacencyDict,
                    //     out var directConnections1);
                    //
                    // MeshCutJobs.GetVerticesCutSides(directConnections1, bakedMesh, position, planeNormal, _goreSimulator.smr.transform,
                    //     out var directConnections1CutSide, out var directConnections1OtherSide);
                    //
                    // indexesCutSide.AddRange(directConnections1CutSide.Except(indexesCutSide));
                    // indexesParentSide.RemoveAll(item => directConnections1CutSide.Contains(item));
                    
                    
                    /********************************************************************************************************************************/
                    
                    cutIndexClass.indexesCutSide = indexesCutSide;
                    cutIndexClass.indexesParentSide = indexesParentSide;
                }
            }


            /********************************************************************************************************************************/
            // Chunk Indexes (Explosion Parts)

            for (var i = 0; i < bonesClasses.Count; i++)
            {
                var progress = (float) i / bonesClasses.Count();
                EditorUtility.DisplayProgressBar("Hold on...", "Initializing mesh parts.", progress);

                var bonesClass = _goreSimulator.bonesClasses[i];
                var bonesStorageClass = _goreSimulator.storage.bonesStorageClasses[i];

                if (bonesClass.bone == _goreSimulator.center) continue;

                ChunkIndexes(bonesClass, bonesStorageClass);
            }

            void ChunkIndexes(BonesClass bonesClass, BonesStorageClass bonesStorageClass)
            {
                
                var firstChildren = BonesUtility.GetFirstChildBoneStorageClasses(bonesClass.firstChildren, bonesStorageClasses);
                for (var i = 0; i < bonesStorageClass.cutIndexClasses.Count; i++)
                {
                    var cutIndexClass = bonesStorageClass.cutIndexClasses[i];
                    var chunkIndexes = new List<int>(cutIndexClass.indexesCutSide);


                    // Removing bone children.
                    for (var k = 0; k < firstChildren.Count; k++)
                    {
                        if (firstChildren[k].cutIndexClasses == null) continue;
                        if (firstChildren[k].cutIndexClasses.Count == 0) continue;
                        var k1 = k;
                        chunkIndexes.RemoveAll(item => firstChildren[k1].cutIndexClasses[0].indexesCutSide.Contains(item));
                    }

                    // Removing chunk children.
                    if (i != bonesStorageClass.cutIndexClasses.Count - 1)
                    {
                        var i1 = i;
                        chunkIndexes.RemoveAll(item => bonesStorageClass.cutIndexClasses[i1 + 1].indexesCutSide.Contains(item));
                    }

                    // Connections from children needed for first center chunk.
                    if (bonesClass.bone == _goreSimulator.center && i == 0)
                        for (var j = 0; j < bonesClass.firstChildren.Count; j++)
                        {
                            var j1 = j;
                            var childBonesStorageClass =
                                bonesStorageClasses.FirstOrDefault(b => b.boneName == bonesClass.firstChildren[j1].name);
                            if (childBonesStorageClass == null || childBonesStorageClass.cutIndexClasses.Count < 1) continue;
                            var childCutIndexClass = childBonesStorageClass.cutIndexClasses[0];

                            MeshCutJobs.GetDirectConnections(chunkIndexes, childCutIndexClass.chunkIndexes,
                                adjacencyDict, out var connectionsCut);

                            chunkIndexes.AddRange(connectionsCut.Except(chunkIndexes));
                            cutIndexClass.indexesCutSide.AddRange(connectionsCut.Except(cutIndexClass.indexesCutSide));
                        }

                    cutIndexClass.chunkIndexes = chunkIndexes;
                }
            }


            /********************************************************************************************************************************/
            // Corrections

            for (var i = 0; i < bonesClasses.Count; i++)
            {
                var progress = (float) i / bonesClasses.Count();
                EditorUtility.DisplayProgressBar("Hold on...", "Initializing mesh parts.", progress);

                var bonesClass = bonesClasses[i];
                var bonesStorageClass = bonesStorageClasses[i];

                if (bonesClass.bone == _goreSimulator.center) continue;

                Corrections(bonesClass, bonesStorageClass);
            }

            void Corrections(BonesClass bonesClass, BonesStorageClass bonesStorageClass)
            {
                // REMOVED: Since center is only one chunk.
                // Remove chunk if center influence is to high.
                // if (bonesClass.bone != _goreSimulator.center)
                //     for (var j = bonesStorageClass.cutIndexClasses.Count - 1; j >= 0; j--)
                //     {
                //         var cutIndexClass = bonesStorageClass.cutIndexClasses[j];
                //         float commonCount = cutIndexClass.chunkIndexes.Count(centerBoneIndexes.Contains);
                //         var percentage = commonCount / bonesStorageClass.cutIndexClasses.Count;
                //         if (percentage > 0.5f) bonesStorageClass.cutIndexClasses.RemoveAt(j);
                //     }


                // Merge chunks if chunk has to few vertices.
                if (bonesStorageClass.cutIndexClasses.Count > 1)
                    for (var j = bonesStorageClass.cutIndexClasses.Count - 1; j >= 1; j--)
                    {
                        var cutIndexClass = bonesStorageClass.cutIndexClasses[j - 1];
                        var cutIndexClass2 = bonesStorageClass.cutIndexClasses[j];

                        if (cutIndexClass2.chunkIndexes.Count < 3)
                        {
                            cutIndexClass.indexesCutSide.AddRange(cutIndexClass2.indexesCutSide.Except(cutIndexClass.indexesCutSide));
                            cutIndexClass.indexesParentSide = MeshCutJobs.GetOppositeIndexes(meshNativeDataClass, cutIndexClass.indexesCutSide);
                            cutIndexClass.chunkIndexes.AddRange(cutIndexClass2.chunkIndexes.Except(cutIndexClass.chunkIndexes));
                            bonesStorageClass.cutIndexClasses.Remove(cutIndexClass2);
                        }
                        else if (cutIndexClass.chunkIndexes.Count < 3)
                        {
                            cutIndexClass2.indexesCutSide.AddRange(cutIndexClass.indexesCutSide.Except(cutIndexClass2.indexesCutSide));
                            cutIndexClass2.indexesParentSide = MeshCutJobs.GetOppositeIndexes(meshNativeDataClass, cutIndexClass2.indexesCutSide);
                            cutIndexClass2.chunkIndexes.AddRange(cutIndexClass.chunkIndexes.Except(cutIndexClass2.chunkIndexes));
                            bonesStorageClass.cutIndexClasses.Remove(cutIndexClass);
                        }
                    }

                // Merge chunks if chunk is in between two chunks that are connected. 
                if (bonesStorageClass.cutIndexClasses.Count > 2)
                    for (var j = bonesStorageClass.cutIndexClasses.Count - 1; j >= 2; j--)
                    {
                        var cutIndexClass1 = bonesStorageClass.cutIndexClasses[j - 2];
                        var cutIndexClassBetween = bonesStorageClass.cutIndexClasses[j - 1];
                        var cutIndexClass2 = bonesStorageClass.cutIndexClasses[j];

                        MeshCutJobs.GetDirectConnections(cutIndexClass1.chunkIndexes, cutIndexClass2.chunkIndexes, adjacencyDict,
                            out var directConnections1);
                        if (directConnections1.Count == 0) continue;

                        cutIndexClass1.indexesCutSide.AddRange(cutIndexClassBetween.indexesCutSide.Except(cutIndexClass1.indexesCutSide));
                        cutIndexClass1.indexesParentSide = MeshCutJobs.GetOppositeIndexes(meshNativeDataClass, cutIndexClass1.indexesCutSide);
                        cutIndexClass1.chunkIndexes.AddRange(cutIndexClassBetween.chunkIndexes.Except(cutIndexClass1.chunkIndexes));
                        bonesStorageClass.cutIndexClasses.Remove(cutIndexClassBetween);
                    }


                // Merge chunks if lower chunk is connected to parent. 
                if (bonesStorageClass.cutIndexClasses.Count > 1)
                    for (var j = bonesStorageClass.cutIndexClasses.Count - 1; j >= 1; j--)
                    {
                        var cutIndexClass = bonesStorageClass.cutIndexClasses[j - 1];
                        var cutIndexClass2 = bonesStorageClass.cutIndexClasses[j];

                        if (!bonesClass.parentExists) continue;
                        var parentBonesClass = bonesClassesAll.FirstOrDefault(b => b.bone == bonesClass.firstParent);
                        var parentBoneName = parentBonesClass.bone.name;
                        var parentBonesStorageClass = bonesStorageClasses.FirstOrDefault(b => b.boneName == parentBoneName);
                        if (parentBonesStorageClass == null) continue;
                        if (parentBonesStorageClass?.cutIndexClasses == null) continue;
                        if (parentBonesStorageClass.cutIndexClasses.Count == 0) continue;

                        var parentChunkIndexes = new List<int>();
                        for (var k = 0; k < parentBonesStorageClass.cutIndexClasses.Count; k++)
                            parentChunkIndexes.AddRange(parentBonesStorageClass.cutIndexClasses[k].chunkIndexes.Except(parentChunkIndexes));


                        MeshCutJobs.GetDirectConnections(parentChunkIndexes, cutIndexClass2.chunkIndexes, adjacencyDict,
                            out var directConnections1);
                        if (directConnections1.Count == 0) continue;

                        cutIndexClass.indexesCutSide.AddRange(cutIndexClass2.indexesCutSide.Except(cutIndexClass.indexesCutSide));
                        cutIndexClass.indexesParentSide = MeshCutJobs.GetOppositeIndexes(meshNativeDataClass, cutIndexClass.indexesCutSide);
                        cutIndexClass.chunkIndexes.AddRange(cutIndexClass2.chunkIndexes.Except(cutIndexClass.chunkIndexes));
                        bonesStorageClass.cutIndexClasses.Remove(cutIndexClass2);
                    }

                // Merge chunks if chunk is connected to children of lower chunk. 
                if (bonesClass.bone != _goreSimulator.center && bonesStorageClass.cutIndexClasses.Count > 1)
                    for (var i = bonesStorageClass.cutIndexClasses.Count - 1; i >= 1; i--)
                    {
                        var cutIndexClass = bonesStorageClass.cutIndexClasses[i - 1];
                        var cutIndexClass2 = bonesStorageClass.cutIndexClasses[i];

                        var cutIndex2Children = new List<int>();
                        var firstChildren = BonesUtility.GetFirstChildBoneStorageClasses(bonesClass.firstChildren, bonesStorageClasses);

                        if (firstChildren == null || firstChildren.Count == 0) continue;

                        for (var k = 0; k < firstChildren.Count; k++)
                        for (var l = 0; l < firstChildren[k].cutIndexClasses.Count; l++)
                            cutIndex2Children.AddRange(firstChildren[k].cutIndexClasses[l].chunkIndexes.Except(cutIndex2Children));

                        MeshCutJobs.GetDirectConnections(cutIndex2Children, cutIndexClass.chunkIndexes, adjacencyDict,
                            out var directConnections1);
                        if (directConnections1.Count == 0) continue;

                        cutIndexClass.indexesCutSide.AddRange(cutIndexClass2.indexesCutSide.Except(cutIndexClass.indexesCutSide));
                        cutIndexClass.indexesParentSide = MeshCutJobs.GetOppositeIndexes(meshNativeDataClass, cutIndexClass.indexesCutSide);
                        cutIndexClass.chunkIndexes.AddRange(cutIndexClass2.chunkIndexes.Except(cutIndexClass.chunkIndexes));
                        bonesStorageClass.cutIndexClasses.Remove(cutIndexClass2);
                    }
                
            }


            /********************************************************************************************************************************/
            // Complete the parent side so it overlaps by default.

            for (var i = 0; i < bonesClasses.Count; i++)
            {
                var bonesClass = bonesClasses[i];
                var bonesStorageClass = _goreSimulator.storage.bonesStorageClasses[i];

                if (bonesClass.bone == _goreSimulator.center) continue;

                CompleteParentSide(bonesStorageClass);
            }

            void CompleteParentSide(BonesStorageClass bonesStorageClass)
            {
                for (var i = 0; i < bonesStorageClass.cutIndexClasses.Count; i++)
                {
                    var cutIndexClass = bonesStorageClass.cutIndexClasses[i];

                    MeshCutJobs.GetDirectConnections(cutIndexClass.indexesParentSide, cutIndexClass.indexesCutSide, adjacencyDict,
                        out var cutIndexesParent);
                    cutIndexClass.indexesParentSide.AddRange(cutIndexesParent.Except(cutIndexClass.indexesParentSide));
                }
            }


            /********************************************************************************************************************************/
            /********************************************************************************************************************************/
            // Cut Indexes (Connections).

            for (var i = 0; i < bonesClasses.Count; i++)
            {
                var progress = (float) i / bonesClasses.Count();
                EditorUtility.DisplayProgressBar("Hold on...", "Initializing connections.", progress);

                var bonesClass = bonesClasses[i];
                var bonesStorageClass = _goreSimulator.storage.bonesStorageClasses[i];

                if (bonesClass.bone == _goreSimulator.center) continue;

                CutIndexes(bonesClass, bonesStorageClass);
            }


            void CutIndexes(BonesClass bonesClass, BonesStorageClass bonesStorageClass)
            {
                for (var i = 0; i < bonesStorageClass.cutIndexClasses.Count; i++)
                {
                    var cutIndexClass = bonesStorageClass.cutIndexClasses[i];

                    /********************************************************************************************************************************/
                    // Parent
                    /********************************************************************************************************************************/

                    /********************************************************************************************************************************/
                    // Cut Indexes Parent Side
                    
                    
                    var sharedIndexes = cutIndexClass.indexesParentSide.Intersect(cutIndexClass.indexesCutSide).ToList();
                    cutIndexClass.cutIndexesParentSide = sharedIndexes;
                    cutIndexClass.chunkIndexes.AddRange(sharedIndexes.Except(cutIndexClass.chunkIndexes));
                    
                    
                                       
                    // if (bonesStorageClass.boneName.Contains("spine"))
                    // {
                    //     GameObject parentOBJ = new GameObject(bonesStorageClass.boneName + "_" + i);
                    //     for (int k = 0; k < cutIndexClass.chunkIndexes.Count; k++)
                    //     {
                    //         var worldSpace = _goreSimulator.smr.transform.TransformPoint(bakedMesh.vertices[cutIndexClass.chunkIndexes[k]]);
                    //         var sphere = PGInformationUtility.CreateSphere(worldSpace, 0.01f);
                    //         sphere.transform.SetParent(parentOBJ.transform);
                    //     }    
                    // }
                    
                    
                    
                    
                    /********************************************************************************************************************************/
                    // Sew indexes parent side. Used for single cut.

                    var seperatedIndexesCutSide = new List<int>(cutIndexClass.indexesCutSide);
                    seperatedIndexesCutSide.RemoveAll(item => cutIndexClass.indexesParentSide.Contains(item));


                    // For some reason, without this additional logic it won't work properly.
                    // Checking if indices have the same position and move them over before GetDirectConnections.
                    // ---- Start ----
                    var sewPositions1 = new HashSet<Vector3>(cutIndexClass.indexesParentSide.Select(index => vertices[index]));
                    var matchingCutIndexes1 = new List<int>();
                    foreach (var index in seperatedIndexesCutSide)
                    {
                        var position = vertices[index];
                        if (sewPositions1.Contains(position)) matchingCutIndexes1.Add(index);
                    }

                    if (matchingCutIndexes1.Count > 0)
                    {
                        var uniqueMatchingIndexes = matchingCutIndexes1.Except(cutIndexClass.indexesParentSide).ToList();
                        cutIndexClass.indexesParentSide.AddRange(uniqueMatchingIndexes);
                    }

                    seperatedIndexesCutSide.RemoveAll(item => cutIndexClass.indexesParentSide.Contains(item));
                    // ---- End ----


                    MeshCutJobs.GetDirectConnections(cutIndexClass.indexesParentSide, seperatedIndexesCutSide, adjacencyDict,
                        out var sewIndexesForParent);
                    cutIndexClass.sewIndexesForParent = sewIndexesForParent;


                    /********************************************************************************************************************************/
                    // Add indexes from cutIndexesParentSide to sewIndexesForParent if they have the same position.

                    var sewPositions = new HashSet<Vector3>(cutIndexClass.sewIndexesForParent.Select(index => vertices[index]));
                    var matchingCutIndexes = new List<int>();
                    foreach (var index in cutIndexClass.cutIndexesParentSide)
                    {
                        var position = vertices[index];
                        if (sewPositions.Contains(position)) matchingCutIndexes.Add(index);
                    }

                    if (matchingCutIndexes.Count > 0)
                    {
                        var uniqueMatchingIndexes = matchingCutIndexes.Except(cutIndexClass.sewIndexesForParent).ToList();
                        cutIndexClass.sewIndexesForParent.AddRange(uniqueMatchingIndexes);
                    }


                    /********************************************************************************************************************************/
                    // Sew Indexes Parent Side

                    var tempSewHelper = new List<int>(cutIndexClass.indexesParentSide);

                    tempSewHelper.RemoveAll(item => cutIndexClass.indexesCutSide.Contains(item));

                    MeshCutJobs.GetDirectConnections(cutIndexClass.indexesCutSide, tempSewHelper,
                        adjacencyDict, out var sewIndexesParentSide);

                    cutIndexClass.sewIndexesParentSide = sewIndexesParentSide;


                    /********************************************************************************************************************************/
                    // Add indexes from cutIndexesParentSide to sewIndexesParentSide if they have the same position.

                    var sewPositionsParentSide = new HashSet<Vector3>(cutIndexClass.sewIndexesParentSide.Select(index => vertices[index]));
                    var matchingCutIndexesParent = new List<int>();
                    foreach (var index in cutIndexClass.cutIndexesParentSide)
                    {
                        var position = vertices[index];
                        if (sewPositionsParentSide.Contains(position)) matchingCutIndexesParent.Add(index);
                    }

                    if (matchingCutIndexesParent.Count > 0)
                    {
                        var uniqueMatchingIndexes = matchingCutIndexesParent.Except(cutIndexClass.sewIndexesForParent).ToList();
                        cutIndexClass.sewIndexesParentSide.AddRange(uniqueMatchingIndexes);
                    }


                    /********************************************************************************************************************************/
                    // Children
                    /********************************************************************************************************************************/

                    /********************************************************************************************************************************/
                    // Cut Indexes Children

                    cutIndexClass.cutIndexChildClasses = new List<CutIndexChildClass>();

                    if (i != bonesStorageClass.cutIndexClasses.Count - 1)
                    {
                        cutIndexClass.cutIndexChildClasses.Add(new CutIndexChildClass());
                        MeshCutJobs.GetDirectConnections(cutIndexClass.chunkIndexes, bonesStorageClass.cutIndexClasses[i + 1].chunkIndexes,
                            adjacencyDict, out var cutIndexesChild);
                        cutIndexClass.cutIndexChildClasses[^1].cutIndexes = cutIndexesChild;
                    }
                    else if (bonesClass.firstChildren.Count > 0)
                    {
                        var firstChildren = BonesUtility.GetFirstChildBoneStorageClasses(bonesClass.firstChildren, bonesStorageClasses);
                        for (var k = 0; k < bonesClass.firstChildren.Count; k++)
                        {
                            cutIndexClass.cutIndexChildClasses.Add(new CutIndexChildClass());
                            var cutIndexChildClass = cutIndexClass.cutIndexChildClasses[^1];
                            cutIndexChildClass.childBoneName = firstChildren[k].boneName;

                            var combinedChild = new List<int>();
                            for (var l = 0; l < firstChildren[k].cutIndexClasses.Count; l++)
                                combinedChild.AddRange(firstChildren[k].cutIndexClasses[l].chunkIndexes.Except(combinedChild));

                            MeshCutJobs.GetDirectConnections(cutIndexClass.chunkIndexes, combinedChild, adjacencyDict,
                                out var cutIndexesChild);
                            cutIndexChildClass.cutIndexes = cutIndexesChild;
                        }
                    }
                    

                    // Adding cut indexes to parent chunk.
                    for (var j = 0; j < cutIndexClass.cutIndexChildClasses.Count; j++)
                    {
                        var cutIndexChildClass = cutIndexClass.cutIndexChildClasses[j];
                        cutIndexClass.chunkIndexes.AddRange(cutIndexChildClass.cutIndexes.Except(cutIndexClass.chunkIndexes));
                    }


                    
                    /********************************************************************************************************************************/
                    // Sew Indexes Children

                    if (i != bonesStorageClass.cutIndexClasses.Count - 1)
                    {
                        var remainingChildChunk = new List<int>(bonesStorageClass.cutIndexClasses[i + 1].chunkIndexes);

                        remainingChildChunk.RemoveAll(item => cutIndexClass.chunkIndexes.Contains(item));

                        MeshCutJobs.GetDirectConnections(cutIndexClass.chunkIndexes, remainingChildChunk,
                            adjacencyDict, out var sewIndexesChild);
                        cutIndexClass.cutIndexChildClasses[^1].sewIndexes = sewIndexesChild;
                    }
                    else if (bonesClass.firstChildren.Count > 0)
                    {
                        var firstChildren = BonesUtility.GetFirstChildBoneStorageClasses(bonesClass.firstChildren, bonesStorageClasses);
                        for (var j = 0; j < bonesClass.firstChildren.Count; j++)
                        {
                            var cutIndexChildClass = cutIndexClass.cutIndexChildClasses[j];

                            var combinedChild = new List<int>();
                            for (var l = 0; l < firstChildren[j].cutIndexClasses.Count; l++)
                                combinedChild.AddRange(firstChildren[j].cutIndexClasses[l].chunkIndexes.Except(combinedChild));

                            combinedChild.RemoveAll(item => cutIndexClass.chunkIndexes.Contains(item));


                            // Again, for some reason, without this additional logic it won't work properly.
                            // Checking if indices have the same position and move them over before GetDirectConnections.
                            // ---- Start ----
                            var sewPositionsC = new HashSet<Vector3>(cutIndexClass.chunkIndexes.Select(index => vertices[index]));
                            var matchingCutIndexesC = new List<int>();
                            foreach (var index in combinedChild)
                            {
                                var position = vertices[index];
                                if (sewPositionsC.Contains(position)) matchingCutIndexesC.Add(index);
                            }

                            if (matchingCutIndexesC.Count > 0)
                            {
                                var uniqueMatchingIndexes = matchingCutIndexesC.Except(cutIndexClass.chunkIndexes).ToList();
                                cutIndexClass.chunkIndexes.AddRange(uniqueMatchingIndexes);
                            }

                            combinedChild.RemoveAll(item => cutIndexClass.chunkIndexes.Contains(item));
                            // ---- End ----


                            MeshCutJobs.GetDirectConnections(cutIndexClass.chunkIndexes, combinedChild, adjacencyDict,
                                out var cutIndexesChild);
                            cutIndexChildClass.sewIndexes = cutIndexesChild;
                        }
                    }


                    /********************************************************************************************************************************/
                    // Add indexes from cutIndexes Children to sewIndexes Children if they have the same position.

                    for (var j = 0; j < cutIndexClass.cutIndexChildClasses.Count; j++)
                    {
                        var cutIndexesChild = cutIndexClass.cutIndexChildClasses[j].cutIndexes;
                        var sewIndexesChild = cutIndexClass.cutIndexChildClasses[j].sewIndexes;

                        var sewPositionsChild = new HashSet<Vector3>(sewIndexesChild.Select(index => vertices[index]));
                        var matchingCutIndexesChild = new List<int>();
                        foreach (var index in cutIndexesChild)
                        {
                            var position = vertices[index];
                            if (sewPositionsChild.Contains(position)) matchingCutIndexesChild.Add(index);
                        }

                        if (matchingCutIndexesChild.Count > 0)
                        {
                            var uniqueMatchingIndexes = matchingCutIndexesChild.Except(cutIndexClass.sewIndexesForParent).ToList();
                            sewIndexesChild.AddRange(uniqueMatchingIndexes);
                        }
                    }


                    /********************************************************************************************************************************/
                    // Put Cut & Sew Indexes in radial order for later uv.
                    /********************************************************************************************************************************/

                    // For Parent
                    var cutIndexesParentSide = cutIndexClass.cutIndexesParentSide;
                    var sewIndexesFParent = cutIndexClass.sewIndexesForParent;

                    var centerCutParent = MathUtility.GetCenterPosition(_goreSimulator.smr, cutIndexesParentSide, bakedMesh.vertices);
                    var centerSewForParent = MathUtility.GetCenterPosition(_goreSimulator.smr, sewIndexesFParent, bakedMesh.vertices);

                    var normalParent = centerCutParent - centerSewForParent;
                    normalParent.Normalize();

                    var refVectorParent = Vector3.Cross(normalParent, Vector3.up);
                    if (refVectorParent.magnitude < 0.01f) // If normal was near parallel to up vector
                        refVectorParent = Vector3.Cross(normalParent, Vector3.right); // then choose another orthogonal vector
                    refVectorParent.Normalize();

                    sewIndexesFParent.Sort((n, m) =>
                    {
                        var vecI = _goreSimulator.smr.transform.TransformPoint(bakedMesh.vertices[n]) - centerCutParent;
                        var vecJ = _goreSimulator.smr.transform.TransformPoint(bakedMesh.vertices[m]) - centerCutParent;

                        var angleI = Vector3.SignedAngle(refVectorParent, vecI, normalParent);
                        var angleJ = Vector3.SignedAngle(refVectorParent, vecJ, normalParent);

                        if (angleI < angleJ) return -1;
                        if (angleI > angleJ) return 1;
                        return 0;
                    });

                    cutIndexesParentSide.Sort((n, m) =>
                    {
                        var vecI = _goreSimulator.smr.transform.TransformPoint(bakedMesh.vertices[n]) - centerCutParent;
                        var vecJ = _goreSimulator.smr.transform.TransformPoint(bakedMesh.vertices[m]) - centerCutParent;

                        var angleI = Vector3.SignedAngle(refVectorParent, vecI, normalParent);
                        var angleJ = Vector3.SignedAngle(refVectorParent, vecJ, normalParent);

                        if (angleI < angleJ) return -1;
                        if (angleI > angleJ) return 1;
                        return 0;
                    });


                    /********************************************************************************************************************************/
                    // Parent Side
                    var sewIndexesParent = cutIndexClass.sewIndexesParentSide;

                    var centerSewParentSide = MathUtility.GetCenterPosition(_goreSimulator.smr, sewIndexesParent, bakedMesh.vertices);

                    var normalParentSide = centerCutParent - centerSewParentSide;
                    normalParentSide.Normalize();

                    var refVectorParentSide = Vector3.Cross(normalParentSide, Vector3.up);
                    if (refVectorParentSide.magnitude < 0.01f) // If normal was near parallel to up vector
                        refVectorParentSide = Vector3.Cross(normalParentSide, Vector3.right); // then choose another orthogonal vector
                    refVectorParentSide.Normalize();

                    sewIndexesParent.Sort((n, m) =>
                    {
                        var vecI = _goreSimulator.smr.transform.TransformPoint(bakedMesh.vertices[n]) - centerCutParent;
                        var vecJ = _goreSimulator.smr.transform.TransformPoint(bakedMesh.vertices[m]) - centerCutParent;

                        var angleI = Vector3.SignedAngle(refVectorParentSide, vecI, normalParentSide);
                        var angleJ = Vector3.SignedAngle(refVectorParentSide, vecJ, normalParentSide);

                        if (angleI < angleJ) return -1;
                        if (angleI > angleJ) return 1;
                        return 0;
                    });


                    /********************************************************************************************************************************/
                    // Child Sides

                    for (var j = 0; j < cutIndexClass.cutIndexChildClasses.Count; j++)
                    {
                        var cutIndexesChild = cutIndexClass.cutIndexChildClasses[j].cutIndexes;
                        var sewIndexesChild = cutIndexClass.cutIndexChildClasses[j].sewIndexes;

                        var centerCutChild = MathUtility.GetCenterPosition(_goreSimulator.smr, cutIndexesChild, bakedMesh.vertices);
                        var centerSewChild = MathUtility.GetCenterPosition(_goreSimulator.smr, sewIndexesChild, bakedMesh.vertices);

                        var normal = centerCutChild - centerSewChild;
                        normal.Normalize();

                        var refVector = Vector3.Cross(normal, Vector3.up);
                        if (refVector.magnitude < 0.01f) // If normal was near parallel to up vector
                            refVector = Vector3.Cross(normal, Vector3.right); // then choose another orthogonal vector
                        refVector.Normalize();

                        var meshTransform = _goreSimulator.smr.transform;

                        sewIndexesChild.Sort((n, m) =>
                        {
                            var vecI = meshTransform.TransformPoint(bakedMesh.vertices[n]) - centerCutChild;
                            var vecJ = meshTransform.TransformPoint(bakedMesh.vertices[m]) - centerCutChild;

                            var angleI = Vector3.SignedAngle(refVector, vecI, normal);
                            var angleJ = Vector3.SignedAngle(refVector, vecJ, normal);

                            if (angleI < angleJ) return -1;
                            if (angleI > angleJ) return 1;
                            return 0;
                        });
                        cutIndexesChild.Sort((n, m) =>
                        {
                            var vecI = meshTransform.TransformPoint(bakedMesh.vertices[n]) - centerCutChild;
                            var vecJ = meshTransform.TransformPoint(bakedMesh.vertices[m]) - centerCutChild;

                            var angleI = Vector3.SignedAngle(refVector, vecI, normal);
                            var angleJ = Vector3.SignedAngle(refVector, vecJ, normal);

                            if (angleI < angleJ) return -1;
                            if (angleI > angleJ) return 1;
                            return 0;
                        });
                    }


                    /********************************************************************************************************************************/
                    // Sew Triangles (used for cut UVs)
                    /********************************************************************************************************************************/

                    // For Parent

                    var trianglesForParent = new List<int>();

                    for (var j = 0; j < triangles.Length; j += 3)
                    {
                        var vertex1 = triangles[j];
                        var vertex2 = triangles[j + 1];
                        var vertex3 = triangles[j + 2];

                        if (!cutIndexClass.sewIndexesForParent.Contains(vertex1) && !cutIndexClass.sewIndexesForParent.Contains(vertex2)
                                                                                 && !cutIndexClass.sewIndexesForParent.Contains(vertex3))
                            continue;

                        var vertex1Exists = cutIndexClass.indexesParentSide.Contains(vertex1) ||
                                            cutIndexClass.sewIndexesForParent.Contains(vertex1);
                        var vertex2Exists = cutIndexClass.indexesParentSide.Contains(vertex2) ||
                                            cutIndexClass.sewIndexesForParent.Contains(vertex2);
                        var vertex3Exists = cutIndexClass.indexesParentSide.Contains(vertex3) ||
                                            cutIndexClass.sewIndexesForParent.Contains(vertex3);

                        if (vertex1Exists && vertex2Exists && vertex3Exists)
                        {
                            trianglesForParent.Add(vertex1);
                            trianglesForParent.Add(vertex2);
                            trianglesForParent.Add(vertex3);
                        }
                    }

                    cutIndexClass.sewTrianglesForParent = trianglesForParent;


                    /********************************************************************************************************************************/
                    // Parent Side

                    var trianglesParentSide = new List<int>();

                    for (var j = 0; j < triangles.Length; j += 3)
                    {
                        var vertex1 = triangles[j];
                        var vertex2 = triangles[j + 1];
                        var vertex3 = triangles[j + 2];

                        if (!cutIndexClass.sewIndexesParentSide.Contains(vertex1) && !cutIndexClass.sewIndexesParentSide.Contains(vertex2)
                                                                                  && !cutIndexClass.sewIndexesParentSide.Contains(vertex3))
                            continue;

                        var vertex1Exists = cutIndexClass.indexesCutSide.Contains(vertex1) ||
                                            cutIndexClass.sewIndexesParentSide.Contains(vertex1);
                        var vertex2Exists = cutIndexClass.indexesCutSide.Contains(vertex2) ||
                                            cutIndexClass.sewIndexesParentSide.Contains(vertex2);
                        var vertex3Exists = cutIndexClass.indexesCutSide.Contains(vertex3) ||
                                            cutIndexClass.sewIndexesParentSide.Contains(vertex3);

                        if (vertex1Exists && vertex2Exists && vertex3Exists)
                        {
                            trianglesParentSide.Add(vertex1);
                            trianglesParentSide.Add(vertex2);
                            trianglesParentSide.Add(vertex3);
                        }
                    }

                    cutIndexClass.sewTrianglesParentSide = trianglesParentSide;


                    /********************************************************************************************************************************/
                    // Child Sides

                    for (var j = 0; j < cutIndexClass.cutIndexChildClasses.Count; j++)
                    {
                        var childClass = cutIndexClass.cutIndexChildClasses[j];
                        var trianglesChild = new List<int>();

                        for (var k = 0; k < triangles.Length; k += 3)
                        {
                            var vertex1 = triangles[k];
                            var vertex2 = triangles[k + 1];
                            var vertex3 = triangles[k + 2];

                            if (!childClass.sewIndexes.Contains(vertex1) && !childClass.sewIndexes.Contains(vertex2)
                                                                         && !childClass.sewIndexes.Contains(vertex3)) continue;

                            var vertex1Exists = cutIndexClass.chunkIndexes.Contains(vertex1) || childClass.sewIndexes.Contains(vertex1);
                            var vertex2Exists = cutIndexClass.chunkIndexes.Contains(vertex2) || childClass.sewIndexes.Contains(vertex2);
                            var vertex3Exists = cutIndexClass.chunkIndexes.Contains(vertex3) || childClass.sewIndexes.Contains(vertex3);

                            if (vertex1Exists && vertex2Exists && vertex3Exists)
                            {
                                trianglesChild.Add(vertex1);
                                trianglesChild.Add(vertex2);
                                trianglesChild.Add(vertex3);
                            }
                        }

                        childClass.sewTriangles = trianglesChild;
                    }


                    // if(!bonesClass.bone.name.Contains("pelvis")) continue;


                    // GameObject parentOBJ = new GameObject(bonesClass.bone.name + "_" + j);
                    // for (int k = 0; k < cutIndexClass.chunkIndexes.Count; k++)
                    // {
                    //     var worldSpace = _goreSimulator.smr.transform.TransformPoint(bakedMesh.vertices[cutIndexClass.chunkIndexes[k]]);
                    //     var sphere = PGInformationUtility.CreateSphere(worldSpace, 0.01f);
                    //     sphere.transform.SetParent(parentOBJ.transform);
                    // }
                    //
                    // GameObject parentOBJ12 = new GameObject(bonesClass.bone.name + "_" + j + "_" + "_Cut");
                    // for (int k = 0; k < cutIndexClass.cutIndexesParentSide.Count; k++)
                    // {
                    //     var worldSpace = _goreSimulator.smr.transform.TransformPoint(bakedMesh.vertices[cutIndexClass.cutIndexesParentSide[k]]);
                    //     var sphere = PGInformationUtility.CreateSphere(worldSpace, 0.01f);
                    //     sphere.transform.SetParent(parentOBJ12.transform);
                    // }
                    //
                    // GameObject parentOBJ123 = new GameObject(bonesClass.bone.name + "_" + j + "_" + "_Sew");
                    // for (int k = 0; k < cutIndexClass.sewIndexesParentSide.Count; k++)
                    // {
                    //     var worldSpace = _goreSimulator.smr.transform.TransformPoint(bakedMesh.vertices[cutIndexClass.sewIndexesParentSide[k]]);
                    //     var sphere = PGInformationUtility.CreateSphere(worldSpace, 0.01f);
                    //     sphere.transform.SetParent(parentOBJ123.transform);
                    // }
                    //
                    // GameObject parentOBJ4 = new GameObject(bonesClass.bone.name + "_" + j + "_" + "_Triangle");
                    // for (int k = 0; k < cutIndexClass.sewTrianglesParentSide.Count; k++)
                    // {
                    //     var worldSpace = _goreSimulator.smr.transform.TransformPoint(bakedMesh.vertices[cutIndexClass.sewTrianglesParentSide[k]]);
                    //     var sphere = PGInformationUtility.CreateSphere(worldSpace, 0.01f);
                    //     sphere.transform.SetParent(parentOBJ4.transform);
                    // }
                }
            }

            /********************************************************************************************************************************/
            // Opposite Parent Indexes

            for (var i = 0; i < bonesClasses.Count; i++)
            {
                var bonesStorageClass = _goreSimulator.storage.bonesStorageClasses[i];

                if (bonesClasses[i].bone == _goreSimulator.center) continue;

                OppositeParentIndexes(bonesStorageClass);
            }

            void OppositeParentIndexes(BonesStorageClass bonesStorageClass)
            {
                for (var i = 0; i < bonesStorageClass.cutIndexClasses.Count; i++)
                {
                    var cutIndexClass = bonesStorageClass.cutIndexClasses[i];
                    
                    cutIndexClass.oppositeParentIndexes = indexes.Except(cutIndexClass.indexesParentSide).ToList();
                    
                    /********************************************************************************************************************************/
                    // Setting the index down here because of reorderings.
                    cutIndexClass.cutIndex = i;
                        
                }
            }


            /********************************************************************************************************************************/
            // Chunk Average Indexes

            for (var i = 0; i < bonesClasses.Count; i++)
            {
                var bonesStorageClass = _goreSimulator.storage.bonesStorageClasses[i];

                if (bonesClasses[i].bone == _goreSimulator.center) continue;

                ChunkAverageIndexes(bonesStorageClass);
            }

            void ChunkAverageIndexes(BonesStorageClass bonesStorageClass)
            {
                for (var i = 0; i < bonesStorageClass.cutIndexClasses.Count; i++)
                {
                    var chunkIndexes = bonesStorageClass.cutIndexClasses[i].chunkIndexes;

                    // For finding the chunk center at runtime.
                    if (chunkIndexes.Count == 0) continue;
                    int minX = chunkIndexes[0],
                        minY = chunkIndexes[0],
                        minZ = chunkIndexes[0],
                        maxX = chunkIndexes[0],
                        maxY = chunkIndexes[0],
                        maxZ = chunkIndexes[0];
                    for (var j = 1; j < chunkIndexes.Count; j++)
                    {
                        if (vertices[chunkIndexes[j]].x < vertices[minX].x) minX = chunkIndexes[j];
                        if (vertices[chunkIndexes[j]].y < vertices[minY].y) minY = chunkIndexes[j];
                        if (vertices[chunkIndexes[j]].z < vertices[minZ].z) minZ = chunkIndexes[j];

                        if (vertices[chunkIndexes[j]].x > vertices[maxX].x) maxX = chunkIndexes[j];
                        if (vertices[chunkIndexes[j]].y > vertices[maxY].y) maxY = chunkIndexes[j];
                        if (vertices[chunkIndexes[j]].z > vertices[maxZ].z) maxZ = chunkIndexes[j];
                    }

                    bonesStorageClass.cutIndexClasses[i].chunkAverageIndexes = new List<int> {minX, minY, minZ, maxX, maxY, maxZ};
                }
            }


            /********************************************************************************************************************************/
            /********************************************************************************************************************************/
            // Center Bone

            var centerClass = _goreSimulator.centerBonesClass;
            centerClass.indexesStorageClass.cutMeshes = new List<Mesh>();
            centerClass.indexesStorageClass.cutMeshes.Add(new Mesh());
            var centerBonesStorageClass = new BonesStorageClass();
            _goreSimulator.storage.centerBonesStorageClass = centerBonesStorageClass;
            centerBonesStorageClass.boneName = centerClass.bone.name;

            centerClass.boneIndexes = Enumerable.Range(0, bakedMesh.vertices.Length).ToList();
            for (var i = 0; i < bonesClasses.Count; i++)
            {
                var bonesClass = bonesClasses[i];
                var bonesStorageClass = _goreSimulator.storage.bonesStorageClasses[i];
                if (bonesClass.bone == _goreSimulator.center) continue;

                // Take all indexes and remove all others.
                for (var j = 0; j < bonesStorageClass.cutIndexClasses.Count; j++)
                    centerClass.boneIndexes.RemoveAll(item => bonesStorageClass.cutIndexClasses[j].chunkIndexes.Contains(item));
            }


            CutSideIndexes(centerClass, centerBonesStorageClass);
            ChunkIndexes(centerClass, centerBonesStorageClass);
            Corrections(centerClass, centerBonesStorageClass);
            CompleteParentSide(centerBonesStorageClass);
            CutIndexes(centerClass, centerBonesStorageClass);
            OppositeParentIndexes(centerBonesStorageClass);
            ChunkAverageIndexes(centerBonesStorageClass);

            _goreSimulator.storage.bonesStorageClasses[^1] = centerBonesStorageClass;

            
            
            /********************************************************************************************************************************/
            // InfluencingIndexes
            
            for (var i = 0; i < bonesClasses.Count; i++)
            {
                var progress = (float) i / bonesClasses.Count();
                EditorUtility.DisplayProgressBar("Hold on...", "Initializing mesh parts.", progress);

                var bonesClass = _goreSimulator.bonesClasses[i];
                var bonesStorageClass = _goreSimulator.storage.bonesStorageClasses[i];
                
                InfluencingIndexes(bonesClass, bonesStorageClass);
            }

            void InfluencingIndexes(BonesClass bonesClass, BonesStorageClass bonesStorageClass)
            {
                for (var i = 0; i < bonesStorageClass.cutIndexClasses.Count; i++)
                {
                    var cutIndexClass = bonesStorageClass.cutIndexClasses[i];
                    var influencingBonesInt = MeshCutJobs.GetInfluencingBones(_goreSimulator.smr.sharedMesh, cutIndexClass.chunkIndexes);
                    var influencingBones = influencingBonesInt.Select(t => _goreSimulator.smr.bones[t]).ToList();
                    var removableChildren = new List<int>();

                    if (influencingBonesInt.Count == 0)
                    {
                        Debug.LogWarning("The bone: " + bonesStorageClass.boneName + " should not be used because it has no bone weights.");
                        continue;
                    }
     
                    var smrBones = _goreSimulator.smr.bones;

                    int highestIndex = influencingBonesInt[0];

                    for (var j = 1; j < influencingBonesInt.Count; j++)
                    {
                        var currentIndex = influencingBonesInt[j];
                        var currentBone = smrBones[currentIndex];
                        var highestBone = smrBones[highestIndex];
                        if (highestBone.IsChildOf(currentBone)) highestIndex = currentIndex;
                    }
                    
                    for (;;)
                    {
                        var currentBone =smrBones[highestIndex];
                        if (_goreSimulator.bones.Contains(currentBone) && currentBone != bonesClass.bone) break;
                        if (currentBone.parent == null) break;                        
                        var currentIndex = Array.IndexOf(smrBones, currentBone.parent);
                        if (currentIndex >= 0)
                        {
                            highestIndex = currentIndex;
                        }
                        else break;
                    }
                    
                    
                    TraverseChildren(smrBones[highestIndex]);
                    void TraverseChildren(Transform parent)
                    {
                        foreach (Transform child in parent)
                        {
                            var currentIndex = Array.IndexOf(smrBones, child);
                            if(currentIndex < 0) continue;
                            AddRemovableChildren(currentIndex);
                            TraverseChildren(child);
                        }
                    }
                    
                    void AddRemovableChildren(int boneIndex)
                    {
                        var bone =  smrBones[boneIndex];

                        bool influencedChildExists = false;
                        for (int k = 0; k < influencingBones.Count; k++)
                        {
                            if (influencingBones[k].IsChildOf(bone))
                            {
                                influencedChildExists = true;
                                break;
                            }
                        }
                        if(influencedChildExists) return;
                        var childIndex = Array.IndexOf(smrBones, bone);
                        if (childIndex < 0) return;
                        if (removableChildren.Contains(childIndex)) return;
                        
                        removableChildren.Add(childIndex);
                    }


                    // Only need most upper parents.
                    for (int j = removableChildren.Count - 1; j >= 0; j--)
                    {
                        var nonInfluencedChild = smrBones[removableChildren[j]];
                        for (int k = 0; k < removableChildren.Count; k++)
                        {
                            if(k == j) continue;
                            var nonInfluencedChild2 = smrBones[removableChildren[k]];
                            if (nonInfluencedChild.IsChildOf(nonInfluencedChild2))
                            {
                                removableChildren.RemoveAt(j);
                                break;
                            }
                        }
                    }

                    // Remove children of most upper parents.
                    for (int j = removableChildren.Count - 1; j >= 0; j--)
                    {
                        var nonInfluencedChild = smrBones[removableChildren[j]];
                        if (nonInfluencedChild.IsChildOf(bonesClass.bone))
                        {
                            removableChildren.RemoveAt(j);
                        }
                    }

                    // Active Children
                    List<int> activeBones = new List<int>();
                    activeBones.Add(Array.IndexOf(smrBones, bonesClass.bone));
                    
                    TraverseHierarchyActive(bonesClass.bone);
                    void TraverseHierarchyActive(Transform parent)
                    {
                        foreach (Transform child in parent)
                        {
                            var childIndex = Array.IndexOf(smrBones, child);
                            if(!activeBones.Contains(childIndex) && _goreSimulator.bones.Contains(child)) 
                                activeBones.Add(childIndex);
                            TraverseHierarchyActive(child);
                        }
                    }
                    
                    List<int> nonActiveBones = new List<int>();
                    if(!activeBones.Contains(highestIndex)) nonActiveBones.Add(highestIndex);
                    
                    TraverseHierarchyNonActive(smrBones[highestIndex]);
                    void TraverseHierarchyNonActive(Transform parent)
                    {
                        foreach (Transform child in parent)
                        {
                            var childIndex = Array.IndexOf(smrBones, child);
                            if(removableChildren.Contains(childIndex)) continue;
                            if(!activeBones.Contains(childIndex) && !nonActiveBones.Contains(childIndex) && _goreSimulator.bones.Contains(child)) 
                                nonActiveBones.Add(Array.IndexOf(smrBones, child));
                            TraverseHierarchyNonActive(child);
                        }
                    }
                    
                    cutIndexClass.boneIndex = Array.IndexOf(smrBones, bonesClass.bone);
                    cutIndexClass.activeBones = activeBones;
                    cutIndexClass.nonActiveBones = nonActiveBones;
                    cutIndexClass.detachedParent = highestIndex;
                    cutIndexClass.RemovableChildren = removableChildren;
                }
            }
            

            
            /********************************************************************************************************************************/
            /********************************************************************************************************************************/
            // Cache meshes (Cut - only empty mesh).

            for (var i = 0; i < bonesClasses.Count; i++)
            {
                var bonesClass = bonesClasses[i];
                var bonesStorageClass = _goreSimulator.storage.bonesStorageClasses[i];
                if (bonesStorageClass.cutIndexClasses == null || bonesStorageClass.cutIndexClasses.Count == 0) continue;
                bonesClass.indexesStorageClass.cutMeshes = new List<Mesh>();
                for (var j = 0; j < bonesStorageClass.cutIndexClasses.Count; j++) bonesClass.indexesStorageClass.cutMeshes.Add(new Mesh());
            }

            /********************************************************************************************************************************/
            // Cache meshes (Explosion - full mesh parts, only vertices are moved in the runtime).

            GameObject createMeshesParent = null;
            if (testMeshes) createMeshesParent = new GameObject(_goreSimulator.name + " - Mesh Parts");

            for (var i = 0; i < bonesClasses.Count; i++)
            {
                var progress = (float) i / bonesClasses.Count;
                EditorUtility.DisplayProgressBar("Hold on...", "Caching mesh.", progress);

                var bonesClass = bonesClasses[i];
                var bonesStorageClass = _goreSimulator.storage.bonesStorageClasses[i];

                if (bonesStorageClass.cutIndexClasses == null || bonesStorageClass.cutIndexClasses.Count == 0) continue;
                bonesClass.indexesStorageClass.chunkClasses = new List<ChunkClass>(bonesStorageClass.cutIndexClasses.Count);
                for (var j = 0; j < bonesStorageClass.cutIndexClasses.Count; j++)
                {
                    var explosionMesh = new Mesh();
                    var cutIndexClass = bonesStorageClass.cutIndexClasses[j];
                    var executionCutClass = new ExecutionCutClass();

                    executionCutClass.newIndexes.UnionWith(cutIndexClass.chunkIndexes);

                    if (bonesClass.bone != _goreSimulator.center)
                        executionCutClass.AddExecutionIndexes(cutIndexClass.cutIndexesParentSide, cutIndexClass.sewIndexesParentSide,
                            cutIndexClass.sewTrianglesParentSide);

                    if (cutIndexClass.cutIndexChildClasses != null)
                        for (var k = 0; k < cutIndexClass.cutIndexChildClasses.Count; k++)
                        {
                            var cutChildClass = cutIndexClass.cutIndexChildClasses[k];
                            executionCutClass.AddExecutionIndexes(cutChildClass.cutIndexes, cutChildClass.sewIndexes, cutChildClass.sewTriangles);
                        }


                    MeshCutJobs.IndexesSnapshotCut(_goreSimulator.smr.transform, meshDataClass, meshNativeDataClass, bakedMesh, executionCutClass, explosionMesh, true,
                        out var cutCenters);

                    if (testMeshes)
                    {
                        var detachedObj = ObjectCreationUtility.CreateMeshObjectEditor(_goreSimulator, explosionMesh,
                            bonesClass.bone.name + " - " + j);
                        detachedObj.transform.SetParent(createMeshesParent.transform);
                    }


                    var explosionIndexClasses = new List<ExplosionIndexClass>();
                    foreach (var cutIndexList in executionCutClass.cutIndexes)
                    {
                        var cutIndexes = new List<int>();
                        foreach (var index in cutIndexList)
                            if (meshNativeDataClass.origToNewMapNative.TryGetValue(index, out var newCutIndex))
                                cutIndexes.Add(newCutIndex);
                            else
                                Debug.LogError("Cut Index not existing!");
                        explosionIndexClasses.Add(new ExplosionIndexClass());
                        explosionIndexClasses[^1].cutIndexes = cutIndexes;
                    }

                    for (var k = 0; k < executionCutClass.sewIndexes.Count; k++)
                    {
                        var sewIndexList = executionCutClass.sewIndexes[k];
                        var sewIndexes = new List<int>();
                        foreach (var index in sewIndexList)
                            if (meshNativeDataClass.origToNewMapNative.TryGetValue(index, out var newSewIndex))
                                sewIndexes.Add(newSewIndex);
                            else
                                Debug.LogError("Sew Index not existing!");
                        explosionIndexClasses[k].sewIndexes = sewIndexes;
                    }

                    bonesClass.indexesStorageClass.chunkClasses.Add(new ChunkClass
                    {
                        boneName = bonesClass.bone.name,
                        cutIndexClassIndex = j,
                        mesh = explosionMesh,
                        keys = meshNativeDataClass.origToNewMapNative.GetKeyArray(Allocator.Temp).ToList(),
                        values = meshNativeDataClass.origToNewMapNative.GetValueArray(Allocator.Temp).ToList(),
                        indexClasses = explosionIndexClasses
                    });
                }
            }
            

            /********************************************************************************************************************************/

            // Clear bone indexes to save space.
            for (var i = 0; i < bonesClasses.Count; i++)
            {
                var bonesClass = bonesClasses[i];
                bonesClass.boneIndexes.Clear();
            }


            /********************************************************************************************************************************/
            /********************************************************************************************************************************/

            meshNativeDataClass.DisposeRuntimeMeshData();

            EditorUtility.ClearProgressBar();
            return true;
        }
    }
}