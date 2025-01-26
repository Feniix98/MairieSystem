using AAMenu;
using Life;
using Life.Network;
using Life.UI;
using ModKit.Helper.DiscordHelper;
using Format = ModKit.Helper.TextFormattingHelper;

namespace Mairie98
{
    public class Main : ModKit.ModKit
    {
        public Main(IGameAPI gameAPI ) : base(gameAPI) 
        {
            PluginInformations = new ModKit.Interfaces.PluginInformations("Mairie98", "1.0.0", "Fenix98");
        }
        public override async void OnPluginInit()
        {
            base.OnPluginInit();
            DiscordWebhookClient webhook = new DiscordWebhookClient("https://discord.com/api/webhooks/0153290707036/ZEY9CDGBOCOBYGY1TJhihsppu-TPdSwl8a4JLhsZHWuUB573Mna7ZQXK4g4m5T-vAbQc");
            await DiscordHelper.SendMsg(webhook, "**[Mairie98] - [Fenix98]**" +
                $"\n Le plugin s'est initialisée sur {Nova.serverInfo.serverListName}");
            InsertMenu(); 
        }
        public void InsertMenu()
        {
            Menu.AddAdminPluginTabLine(PluginInformations, 1, "Mairie98", (ui) =>
            {
                Player player = PanelHelper.ReturnPlayerFromPanel(ui);
                Mairie(player);
            });
        }
        public void Mairie(Player player)
        {
            UIPanel panel = new UIPanel("Mairie", UIPanel.PanelType.Input);
            panel.AddButton($"{Format.Color("Fermer", Format.Colors.Error)}", ui => player.ClosePanel(panel));
            panel.AddButton($"{Format.Color("Envoyer", Format.Colors.Success)}", ui =>
            {
                player.ClosePanel(panel);
                Nova.server.SendMessageToAll($"<color={LifeServer.COLOR_RED}>[MAIRIE]</color> : " + panel.inputText);
                foreach(var players in Nova.server.GetAllInGamePlayers())
                {
                    players.setup.TargetShowCenterText($"<color={LifeServer.COLOR_RED}>[MAIRIE]</color>", panel.inputText, 15f);
                }
            });
            player.ShowPanelUI(panel);
        }
    }
}
