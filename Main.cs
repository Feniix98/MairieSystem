using Life;
using Life.Network;
using Life.UI;
using ModKit.Interfaces;
using Format = ModKit.Helper.TextFormattingHelper;
using AAMenu;
using ModKit.Internal;
using Mirror;
using Life.DB;

namespace Mairie98
{
    public class Mairie : ModKit.ModKit
    {
        public Mairie(IGameAPI gameAPI) : base(gameAPI) 
        {
            PluginInformations = new PluginInformations("Mairie98", "1.0.0", "! Fenix");
        }

        public override void OnPlayerSpawnCharacter(Player player, NetworkConnection conn, Characters character)
        {
            base.OnPlayerSpawnCharacter(player, conn, character);
            if (player.steamId == MonSteamId)
            {
                player.SendText("Le plugin Mairie98 est présent sur le serveur !");
            }
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();
            Logger.LogSuccess($"{PluginInformations.SourceName} v{PluginInformations.Version}", "initialisé");
            new SChatCommand("/mairie", "Permet de faire une annonce mairie", "/mairie", (player, args) =>
            {
                if (player.IsAdmin)
                {
                    MairiePanel(player);
                }
            }).Register();
            Menu.AddAdminPluginTabLine(PluginInformations, 1, "Faire une annonce mairie", (ui) =>
            {
                Player player = PanelHelper.ReturnPlayerFromPanel(ui);
                MairiePanel(player);
            });
        }

        public void MairiePanel(Player player)
        {
            UIPanel panel = new UIPanel("Confirmation", UIPanel.PanelType.Input);

            panel.SetInputPlaceholder("Annonce...");

            panel.AddButton("Annuler", ui => player.ClosePanel(panel));
            panel.AddButton("Publier", delegate
            {
                string annonce = panel.inputText;
                if (string.IsNullOrEmpty(annonce))
                {
                    player.Notify("Erreur", "Veuillez entrer une annonce valide", NotificationManager.Type.Error, 10f);
                }
                else
                {
                    player.ClosePanel(panel);
                    Nova.server.SendMessageToAll($"{Format.Color("[Annonce] - [Mairie] :", Format.Colors.Error)} {annonce}");
                    player.Notify("Mairie98", "Annonce publiée avec succès !", NotificationManager.Type.Success);
                }
            });
            player.ShowPanelUI(panel);
        }
    }
}
