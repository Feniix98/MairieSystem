using Life.Network;
using Life;
using Life.UI;
using AAMenu;
using ModKit.Interfaces;
using mk = ModKit.Helper.TextFormattingHelper;
using System;
using Socket.Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MairieSystem
{
    public class MairieSystem : ModKit.ModKit
    {
        public MairieSystem(IGameAPI aPI) : base(aPI)
        {
            PluginInformations = new PluginInformations("MairieSystem", "1.1.0", "! Fenix");
        }

        public static void Notify(Player player, string message, NotificationManager.Type type, float seconds = 6f)
        {
            mk.Colors color = mk.Colors.Info;
            if (type == NotificationManager.Type.Error) color = mk.Colors.Error;
            else if (type == NotificationManager.Type.Warning) color = mk.Colors.Warning;
            else if (type == NotificationManager.Type.Success) color = mk.Colors.Success;

            player.Notify(mk.Color(nameof(MairieSystem), color), message, type, seconds);
        }

        private static void Debug(object message, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine($"[{nameof(MairieSystem)}] " + (message?.ToString() ?? "unknown message"));
            Console.ResetColor();
        }


        public override async void OnPluginInit()
        {
            base.OnPluginInit();

            new SChatCommand("/mairie", new string[] { "/ms" }, "Implémente un système de mairie", "/mairie", (player, args) =>
            {
                if (player.IsAdmin)
                {
                    SendAnnounce(player);
                }
                else Notify(player, "Vous n'avez pas les permissions administrateurs", NotificationManager.Type.Error);
            });
            Menu.AddAdminPluginTabLine(PluginInformations, 1, "MairieSystem", (ui) =>
            {
                Player player = PanelHelper.ReturnPlayerFromPanel(ui);
                SendAnnounce(player);
            });

            Debug("Plugin is ready !", ConsoleColor.DarkGreen);
        }

        public void SendAnnounce(Player player)
        {
            UIPanel panel = new UIPanel(mk.Color("MairieSystem", mk.Colors.Info), UIPanel.PanelType.Input);

            panel.SetInputPlaceholder("Message...");

            panel.AddButton(mk.Color("Fermer", mk.Colors.Error), ui => player.ClosePanel(panel));
            panel.AddButton("Envoyer", delegate
            {
                if (string.IsNullOrEmpty(panel.inputText))
                {
                    Notify(player, "Veuillez entrer une annonce valide !", NotificationManager.Type.Warning);
                    return;
                }
                player.ClosePanel(panel);
                Nova.server.SendMessageToAll(mk.Color("[MairieSystem]", mk.Colors.Info) + " : " + panel.inputText);
                Notify(player, "Votre message a bien été envoyé !", NotificationManager.Type.Success);
            });

            player.ShowPanelUI(panel);
        }
    }
}
