#include <amxmodx>
#include <hl_player_models_api>
#include <fakemeta>

#define PLUGIN "Team Menu"
#define VERSION "1.0"
#define AUTHOR "Glaster"

new const g_Skins[][] =
    {
        "USSR",
        "GERMANY",
        "USA",
        "CHINA",
        "ALIENS",
        "VIP"};

public
plugin_precache()
{
    for (new i; i < sizeof(g_Skins); i++)
        precache_model(fmt("models/player/%s/%s.mdl", g_Skins[i], g_Skins[i]));
}

public
plugin_init()
{
    register_clcmd("say /team", "TeamMenu");
    register_plugin(PLUGIN, VERSION, AUTHOR);
}

public
TeamMenu(id)
{
    new i_Menu = menu_create("\yTEAM MENU", "TeamMenu_handler");
    menu_additem(i_Menu, "\w[1] GERMANY", "1", 0);
    menu_additem(i_Menu, "\w[2] USSR", "2", 0);
    menu_additem(i_Menu, "\w[3] USA", "3", 0);
    menu_additem(i_Menu, "\w[4] CHINA", "4", 0);
    menu_additem(i_Menu, "\w[5] ALIENS", "5", 0);
    menu_additem(i_Menu, "\y[6] VIP", "6", 0);
    menu_additem(i_Menu, "\r[7] EXIT", "7", 0);
    menu_display(id, i_Menu, 0)
}

public
TeamMenu_handler(id, menu, item)
{
    if (item < 0)
        return PLUGIN_CONTINUE;
    new cmd[3], access, callback;
    menu_item_getinfo(menu, item, access, cmd, 2, _, _, callback);
    new Choise = str_to_num(cmd);
    switch (Choise)
    {
    case 1:
    {
        hl_set_player_team(id, 0);
        hl_set_player_model(id, g_Skins[0]);
        client_print(id, print_chat, "Пытаешься поменять тиму? нуну")
    }
    case 2:
    {
        client_print(id, print_chat, "Пытаешься поменять тиму? нуну")
            hl_set_player_team(id, 1);
        hl_set_player_model(id, g_Skins[1]);
    }
    case 3:
    {
        client_print(id, print_chat, "Пытаешься поменять тиму? нуну")
            hl_set_player_team(id, 2);
        hl_set_player_model(id, g_Skins[2]);
    }
    case 4:
    {
        hl_set_player_team(id, 3);
        hl_set_player_model(id, g_Skins[3]);
        client_print(id, print_chat, "Пытаешься поменять тиму? нуну")
    }
    case 5:
    {
        hl_set_player_team(id, 4);
        hl_set_player_model(id, g_Skins[4]);
        client_print(id, print_chat, "Пытаешься поменять тиму? нуну")
    }
    case 6:
    {
        if (get_user_flags(id) & ADMIN_IMMUNITY || get_user_flags(id) & ADMIN_RESERVATION || get_user_flags(id) & ADMIN_KICK || get_user_flags(id) & ADMIN_BAN || get_user_flags(id) & ADMIN_SLAY || get_user_flags(id) & ADMIN_MAP)
        {
            hl_set_player_team(id, 5);
            hl_set_player_model(id, g_Skins[5]);
            client_print(id, print_chat, "Пытаешься поменять тиму? нуну")
        }
        else
        {
            client_print(id, print_chat, "You shold be VIP or Admin to be a part of this team")
        }
    }
    case 7:
    {
    }
    }
    return PLUGIN_HANDLED;
}
