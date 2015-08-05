using System;
using System.Collections;
using System.Data.SqlClient;

namespace Data.SQLServer
{
    public sealed class DBParameter : IDisposable
    {
        #region [ Atributos ]

        //ATRIBUTO DE RETORNO DE UMA COLEÇÃO DE PARAMETROS INDEPENDENTE DO PROVEDOR
        private ArrayList _parameterCollection;
        //ATRIBUTO QUE ARMAZENA O NOME DO PARAMETRO
        private string _parameterName;
        //ATRIBUTO QUE ARMAZENA O VALOR DO PARAMETRO
        private string _parameterValue;
        //ATRIBUTO QUE ARMAZENA O PARAMETRO DE RETORNO DO COMANDO DE EXECUÇÃO 
        private string _parameterOutput;

        #endregion

        #region [ IDisposable Members ]

        public void Dispose()
        {
            _parameterCollection = null;
        }

        #endregion

        #region [ Construtores ]

        public DBParameter()
        {
            _parameterCollection = new ArrayList();
        }

        public static DBParameter NewConstructor()
        {
            return new DBParameter();
        }

        #endregion

        #region [ Propriedades ]

        public string ParameterName
        {
            get { return _parameterName; }
            set { _parameterName = "@" + value; }
        }

        public string ParameterValue
        {
            get { return _parameterValue; }
            set { _parameterValue = value; }
        }

        public string ParameterOutput
        {
            get { return _parameterOutput; }
            set { _parameterOutput = value; }
        }

        public ArrayList Parameter
        {
            get { return _parameterCollection; }
        }

        #endregion

        #region [ Parâmetros ]

        public void AddParameter(string pName, object pValue, System.Data.SqlDbType pDbType, System.Data.ParameterDirection pDirection, int pSize = 0)
        {
            if (!Valid(pName)) throw new InvalidOperationException("Parâmetro inválido");

            var _parameter = new SqlParameter();

            _parameter.ParameterName = pName;
            _parameter.Value = pValue;
            _parameter.SqlDbType = pDbType;
            _parameter.Direction = pDirection;
            _parameter.Size = pSize > 0 ? pSize : 0;

            _parameterCollection.Add(_parameter);
        }

        private bool Valid(string pParameterName)
        {
            return (!string.IsNullOrEmpty(pParameterName) ? true : false);
        }

        public void Clear()
        {
            _parameterCollection.Clear();
        }

        #endregion
    }
}