public class SingletonDatabase
{
    private static SingletonDatabase instance;

    public string myUserUuid;
    public string myToken;
    public string myUserName;
    public RehabilitationCondition currentRehabilitationCondition;
    public RehabilitationSaveDataContent loadedSaveData;
    public AvatarType avatarType;

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
