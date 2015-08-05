using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.SQLServer.Daos
{
    public abstract class BaseDao<TDto> where TDto : class
    {
        #region [ Atributos ]

        private SqlConnection _Connection;
        private SqlTransaction _Transaction;

        #endregion

        #region [ Propriedades ]

        public SqlConnection Connection
        {
            get { return this._Connection; }
            set { this._Connection = value; }
        }

        public SqlTransaction Transaction
        {
            get { return this._Transaction; }
            set { this._Transaction = value; }
        }

        #endregion

        #region [ Abstratos ]

        #region [ Error Messages ]

        protected internal abstract string ErrorMessageInsert { get; }

        protected internal abstract string ErrorMessageUpdate { get; }

        protected internal abstract string ErrorMessageDelete { get; }

        protected internal abstract string ErrorMessageGet { get; }

        protected internal abstract string ErrorMessageGetByParameters { get; }

        #endregion

        #region [ Command Names ]

        protected internal abstract string CommandInsert { get; }

        protected internal abstract string CommandUpdate { get; }

        protected internal abstract string CommandDelete { get; }

        protected internal abstract string CommandGet { get; }

        protected internal abstract string CommandGetByParameters { get; }

        #endregion

        protected internal abstract TDto Convert(DataRow pRow);

        protected internal abstract void FillParameters(ref TDto pDto, ref DBParameter pParameter, ExecutionTypes pExecutionType);

        #endregion

        #region [ Métodos para Confirmação e Cancelamento de Transações ]

        //TODO: FUTURO...

        #endregion

        #region [ Métodos para Persistência do objeto no banco de dados ]

        /// <summary>
        /// Insere o objeto no banco de dados
        /// </summary>
        /// <param name="pDto"></param>
        /// <param name="pReturnIdentity"></param>
        /// <returns></returns>
        public object Insert(TDto pDto, bool pReturnIdentity = false)
        {
            DBExecution _execution = DBExecution.NewConstructor();
            DBParameter _parameter = DBParameter.NewConstructor();

            try
            {
                this.FillParameters(ref pDto, ref _parameter, ExecutionTypes.Insert);

                if (pReturnIdentity)
                    return _execution.ExecuteScalar(_parameter.Parameter, System.Data.CommandType.StoredProcedure, this.CommandInsert);
                else
                    return _execution.ExecuteNonQuery(_parameter.Parameter, System.Data.CommandType.StoredProcedure, this.CommandInsert);
            }
            catch (Exception ex)
            {
                if (ex is Exception) throw;
                else throw new Exception(this.ErrorMessageInsert);
            }
            finally { _execution.Dispose(); }
        }

        /// <summary>
        /// Atualiza o objeto no banco de dados
        /// </summary>
        /// <param name="pDto"></param>
        /// <returns></returns>
        public bool Update(TDto pDto)
        {
            DBExecution _execution = DBExecution.NewConstructor();
            DBParameter _parameter = DBParameter.NewConstructor();

            try
            {
                this.FillParameters(ref pDto, ref _parameter, ExecutionTypes.Update);

                return _execution.ExecuteNonQuery(_parameter.Parameter, CommandType.StoredProcedure, this.CommandUpdate) > 0;
            }
            catch (Exception ex)
            {
                if (ex is Exception) throw;
                else throw new Exception(this.ErrorMessageUpdate);
            }
            finally { _execution.Dispose(); }
        }

        /// <summary>
        /// "Exclui" o objeto do banco de dados
        /// </summary>
        /// <param name="pDto"></param>
        /// <returns></returns>
        public bool Delete(TDto pDto)
        {
            DBExecution _execution = DBExecution.NewConstructor();
            DBParameter _parameter = DBParameter.NewConstructor();

            try
            {
                this.FillParameters(ref pDto, ref _parameter, ExecutionTypes.Delete);

                return _execution.ExecuteNonQuery(_parameter.Parameter, CommandType.StoredProcedure, this.CommandDelete) > 0;
            }
            catch (Exception ex)
            {
                if (ex is Exception) throw;
                else throw new Exception(this.ErrorMessageDelete);
            }
            finally { _execution.Dispose(); }
        }

        #endregion

        #region [ Métodos públicos para consultas ]

        /// <summary>
        /// Retorna objeto somente pela PK
        /// </summary>
        /// <param name="pDto">Objeto</param>
        public TDto Get(TDto pDto)
        {
            DBExecution _execution = DBExecution.NewConstructor();
            DBParameter _parameter = DBParameter.NewConstructor();

            try
            {
                this.FillParameters(ref pDto, ref _parameter, ExecutionTypes.Get);

                var _dataSet = _execution.ExecuteDataSet(_parameter.Parameter, CommandType.StoredProcedure, this.CommandGet);

                if (_dataSet != null && _dataSet.Tables[0].Rows.Count > 0)
                    return this.Convert(_dataSet.Tables[0].Rows[0]);
                else
                    return null;
            }
            catch (Exception ex)
            {
                if (ex is Exception) throw;
                else throw new Exception(this.ErrorMessageGet);
            }
            finally { _execution.Dispose(); }
        }

        /// <summary>
        /// Retorna objeto aplicando os filtros informados
        /// </summary>
        /// <param name="pDto">Objeto</param>
        public List<TDto> GetByParameters(TDto pDto)
        {
            DBExecution _execution = DBExecution.NewConstructor();
            DBParameter _parameter = DBParameter.NewConstructor();

            try
            {
                this.FillParameters(ref pDto, ref _parameter, ExecutionTypes.GetByParameters);

                var _objDataSet = _execution.ExecuteDataSet(_parameter.Parameter, CommandType.StoredProcedure, this.CommandGetByParameters);

                if (_objDataSet != null && _objDataSet.Tables[0].Rows.Count > 0)
                {
                    var _resultSet = new List<TDto>();

                    foreach (DataRow _objDataRow in _objDataSet.Tables[0].Rows)
                    {
                        _resultSet.Add(this.Convert(_objDataRow));
                    }

                    return _resultSet;
                }
                else
                    return new List<TDto>();
            }
            catch (Exception ex)
            {
                if (ex is Exception) throw;
                else throw new Exception(this.ErrorMessageGetByParameters);
            }
            finally { _execution.Dispose(); }
        }

        #endregion
    }
}

