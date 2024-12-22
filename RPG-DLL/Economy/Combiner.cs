using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_DLL.Entities;
using RPG_DLL.Systems;

namespace RPG_DLL.Economy
{
    public class Combiner
    {
        public void CombineMineStones(Player player, MineStone mineStone)
        {
            const int requiredCount = 10;
            Inventory PlayerInv = player.Inventory;
            var mineStoneType = mineStone.GetType();

            // Trouver les pierres de mine du même type avec une quantité suffisante
            var mineStonesToCombine = PlayerInv.MineStones
                .Where(ms => ms.Key.GetType() == mineStoneType && ms.Value >= requiredCount)
                .Select(ms => ms.Key)
                .FirstOrDefault();

            if (mineStonesToCombine != null)
            {
                // Décrémenter le compteur des pierres de mine
                PlayerInv.RemoveMineStone(mineStonesToCombine);
                for (int i = 1; i < requiredCount; i++)
                {
                    PlayerInv.RemoveMineStone(mineStonesToCombine);
                }

                // Créer une nouvelle pierre de mine de niveau supérieur
                var higherTierMineStone = CreateHigherTierMineStone(mineStone);

                // Ajouter la nouvelle pierre de mine de niveau supérieur à l'inventaire
                higherTierMineStone.AddToPlayer(player);
            }
        }

        private MineStone CreateHigherTierMineStone(MineStone mineStone)
        {
            return new MineStone(mineStone.Tier + 1, mineStone.StatTypes);
        }
    }
}
