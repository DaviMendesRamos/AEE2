﻿
using App_AEE.Services;
using App_AEE.Pages;
using App_AEE.Validations;
using System.Diagnostics;

namespace App_AEE;

public partial class App : Application
{
	private readonly ApiService _apiService;
	private readonly IValidator _validator;

	// Construtor que recebe dependências via injeção
	public App(ApiService apiService, IValidator validator)
	{
		InitializeComponent();
		_apiService = apiService;
		_validator = validator;

		// Captura global de erros
		AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
		{
			// Exibe a exceção para depuração
			Debug.WriteLine($"Unhandled exception: {e.ExceptionObject}");
			// Aqui não podemos usar DisplayAlert diretamente, então vamos usar o método para exibir o alerta na página ativa.
			ShowErrorPage();
		};

		// Configuração da MainPage
		SetMainPage();
	}

	// Método para definir a página inicial do aplicativo
	private void SetMainPage()
	{
		// Recupera o token de autenticação salvo nas preferências
		var accessToken = Preferences.Get("accesstoken", string.Empty);

		if (string.IsNullOrEmpty(accessToken))
		{
			// Usuário não está autenticado, redireciona para a página de login
			MainPage = new NavigationPage(new LoginUsuarioPage(_apiService, _validator));
		}
		else
		{
			// Usuário autenticado, redireciona para a página inicial (HomePage, ou qualquer página principal definida)
			MainPage = new AppShell(_apiService, _validator);
		}
	}

	// Método para exibir a página de erro
	private void ShowErrorPage()
	{
		// Aqui você poderia navegar para uma página de erro específica, ou mostrar um erro geral
		MainPage = new ErrorPage();
	}
}
