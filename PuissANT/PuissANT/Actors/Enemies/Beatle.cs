﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PuissANT.Actors.Ants;

namespace PuissANT.Actors.Enemies
{
    public class Beatle : Enemy
    {
        public override void Update(GameTime time)
        {
            //Move towards queen.
            QueenAnt qa = ActorManager.Instance.GetActorsByType<QueenAnt>().First();
            throw new NotImplementedException();
        }

        public override void Render(GameTime time)
        {
            throw new NotImplementedException();
        }
    }
}