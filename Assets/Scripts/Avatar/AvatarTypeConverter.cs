public class AvatarTypeConverter
{
    private const string female1AssetPath = "Prefabs/Avatars/Female_Adult_01 Variant";
    private const string female2AssetPath = "Prefabs/Avatars/Female_Adult_01 Variant";
    private const string male1AssetPath = "Prefabs/Avatars/Male_Adult_02 Variant";
    private const string male2AssetPath = "Prefabs/Avatars/Male_Adult_02 Variant";

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
}
