using CommunityToolkit.Maui.Views;
using App_AEE.Model;
using System;
using System.Collections.Generic;

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

            // Definindo o ItemsSource da CollectionView
            equipesCollectionView.ItemsSource = Equipes;
        }

        // Método que será chamado ao clicar no botão de confirmar
        private void ConfirmarButton_Clicked(object sender, EventArgs e)
        {
            OnPopupClosed?.Invoke(EquipeSelecionada);
            this.Close();
        }

        // Método para lidar com a seleção da equipe
        private void EquipesCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Atribuindo a equipe selecionada
            EquipeSelecionada = e.CurrentSelection.FirstOrDefault() as Equipe;
        }
    }
}