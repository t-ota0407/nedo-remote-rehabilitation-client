using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonDatabase
{
    private static SingletonDatabase instance;

    public string myUserUuid;
    public string myToken;
    public string myUserName;
    public string currentRehabilitationCondition;
    public RehabilitationSaveDataContent loadedSaveData;

    public static SingletonDatabase Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SingletonDatabase();
                instance.loadedSaveData = new(0);
            }
            return instance;
        }
    }
}
