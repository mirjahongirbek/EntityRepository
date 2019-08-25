using LiteDB;

namespace LiteRepository.Context
{
    public interface ILiteContext
    {
         LiteDatabase Database { get; }
    }
}
