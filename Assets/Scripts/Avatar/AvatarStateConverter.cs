public class AvatarStateConverter
{
    private const string WALKING_STRING = "WALKING";
    private const string KNIFE_SHARPENING_STRING = "KNIFE_SHARPENING";
    private const string INTERACTING_WITH_UI_STRING = "INTERACTING_WITH_UI";

    public static string ToString(AvatarState avatarState)
    {
        string stringExpression = WALKING_STRING;
        switch (avatarState)
        {
            case AvatarState.Walking:
                stringExpression = WALKING_STRING;
                break;
            case AvatarState.KnifeSharpening:
                stringExpression = KNIFE_SHARPENING_STRING;
                break;
            case AvatarState.InteractingWithUI:
                stringExpression = INTERACTING_WITH_UI_STRING;
                break;
        }
        return stringExpression;
    }

    public static AvatarState FromString(string stringExpression)
    {
        AvatarState avatarState = AvatarState.Walking;

        if (stringExpression.Equals(WALKING_STRING))
            avatarState = AvatarState.Walking;
        if (stringExpression.Equals(KNIFE_SHARPENING_STRING))
            avatarState = AvatarState.KnifeSharpening;
        if (stringExpression.Equals(INTERACTING_WITH_UI_STRING))
            avatarState = AvatarState.InteractingWithUI;

        return avatarState;
    }
}
