using Microsoft.Data.SqlClient;
using System.Data;

namespace CityTechWEBAPI
{
	public class DataLogic
	{


		private readonly IConfiguration _configuration;
		private readonly string connec;
		public DataLogic(IConfiguration configuration)
		{
			_configuration = configuration;
			connec = _configuration.GetConnectionString("CityTechConStr");

			con = new SqlConnection(connec);
			Da = new SqlDataAdapter();
			cmd = new SqlCommand();
		}

		public SqlConnection con { get; private set; }
		public SqlDataAdapter Da { get; private set; }
		public SqlCommand cmd { get; private set; }

		private string Qry;

		public void LoadList(DataSet Ds, string qry)
		{
			try
			{
				if (con != null && con.State == ConnectionState.Closed)
				{
					con.ConnectionString = connec;
					con.Open();
				}
				Ds.Tables.Clear();
				Da = new SqlDataAdapter(qry, con);
				Da.Fill(Ds);
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				con.Close();
			}

		}

		public int MaxNumber(string field, string table)
		{
			try
			{

				if (con != null && con.State == ConnectionState.Closed)

				{
					con.ConnectionString = connec;
					con.Open();
				}
				DataSet Ds = new DataSet();
				int MAX = 0;
				Qry = "Select Isnull(Max(" + field + "),0)+1 as No From " + table + "";


				Da = new SqlDataAdapter(Qry, con);
				Da.Fill(Ds);
				MAX = Convert.ToInt32(Ds.Tables[0].Rows[0]["No"].ToString());
				return MAX;

			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				Da.Dispose();

				con.Close();

			}

		}

		public void ManData1(String qry, SqlTransaction CurrentTransaction)
		{
			try
			{
				if (con != null && con.State == ConnectionState.Closed)

				{
					con.ConnectionString = connec;
					con.Open();
				}

				cmd.Connection = con;
				cmd.Transaction = CurrentTransaction;
				cmd.CommandText = qry;
				cmd.ExecuteNonQuery();
				cmd.Dispose();

			}

			catch (Exception ex)

			{

				if (qry.Contains("transmain"))
				{
					con.Close();
					con.Dispose();
				}

			}

			finally
			{

			}

		}

		public DataTable LoadData(string qry)
		{
			try
			{
				DataTable dt = new();
				if (con != null && con.State == ConnectionState.Closed)
				{
					con.ConnectionString = connec;
					con.Open();
				}
				dt.Clear();
				Da = new SqlDataAdapter(qry, con);
				Da.Fill(dt);
				return dt;
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				con.Close();
			}
		}


	}
}
