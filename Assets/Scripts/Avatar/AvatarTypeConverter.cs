public class AvatarTypeConverter
{
    private const string female1AssetPath = "Prefabs/Avatars/Female_Adult_01 Variant";
    private const string female2AssetPath = "Prefabs/Avatars/Female_Adult_01 Variant";
    private const string male1AssetPath = "Prefabs/Avatars/Male_Adult_02 Variant";
    private const string male2AssetPath = "Prefabs/Avatars/Male_Adult_02 Variant";

    private const string FEMALE_1_STRING = "AVATAR_FEMALE_1";
    private const string FEMALE_2_STRING = "AVATAR_FEMALE_2";
    private const string MALE_1_STRING = "AVATAR_MALE_1";
    private const string MALE_2_STRING = "AVATAR_MALE_2";

    public static string ToAssetPath(AvatarType avatarType)
    {
        string assetPath = female1AssetPath;
        switch (avatarType)
        {
            case AvatarType.AVATAR_FEMALE_1:
                assetPath = female1AssetPath;
                break;
            case AvatarType.AVATAR_FEMALE_2:
                assetPath = female2AssetPath;
                break;
            case AvatarType.AVATAR_MALE_1:
                assetPath = male1AssetPath;
                break;
            case AvatarType.AVATAR_MALE_2:
                assetPath = male2AssetPath;
                break;
        }
        return assetPath;
    }

    public static string ToString(AvatarType avatarType)
    {
        string stringExpression = FEMALE_1_STRING;
        switch (avatarType)
        {
            case AvatarType.AVATAR_FEMALE_1:
                stringExpression = FEMALE_1_STRING;
                break;
            case AvatarType.AVATAR_FEMALE_2:
                stringExpression = FEMALE_2_STRING;
                break;
            case AvatarType.AVATAR_MALE_1:
                stringExpression = MALE_1_STRING;
                break;
            case AvatarType.AVATAR_MALE_2:
                stringExpression = MALE_2_STRING;
                break;
        }
        return stringExpression;
    }

    public static AvatarType FromString(string stringExpression)
    {
        AvatarType avatarType = AvatarType.AVATAR_FEMALE_1;

        if (stringExpression.Equals(FEMALE_1_STRING))
            avatarType = AvatarType.AVATAR_FEMALE_1;
        if (stringExpression.Equals(FEMALE_2_STRING))
            avatarType = AvatarType.AVATAR_FEMALE_2;
        if (stringExpression.Equals(MALE_1_STRING))
            avatarType = AvatarType.AVATAR_MALE_1;
        if (stringExpression.Equals(MALE_2_STRING))
            avatarType = AvatarType.AVATAR_MALE_2;

        return avatarType;
    }
}
