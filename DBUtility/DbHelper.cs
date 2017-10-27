using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
/*
*/

namespace DBUtility {
	public abstract class DbHelper {
		#region private utility methods & constructors

		/// <summary>
		/// 附加参数
		/// </summary>
		/// <param name="command">执行命令</param>
		/// <param name="commandParameters">参数</param>
		private void AttachParameters(IDbCommand command, IDbDataParameter[] commandParameters) {
			if (command == null) throw new ArgumentNullException("command");
			if (commandParameters != null) {
				foreach (IDbDataParameter p in commandParameters) {
					if (p != null) {
						if ((p.Direction == ParameterDirection.InputOutput ||
							p.Direction == ParameterDirection.Input) &&
							(p.Value == null)) {
							p.Value = DBNull.Value;
						}
						command.Parameters.Add(p);
					}
				}
			}
		}

		/// <summary>
		/// 准备命令
		/// </summary>
		/// <param name="command">执行命令</param>
		/// <param name="connection">数据库连接语句</param>
		/// <param name="transaction">事务处理</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <param name="mustCloseConnection">是否关闭连接</param>
		private void PrepareCommand(IDbCommand command, IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDbDataParameter[] commandParameters, out bool mustCloseConnection) {
			if (command == null) throw new ArgumentNullException("command");
			if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");
			if (connection.State != ConnectionState.Open) {
				mustCloseConnection = true;
				connection.Open();
			}
			else {
				mustCloseConnection = false;
			}
			command.Connection = connection;
			command.CommandText = commandText;
			if (transaction != null) {
				if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
				command.Transaction = transaction;
			}
			command.CommandType = commandType;
			if (commandParameters != null) {
				AttachParameters(command, commandParameters);
			}
			return;
		}

		#endregion private utility methods & constructors

		#region ExecuteNonQuery


		/// <summary>
		/// ExecuteNonQuery
		/// </summary>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <returns></returns>
		public int ExecuteNonQuery(CommandType commandType, string commandText) {
			return ExecuteNonQuery(commandType, commandText, (IDbDataParameter[])null);
		}

		/// <summary>
		/// ExecuteNonQuery
		/// </summary>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <returns></returns>
		public int ExecuteNonQuery(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters) {
			if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("connectionString");
			using (IDbConnection connection = CreateConnection()) {
				connection.Open();
				return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
			}
		}

		/// <summary>
		/// ExecuteNonQuery
		/// </summary>
		/// <param name="connection">数据库连接语句</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <returns></returns>
		public int ExecuteNonQuery(IDbConnection connection, CommandType commandType, string commandText) {
			return ExecuteNonQuery(connection, commandType, commandText, (IDbDataParameter[])null);
		}

