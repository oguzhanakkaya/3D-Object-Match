using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[Serializable]
public struct GoalsStruct
{
    public ItemsEnum item;
    public int goalNumber;
}
[Serializable]
public struct LevelObjectsStruct
{
    public ItemsEnum item;
    public int itemNumber;
}
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelData", order = 0)]
public class LevelData : ScriptableObject
{
    public List<LevelObjectsStruct> levelObjects = new List<LevelObjectsStruct>();
    public List<GoalsStruct> goals = new List<GoalsStruct>();

    public int time;
}