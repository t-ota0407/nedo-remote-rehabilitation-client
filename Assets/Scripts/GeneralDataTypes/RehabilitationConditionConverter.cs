using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RehabilitationConditionConverter : MonoBehaviour
{
    private const string SIMPLE_STRING = "SIMPLE";
    private const string GAMIFICATION_STRING = "GAMIFICATION";
    private const string COMMUNICATION_STRING = "COMMUNICATION";

    public static string ToString(RehabilitationCondition rehabilitationCondition)
    {
        string stringExpression = SIMPLE_STRING;
        switch (rehabilitationCondition)
        {
            case RehabilitationCondition.SIMPLE:
                stringExpression = SIMPLE_STRING;
                break;
            case RehabilitationCondition.GAMIFICATION:
                stringExpression = GAMIFICATION_STRING;
                break;
            case RehabilitationCondition.COMMUNICATION:
                stringExpression = COMMUNICATION_STRING;
                break;
        }
        return stringExpression;
    }

    public static RehabilitationCondition FromString(string stringExpression)
    {
        RehabilitationCondition rehabilitationCondition = RehabilitationCondition.SIMPLE;
        if (stringExpression.Equals(SIMPLE_STRING))
            rehabilitationCondition = RehabilitationCondition.SIMPLE;
        if (stringExpression.Equals(GAMIFICATION_STRING))
            rehabilitationCondition = RehabilitationCondition.GAMIFICATION;
        if (stringExpression.Equals(COMMUNICATION_STRING))
            rehabilitationCondition = RehabilitationCondition.COMMUNICATION;
        return rehabilitationCondition;
    }
}
