using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Prog_Gold
{
	/// <summary>
	/// Логика взаимодействия для Edit_Buhg.xaml
	/// </summary>
	public partial class Edit_Buhg : Window
	{

		private DataRowView _dataRow;
		private MainWindow mainWindow;

		public Edit_Buhg(MainWindow mainWindow, DataRowView dataRow)
		{
			InitializeComponent();

			_dataRow = dataRow;
			this.mainWindow = mainWindow; // сохраняем ссылку на главное окно


			DateBuhg.Text = _dataRow["Date"].ToString();

			INP.Text = _dataRow["INP"].ToString();
			PrihodSol.Text = _dataRow["PrihodSol"].ToString();
			PrihodSoderSol.Text = _dataRow["PrihodSoderSol"].ToString();
			SpisVproizSol.Text = _dataRow["SpisVproizSol"].ToString() ;
			SpisSproizSol.Text = _dataRow["SpisSproizSol"].ToString();
			AfinSol.Text = _dataRow["AfinSol"].ToString();
			AfinGold.Text = _dataRow["AfinGold"].ToString();
			NormPotSol.Text = _dataRow["AfinGold"].ToString();
			NormPotGold.Text = _dataRow["NormPotGold"].ToString();



		}



		private void EditBuhg_Click(object sender, RoutedEventArgs e)
		{
			CultureInfo culture = CultureInfo.InvariantCulture;

			var DateBuhg1 = DateBuhg.SelectedDate;
			var INP1 = INP.Text;
			double PrihodSol1, PrihodSoderSol1, OstSol1, OstGold1, SpisVproizSol1, SpisVproizGold1, SpisSproizSol1, SpisSproizGold1, AfinSol1, AfinGold1, NormPotSol1, NormPotGold1;

			var id = _dataRow["ID"];

			Double.TryParse(PrihodSol.Text.Replace(',', '.'), NumberStyles.Any, culture, out PrihodSol1);
			Double.TryParse(PrihodSoderSol.Text.Replace(',', '.'), NumberStyles.Any, culture, out PrihodSoderSol1);
			Double.TryParse(SpisVproizSol.Text.Replace(',', '.'), NumberStyles.Any, culture, out SpisVproizSol1);
			Double.TryParse(SpisSproizSol.Text.Replace(',', '.'), NumberStyles.Any, culture, out SpisSproizSol1);
			Double.TryParse(AfinSol.Text.Replace(',', '.'), NumberStyles.Any, culture, out AfinSol1);
			Double.TryParse(AfinGold.Text.Replace(',', '.'), NumberStyles.Any, culture, out AfinGold1);
			Double.TryParse(NormPotSol.Text.Replace(',', '.'), NumberStyles.Any, culture, out NormPotSol1);
			Double.TryParse(NormPotGold.Text.Replace(',', '.'), NumberStyles.Any, culture, out NormPotGold1);

			// Автоматические вычисления
			double PrihodMassGold1 = Math.Round((PrihodSol1 * PrihodSoderSol1) / 100, 2);
			SpisVproizGold1 = Math.Round((SpisVproizSol1 * PrihodSoderSol1) / 100, 2);
			SpisSproizGold1 = Math.Round((SpisSproizSol1 * PrihodSoderSol1) / 100, 2);
			OstSol1 = Math.Round(PrihodSol1 - SpisSproizSol1 - AfinSol1 - NormPotSol1, 2);
			OstGold1 = Math.Round(PrihodMassGold1 - SpisSproizGold1 - AfinGold1 - NormPotGold1, 2);

			string query = "UPDATE [Golg].[dbo].[Gold_Buhg] SET " +
			   "[Date] = @Date, " +
			   "[INP] = @INP, " +
			   "[PrihodSol] = @PrihodSol, " +
			   "[PrihodMassGold] = @PrihodMassGold, " +
			   "[PrihodSoderSol] = @PrihodSoderSol, " +
			   "[SpisVproizSol] = @SpisVproizSol, " +
			   "[SpisVproizGold] = @SpisVproizGold, " +
			   "[SpisSproizSol] = @SpisSproizSol, " +
			   "[SpisSproizGold] = @SpisSproizGold, " +
			   "[AfinSol] = @AfinSol, " +
			   "[AfinGold] = @AfinGold, " +
			   "[NormPotSol] = @NormPotSol, " +
			   "[NormPotGold] = @NormPotGold, " +
			   "[OstSol] = @OstSol, " +
			   "[OstGold] = @OstGold " +
			   "WHERE [ID] = @ID";


			try
			{
				using (SqlConnection connection = new SqlConnection(WC.ConnectionString))
				{
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@Date", (object)DateBuhg1 ?? DBNull.Value);
						command.Parameters.AddWithValue("@INP", INP1 ?? (object)DBNull.Value);
						command.Parameters.Add("@PrihodSol", System.Data.SqlDbType.Float).Value = PrihodSol1;
						command.Parameters.Add("@PrihodMassGold", System.Data.SqlDbType.Float).Value = PrihodMassGold1;
						command.Parameters.Add("@PrihodSoderSol", System.Data.SqlDbType.Float).Value = PrihodSoderSol1;
						command.Parameters.Add("@SpisVproizSol", System.Data.SqlDbType.Float).Value = SpisVproizSol1;
						command.Parameters.Add("@SpisVproizGold", System.Data.SqlDbType.Float).Value = SpisVproizGold1;
						command.Parameters.Add("@SpisSproizSol", System.Data.SqlDbType.Float).Value = SpisSproizSol1;
						command.Parameters.Add("@SpisSproizGold", System.Data.SqlDbType.Float).Value = SpisSproizGold1;
						command.Parameters.Add("@AfinSol", System.Data.SqlDbType.Float).Value = AfinSol1;
						command.Parameters.Add("@AfinGold", System.Data.SqlDbType.Float).Value = AfinGold1;
						command.Parameters.Add("@NormPotSol", System.Data.SqlDbType.Float).Value = NormPotSol1;
						command.Parameters.Add("@NormPotGold", System.Data.SqlDbType.Float).Value = NormPotGold1;
						command.Parameters.Add("@OstSol", System.Data.SqlDbType.Float).Value = OstSol1;
						command.Parameters.Add("@OstGold", System.Data.SqlDbType.Float).Value = OstGold1;
						command.Parameters.AddWithValue("@ID", id);

						connection.Open();
						int rowsAffected = command.ExecuteNonQuery();
						mainWindow.LoadData_Gold_Buhg();
						mainWindow.HighlightZeroOstSolRows();
						this.Close();


					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Ошибка: " + ex.Message);
			}
		}














































		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
        }

		
	}
}
