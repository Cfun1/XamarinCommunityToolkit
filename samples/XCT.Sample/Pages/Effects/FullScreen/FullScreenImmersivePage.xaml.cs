﻿namespace Xamarin.CommunityToolkit.Sample.Pages.Effects
{
	public partial class FullScreenImmersivePage
	{
		public FullScreenImmersivePage() => InitializeComponent();

		async void Button_Clicked(object sender, System.EventArgs e)
		{
			await Navigation.PopAsync();
		}
	}
}