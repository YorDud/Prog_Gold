using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Security.Policy;
using MaterialDesignThemes.Wpf;

namespace Prog_Gold
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	/// 
	public partial class MainWindow : Window
	{

		public MainWindow()
		{
			InitializeComponent();



			LoadData_Gold_Tech();
			LoadData_Gold_Buhg();

			KofUnos.Text = "0,08";
			KofPokr.Text = "0,01158";
		}

		private void SetVisibility(FrameworkElement visibleElement)
		{
			// Список всех элементов, которые нужно скрыть
			var elements = new List<FrameworkElement>
	{
		Tech,
		Buhg
	};

			// Устанавливаем видимость для каждого элемента
			foreach (var element in elements)
			{
				element.Visibility = (element == visibleElement) ? Visibility.Visible : Visibility.Hidden;
			}
		}




		public void LoadData_Gold_Tech()
		{
			using (SqlConnection connection = new SqlConnection(WC.ConnectionString))
			{
				// SQL-запрос с JOIN для получения данных из таблиц Naryad и Types_TO
				string query = @"
            SELECT [ID]
      ,[DateCreate]
      ,[DateZakaz]
      ,[NumberZakaz]
      ,[IndivNumber]
      ,[LengthZag]
      ,[HeightZag]
      ,[KolichZag]
      ,[PloshPoverh]
      ,[PloshTOP]
      ,[PloshBOT]
      ,[PloshPokrit]
      ,[RashodUnos]
      ,[RashodPokrit]
      ,[RashLab]
      ,[SumRash]
      ,[SolZol]
      ,[SumRashSolZol]
  FROM [Golg].[dbo].[Gold_Tech] order by [ID] desc";

				SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
				DataTable dataTable = new DataTable();

				connection.Open();
				adapter.Fill(dataTable);
				connection.Close();

				dataGridGold_Tech.ItemsSource = dataTable.DefaultView;
			}
		}

		private void Dobav_Click(object sender, RoutedEventArgs e)
		{
			// Используем InvariantCulture для обработки чисел независимо от локали
			CultureInfo culture = CultureInfo.InvariantCulture;

			// Получаем значения из полей
			var DateZakaz1 = DateZakaz.SelectedDate;
			var NumberZakaz1 = NumberZakaz.Text;
			var IndivNumber1 = IndivNumber.Text;

			double LengthZag1, HeightZag1, PloshTOP1, PloshBOT1, RashLab1, SolZol1, KofUnos1, KofPokr1;
			int KolichZag1;

			// Преобразуем значения, заменяя запятую на точку
			Double.TryParse(LengthZag.Text.Replace(',', '.'), NumberStyles.Any, culture, out LengthZag1);
			Double.TryParse(HeightZag.Text.Replace(',', '.'), NumberStyles.Any, culture, out HeightZag1);
			Int32.TryParse(KolichZag.Text, out KolichZag1);
			Double.TryParse(PloshTOP.Text.Replace(',', '.'), NumberStyles.Any, culture, out PloshTOP1);
			Double.TryParse(PloshBOT.Text.Replace(',', '.'), NumberStyles.Any, culture, out PloshBOT1);
			Double.TryParse(RashLab.Text.Replace(',', '.'), NumberStyles.Any, culture, out RashLab1);
			Double.TryParse(SolZol.Text.Replace(',', '.'), NumberStyles.Any, culture, out SolZol1);

			Double.TryParse(KofUnos.Text.Replace(',', '.'), NumberStyles.Any, culture, out KofUnos1);
			Double.TryParse(KofPokr.Text.Replace(',', '.'), NumberStyles.Any, culture, out KofPokr1);

			// Вычисления



			double LengthZag2 = LengthZag1 * 0.001;
			double HeightZag2 = HeightZag1 * 0.001;
			double PloshPoverh1 = LengthZag2 * HeightZag2 * KolichZag1 * 2;
			double PloshPokrit1 = Math.Round((PloshTOP1 + PloshBOT1) * KolichZag1, 2);
			double RashodUnos1 = Math.Round(PloshPoverh1 * KofUnos1, 2);
			double RashodPokrit1 = Math.Round(PloshPokrit1 * KofPokr1, 2);
			double SumRash1 = Math.Round(RashodUnos1 + RashodPokrit1 + RashLab1, 2);
			double SumRashSolZol1 = SolZol1 != 0 ? Math.Round(SumRash1 / SolZol1 * 100, 2) : 0;

			// SQL-запрос на добавление
			string query = "INSERT INTO [Golg].[dbo].[Gold_Tech] " +
			   "([DateCreate], [DateZakaz], [NumberZakaz], [IndivNumber], [LengthZag], [HeightZag], [KolichZag], [PloshPoverh], " +
			   "[PloshTOP], [PloshBOT], [PloshPokrit], [RashodUnos], [RashodPokrit], [RashLab], [SumRash], [SolZol], [SumRashSolZol]) " +
			   "VALUES (@DateCreate, @DateZakaz, @NumberZakaz, @IndivNumber, @LengthZag, @HeightZag, @KolichZag, @PloshPoverh, " +
			   "@PloshTOP, @PloshBOT, @PloshPokrit, @RashodUnos, @RashodPokrit, @RashLab, @SumRash, @SolZol, @SumRashSolZol)";

			try
			{
				using (SqlConnection connection = new SqlConnection(WC.ConnectionString))
				{
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						// Добавление значений для параметров
						command.Parameters.AddWithValue("@DateCreate", DateTime.Now);
						command.Parameters.AddWithValue("@DateZakaz", (object)DateZakaz1 ?? DBNull.Value);
						command.Parameters.AddWithValue("@NumberZakaz", NumberZakaz1 ?? (object)DBNull.Value);
						command.Parameters.AddWithValue("@IndivNumber", IndivNumber1 ?? (object)DBNull.Value);
						command.Parameters.Add("@LengthZag", System.Data.SqlDbType.Float).Value = LengthZag2;
						command.Parameters.Add("@HeightZag", System.Data.SqlDbType.Float).Value = HeightZag2;
						command.Parameters.Add("@KolichZag", System.Data.SqlDbType.Int).Value = KolichZag1;
						command.Parameters.Add("@PloshPoverh", System.Data.SqlDbType.Float).Value = PloshPoverh1;
						command.Parameters.Add("@PloshTOP", System.Data.SqlDbType.Float).Value = PloshTOP1;
						command.Parameters.Add("@PloshBOT", System.Data.SqlDbType.Float).Value = PloshBOT1;
						command.Parameters.Add("@PloshPokrit", System.Data.SqlDbType.Float).Value = PloshPokrit1;
						command.Parameters.Add("@RashodUnos", System.Data.SqlDbType.Float).Value = RashodUnos1;
						command.Parameters.Add("@RashodPokrit", System.Data.SqlDbType.Float).Value = RashodPokrit1;
						command.Parameters.Add("@RashLab", System.Data.SqlDbType.Float).Value = RashLab1;
						command.Parameters.Add("@SumRash", System.Data.SqlDbType.Float).Value = SumRash1;
						command.Parameters.Add("@SolZol", System.Data.SqlDbType.Float).Value = SolZol1;
						command.Parameters.Add("@SumRashSolZol", System.Data.SqlDbType.Float).Value = SumRashSolZol1;

						// Открытие соединения и выполнение команды
						connection.Open();
						int rowsAffected = command.ExecuteNonQuery();

						LoadData_Gold_Tech();
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Ошибка: " + ex.Message);
			}
		}

		private void Tech_Click(object sender, RoutedEventArgs e)
		{
			SetVisibility(Tech);
		}

		private void Buhg_Click(object sender, RoutedEventArgs e)
		{
			SetVisibility(Buhg);
		}

		private void GenerateReport_Click(object sender, RoutedEventArgs e)
		{
			DateTime? startDate = StartDatePicker.SelectedDate;
			DateTime? endDate = EndDatePicker.SelectedDate;

			if (startDate == null || endDate == null)
			{
				MessageBox.Show("Выберите диапазон дат.");
				return;
			}

			// Устанавливаем время начала на 00:00:00 и конец на 23:59:59
			DateTime startDateTime = startDate.Value.Date;
			DateTime endDateTime = endDate.Value.Date.AddDays(1).AddSeconds(-1);

			string query = "SELECT SUM(CAST(KolichZag AS FLOAT)), SUM(CAST(PloshPoverh AS FLOAT)), " +
						   "SUM(CAST(PloshPokrit AS FLOAT)), SUM(CAST(RashodUnos AS FLOAT)), " +
						   "SUM(CAST(RashodPokrit AS FLOAT)), SUM(CAST(RashLab AS FLOAT)), " +
						   "SUM(CAST(SumRash AS FLOAT)), SUM(CAST(SumRashSolZol AS FLOAT)) " +
						   "FROM [Golg].[dbo].[Gold_Tech] WHERE DateCreate BETWEEN @StartDate AND @EndDate";

			try
			{
				using (SqlConnection connection = new SqlConnection(WC.ConnectionString))
				{
					using (SqlCommand command = new SqlCommand(query, connection))
					{
						command.Parameters.AddWithValue("@StartDate", startDateTime);
						command.Parameters.AddWithValue("@EndDate", endDateTime);

						connection.Open();
						SqlDataReader reader = command.ExecuteReader();

						if (reader.Read())
						{
							ReportWindow reportWindow = new ReportWindow(startDate.Value, endDate.Value, reader);
							reportWindow.ShowDialog();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Ошибка: " + ex.Message);
			}
		}













		public void LoadData_Gold_Buhg()
		{
			using (SqlConnection connection = new SqlConnection(WC.ConnectionString))
			{
				string query = @"
            SELECT [ID]
                  ,[Date]
                  ,[INP]
                  ,[PrihodSol]
                  ,[PrihodMassGold]
                  ,[PrihodSoderSol]
                  ,[OstSol]
                  ,[OstGold]
                  ,[SpisVproizSol]
                  ,[SpisVproizGold]
                  ,[SpisSproizSol]
                  ,[SpisSproizGold]
                  ,[AfinSol]
                  ,[AfinGold]
                  ,[NormPotSol]
                  ,[NormPotGold]
            FROM [Golg].[dbo].[Gold_Buhg] order by [ID] desc";

				SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
				DataTable dataTable = new DataTable();

				connection.Open();
				adapter.Fill(dataTable);
				connection.Close();

				dataGridGold_Buhg.ItemsSource = dataTable.DefaultView;

				// Добавляем обработчик события Loaded, чтобы корректно окрасить строки
				dataGridGold_Buhg.Loaded += (s, e) => HighlightZeroOstSolRows();
			}
		}

		// Метод для перекрашивания строк с OstSol == 0
		public void HighlightZeroOstSolRows()
		{
			dataGridGold_Buhg.Dispatcher.InvokeAsync(() =>
			{
				foreach (var item in dataGridGold_Buhg.Items)
				{
					if (item is DataRowView rowView)
					{
						if (rowView["OstSol"] != DBNull.Value &&
							double.TryParse(rowView["OstSol"].ToString(), out double ostSol) &&
							ostSol == 0)
						{
							DataGridRow row = (DataGridRow)dataGridGold_Buhg.ItemContainerGenerator.ContainerFromItem(item);
							if (row != null)
							{
								row.Background = new SolidColorBrush(Colors.Red);
							}
						}
					}
				}
			}, System.Windows.Threading.DispatcherPriority.Background);
		}







		//		public void LoadData_Gold_Buhg()
		//		{
		//			using (SqlConnection connection = new SqlConnection(WC.ConnectionString))
		//			{
		//				// SQL-запрос с JOIN для получения данных из таблиц Naryad и Types_TO
		//				string query = @"
		//            SELECT [ID]
		//      ,[Date]
		//      ,[INP]
		//,[PrihodSol]
		//      ,[PrihodMassGold]
		//      ,[PrihodSoderSol]
		//      ,[OstSol]
		//      ,[OstGold]
		//      ,[SpisVproizSol]
		//      ,[SpisVproizGold]
		//      ,[SpisSproizSol]
		//      ,[SpisSproizGold]
		//      ,[AfinSol]
		//      ,[AfinGold]
		//      ,[NormPotSol]
		//      ,[NormPotGold]
		//  FROM [Golg].[dbo].[Gold_Buhg]";

		//				SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
		//				DataTable dataTable = new DataTable();

		//				connection.Open();
		//				adapter.Fill(dataTable);
		//				connection.Close();

		//				dataGridGold_Buhg.ItemsSource = dataTable.DefaultView;
		//			}
		//		}

		//private void DobavBuhg_Click(object sender, RoutedEventArgs e)
		//{
		//	CultureInfo culture = CultureInfo.InvariantCulture;

		//	var DateBuhg1 = DateBuhg.SelectedDate;
		//	var INP1 = INP.Text;
		//	double PrihodSol1, PrihodSoderSol1, OstSol1, OstGold1, SpisVproizSol1, SpisVproizGold1, SpisSproizSol1, SpisSproizGold1, AfinSol1, AfinGold1, NormPotSol1, NormPotGold1;

		//	Double.TryParse(PrihodSol.Text.Replace(',', '.'), NumberStyles.Any, culture, out PrihodSol1);

		//	Double.TryParse(PrihodSoderSol.Text.Replace(',', '.'), NumberStyles.Any, culture, out PrihodSoderSol1);
		//	//Double.TryParse(SpisVproizSol.Text.Replace(',', '.'), NumberStyles.Any, culture, out PrihodSoderSol1);
		//	//Double.TryParse(KolidchZag.Text.Replace(',', '.'), NumberStyles.Any, culture, out OstSol1);
		//	//Double.TryParse(PlodshTOP.Text.Replace(',', '.'), NumberStyles.Any, culture, out OstGold1);
		//	Double.TryParse(SpisVproizSol.Text.Replace(',', '.'), NumberStyles.Any, culture, out SpisVproizSol1);
		//	//Double.TryParse(SpisVproizGold.Text.Replace(',', '.'), NumberStyles.Any, culture, out SpisVproizGold1);
		//	Double.TryParse(SpisSproizSol.Text.Replace(',', '.'), NumberStyles.Any, culture, out SpisSproizSol1);
		//	//Double.TryParse(SpisSproizGold.Text.Replace(',', '.'), NumberStyles.Any, culture, out SpisSproizGold1);
		//	Double.TryParse(AfinSol.Text.Replace(',', '.'), NumberStyles.Any, culture, out AfinSol1);
		//	Double.TryParse(AfinGold.Text.Replace(',', '.'), NumberStyles.Any, culture, out AfinGold1);
		//	Double.TryParse(NormPotSol.Text.Replace(',', '.'), NumberStyles.Any, culture, out NormPotSol1);
		//	Double.TryParse(NormPotGold.Text.Replace(',', '.'), NumberStyles.Any, culture, out NormPotGold1);

		//	double PrihodMassGold1 = Math.Round((PrihodSol1 * PrihodSoderSol1) /100, 2);

		//	string query = "INSERT INTO [Golg].[dbo].[Gold_Buhg] " +
		//	   "([Date], [INP], [PrihodSol], [PrihodMassGold], [PrihodSoderSol], [SpisVproizSol], [SpisVproizGold], [SpisSproizSol], [SpisSproizGold], [AfinSol], [AfinGold], [NormPotSol], [NormPotGold]) " +
		//	   "VALUES (@Date, @INP, @PrihodSol, @PrihodMassGold, @PrihodSoderSol, @SpisVproizSol, @SpisVproizGold, @SpisSproizSol, @SpisSproizGold, @AfinSol, @AfinGold, @NormPotSol, @NormPotGold)";
		//	//string query = "INSERT INTO [Golg].[dbo].[Gold_Buhg] " +
		// // "([Date], [INP], [PrihodSol], [PrihodMassGold], [PrihodSoderSol], [OstSol], [OstGold], [SpisVproizSol], [SpisVproizGold], [SpisSproizSol], [SpisSproizGold], [AfinSol], [AfinGold], [NormPotSol], [NormPotGold]) " +
		// // "VALUES (@Date, @INP, @PrihodSol, @PrihodMassGold, @PrihodSoderSol, @OstSol, @OstGold, @SpisVproizSol, @SpisVproizGold, @SpisSproizSol, @SpisSproizGold, @AfinSol, @AfinGold, @NormPotSol, @NormPotGold)";

		//	try
		//	{
		//		using (SqlConnection connection = new SqlConnection(WC.ConnectionString))
		//		{
		//			using (SqlCommand command = new SqlCommand(query, connection))
		//			{
		//				command.Parameters.AddWithValue("@Date", (object)DateBuhg1 ?? DBNull.Value);
		//				command.Parameters.AddWithValue("@INP", INP1 ?? (object)DBNull.Value);
		//				command.Parameters.Add("@PrihodSol", System.Data.SqlDbType.Float).Value = PrihodSol1;
		//				command.Parameters.Add("@PrihodMassGold", System.Data.SqlDbType.Float).Value = PrihodMassGold1;
		//				command.Parameters.Add("@PrihodSoderSol", System.Data.SqlDbType.Float).Value = PrihodSoderSol1;
		//				//command.Parameters.Add("@OstSol", System.Data.SqlDbType.Float).Value = OstSol1;
		//				//command.Parameters.Add("@OstGold", System.Data.SqlDbType.Float).Value = OstGold1;
		//				command.Parameters.Add("@SpisVproizSol", System.Data.SqlDbType.Float).Value = SpisVproizSol1;
		//				//command.Parameters.Add("@SpisVproizGold", System.Data.SqlDbType.Float).Value = SpisVproizGold1;
		//				command.Parameters.Add("@SpisSproizSol", System.Data.SqlDbType.Float).Value = SpisSproizSol1;
		//				//command.Parameters.Add("@SpisSproizGold", System.Data.SqlDbType.Float).Value = SpisSproizGold1;
		//				command.Parameters.Add("@AfinSol", System.Data.SqlDbType.Float).Value = AfinSol1;
		//				command.Parameters.Add("@AfinGold", System.Data.SqlDbType.Float).Value = AfinGold1;
		//				command.Parameters.Add("@NormPotSol", System.Data.SqlDbType.Float).Value = NormPotSol1;
		//				command.Parameters.Add("@NormPotGold", System.Data.SqlDbType.Float).Value = NormPotGold1;

		//				connection.Open();
		//				int rowsAffected = command.ExecuteNonQuery();
		//				LoadData_Gold_Buhg();
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		MessageBox.Show("Ошибка: " + ex.Message);
		//	}
		//}


		private void DobavBuhg_Click(object sender, RoutedEventArgs e)
		{
			CultureInfo culture = CultureInfo.InvariantCulture;

			var DateBuhg1 = DateBuhg.SelectedDate;
			var INP1 = INP.Text;
			double PrihodSol1, PrihodSoderSol1, OstSol1, OstGold1, SpisVproizSol1, SpisVproizGold1, SpisSproizSol1, SpisSproizGold1, AfinSol1, AfinGold1, NormPotSol1, NormPotGold1;

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

			string query = "INSERT INTO [Golg].[dbo].[Gold_Buhg] " +
			   "([Date], [INP], [PrihodSol], [PrihodMassGold], [PrihodSoderSol], [SpisVproizSol], [SpisVproizGold], [SpisSproizSol], [SpisSproizGold], [AfinSol], [AfinGold], [NormPotSol], [NormPotGold], [OstSol], [OstGold]) " +
			   "VALUES (@Date, @INP, @PrihodSol, @PrihodMassGold, @PrihodSoderSol, @SpisVproizSol, @SpisVproizGold, @SpisSproizSol, @SpisSproizGold, @AfinSol, @AfinGold, @NormPotSol, @NormPotGold, @OstSol, @OstGold)";

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

						connection.Open();
						int rowsAffected = command.ExecuteNonQuery();
						LoadData_Gold_Buhg();
						HighlightZeroOstSolRows();


					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Ошибка: " + ex.Message);
			}
		}

		private void dataGridEdit_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{

			if (dataGridGold_Buhg.SelectedItem is DataRowView selectedRow)
			{
				Edit_Buhg edit_Buhg = new Edit_Buhg(this, selectedRow);
				edit_Buhg.ShowDialog();
			}
		}
		//f 






		private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			// Разрешаем только цифры, запятую и точку
			if (!IsTextAllowed(e.Text))
			{
				e.Handled = true; // Блокируем ввод если символы не разрешены
			}
		}

		private static bool IsTextAllowed(string text)
		{
			// Проверяем если текст содержит только разрешенные символы
			foreach (char c in text)
			{
				if (!char.IsDigit(c) && c != '.' && c != ',')
				{
					return false;
				}
			}
			return true;
		}










		private void Window_Closed(object sender, EventArgs e)
		{
			System.Windows.Application.Current.Shutdown();
		}





		private void ReportBuhg_Click(object sender, RoutedEventArgs e)
		{
	
			string query = "SELECT [INP], [OstSol], [OstGold], [SpisVproizSol], [SpisVproizGold] " +
						   "FROM [Golg].[dbo].[Gold_Buhg] WHERE [OstSol] <> 0";

			List<Dictionary<string, string>> reportData = new List<Dictionary<string, string>>();

			using (SqlConnection connection = new SqlConnection(WC.ConnectionString))
			{
				SqlCommand command = new SqlCommand(query, connection);
				connection.Open();

				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						reportData.Add(new Dictionary<string, string>()
				{
					{ "INP", reader["INP"].ToString() },
					{ "OstSol", reader["OstSol"].ToString() },
					{ "OstGold", reader["OstGold"].ToString() },
					{ "SpisVproizSol", reader["SpisVproizSol"].ToString() },
					{ "SpisVproizGold", reader["SpisVproizGold"].ToString() }
				});
					}
				}
			}

			// Создаем и настраиваем документ для печати
			PrintDialog printDialog = new PrintDialog();
			if (printDialog.ShowDialog() == true)
			{
				FlowDocument doc = new FlowDocument();
				doc.PagePadding = new Thickness(72); // 1 дюйм со всех сторон
				doc.PageWidth = 793;
				doc.PageHeight = 1122;
				doc.FontFamily = new FontFamily("Times New Roman");

				// Заголовок
				Paragraph title = new Paragraph(new Run("Отчет о движении материальных ценностей, содержащих драгоценные металлы"))
				{
					FontSize = 18,
					FontWeight = FontWeights.Bold,
					TextAlignment = TextAlignment.Center
				};
				doc.Blocks.Add(title);

				// Дата печати
				doc.Blocks.Add(new Paragraph(new Run("На: " + DateTime.Now.ToShortDateString()))
				{
					FontWeight = FontWeights.Bold,
					TextAlignment = TextAlignment.Center
				});

				// Создание таблицы
				Table table = new Table();
				foreach (var col in new[] { "INP", "OstSol", "OstGold", "SpisVproizSol", "SpisVproizGold" })
				{
					table.Columns.Add(new TableColumn { Width = new GridLength(128) });
				}

				// Заголовки таблицы
				TableRowGroup headerRowGroup = new TableRowGroup();
				TableRow headerRow = new TableRow();
				foreach (var col in new[] { "ИНП", "Остаток - Масса общаяя (лигат.)", "Остаток - Масса хим. Чистого вещества (ДМ)", "Списания в пр-во - Масса общаяя (лигат.)", "Списания в пр-во - Масса хим. Чистого вещества (ДМ)" })
				{
					headerRow.Cells.Add(new TableCell(new Paragraph(new Run(col)))
					{
						BorderThickness = new Thickness(1),
						BorderBrush = Brushes.Black,
						Padding = new Thickness(5),
						FontSize = 16,
						FontWeight = FontWeights.Bold,
						TextAlignment = TextAlignment.Center

					});
				}
				headerRowGroup.Rows.Add(headerRow);
				table.RowGroups.Add(headerRowGroup);

				// Данные таблицы
				TableRowGroup rowGroup = new TableRowGroup();
				foreach (var item in reportData)
				{
					TableRow row = new TableRow();
					foreach (var col in new[] { "INP", "OstSol", "OstGold", "SpisVproizSol", "SpisVproizGold" })
					{
						row.Cells.Add(new TableCell(new Paragraph(new Run(item[col])))
						{
							BorderThickness = new Thickness(1),
							BorderBrush = Brushes.Black,
							Padding = new Thickness(5),
							FontSize= 14,
							TextAlignment = TextAlignment.Center
						});
					}
					rowGroup.Rows.Add(row);
				}
				table.RowGroups.Add(rowGroup);

				table.TextAlignment = TextAlignment.Center;
				doc.Blocks.Add(table);

				// Печать документа
				IDocumentPaginatorSource paginator = doc;
				printDialog.PrintDocument(paginator.DocumentPaginator, "Отчет");
			}
		}







	}
}


