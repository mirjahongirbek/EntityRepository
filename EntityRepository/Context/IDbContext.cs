using Microsoft.EntityFrameworkCore;


namespace EntityRepository.Context
{
  public  interface IDbContext
    {
        DbContext DataContext { get; }
    }
}
