using Elsa.Common.Entities;
using Elsa.Workflows.Persistence.Entities;
using Volte.Data.Dapper;
using Volte.Data.SqlKata;

namespace Elsa.Workflows.Persistence.Implementations;

public class VolteStore<TEntity> where TEntity : Entity
{
    public IDbContext Trans { get; private set; }

    public VolteStore(IDbContext DbContext)
    {
        this.Trans = DbContext;
        this.Trans.Open("master");

    }

    public void Open()
    {
        this.Trans.Open("master");
    }

    private IDictionary<string, TEntity> Entities { get; set; } = new Dictionary<string, TEntity>();
    public void Save(TEntity entity)
    {
        this.Trans.AddNewAsync<TEntity>(entity);
    }

    public void SaveMany(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            this.Trans.AddNewAsync<TEntity>(entity);
        }
    }

    public TEntity? Find(Query _qEntity)
    {
        return Trans.SingleOrDefault<TEntity>(_qEntity).Result;
    }
    public IEnumerable<TEntity> FindMany(Query _qEntity)
    {
        return Trans.QueryAsync<TEntity>(_qEntity).Result;
    }
    public IEnumerable<TEntity> List()
    {
        QueryBuilder _qEntity = QueryBuilder<TEntity>.Builder(Trans);
        _qEntity.Top(10);
        return Trans.Query<TEntity>(_qEntity);
    }

    public bool Delete(Query _qEntity)
    {

        return Trans.DeleteAsync<TEntity>(_qEntity).Result>0;
    }

    public int DeleteWhere(Query _qEntity)
    {
        return Trans.DeleteAsync<TEntity>(_qEntity).Result;
    }

    public int DeleteMany(Query _qEntity)
    {
        Trans.DeleteAsync<TEntity>(_qEntity);
        return 1;
    }

    public int DeleteMany(IEnumerable<TEntity> entities)
    {
        var count = 0;
        var list = entities.ToList();

        foreach (var entity in list)
        {
            count++;
            Trans.Delete<TEntity>(entity);
        }

        return count;
    }

    public IEnumerable<TEntity> Query(Query _qEntity)
    {
        return Trans.QueryAsync<TEntity>(_qEntity).Result;
    }

    public bool AnyAsync(Func<TEntity, bool> predicate)
    {
        return Entities.Values.Any(predicate);
    }
}