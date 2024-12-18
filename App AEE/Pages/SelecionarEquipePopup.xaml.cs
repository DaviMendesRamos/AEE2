using CommunityToolkit.Maui.Views;
using App_AEE.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App_AEE.Pages
{
    public partial class SelecionarEquipePopup : Popup
    {
        public List<Equipe> Equipes { get; }
        public Equipe EquipeSelecionada { get; set; }
        public event Action<Equipe> OnPopupClosed;

        public SelecionarEquipePopup(List<Equipe> equipes)
        {
            InitializeComponent();
            Equipes = equipes;
            equipesCollectionView.ItemsSource = Equipes;
        }

        // M�todo chamado quando um item da CollectionView � clicado
        private void OnItemTapped(object sender, EventArgs e)
        {
            // Obtemos o Frame que foi tocado e associamos a equipe a ele
            var tappedFrame = sender as Frame;
            var equipe = tappedFrame?.BindingContext as Equipe;

            if (equipe != null)
            {
                // Atualizando a equipe selecionada
                EquipeSelecionada = equipe;

                // Atualizando a sele��o visualmente
                equipesCollectionView.SelectedItem = equipe;
            }
        }

        // M�todo que ser� chamado quando a sele��o de item for alterada
        private void EquipesCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Atribuindo a equipe selecionada com base na sele��o atual
            EquipeSelecionada = e.CurrentSelection.FirstOrDefault() as Equipe;
        }

        // M�todo que ser� chamado ao clicar no bot�o de confirmar
        private async void ConfirmarButton_Clicked(object sender, EventArgs e)
        {
            if (EquipeSelecionada == null)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Por favor, selecione uma equipe.", "OK");
                return;
            }

            // Invoca o evento para notificar que a equipe foi selecionada
            OnPopupClosed?.Invoke(EquipeSelecionada);
            await this.CloseAsync();
        }
    }
}
