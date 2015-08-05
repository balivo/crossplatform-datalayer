using Data.Dtos;
using System;
using System.Data;

namespace Data.SQLServer.Daos
{
    public sealed class CidadeDao : BaseDao<CidadeDto>
    {
        protected internal override string ErrorMessageInsert { get { return "Não foi possível inserir a cidade."; } }

        protected internal override string ErrorMessageUpdate { get { return "Não foi possível atualizar a cidade."; } }

        protected internal override string ErrorMessageDelete { get { return "Não foi possível excluir a cidade."; } }

        protected internal override string ErrorMessageGet { get { return "Não foi possível selecionar a cidade."; } }

        protected internal override string ErrorMessageGetByParameters { get { return "Não foi possível selecionar as cidades."; } }

        protected internal override string CommandInsert { get { return "spCidadeInsert"; } }

        protected internal override string CommandUpdate { get { return "spCidadeUpdate"; } }

        protected internal override string CommandDelete { get { return "spCidadeDelete"; } }

        protected internal override string CommandGet { get { return "spCidadeGet"; } }

        protected internal override string CommandGetByParameters { get { return "spCidadeGetByParameters"; } }

        protected internal override CidadeDto Convert(DataRow pRow)
        {
            try
            {
                if (pRow == null)
                    throw new InvalidOperationException();

                var _return = new CidadeDto();
                _return.CidadeId = int.Parse(pRow["CidadeId"].ToString());
                _return.UnidadeFederacaoId = int.Parse(pRow["UnidadeFederacaoId"].ToString());
                _return.Nome = pRow["Nome"].ToString();
                _return.DDD = pRow.IsNull("DDD") ? null : pRow["DDD"].ToString();
                _return.CEPInicial = pRow.IsNull("CEPInicial") ? null : pRow["CEPInicial"].ToString();
                _return.CEPFinal = pRow.IsNull("CEPFinal") ? null : pRow["CEPFinal"].ToString();
                _return.Classe = pRow.IsNull("Classe") ? null : pRow["Classe"].ToString();

                return _return;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao converter objeto DataRow em CidadeDto. Motivo: " + ex.Message);
            }
        }

        protected internal override void FillParameters(ref CidadeDto pDto, ref DBParameter pParameter, ExecutionTypes pExecutionType)
        {
            try
            {
                switch (pExecutionType)
                {
                    case ExecutionTypes.Get:
                    case ExecutionTypes.Update:
                    case ExecutionTypes.Delete:
                        pParameter.AddParameter("CidadeId", pDto.CidadeId, SqlDbType.Int, ParameterDirection.Input);
                        break;
                }

                switch (pExecutionType)
                {
                    case ExecutionTypes.GetByParameters:
                    case ExecutionTypes.Insert:
                    case ExecutionTypes.Update:
                        if (pDto.UnidadeFederacaoId > 0)
                            pParameter.AddParameter("UnidadeFederacaoId", pDto.UnidadeFederacaoId, SqlDbType.Int, ParameterDirection.Input);

                        if (!string.IsNullOrWhiteSpace(pDto.Nome))
                            pParameter.AddParameter("Nome", pDto.Nome, SqlDbType.VarChar, ParameterDirection.Input, 255);

                        if (!string.IsNullOrWhiteSpace(pDto.DDD))
                            pParameter.AddParameter("DDD", pDto.DDD, SqlDbType.VarChar, ParameterDirection.Input, 255);

                        if (!string.IsNullOrWhiteSpace(pDto.CEPInicial))
                            pParameter.AddParameter("CEPInicial", pDto.CEPInicial, SqlDbType.VarChar, ParameterDirection.Input, 255);

                        if (!string.IsNullOrWhiteSpace(pDto.CEPFinal))
                            pParameter.AddParameter("CEPFinal", pDto.CEPFinal, SqlDbType.VarChar, ParameterDirection.Input, 255);

                        if (!string.IsNullOrWhiteSpace(pDto.Classe))
                            pParameter.AddParameter("Classe", pDto.Classe, SqlDbType.VarChar, ParameterDirection.Input, 255);

                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao preencher parametros do objeto Cidade. Motivo: " + ex.Message);
            }
        }
    }
}