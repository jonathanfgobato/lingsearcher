using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using System.Linq;
using System;
using Lingsearcher.Entity;

namespace Lingsearcher.DAL
{
    public class BaseDAO<T> : IDAO<T> where T : Domain
    {
        //Pegar connection string do json depois
        protected string ConnectionString = "Server=lingsearcher.database.windows.net;Initial Catalog=lingsearcher;MultipleActiveResultSets=true;User ID=gobatoj;Password=wjfo$nfD";
        //protected string ConnectionString = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;

        protected string Name;

        public BaseDAO()
        {
            Name = typeof(T).Name.Replace("DTO", "");
        }

        public virtual T Insert(T entity)
        {
            entity.Id = Convert.ToInt32(Scalar<int>($"Spr_Insert_{Name}", entity.GetProperties()));
            return entity;
        }

        public virtual void Update(T entidade)
        {
            ExecuteProcedure($"Spr_Alterar_{Name}", entidade.GetProperties());
        }

        public virtual int Delete(int id)
        {
            ExecuteProcedure($"Spr_Excluir_{Name}", new { Id = id });
            return id;
        }

        public virtual T GetById(int id)
        {
            return Query($"Spr_Buscar_{Name}_PorId", new { Id = id }).SingleOrDefault();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Query($"Spr_Listar_{Name}");
        }

        protected virtual IEnumerable<T> Query(string procedure, object parameter = null)
        {
            return Query<T>(procedure, parameter);
        }

        protected virtual IEnumerable<U> Query<U>(string procedure, object parameter = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<U>(procedure, parameter, commandType: CommandType.StoredProcedure);
            }
        }

        protected virtual U Scalar<U>(string procedure, object parameter)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    //try
                    //{
                    var item = connection.ExecuteScalar<U>(procedure, parameter, transaction, commandType: CommandType.StoredProcedure);
                    transaction.Commit();

                    return item;
                    //}
                    //catch (SqlException e)
                    //{
                    //transacao.Rollback();
                    //throw ValidarException(e);
                    //}
                }
            }
        }

        protected virtual void ExecuteProcedure(string procedure, object parameter)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                using (var transaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        sqlConnection.Execute(procedure, parameter, transaction, commandType: CommandType.StoredProcedure);
                        transaction.Commit();
                    }
                    catch (SqlException e)
                    {
                        transaction.Rollback();
                        //throw ValidarException(e);
                    }
                }
            }
        }

        /*
        private Exception ValidarException(SqlException ex)
        {
            if (ex.Message.ContainsNormalizado("UNIQUE"))
            {
                var duplicado = new Regex(@"The duplicate key value is \((\w+)\)").Match(ex.Message).Groups[1];
                return new ValidacaoException($"Jรก existe um registro com o nome '{duplicado}'");
            }

            return ex;
        }
        */
    }
}
