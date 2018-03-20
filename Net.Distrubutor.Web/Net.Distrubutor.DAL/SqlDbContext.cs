using Microsoft.EntityFrameworkCore;
using Net.Distrubutor.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Net.Distrubutor.DAL
{
    public class SqlDbContext : DbContext, IDbContext
    {
        private DbContextOptions<SqlDbContext> conStr;
        private string connectStr;
        public SqlDbContext(DbContextOptions<SqlDbContext> connectString) : base(connectString)
        {
            this.conStr = connectString;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()//load from current assembly
            .Where(type => !string.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(BaseDataModel));
            foreach (var type in typesToRegister)
            {
                //dynamic configurationInstance = Activator.CreateInstance(type);
                builder.Model.AddEntityType(type);
            }

            base.OnModelCreating(builder); 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseMySql(connectStr);                
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public bool ProxyCreationEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AutoDetectChangesEnabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Detach(object entity)
        {
            throw new NotImplementedException();
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
