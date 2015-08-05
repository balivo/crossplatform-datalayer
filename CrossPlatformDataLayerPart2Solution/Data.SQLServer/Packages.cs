namespace Data.SQLServer
{
    /// <summary>
    /// Tipos de Transações
    /// </summary>
    public enum TransactionTypes
    {
        Commit,
        None
    }

    /// <summary>
    /// Tipos de comandos de Execução
    /// </summary>
    public enum ExecutionTypes
    {
        Insert,
        Update,
        Delete,
        Get,
        GetByParameters,
    }
}