using Elsa.Persistence.Common.Entities;
using Volte.Data.Dapper;

namespace Elsa.Workflows.Persistence.Implementations;

public class VolteStore<TEntity> where TEntity : Entity
{
    public IDbContext Trans { get; private set; }

    public VolteStore(IDbContext DbContext)
    {
        this.Trans = DbContext;
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

    public TEntity? Find(QueryBuilder _qEntity)
    {
        return Trans.SingleOrDefault<TEntity>(_qEntity);
    }
    public IEnumerable<TEntity> FindMany(QueryBuilder _qEntity)
    {
        return Trans.Query<TEntity>(_qEntity);
    }
    public IEnumerable<TEntity> List()
    {
        QueryBuilder _qEntity = QueryBuilder<TEntity>.Builder(Trans);
        _qEntity.Top(10);
        return Trans.Query<TEntity>(_qEntity);
    }

    public bool Delete(QueryBuilder _qEntity)
    {

        return Trans.Delete<TEntity>(_qEntity);
    }

    public int DeleteWhere(QueryBuilder _qEntity)
    {

        Trans.Delete<TEntity>(_qEntity);
        return 1;
    }

    public int DeleteMany(QueryBuilder _qEntity)
    {
        Trans.Delete<TEntity>(_qEntity);
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

    public IEnumerable<TEntity> Query(QueryBuilder _qEntity)
    {
        return Trans.Query<TEntity>(_qEntity);
    }

    public bool AnyAsync(Func<TEntity, bool> predicate)
    {
        return Entities.Values.Any(predicate);
    }
}