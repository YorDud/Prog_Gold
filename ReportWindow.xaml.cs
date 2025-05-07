using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
	/// Логика взаимодействия для ReportWindow.xaml
	/// </summary>
	public partial class ReportWindow : Window
	{
		private List<KeyValuePair<string, string>> reportData;

		public ReportWindow(DateTime startDate, DateTime endDate, SqlDataReader reader)
		{
			InitializeComponent();
			DateRangeText.Text = $"{startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}";
			reportData = new List<KeyValuePair<string, string>>();

			string[] labels = { "Количество заготовок, шт", "Площадь поверхности заготовок, м²", "Площадь покрытия всего, дм²",
								"Расход металла золота на унос, г", "Расход металла золота на покрытие, г",
								"Расход металла золота лаборатория, г", "Сумма расхода металла золота, г",
								"Сумма расхода соли золота, г" };

			for (int i = 0; i < labels.Length; i++)
			{
				reportData.Add(new KeyValuePair<string, string>(labels[i], reader[i]?.ToString() ?? "0"));
			}

			ReportDataGrid.ItemsSource = reportData;



	//		InitializeComponent();
	//		DateRangeText.Text = $"{startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}";
	//		var reportData = new List<KeyValuePair<string, string>>
	//{
	//	new KeyValuePair<string, string>("Количество заготовок, шт", reader[0]?.ToString()),
	//	new KeyValuePair<string, string>("Площадь поверхности заготовок, м²", reader[1]?.ToString()),
	//	new KeyValuePair<string, string>("Площадь покрытия всего, дм²", reader[2]?.ToString()),
	//	new KeyValuePair<string, string>("Расход металла золота на унос, г", reader[3]?.ToString()), 
	//	new KeyValuePair<string, string>("Расход металла золота на покрытие, г", reader[4]?.ToString()),
	//	new KeyValuePair<string, string>("Расход металла золота лаборатория, г", reader[5]?.ToString()),
	//	new KeyValuePair<string, string>("Сумма расхода металла золота, г", reader[6]?.ToString()),
	//	new KeyValuePair<string, string>("Сумма расхода соли золота, г", reader[7]?.ToString())
	//};
	//		ReportDataGrid.ItemsSource = reportData;
		}



		private void PrintReport_Click(object sender, RoutedEventArgs e)
		{
			PrintDialog printDialog = new PrintDialog();
			if (printDialog.ShowDialog() == true)
			{
				FlowDocument doc = new FlowDocument();

				// Устанавливаем поля документа
				doc.PagePadding = new Thickness(72); // 1 дюйм на всех сторонах (72 пикселя на 96 DPI)

				// Устанавливаем размер страницы A4
				doc.PageWidth = 793; // A4 ширина в пикселях при 96 DPI
				doc.PageHeight = 1122; // A4 высота в пикселях при 96 DPI
				doc.FontFamily = new FontFamily("Times New Roman");

				// Центрирование заголовка
				Paragraph title = new Paragraph(new Run("Отчет по расходу материальных ценностей, содержащих драгоценные металлы за отчетный период"))
				{
					FontSize = 18,
					FontWeight = FontWeights.Bold,
					TextAlignment = TextAlignment.Center
				};
				doc.Blocks.Add(title);

				// Диапазон дат
				doc.Blocks.Add(new Paragraph(new Run("Диапазон дат: " + DateRangeText.Text))
				{
					FontWeight = FontWeights.Bold,
					TextAlignment = TextAlignment.Center
				});

				// Создание таблицы с выравниванием по центру
				Table table = new Table();
				table.Columns.Add(new TableColumn { Width = new GridLength(350) });
				table.Columns.Add(new TableColumn { Width = new GridLength(250) });
				TableRowGroup rowGroup = new TableRowGroup();

				foreach (var item in reportData)
				{
					TableRow row = new TableRow();
					row.Cells.Add(new TableCell(new Paragraph(new Run(item.Key)))
					{
						BorderThickness = new Thickness(1),
						BorderBrush = Brushes.Black,
						Padding = new Thickness(5),
						TextAlignment = TextAlignment.Center
					});
					row.Cells.Add(new TableCell(new Paragraph(new Run(item.Value)))
					{
						BorderThickness = new Thickness(1),
						BorderBrush = Brushes.Black,
						Padding = new Thickness(5),
						TextAlignment = TextAlignment.Center
					});
					rowGroup.Rows.Add(row);
				}

				table.RowGroups.Add(rowGroup);
				table.TextAlignment = TextAlignment.Center;

				// Добавляем таблицу в документ
				doc.Blocks.Add(table);

				// Печать документа
				IDocumentPaginatorSource paginator = doc;
				printDialog.PrintDocument(paginator.DocumentPaginator, "Отчет");
			}
		}


	}
}