		/// <summary>
		/// ExecuteNonQuery
		/// </summary>
		/// <param name="connection">数据库连接语句</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <returns></returns>
		public int ExecuteNonQuery(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters) {
			if (connection == null) throw new ArgumentNullException("connection");
			IDbCommand cmd = CreateCommand();
			bool mustCloseConnection = false;
			PrepareCommand(cmd, connection, (IDbTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);
			int retval = cmd.ExecuteNonQuery();
			cmd.Parameters.Clear();
			if (mustCloseConnection)
				connection.Close();
			return retval;
		}
        /// <summary>
        /// 直接执行SQL插入
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(List<string> strsql)
        {
            int i = 0;
            using (IDbConnection conn = CreateConnection())
            {
                conn.Open();
                IDbTransaction trans = conn.BeginTransaction();
                try
                {
                    foreach (string item in strsql)
                    {
                        ExecuteNonQuery(trans, CommandType.Text, item);
                    }
                    trans.Commit(); // 提交数据库事务
                    i = 1;
                }
                catch (Exception ex)
                {
                    i = 0;
                    trans.Rollback(); // 回滚
                }
            }
            return i;
        }

		/// <summary>
		/// ExecuteNonQuery
		/// </summary>
		/// <param name="transaction">事务处理</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <returns></returns>
		public int ExecuteNonQuery(IDbTransaction transaction, CommandType commandType, string commandText) {
			return ExecuteNonQuery(transaction, commandType, commandText, (IDbDataParameter[])null);
		}

		/// <summary>
		/// ExecuteNonQuery
		/// </summary>
		/// <param name="transaction">事务处理</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <returns></returns>
		public int ExecuteNonQuery(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters) {
			if (transaction == null) throw new ArgumentNullException("transaction");
			if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
			IDbCommand cmd = CreateCommand();
			bool mustCloseConnection = false;
			PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
			int retval = cmd.ExecuteNonQuery();
			cmd.Parameters.Clear();
			return retval;
		}
        /// <summary>
        /// ExecuteNonQuery 通过事务的方式提交数据。
        /// </summary>
        /// <param name="ListSql">执行的SQL语句</param>
        /// <param name="ErrorStr">如果有错误，系统将输出错误字符串</param>
        /// <returns></returns>
        public int ExecuteNonQuery(List<string> ListSql,out string ErrorStr) 
        {
            int i = 0;
            ErrorStr = string.Empty;
            using (IDbConnection conn = CreateConnection())
            {
                conn.Open();
                IDbTransaction trans = conn.BeginTransaction();
                try
                {
                    foreach (var item in ListSql)
                    {
                        ExecuteNonQuery(trans, CommandType.Text, item);
                    }
                    trans.Commit(); //提交事务
                    i = 1;
                }
                catch(Exception ex)
                {
                    i = 0;
                    trans.Rollback(); // 回滚
                    ErrorStr = ex.Message;
                }
            }
            return i;
        }

		#endregion ExecuteNonQuery

		#region ExecuteDataset

		/// <summary>
		/// ExecuteDataset
		/// </summary>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <returns></returns>
		public DataSet ExecuteDataset(CommandType commandType, string commandText, string tableName = "Table") {
			return ExecuteDataset(commandType, commandText, tableName, (IDbDataParameter[])null);
		}

		/// <summary>
		/// ExecuteDataset
		/// </summary>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <returns></returns>
		public DataSet ExecuteDataset(CommandType commandType, string commandText, string tableName = "Table", params IDbDataParameter[] commandParameters) {
			if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("connectionString");
			using (IDbConnection connection = CreateConnection()) {
				connection.Open();
				return ExecuteDataset(connection, commandType, commandText, tableName, commandParameters);
			}
		}

		/// <summary>
		/// ExecuteDataset
		/// </summary>
		/// <param name="connection">数据库连接语句</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <returns></returns>
		public DataSet ExecuteDataset(IDbConnection connection, CommandType commandType, string commandText, string tableName = "Table") {
			return ExecuteDataset(connection, commandType, commandText, tableName, (IDbDataParameter[])null);
		}

		/// <summary>
		/// ExecuteDataset
		/// </summary>
		/// <param name="connection">数据库连接语句</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <returns></returns>
		public DataSet ExecuteDataset(IDbConnection connection, CommandType commandType, string commandText, string tableName = "Table", params IDbDataParameter[] commandParameters) {
			if (connection == null) throw new ArgumentNullException("connection");
			IDbCommand cmd = CreateCommand();
			bool mustCloseConnection = false;
			PrepareCommand(cmd, connection, (IDbTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);
			using (DbDataAdapter da = CreateDataAdapter(cmd)) {
				DataSet ds = new DataSet();
				da.Fill(ds, tableName);
				cmd.Parameters.Clear();
				if (mustCloseConnection)
					connection.Close();
				return ds;
			}
		}

		/// <summary>
		/// ExecuteDataset
		/// </summary>
		/// <param name="transaction">事务处理</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <returns></returns>
		public DataSet ExecuteDataset(IDbTransaction transaction, CommandType commandType, string commandText, string tableName = "Table") {
			return ExecuteDataset(transaction, commandType, commandText, tableName, (IDbDataParameter[])null);
		}

		/// <summary>
		/// ExecuteDataset
		/// </summary>
		/// <param name="transaction">事务处理</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <returns></returns>
		public DataSet ExecuteDataset(IDbTransaction transaction, CommandType commandType, string commandText, string tableName = "Table", params IDbDataParameter[] commandParameters) {
			if (transaction == null) throw new ArgumentNullException("transaction");
			if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
			IDbCommand cmd = CreateCommand();
			bool mustCloseConnection = false;
			PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
			using (DbDataAdapter da = CreateDataAdapter(cmd)) {
				DataSet ds = new DataSet();
				da.Fill(ds, tableName);
				cmd.Parameters.Clear();
				return ds;
			}
		}

		#endregion ExecuteDataset

		#region ExecuteDataTable


		/// <summary>
		/// ExecuteDataTable
		/// </summary>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <returns></returns>
		public DataTable ExecuteDataTable(CommandType commandType, string commandText, string tableName = "Table") {
			return ExecuteDataTable(commandType, commandText, tableName, (IDbDataParameter[])null);
		}


		/// <summary>
		/// ExecuteDataTable
		/// </summary>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <returns></returns>
		public DataTable ExecuteDataTable(CommandType commandType, string commandText, string tableName = "Table", params IDbDataParameter[] commandParameters) {
			if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("connectionString");
			using (IDbConnection connection = CreateConnection()) {
				connection.Open();
				return ExecuteDataTable(connection, commandType, commandText, tableName, commandParameters);
			}
		}

		/// <summary>
		/// ExecuteDataTable
		/// </summary>
		/// <param name="connection">数据库连接语句</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <returns></returns>
		public DataTable ExecuteDataTable(IDbConnection connection, CommandType commandType, string commandText, string tableName = "Table") {
			return ExecuteDataTable(connection, commandType, commandText, tableName, (IDbDataParameter[])null);
		}


		/// <summary>
		/// ExecuteDataTable
		/// </summary>
		/// <param name="connection">数据库连接语句</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <returns></returns>
		public DataTable ExecuteDataTable(IDbConnection connection, CommandType commandType, string commandText, string tableName = "Table", params IDbDataParameter[] commandParameters) {
			if (connection == null) throw new ArgumentNullException("connection");
			IDbCommand cmd = CreateCommand();
			bool mustCloseConnection = false;
			PrepareCommand(cmd, connection, (IDbTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);
			using (DbDataAdapter da = CreateDataAdapter(cmd)) {
				DataTable dt = new DataTable(tableName);
				da.Fill(dt);
				cmd.Parameters.Clear();
				if (mustCloseConnection)
					connection.Close();
				return dt;
			}
		}

		/// <summary>
		/// ExecuteDataTable
		/// </summary>
		/// <param name="transaction">事务处理</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <returns></returns>
		public DataTable ExecuteDataTable(IDbTransaction transaction, CommandType commandType, string commandText, string tableName = "Table") {
			return ExecuteDataTable(transaction, commandType, commandText, tableName, (IDbDataParameter[])null);
		}

		/// <summary>
		/// ExecuteDataTable
		/// </summary>
		/// <param name="transaction">事务处理</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <returns></returns>
		public DataTable ExecuteDataTable(IDbTransaction transaction, CommandType commandType, string commandText, string tableName = "Table", params IDbDataParameter[] commandParameters) {
			if (transaction == null) throw new ArgumentNullException("transaction");
			if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
			IDbCommand cmd = CreateCommand();
			bool mustCloseConnection = false;
			PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
			using (DbDataAdapter da = CreateDataAdapter(cmd)) {
				DataTable dt = new DataTable(tableName);
				da.Fill(dt);
				cmd.Parameters.Clear();
				return dt;
			}
		}

		#endregion ExecuteDataTable

		#region ExecuteDataView


		/// <summary>
		/// ExecuteDataView
		/// </summary>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <returns></returns>
		public DataView ExecuteDataView(CommandType commandType, string commandText) {
			return ExecuteDataView(commandType, commandText, (IDbDataParameter[])null);
		}


		/// <summary>
		/// ExecuteDataView
		/// </summary>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <returns></returns>
		public DataView ExecuteDataView(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters) {
			if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("connectionString");
			using (IDbConnection connection = CreateConnection()) {
				connection.Open();
				return ExecuteDataView(connection, commandType, commandText, commandParameters);
			}
		}


		/// <summary>
		/// ExecuteDataView
		/// </summary>
		/// <param name="connection">数据库连接语句</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <returns></returns>
		public DataView ExecuteDataView(IDbConnection connection, CommandType commandType, string commandText) {
			return ExecuteDataView(connection, commandType, commandText, (IDbDataParameter[])null);
		}


		/// <summary>
		/// ExecuteDataView
		/// </summary>
		/// <param name="connection">数据库连接语句</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <returns></returns>
		public DataView ExecuteDataView(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters) {
			if (connection == null) throw new ArgumentNullException("connection");
			IDbCommand cmd = CreateCommand();
			bool mustCloseConnection = false;
			PrepareCommand(cmd, connection, (IDbTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);
			using (DbDataAdapter da = CreateDataAdapter(cmd)) {
				DataView dv = new DataView();
				DataSet ds = new DataSet();
				da.Fill(ds, "ds");
				dv = ds.Tables[0].DefaultView;
				cmd.Parameters.Clear();
				if (mustCloseConnection)
					connection.Close();
				return dv;
			}
		}

		/// <summary>
		/// ExecuteDataView
		/// </summary>
		/// <param name="transaction">事务处理</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <returns></returns>
		public DataView ExecuteDataView(IDbTransaction transaction, CommandType commandType, string commandText) {
			return ExecuteDataView(transaction, commandType, commandText, (IDbDataParameter[])null);
		}


		/// <summary>
		/// ExecuteDataView
		/// </summary>
		/// <param name="transaction">事务处理</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <returns></returns>
		public DataView ExecuteDataView(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters) {
			if (transaction == null) throw new ArgumentNullException("transaction");
			if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
			IDbCommand cmd = CreateCommand();
			bool mustCloseConnection = false;
			PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
			using (DbDataAdapter da = CreateDataAdapter(cmd)) {
				DataView dv = new DataView();
				DataSet ds = new DataSet();
				da.Fill(ds, "ds");
				dv = ds.Tables[0].DefaultView;
				cmd.Parameters.Clear();
				return dv;
			}
		}

		#endregion ExecuteDataView

		#region ExecuteReader

		private enum DbConnectionOwnership {
			Internal,
			External
		}

		/// <summary>
		/// ExecuteReader
		/// </summary>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <returns></returns>
		public IDataReader ExecuteReader(CommandType commandType, string commandText) {
			return ExecuteReader(commandType, commandText, (IDbDataParameter[])null);
		}

		/// <summary>
		/// ExecuteReader
		/// </summary>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <returns></returns>
		public IDataReader ExecuteReader(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters) {
			if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("connectionString");
			IDbConnection connection = null;
			try {
				connection = CreateConnection();
				connection.Open();
				return ExecuteReader(connection, null, commandType, commandText, commandParameters, DbConnectionOwnership.Internal);
			}
			catch {
				if (connection != null) connection.Close();
				throw;
			}
		}

		/// <summary>
		/// ExecuteReader
		/// </summary>
		/// <param name="connection">数据库连接语句</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <returns></returns>
		public IDataReader ExecuteReader(IDbConnection connection, CommandType commandType, string commandText) {
			return ExecuteReader(connection, commandType, commandText, (IDbDataParameter[])null);
		}

		/// <summary>
		/// ExecuteReader
		/// </summary>
		/// <param name="connection">数据库连接语句</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <returns></returns>
		public IDataReader ExecuteReader(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters) {
			return ExecuteReader(connection, (IDbTransaction)null, commandType, commandText, commandParameters, DbConnectionOwnership.External);
		}

		/// <summary>
		/// ExecuteReader
		/// </summary>
		/// <param name="connection">数据库连接语句</param>
		/// <param name="transaction">事务处理</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <param name="connectionOwnership"></param>
		/// <returns></returns>
		private IDataReader ExecuteReader(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDbDataParameter[] commandParameters, DbConnectionOwnership connectionOwnership) {
			if (connection == null) throw new ArgumentNullException("connection");
			bool mustCloseConnection = false;
			IDbCommand cmd = CreateCommand();
			try {
				PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
				IDataReader dataReader;
				if (connectionOwnership == DbConnectionOwnership.External) {
					dataReader = cmd.ExecuteReader();
				}
				else {
					dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				}
				bool canClear = true;
				foreach (IDbDataParameter commandParameter in cmd.Parameters) {
					if (commandParameter.Direction != ParameterDirection.Input)
						canClear = false;
				}
				if (canClear) {
					cmd.Parameters.Clear();
				}
				return dataReader;
			}
			catch {
				if (mustCloseConnection)
					connection.Close();
				throw;
			}
		}

		/// <summary>
		/// ExecuteReader
		/// </summary>
		/// <param name="transaction">事务处理</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <returns></returns>
		public IDataReader ExecuteReader(IDbTransaction transaction, CommandType commandType, string commandText) {
			return ExecuteReader(transaction, commandType, commandText, (IDbDataParameter[])null);
		}

		/// <summary>
		/// ExecuteReader
		/// </summary>
		/// <param name="transaction">事务处理</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <returns></returns>
		public IDataReader ExecuteReader(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters) {
			if (transaction == null) throw new ArgumentNullException("transaction");
			if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
			return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, DbConnectionOwnership.External);
		}
		#endregion ExecuteReader

		#region ExecuteScalar
		/// <summary>
		/// ExecuteScalar
		/// </summary>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <returns></returns>
		public object ExecuteScalar(CommandType commandType, string commandText) {
			return ExecuteScalar(commandType, commandText, (IDbDataParameter[])null);
		}

		/// <summary>
		/// ExecuteScalar
		/// </summary>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <returns></returns>
		public object ExecuteScalar(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters) {
			if (ConnectionString == null || ConnectionString.Length == 0) throw new ArgumentNullException("connectionString");
			using (IDbConnection connection = CreateConnection()) {
				connection.Open();
				return ExecuteScalar(connection, commandType, commandText, commandParameters);
			}
		}

		/// <summary>
		/// ExecuteScalar
		/// </summary>
		/// <param name="connection">数据库连接语句</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <returns></returns>
		public object ExecuteScalar(IDbConnection connection, CommandType commandType, string commandText) {
			return ExecuteScalar(connection, commandType, commandText, (IDbDataParameter[])null);
		}

		/// <summary>
		/// ExecuteScalar
		/// </summary>
		/// <param name="connection">数据库连接语句</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <returns></returns>
		public object ExecuteScalar(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters) {
			if (connection == null) throw new ArgumentNullException("connection");
			IDbCommand cmd = CreateCommand();
			bool mustCloseConnection = false;
			PrepareCommand(cmd, connection, (IDbTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);
			object retval = cmd.ExecuteScalar();
			cmd.Parameters.Clear();
			if (mustCloseConnection)
				connection.Close();
			return retval;
		}

		/// <summary>
		/// ExecuteScalar
		/// </summary>
		/// <param name="transaction">事务处理</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <returns></returns>
		public object ExecuteScalar(IDbTransaction transaction, CommandType commandType, string commandText) {
			return ExecuteScalar(transaction, commandType, commandText, (IDbDataParameter[])null);
		}

		/// <summary>
		/// ExecuteScalar
		/// </summary>
		/// <param name="transaction">事务处理</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		/// <returns></returns>
		public object ExecuteScalar(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters) {
			if (transaction == null) throw new ArgumentNullException("transaction");
			if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
			IDbCommand cmd = CreateCommand();
			bool mustCloseConnection = false;
			PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
			object retval = cmd.ExecuteScalar();
			cmd.Parameters.Clear();
			return retval;
		}
		#endregion ExecuteScalar

		#region FillDataset
		/// <summary>
		/// FillDataset
		/// </summary>
		/// <param name="connectionString">数据库连接语句</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="dataSet">数据集</param>
		/// <param name="tableNames">表名</param>
		public void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames) {
			if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
			if (dataSet == null) throw new ArgumentNullException("dataSet");
			using (IDbConnection connection = CreateConnection()) {
				connection.Open();
				FillDataset(connection, commandType, commandText, dataSet, tableNames);
			}
		}

		/// <summary>
		/// FillDataset
		/// </summary>
		/// <param name="connectionString">数据库连接语句</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="dataSet">数据集</param>
		/// <param name="tableNames">表名</param>
		/// <param name="commandParameters">参数</param>
		public void FillDataset(string connectionString, CommandType commandType,
			string commandText, DataSet dataSet, string[] tableNames,
			params IDbDataParameter[] commandParameters) {
			if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
			if (dataSet == null) throw new ArgumentNullException("dataSet");
			using (IDbConnection connection = CreateConnection()) {
				connection.Open();
				FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
			}
		}

		/// <summary>
		/// FillDataset
		/// </summary>
		/// <param name="connection">数据库连接语句</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="dataSet">数据集</param>
		/// <param name="tableNames">表名</param>
		public void FillDataset(IDbConnection connection, CommandType commandType,
			string commandText, DataSet dataSet, string[] tableNames) {
			FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
		}

		/// <summary>
		/// FillDataset
		/// </summary>
		/// <param name="connection">数据库连接语句</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="dataSet">数据集</param>
		/// <param name="tableNames">表名</param>
		/// <param name="commandParameters">参数</param>
		public void FillDataset(IDbConnection connection, CommandType commandType,
			string commandText, DataSet dataSet, string[] tableNames,
			params IDbDataParameter[] commandParameters) {
			FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
		}

		/// <summary>
		/// FillDataset
		/// </summary>
		/// <param name="transaction">事务处理</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="dataSet">数据集</param>
		/// <param name="tableNames">表名</param>
		public void FillDataset(IDbTransaction transaction, CommandType commandType,
			string commandText,
			DataSet dataSet, string[] tableNames) {
			FillDataset(transaction, commandType, commandText, dataSet, tableNames, null);
		}

		/// <summary>
		/// FillDataset
		/// </summary>
		/// <param name="transaction">事务处理</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="dataSet">数据集</param>
		/// <param name="tableNames">表名</param>
		/// <param name="commandParameters">参数</param>
		public void FillDataset(IDbTransaction transaction, CommandType commandType,
			string commandText, DataSet dataSet, string[] tableNames,
			params IDbDataParameter[] commandParameters) {
			FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
		}

		/// <summary>
		/// FillDataset
		/// </summary>
		/// <param name="connection">数据库连接语句</param>
		/// <param name="transaction">事务处理</param>
		/// <param name="commandType">命令类型</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="dataSet">数据集</param>
		/// <param name="tableNames">表名</param>
		/// <param name="commandParameters">参数</param>
		private void FillDataset(IDbConnection connection, IDbTransaction transaction, CommandType commandType,
			string commandText, DataSet dataSet, string[] tableNames,
			params IDbDataParameter[] commandParameters) {
			if (connection == null) throw new ArgumentNullException("connection");
			if (dataSet == null) throw new ArgumentNullException("dataSet");
			IDbCommand command = CreateCommand();
			bool mustCloseConnection = false;
			PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);
			using (DbDataAdapter dataAdapter = CreateDataAdapter(command)) {
				if (tableNames != null && tableNames.Length > 0) {
					string tableName = "Table";
					for (int index = 0; index < tableNames.Length; index++) {
						if (tableNames[index] == null || tableNames[index].Length == 0) throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
						dataAdapter.TableMappings.Add(tableName, tableNames[index]);
						tableName += (index + 1).ToString();
					}
				}
				dataAdapter.Fill(dataSet);
				command.Parameters.Clear();
			}
			if (mustCloseConnection)
				connection.Close();
		}
		#endregion

		#region UpdateDataset
		/// <summary>
		/// UpdateDataset
		/// </summary>
		/// <param name="insertCommand">新增命令</param>
		/// <param name="deleteCommand">删除命令</param>
		/// <param name="updateCommand">更新命令</param>
		/// <param name="dataSet">数据集</param>
		/// <param name="tableNames">表名</param>
		public void UpdateDataset(IDbCommand insertCommand, IDbCommand deleteCommand, IDbCommand updateCommand, DataSet dataSet, string tableName) {
			if (insertCommand == null) throw new ArgumentNullException("insertCommand");
			if (deleteCommand == null) throw new ArgumentNullException("deleteCommand");
			if (updateCommand == null) throw new ArgumentNullException("updateCommand");
			if (tableName == null || tableName.Length == 0) throw new ArgumentNullException("tableName");
			using (DbDataAdapter dataAdapter = CreateDataAdapter(null,
				insertCommand, deleteCommand, updateCommand)) {
				dataAdapter.Update(dataSet, tableName);
				dataSet.AcceptChanges();
			}
		}

		#endregion
		/// <summary>
		/// 创建一个DbHelper的子类的实例，通过反射类名创建
		/// </summary>
		/// <param name="className">不区分大小写，可选值：Advantage、Asa、Ase、DB2、Firebird、Mimer、MySql、NexusDB、OleDb、Oracle、
		/// PervasiveSql、PostgreSql、SQLite、SqlServer、SqlServerCe、Teradata、VistaDB</param>
		public static DbHelper Create(string className) {
			Type type = Type.GetType("DBUtility." + className, true, true);
			ConstructorInfo ci = type.GetConstructor(new Type[] { });
			return (DbHelper)ci.Invoke(null);
		}

		/// <summary>
		/// 获取或设置数据库连接
		/// </summary>
		private string connectionString;

		public string ConnectionString {
			get { return connectionString; }
			set { connectionString = value; }
		}

		/// <summary>
		/// 创建IDbConnection的实例
		/// </summary>
		public abstract IDbConnection CreateConnection();

		/// <summary>
		/// 创建IDbCommand的实例
		/// </summary>
		public abstract IDbCommand CreateCommand();

		/// <summary>
		/// 创建DbDataAdapter的实例
		/// </summary>
		public abstract DbDataAdapter CreateDataAdapter(IDbCommand selectCommand,
			IDbCommand insertCommand, IDbCommand deleteCommand, IDbCommand updateCommand);

		/// <summary>
		/// 创建DbDataAdapter的实例
		/// </summary>
		public DbDataAdapter CreateDataAdapter(IDbCommand cmd) {
			DbDataAdapter adapter = CreateDataAdapter(cmd, null, null, null);
			return adapter;
		}
		#region CreateDbParameter

		/// <summary>
		/// 创建IDbDataParameter的实例
		/// </summary>
		public abstract IDbDataParameter CreateDbParameter(string parameterName);

		/// <summary>
		/// 创建IDbDataParameter的实例
		/// </summary>
		public IDbDataParameter CreateDbParameter(string parameterName, ParameterDirection direction, object value) {
			IDbDataParameter param = CreateDbParameter(parameterName);
			param.Direction = direction;
			param.Value = value;
			return param;
		}

		/// <summary>
		/// 创建IDbDataParameter的实例
		/// </summary>
		public IDbDataParameter CreateDbParameter(string parameterName, DbType dbType, ParameterDirection direction, object value) {
			IDbDataParameter param = CreateDbParameter(parameterName);
			param.DbType = dbType;
			param.Direction = direction;
			param.Value = value;
			return param;
		}

		/// <summary>
		/// 创建IDbDataParameter的实例
		/// </summary>
		public IDbDataParameter CreateDbParameter(string parameterName, DbType dbType, ParameterDirection direction) {
			return CreateDbParameter(parameterName, dbType, 0, direction);
		}

		/// <summary>
		/// 创建IDbDataParameter的实例
		/// </summary>
		public IDbDataParameter CreateDbParameter(string parameterName, DbType dbType, int size, ParameterDirection direction) {
			IDbDataParameter param = CreateDbParameter(parameterName);
			param.DbType = dbType;
			if (size > 0) param.Size = size;
			param.Direction = direction;
			return param;
		}

		#endregion
	}

	/// <summary>
	/// DbParameterCache
	/// </summary>
	public sealed class DbParameterCache {
		#region private methods, variables, and constructors

		private DbParameterCache() { }
		private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

		/// <summary>
		/// 
		/// </summary>
		/// <param name="originalParameters"></param>
		/// <returns></returns>
		private static IDbDataParameter[] CloneParameters(IDbDataParameter[] originalParameters) {
			IDbDataParameter[] clonedParameters = new IDbDataParameter[originalParameters.Length];
			for (int i = 0, j = originalParameters.Length; i < j; i++) {
				clonedParameters[i] = (IDbDataParameter)((ICloneable)originalParameters[i]).Clone();
			}
			return clonedParameters;
		}
		#endregion private methods, variables, and constructors

		#region caching functions

		/// <summary>
		/// CacheParameterSet
		/// </summary>
		/// <param name="connectionString">数据库连接语句</param>
		/// <param name="commandText">Sql语句</param>
		/// <param name="commandParameters">参数</param>
		public static void CacheParameterSet(string connectionString, string commandText, params IDbDataParameter[] commandParameters) {
			if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
			if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");
			string hashKey = connectionString + ":" + commandText;
			paramCache[hashKey] = commandParameters;
		}

		/// <summary>
		/// GetCachedParameterSet
		/// </summary>
		/// <param name="connectionString">数据库连接语句</param>
		/// <param name="commandText">Sql语句</param>
		/// <returns></returns>
		public static IDbDataParameter[] GetCachedParameterSet(string connectionString, string commandText) {
			if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
			if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");
			string hashKey = connectionString + ":" + commandText;
			IDbDataParameter[] cachedParameters = paramCache[hashKey] as IDbDataParameter[];
			if (cachedParameters == null) {
				return null;
			}
			else {
				return CloneParameters(cachedParameters);
			}
		}
		#endregion caching functions
	}
}
