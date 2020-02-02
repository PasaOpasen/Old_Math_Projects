using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.MapObjects
{
    interface IOwnerable { int Owner { get; set; } }

    interface ITreasurable { Treasure Treasure { get; set; } }

    interface IMillitary { Army Army { get; set; } }

    public class Dwelling : IOwnerable
    {
        public int Owner { get; set; }
    }

    public class Mine : IOwnerable, IMillitary, ITreasurable
    {
        public int Owner { get; set; }
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Creeps : IMillitary, ITreasurable
    {
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Wolfs : IMillitary
    {
        public Army Army { get; set; }
    }

    public class ResourcePile : ITreasurable
    {
        public Treasure Treasure { get; set; }
    }

    public static class Interaction
    {
        public static void Make(Player player, object mapObject)
        {
            if (mapObject is IMillitary)
            {
                if (!player.CanBeat(((IMillitary)mapObject).Army))
                {
                    player.Die(); 
                    return;
                }
            }

            if (mapObject is IOwnerable)
            {
                ((IOwnerable)mapObject).Owner = player.Id;
            }

            if (mapObject is ITreasurable)
            {
                player.Consume(((ITreasurable)mapObject).Treasure);
            }

            //if (mapObject is Mine)
            //{
            //    if (player.CanBeat(((Mine)mapObject).Army))
            //    {
            //        ((Mine)mapObject).Owner = player.Id;
            //        player.Consume(((Mine)mapObject).Treasure);
            //    }
            //    else player.Die();
            //    return;
            //}
            //if (mapObject is Creeps)
            //{
            //    if (player.CanBeat(((Creeps)mapObject).Army))
            //        player.Consume(((Creeps)mapObject).Treasure);
            //    else
            //        player.Die();
            //    return;
            //}
        }
    }
}
