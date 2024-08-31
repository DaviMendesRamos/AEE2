using App_AEE.Data;
using App_AEE.Model;
using App_AEE.Pages;
using System.Runtime.CompilerServices;

namespace App_AEE
{
	public partial class App : Application
	{
		static SQLiteData _bancoDados;

		public static SQLiteData BancoDados
		{
			get
			{
				if (_bancoDados == null)
				{
					_bancoDados =
						new SQLiteData(Path.Combine(Environment.
						GetFolderPath(Environment.SpecialFolder.
						LocalApplicationData), "Dados.db3"));
				}
				return _bancoDados;
			}
		}

		public static Usuario Usuario { get; set; }
		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage( new LoginUsuarioPage());
		}
	}
}
