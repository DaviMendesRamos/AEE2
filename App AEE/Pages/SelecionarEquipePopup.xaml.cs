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

        // M�todo que ser� chamado ao clicar no bot�o de confirmar
        private void ConfirmarButton_Clicked(object sender, EventArgs e)
        {
            OnPopupClosed?.Invoke(EquipeSelecionada);
            this.Close();
        }

        // M�todo para lidar com a sele��o da equipe
        private void EquipesCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Atribuindo a equipe selecionada
            EquipeSelecionada = e.CurrentSelection.FirstOrDefault() as Equipe;
        }
    }
}