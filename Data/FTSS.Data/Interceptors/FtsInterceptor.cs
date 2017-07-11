namespace FTSS.Data.Interceptors
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.Entity.Infrastructure.Interception;
    using System.Text.RegularExpressions;
    using FTSS.Common.Extensions;

    public class FtsInterceptor : IDbCommandInterceptor
    {
        private const string FullTextPrefix = "-FTSPREFIX-";

        public static string Fts(string search)
        {
            return string.Format("({0}{1})", FullTextPrefix, search);
        }

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            this.RewriteFullTextQuery(command);
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            this.RewriteFullTextQuery(command);
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
        }

        private void RewriteFullTextQuery(DbCommand command)
        {
            string text = command.CommandText;
            for (int i = 0; i < command.Parameters.Count; i++)
            {
                DbParameter parameter = command.Parameters[i];
                if (parameter.DbType.In(DbType.String, DbType.AnsiString, DbType.StringFixedLength, DbType.AnsiStringFixedLength))
                {
                    if (parameter.Value == DBNull.Value)
                    {
                        continue;
                    }

                    var value = (string)parameter.Value;
                    if (value.IndexOf(FullTextPrefix) >= 0)
                    {
                        parameter.Size = 4096;
                        parameter.DbType = DbType.AnsiStringFixedLength;
                        value = value.Replace(FullTextPrefix, string.Empty); // remove prefix we added in linq query
                        value = value.Substring(1, value.Length - 2); // remove %% escaping by linq translator from string.Contains to SQL LIKE
                        parameter.Value = value;
                        command.CommandText = Regex.Replace(
                            text,
                            string.Format(
                                @"\[(\w*)\].\[(\w*)\]\s*LIKE\s*@{0}\s?(?:ESCAPE N?'~')",
                                parameter.ParameterName),
                            string.Format(
                                @"contains([$1].[$2], @{0})",
                                parameter.ParameterName));

                        if (text == command.CommandText)
                        {
                            throw new Exception("FTS was not replaced on: " + text);
                        }

                        text = command.CommandText;
                    }
                }
            }
        }
    }
}
