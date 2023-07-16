public class ReleasedFacility
{
    public readonly FacilityType facilityType;
    public int amount;

    public ReleasedFacility(FacilityType facilityType, int amount)
    {
        this.facilityType = facilityType;
        this.amount = amount;
    }
}
