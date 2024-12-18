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

        // Método chamado quando um item da CollectionView é clicado
        private void OnItemTapped(object sender, EventArgs e)
        {
            // Obtemos o Frame que foi tocado e associamos a equipe a ele
            var tappedFrame = sender as Frame;
            var equipe = tappedFrame?.BindingContext as Equipe;

            if (equipe != null)
            {
                // Atualizando a equipe selecionada
                EquipeSelecionada = equipe;

                // Atualizando a seleção visualmente
                equipesCollectionView.SelectedItem = equipe;
            }
        }

        // Método que será chamado quando a seleção de item for alterada
        private void EquipesCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Atribuindo a equipe selecionada com base na seleção atual
            EquipeSelecionada = e.CurrentSelection.FirstOrDefault() as Equipe;
        }

        // Método que será chamado ao clicar no botão de confirmar
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
