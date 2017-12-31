namespace SourcePoint.Data.BaseData.Interface
{
    public interface IBaseEntity
    {

    }

    public interface IBaseEntity<TType>:IBaseEntity
    {
        TType Id { get; set; }
    }
}
