using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.SQLServer
{
    public sealed class DBExecution : IDisposable
    {
        #region [ Atributos ]

        private static DBExecution _objDBExecution = null;
        private static SqlTransaction _objTransaction = null;
        private static IsolationLevel _objIsolationLevel = IsolationLevel.ReadCommitted;
        private static SqlConnection _objConnection = new SqlConnection();

        private SqlCommand _objSqlCommand = null;

        private static string _configManagerDatabase = null;
        private string _enviroment = string.Empty;
        private string _connectionString = string.Empty;
        private bool _isUsingMock = false;

        private bool _blnConexaoTrafegada = false;
        private bool _blnTransacaoTrafegada = false;

        #endregion

        #region [ Propriedades ]

        //TODO: Descrever
        public SqlTransaction Transaction { get { return _objTransaction; } }

        //TODO: Descrever
        public SqlConnection Connection { get { return _objConnection; } }

        public string ConnectionString
        {
            get
            {
                if (_isUsingMock)
                {
                    return ConfigurationManager.ConnectionStrings["MOCK"].ConnectionString;
                }
                else if (_configManagerDatabase == null)
                {
                    _enviroment = ConfigurationManager.AppSettings["DATABASE"].ToString();

                    return ConfigurationManager.ConnectionStrings[_enviroment].ConnectionString;
                }
                else
                    return ConfigurationManager.ConnectionStrings[_configManagerDatabase].ConnectionString;
            }
        }

        #endregion

        #region [ Construtores ]

        //TODO: Descrever
        private DBExecution() { }

        //TODO: Descrever
        public static DBExecution NewConstructor()
        {
            if (_objDBExecution == null)
            {
                _objDBExecution = new DBExecution();
                _configManagerDatabase = null;
            }

            return _objDBExecution;
        }

        //TODO: Descrever
        private DBExecution(ref SqlConnection _connection)
        {
            _objConnection = _connection;

            ConnectionDatabase();

            _connection = _objConnection;
            _blnConexaoTrafegada = true;
        }

        //TODO: Descrever
        public DBExecution NewConstructor(ref SqlConnection _connection)
        {
            _objConnection = _connection;
            _blnConexaoTrafegada = true;

            return new DBExecution();
        }

        //TODO: Descrever
        private DBExecution(ref SqlConnection Connection, ref SqlTransaction Transaction)
        {
            _objConnection = Connection;
            _objTransaction = Transaction;

            ConnectionDatabase();

            Connection = _objConnection;

            _blnConexaoTrafegada = true;
            _blnTransacaoTrafegada = true;
        }

        //TODO: Descrever
        //public static DBExecution NewConstructor(ref SqlConnection Connection, ref SqlTransaction Transaction)
        //{
        //    if (_objDBExecution == null)
        //        _objDBExecution = new DBExecution(ref Connection, ref Transaction);

        //    return _objDBExecution;
        //}

        public static DBExecution NewConstructor(string _configManager)
        {
            _configManagerDatabase = _configManager;
            return new DBExecution();
        }

        #endregion

        #region [ Conexão ]

        //Cria uma conexão com o banco, independente do comando
        public void ConnectionDatabase()
        {
            ConnectionDatabase(string.Empty);
        }

        //Utiliza uma conexão existente
        public void ConnectionDatabase(SqlConnection Connection)
        {
            ConnectionDatabase(string.Empty, Connection);
        }

        //Executa CommandName na conexão existente
        public void ConnectionDatabase(string CommandName, SqlConnection Connection)
        {
            _objConnection = Connection;

            if (_objConnection == null)
                _objConnection = new SqlConnection();

            if (_objConnection.State == System.Data.ConnectionState.Closed)
                ConnectDatabase(CommandName);
        }

        //Instancia uma conexão com banco informando o comando a ser executado
        public void ConnectionDatabase(string CommandName)
        {
            if (_objConnection == null)
                _objConnection = new SqlConnection();

            if (_objConnection.State == System.Data.ConnectionState.Closed)
                ConnectDatabase(CommandName);
        }

        //Cria conexão com o banco de dados
        private void ConnectDatabase(string _commandName)
        {
            if (_isUsingMock)
            {
                string _mockPath = null;
                if (!string.IsNullOrEmpty(_commandName))
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MOCK_PATH"].ToString()))
                        _mockPath = ConfigurationManager.AppSettings["MOCK_PATH"].ToString();

                    if (!string.IsNullOrEmpty(_mockPath))
                    {
                        AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Path.GetFullPath(_mockPath));
                        string _proceduresPath = System.IO.Path.Combine(_mockPath, "Procedures.mock");
                        string _procedures = string.Empty;
                        System.IO.StreamReader _stream = null;

                        if (System.IO.File.Exists(_proceduresPath))
                        {
                            try
                            {
                            }
                            catch (Exception)
                            {
                                _stream = new System.IO.StreamReader(_proceduresPath);
                                _procedures = _stream.ReadToEnd();
                            }
                            finally
                            {
                                if ((_stream != null))
                                    _stream.Close();
                            }
                        }

                        _isUsingMock = _procedures.Contains(_commandName);
                    }
                }
            }

            try
            {
                if (_objTransaction == null)
                {
                    if (_objConnection.State == ConnectionState.Open)
                        _objConnection.Close();

                    _objConnection.ConnectionString = ConnectionString;
                    _objConnection.Open();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Finaliza conexão com banco de dados
        public void CloseConnection()
        {
            Dispose();
        }

        #endregion

        #region [ Transação ]

        /// <summary>
        /// Inicia uma transação com o banco de dados
        /// </summary>
        public void BeginTransaction()
        {
            try
            {
                if (_objConnection.State == ConnectionState.Closed)
                {
                    _objConnection.ConnectionString = ConnectionString;
                    _objConnection.Open();
                }

                _objTransaction = _objConnection.BeginTransaction(_objIsolationLevel);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao iniciar transação: " + ex.Message);
            }
        }

        /// <summary>
        /// Persiste uma transação aberta com o banco de dados
        /// </summary>
        public void CommitTransaction()
        {
            if (_objConnection != null)
            {
                if (_objConnection.State == System.Data.ConnectionState.Closed)
                    throw new Exception("Nenhuma conexão está aberta para receber commit.");
                else if (_objConnection.State == System.Data.ConnectionState.Broken)
                {
                    _objSqlCommand.Transaction.Rollback();
                    throw new Exception("A conexão com banco de dados foi interrompida.");
                }

                if (_objSqlCommand != null)
                    _objSqlCommand.Transaction.Commit();
                else
                    throw new Exception("Não existe um comando de referência.");
            }
            else
                throw new Exception("Não existe uma conexão de referência");

            _objTransaction = null;
        }

        /// <summary>
        /// Desfaz as operações enviadas por uma transação no banco de dados
        /// </summary>
        public void RollbackTransaction()
        {
            if (_objConnection != null)
            {
                if (_objConnection.State == System.Data.ConnectionState.Closed)
                    throw new Exception("Nenhuma conexão aberta para retroceder os comando de uma transação.");

                if (_objSqlCommand != null && _objSqlCommand.Transaction != null)
                    _objSqlCommand.Transaction.Rollback();
                //TODO: VERIFICAR POSTERIORMENTE
                //else
                //    throw new Exception("Não existe um comando de referência.");
            }
            else
                throw new Exception("Não existe uma conexão de referência");

            _objTransaction = null;
        }

        #endregion

        #region [ Execução ]

        //Executa o comando informado e retorna um SqlDataReader
        public SqlDataReader ExecuteReader(ArrayList cmdParameters, CommandType cmdType, string CommandName)
        {
            try
            {
                ConnectDatabase(CommandName);

                _objSqlCommand = new SqlCommand();
                _objSqlCommand.CommandType = cmdType;
                _objSqlCommand.CommandText = CommandName;
                _objSqlCommand.Connection = _objConnection;
                _objSqlCommand.CommandTimeout = 300;

                if ((_objTransaction != null))
                    _objSqlCommand.Transaction = _objTransaction;

                if ((cmdParameters != null))
                {
                    foreach (SqlParameter sqlParameter in cmdParameters)
                        _objSqlCommand.Parameters.Add(sqlParameter);
                }

                return _objSqlCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { _objSqlCommand = null; }
        }

        //Executa o comando informado e retorna um SqlDataReader
        public SqlDataReader ExecuteReader(ArrayList cmdParameters, CommandType cmdType, string CommandName, SqlConnection Connection, SqlTransaction Transaction)
        {
            try
            {
                _objSqlCommand = new SqlCommand();

                if (Connection == null)
                    ConnectionDatabase(CommandName);
                else
                    ConnectionDatabase(CommandName, Connection);

                _objSqlCommand.CommandType = cmdType;
                _objSqlCommand.CommandText = CommandName;
                _objSqlCommand.CommandTimeout = 100;

                if ((Transaction != null))
                    _objTransaction = Transaction;

                _objSqlCommand.Transaction = _objTransaction;

                foreach (Array Parameters in cmdParameters)
                    _objSqlCommand.Parameters.Add(Parameters);

                return _objSqlCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Executa o comando informado e retorna a primeira coluna da primeira linha
        public object ExecuteScalar(ArrayList pParameters, CommandType pCommandType, string pCommand)
        {
            try
            {
                _objSqlCommand = new SqlCommand();

                if (_objConnection == null || _objConnection.State == ConnectionState.Closed)
                    ConnectDatabase(pCommand);

                _objSqlCommand.CommandType = pCommandType;
                _objSqlCommand.CommandText = pCommand;
                _objSqlCommand.Connection = _objConnection;
                _objSqlCommand.CommandTimeout = 100;

                if ((_objTransaction != null))
                    _objSqlCommand.Transaction = _objTransaction;

                foreach (SqlParameter _param in pParameters)
                    _objSqlCommand.Parameters.Add(_param);

                return _objSqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Executa o comando informado e retorna a primeira coluna da primeira linha
        public object ExecuteScalar(ArrayList cmdParameters, CommandType cmdType, string CommandName, SqlConnection Connection, SqlTransaction Transaction)
        {
            try
            {
                _objSqlCommand = new SqlCommand();

                if (Connection == null)
                    ConnectionDatabase(CommandName);
                else
                    ConnectionDatabase(CommandName, Connection);

                _objSqlCommand.CommandType = cmdType;
                _objSqlCommand.CommandText = CommandName;
                _objSqlCommand.CommandTimeout = 300;

                if ((Transaction != null))
                    _objTransaction = Transaction;

                _objSqlCommand.Transaction = _objTransaction;

                foreach (SqlParameter Parameter in cmdParameters)
                    _objSqlCommand.Parameters.Add(Parameter);

                return _objSqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Executa o comando informado e retorna o número de linhas afetadas
        public int ExecuteNonQuery(ArrayList pParameters, CommandType pCommandType, string pCommand)
        {
            try
            {
                _objSqlCommand = new SqlCommand();

                if (_objConnection == null || _objConnection.State == ConnectionState.Closed)
                    ConnectDatabase(pCommand);

                _objSqlCommand.CommandType = pCommandType;
                _objSqlCommand.CommandText = pCommand;
                _objSqlCommand.Connection = _objConnection;
                _objSqlCommand.CommandTimeout = 300;

                if ((_objTransaction != null))
                    _objSqlCommand.Transaction = _objTransaction;

                foreach (SqlParameter Parameter in pParameters)
                    _objSqlCommand.Parameters.Add(Parameter);

                return _objSqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Executa o comando informado e retorna o número de linhas afetadas
        public int ExecuteNonQuery(ArrayList cmdParameters, CommandType cmdType, string CommandName, SqlConnection Connection, SqlTransaction Transaction)
        {
            try
            {
                _objSqlCommand = new SqlCommand();

                if (Connection == null)
                    ConnectionDatabase(CommandName);
                else
                    ConnectionDatabase(CommandName, Connection);

                _objSqlCommand.CommandType = cmdType;
                _objSqlCommand.CommandText = CommandName;
                _objSqlCommand.CommandTimeout = 300;

                if ((Transaction != null))
                    _objTransaction = Transaction;

                _objSqlCommand.Transaction = _objTransaction;

                foreach (Array Parameter in cmdParameters)
                    _objSqlCommand.Parameters.Add(Parameter);

                return _objSqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Executa o comando informado e devolve um XML
        public System.Xml.XmlReader ExecuteXMLReader(ArrayList cmdParameters, CommandType cmdType, string CommandName)
        {
            try
            {
                _objSqlCommand = new SqlCommand();

                if (_objConnection == null || _objConnection.State == ConnectionState.Closed)
                    ConnectDatabase(CommandName);

                _objSqlCommand.CommandType = cmdType;
                _objSqlCommand.CommandText = CommandName;
                _objSqlCommand.Connection = _objConnection;
                _objSqlCommand.CommandTimeout = 300;

                if ((_objTransaction != null))
                    _objSqlCommand.Transaction = _objTransaction;

                foreach (Array _param in cmdParameters)
                {
                    _objSqlCommand.Parameters.Add(_param);
                }

                return _objSqlCommand.ExecuteXmlReader();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        //Executa o comando informado e devolve um XML
        public System.Xml.XmlReader ExecuteXMLReader(ArrayList cmdParameters, CommandType cmdType, string CommandName, SqlConnection Connection, SqlTransaction Transaction)
        {
            try
            {
                _objSqlCommand = new SqlCommand();

                if (Connection == null)
                    ConnectionDatabase(CommandName);
                else
                    ConnectionDatabase(CommandName, Connection);

                _objSqlCommand.CommandType = cmdType;
                _objSqlCommand.CommandText = CommandName;
                _objSqlCommand.CommandTimeout = 300;

                if ((Transaction != null))
                    _objTransaction = Transaction;

                _objSqlCommand.Transaction = _objTransaction;

                foreach (Array Parameter in cmdParameters)
                    _objSqlCommand.Parameters.Add(Parameter);

                return _objSqlCommand.ExecuteXmlReader();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Executa o comando informado e devolve uma Coleção de XMLs
        public DataSet ExecuteDataSet(ArrayList cmdParameters, CommandType cmdType, string CommandName)
        {
            try
            {
                ConnectDatabase(CommandName);

                _objSqlCommand = new SqlCommand();
                _objSqlCommand.CommandType = cmdType;
                _objSqlCommand.CommandText = CommandName;
                _objSqlCommand.Connection = _objConnection;
                _objSqlCommand.CommandTimeout = 300;

                if ((_objTransaction != null))
                    _objSqlCommand.Transaction = _objTransaction;

                if (cmdParameters != null)
                {
                    foreach (SqlParameter Parameter in cmdParameters)
                        _objSqlCommand.Parameters.Add(Parameter);
                }

                SqlDataAdapter _objSqlAdapter = new SqlDataAdapter(_objSqlCommand);

                DataSet _objDataSet = new DataSet();

                _objSqlAdapter.Fill(_objDataSet);

                return _objDataSet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //Executa o comando informado e devolve uma Coleção de XMLs
        public DataSet ExecuteDataSet(ArrayList cmdParameters, CommandType cmdType, string CommandName, SqlConnection Connection, SqlTransaction Transaction)
        {
            try
            {
                _objSqlCommand = new SqlCommand();

                if (Connection == null)
                    ConnectionDatabase(CommandName);
                else
                    ConnectionDatabase(CommandName, Connection);

                _objSqlCommand.CommandType = cmdType;
                _objSqlCommand.CommandText = CommandName;
                _objSqlCommand.CommandTimeout = 300;

                if ((Transaction != null))
                    _objTransaction = Transaction;

                _objSqlCommand.Transaction = _objTransaction;

                foreach (Array _param in cmdParameters)
                    _objSqlCommand.Parameters.Add(_param);

                SqlDataAdapter _objSqlAdapter = new SqlDataAdapter(_objSqlCommand);

                DataSet _objDataSet = new DataSet();

                _objSqlAdapter.Fill(_objDataSet);

                return _objDataSet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region [ Dispose ]

        //Invoca GarbageCollector
        public void Dispose()
        {
            if (_blnTransacaoTrafegada && _objTransaction != null)
                _objTransaction.Dispose();

            if (_objSqlCommand != null)
                _objSqlCommand.Dispose();

            if (_objConnection != null)
            {
                //_objConnection.Close();
                //_objConnection.Dispose();
            }

            bool x = _blnConexaoTrafegada; //TODO: Verificar uso de _blnConexaoTrafegada!!!

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}